using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tractor_Handler : MonoBehaviour
{
    public GameObject IconGroup;
    
    // For changing model of tractor
    public GameObject[] TractorModels; // Three models total
    public int currentTractorStage = 0;

    public Rigidbody tractorRigidbody;
    public float forwardSpeed = 5f;
    public float reverseSpeed = 2f;
    public float forwardTurnSpeed = 1f;
    public float reverseTurnSpeed = .4f;

    public float horizontalInput;
    public float verticalInput;

    public GameObject FrontRightWheel;
    public GameObject FrontLeftWheel;
    public GameObject BackRightWheel;
    public GameObject BackLeftWheel;
    public float rotateModifier = 10f;

    public GameObject Seed;

    public enum currentEquippedItem { None, Seed, Fertilizer, Water }
    [SerializeField] public currentEquippedItem currentItem = currentEquippedItem.None;

    void Start()
    {
        if (tractorRigidbody == null)
            tractorRigidbody = GetComponent<Rigidbody>();

        // Set initial tractor to best model
        SetModel(0);
    }

    void Update()
    {
        // Handle flying off
        bool isGround = false;
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            isGround = true;
        }

        if (isGround)
        {
            // Handle driving input...
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput   = Input.GetAxis("Vertical");

            if (verticalInput > 0)
            {
                tractorRigidbody.velocity = Vector3.Lerp(
                    tractorRigidbody.velocity,
                    transform.forward * verticalInput * forwardSpeed,
                    Time.deltaTime * 10f
                );
                if (horizontalInput != 0)
                    tractorRigidbody.angularVelocity = new Vector3(0, horizontalInput * forwardTurnSpeed, 0);
            }
            else if (verticalInput < 0)
            {
                tractorRigidbody.velocity = transform.forward * verticalInput * reverseSpeed;
                if (horizontalInput != 0)
                    tractorRigidbody.angularVelocity = new Vector3(0, -horizontalInput * reverseTurnSpeed, 0);
            }
            else if (tractorRigidbody.velocity.magnitude > 0)
            {
                tractorRigidbody.velocity = Vector3.Lerp(
                    tractorRigidbody.velocity,
                    Vector3.zero,
                    Time.deltaTime * 100f
                );
            }

            if (horizontalInput == 0 && verticalInput == 0)
                tractorRigidbody.angularVelocity = Vector3.zero;
        }

        // Rotate wheels
        float wheelRot = tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier;
        FrontLeftWheel.transform.Rotate(0, wheelRot, 0);
        FrontRightWheel.transform.Rotate(0, wheelRot, 0);
        BackLeftWheel.transform.Rotate(0, wheelRot, 0);
        BackRightWheel.transform.Rotate(0, wheelRot, 0);

        // Modified equipping method
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentItem = currentEquippedItem.Seed;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentItem = currentEquippedItem.Fertilizer;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentItem = currentEquippedItem.Water;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("SeedStorage"))
    //        currentItem = currentEquippedItem.Seed;
    //    else if (other.CompareTag("FertilizerStorage"))
    //        currentItem = currentEquippedItem.Fertilizer;
    //    else if (other.CompareTag("WaterTower"))
    //        currentItem = currentEquippedItem.Water;
    //}

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("PlanterBox")) return;
        var box = other.GetComponent<PlantingBoxScript>();
        if (box == null) return;

        // PHASE 2: Planting & Fertilizing
        if (GameManager.Instance.currentPhase == GameManager.Phase.Phase2)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Planting
                if (box.currentState == PlantingBoxScript.BoxState.Empty
                    && currentItem == currentEquippedItem.Seed)
                {
                    const string seedType = "Corn";
                    if (!GameManager.Instance.UseSeed(seedType, 1))
                    {
                        Debug.Log($"Planting aborted: Not enough {seedType} seeds!");
                        return;
                    }

                    float w = other.transform.localScale.x;
                    float l = other.transform.localScale.z;
                    int cols = Mathf.RoundToInt(w * 25);
                    int rows = Mathf.RoundToInt(l * 25);

                    for (int i = 0; i < cols; i++)
                        for (int j = 0; j < rows; j++)
                        {
                            var newCrop = Instantiate(Seed,
                                Vector3.zero,
                                Quaternion.identity,
                                other.transform);
                            newCrop.transform.localScale = new Vector3(1f/(cols+2),1,1f/(rows+2));
                            newCrop.transform.localPosition = new Vector3(
                                -0.5f + (i + 0.5f) * (1f/cols),
                                0,
                                -0.5f + (j + 0.5f) * (1f/rows)
                            );
                            newCrop.tag = "Crop";
                        }
                    box.currentState = PlantingBoxScript.BoxState.Planted;
                }
                // Fertilizing
                else if (box.currentState == PlantingBoxScript.BoxState.Planted
                         && currentItem == currentEquippedItem.Fertilizer)
                {
                    box.currentState = PlantingBoxScript.BoxState.Fertilized;
                }
                // Watering
                else if (box.currentState == PlantingBoxScript.BoxState.Fertilized
                         && currentItem == currentEquippedItem.Water)
                {
                    box.currentState = PlantingBoxScript.BoxState.Watered;
                }
            }
            return;
        }

        // PHASE 4: Manual Harvesting
        if (GameManager.Instance.currentPhase == GameManager.Phase.Phase4)
        {
            if (box.currentState == PlantingBoxScript.BoxState.Ready
                && Input.GetKeyDown(KeyCode.Space))
            {
                HarvestPlot(box);
            }
            return;
        }
    }

    /// <summary>
    /// Destroys all crop children of the planter box, counts them, adds to inventory, and resets the box.
    /// </summary>
    private void HarvestPlot(PlantingBoxScript box)
    {
        GameObject plot = box.gameObject;
        int harvestedCount = 0;

        // Children 0 & 1 reserved; crops start at 2
        for (int i = plot.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(plot.transform.GetChild(i).gameObject);
            harvestedCount++;
        }

        if (harvestedCount > 0)
        {
            GameManager.Instance.AddCrop("Corn", harvestedCount);
            Debug.Log($"Harvested {harvestedCount} Corn into inventory.");
        }

        box.currentState = PlantingBoxScript.BoxState.Empty;
    }

    // Downgrade the tractor by changing its physical model
    public void Downgrade()
    {   
        Debug.Log("Downgrade triggered! Current stage: " + currentTractorStage);

        currentTractorStage++;

        if (currentTractorStage < TractorModels.Length)
        {
            SetModel(currentTractorStage);
        }
        else
        {
            // Otherwise, game over
            Debug.Log($"No more tractor models left! GAME OVER.");
            SceneManager.LoadScene("GameOver");
        }
    }

    // Set models of the tractor
    void SetModel(int index)
    {
        for (int i = 0; i < TractorModels.Length; i++)
        {
            TractorModels[i].SetActive(i == index);
            // currentModel = TractorModels[i];
            // FrontRightWheel = currentModel.transform.Find("FrontRightWheel");
            // FrontLeftWheel = currentModel.transform.Find("FrontLeftWheel");
            // BackRightWheel = currentModel.transform.Find("BackRightWheel");
            // BackLeftWheel = currentModel.transform.Find("BackLeftWheel");
        }
    }
}

