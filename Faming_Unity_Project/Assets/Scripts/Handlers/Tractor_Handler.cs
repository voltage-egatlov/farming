using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tractor_Handler : MonoBehaviour
{
    public GameObject IconGroup;

    public Rigidbody tractorRigidbody; // Reference to the Rigidbody component of the tractor
    public float forwardSpeed = 5f; // Maximum speed of the tractor
    public float reverseSpeed = 2f; // Maximum reverse speed of the tractor

    public float forwardTurnSpeed = 1f; // Speed at which the tractor turns
    public float reverseTurnSpeed = .4f; // Speed at which the tractor turns in reverse

    public float horizontalInput; // Variable to store horizontal input for steering
    public float verticalInput; // Variable to store vertical input for acceleration/deceleration


    public GameObject FrontRightWheel;
    public GameObject FrontLeftWheel;
    public GameObject BackRightWheel;
    public GameObject BackLeftWheel;

    public float rotateModifier = 10f; // Modifier for wheel rotation speed based on tractor's velocity

    public GameObject Seed;

    public enum currentEquippedItem
    {
        None,
        Seed,
        Fertilizer,
        Water
    } // Enum to represent the currently equipped item

    [SerializeField] public currentEquippedItem currentItem = currentEquippedItem.None; // Current equipped item, default is None

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component if not assigned
        if (tractorRigidbody == null)
        {
            tractorRigidbody = GetComponent<Rigidbody>();
        }


    }
    // Update is called once per frame
    void Update()
    {
        // Get the horizontal and vertical input from the user
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Update the tractor's velocity based on the input
        if (verticalInput > 0)
        {
            // Move forward
            tractorRigidbody.velocity = Vector3.Lerp(tractorRigidbody.velocity, transform.forward * verticalInput * forwardSpeed, Time.deltaTime * 10f); // Gradually increase speed
            if (horizontalInput != 0)
            {
                // Apply steering if there is horizontal input
                tractorRigidbody.angularVelocity = new Vector3(0, horizontalInput * forwardTurnSpeed, 0);
            }
        }
        else if (verticalInput < 0)
        {
            // Move in reverse
            tractorRigidbody.velocity = transform.forward * verticalInput * reverseSpeed;
            if (horizontalInput != 0)
            {
                // Apply steering if there is horizontal input
                tractorRigidbody.angularVelocity = new Vector3(0, -horizontalInput * reverseTurnSpeed, 0);
            }
        }
        else if (verticalInput == 0 && tractorRigidbody.velocity.magnitude > 0)
        {
            // If no input is given, slow down the tractor gradually
            tractorRigidbody.velocity = Vector3.Lerp(tractorRigidbody.velocity, Vector3.zero, Time.deltaTime * 100f); // Gradually reduce speed to zero
        }
        else if (horizontalInput == 0 && verticalInput==0) {
            // If no horizontal input is given, stop the tractor from turning
            tractorRigidbody.angularVelocity = Vector3.zero; // Stop rotation
        }
        // Update wheel rotation based on the tractor's velocity
        FrontLeftWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
        FrontRightWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
        BackLeftWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
        BackRightWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
    }

    void OnTriggerEnter (Collider other)
    {
        // Check if the object that entered the trigger is the planting box
        if (other.CompareTag("SeedStorage"))
        {
            currentItem = currentEquippedItem.Seed; // Set the current equipped item to Seed
        }
        else if (other.CompareTag("FertilizerStorage"))
        {
            currentItem = currentEquippedItem.Fertilizer; // Set the current equipped item to Fertilizer
        }
        else if (other.CompareTag("WaterTower"))
        {
            currentItem = currentEquippedItem.Water; // Set the current equipped item to Water
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlanterBox"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlantingBoxScript plantingBox = other.GetComponent<PlantingBoxScript>();

                if(GameManager.Instance.currentPhase == GameManager.Phase.Phase2)
                {
                    if (plantingBox != null && plantingBox.currentState == PlantingBoxScript.BoxState.Empty && currentItem == currentEquippedItem.Seed)
                    {
                        
                        string seedType = "Corn";

                        // Check if the inventory has at least one seed of this type.
                        if (!GameManager.Instance.UseSeed(seedType, 1))
                        {
                            Debug.Log("Planting aborted: Not enough " + seedType + " seeds available!");
                            return;
                        }
                        
                        float plantingWidth = other.transform.localScale.x; // Width of the planting box
                        float plantingLength = other.transform.localScale.z; // Length of the planting box

                        float numCropsWidth = Mathf.Round(plantingWidth * 25); // Calculate number of crops that can fit in the width
                        float numCropsLength = Mathf.Round(plantingLength * 25); // Calculate number of crops that can fit in the length

                        for (int i = 0; i < numCropsWidth; i++) // Loop through the width
                        {
                            for (int j = 0; j < numCropsLength; j++) // Loop through the length
                            {
                                GameObject newCrop = Instantiate(Seed, new Vector3(1, 1, 1), Quaternion.identity, other.transform);
                                newCrop.transform.localScale = new Vector3(1/(numCropsWidth+2), 1, 1/(numCropsWidth+2)); // Set crop scale
                                newCrop.transform.localPosition = new Vector3(-0.5f + (i + 0.5f) * (1 / numCropsWidth), 0, -0.5f + (j + 0.5f) * (1 / numCropsLength)); // Position the crop
                            }
                        }
                        plantingBox.currentState = PlantingBoxScript.BoxState.Planted; // Update state to Planted
                    }
                    else if (plantingBox != null && plantingBox.currentState == PlantingBoxScript.BoxState.Planted && currentItem == currentEquippedItem.Fertilizer)
                    {
                        plantingBox.currentState = PlantingBoxScript.BoxState.Fertilized; // Update state to Fertilized
                    }
                    else if (plantingBox != null && plantingBox.currentState == PlantingBoxScript.BoxState.Fertilized && currentItem == currentEquippedItem.Water)
                    {
                        plantingBox.currentState = PlantingBoxScript.BoxState.Watered; // Update state to Watered
                    }
                }
                else if (GameManager.Instance.currentPhase == GameManager.Phase.Phase4)
                {
                    if (plantingBox != null && plantingBox.currentState == PlantingBoxScript.BoxState.Ready)
                    {
                        harvestChildrenOfPlot(other.gameObject); // Clear the planting box
                        plantingBox.currentState = PlantingBoxScript.BoxState.Empty; // Update state to Empty
                    }
                }
            }
        }
    }

    void clearChildrenOfPlot(GameObject plot) // Function to clear the children of a planting box
    {
        for(int i = plot.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(plot.transform.GetChild(i).gameObject); // Destroy each child object of the planting box
        }
    }

    void harvestChildrenOfPlot(GameObject plot)
    {
        int harvestedCount = 0;
        // Assumes first two children (indices 0 and 1) are reserved (e.g., visuals or UI elements)
        for (int i = plot.transform.childCount - 1; i >= 2; i--)
        {
            // Destroy each crop child
            Destroy(plot.transform.GetChild(i).gameObject);
            harvestedCount++;
        }
        
        if (harvestedCount > 0)
        {
            // Add the harvested crops to inventory. Here we assume the crop type is "Corn".
            GameManager.Instance.AddCrop("Corn", harvestedCount);
            Debug.Log("Harvested " + harvestedCount + " crops and added them to inventory.");
        }
    }

}
