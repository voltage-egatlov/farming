using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tractor_Handler : MonoBehaviour
{
    public TextMeshProUGUI SpeedText; // Reference to the UI TextMeshPro component for displaying forward/reverse gear

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

    public GameObject Crop;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component if not assigned
        if (tractorRigidbody == null)
        {
            tractorRigidbody = GetComponent<Rigidbody>();
        }

        SpeedText = GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>(); // Find the UI TextMeshPro component in the scene
        // Initialize the UI text to show the initial gear
        SpeedText.text ="0"; // Default to forward gear

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
        
        SpeedText.text = Mathf.Round(tractorRigidbody.velocity.magnitude*10).ToString(); // Update the UI text to show the current speed of the tractor

        // Update wheel rotation based on the tractor's velocity
        FrontLeftWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
        FrontRightWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
        BackLeftWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
        BackRightWheel.transform.Rotate(0, tractorRigidbody.velocity.x * Time.deltaTime * rotateModifier, 0);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlanterBox"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlantingBoxScript plantingBox = other.GetComponent<PlantingBoxScript>();
                if (plantingBox != null && plantingBox.currentState == PlantingBoxScript.BoxState.Empty)
                {
                    float plantingWidth = other.transform.localScale.x; // Get the width of the planting box
                    float plantingLength = other.transform.localScale.z; // Get the length of the planting box

                    float numCropsWidth = Mathf.Round(plantingWidth*25); // Calculate the number of crops that can fit in the width of the planting box
                    float numCropsLength = Mathf.Round(plantingLength*25); // Calculate the number of crops that can fit in the length of the planting box

                    for (int i = 0; i < numCropsWidth; i++) // Loop through the width of the planting box
                    {
                        for (int j = 0; j < numCropsLength; j++) // Loop through the length of the planting box
                        {
                            GameObject newCrop = Instantiate(Crop, new Vector3(0, 0, 0), Quaternion.identity, other.transform);
                            newCrop.transform.localScale = new Vector3(.75f, 7.5f, .75f); // Set the scale of the crop object
                            newCrop.transform.localPosition = new Vector3(-.5f + (i + .5f) * (1 / numCropsWidth), 0, -.5f + (j + .5f) * (1 / numCropsLength)); // Position the crop within the planting box
                        }
                    }
                    plantingBox.currentState = PlantingBoxScript.BoxState.Planted; // Change the state of the planting box to Planted
                }
            }   
        }
    }
}
