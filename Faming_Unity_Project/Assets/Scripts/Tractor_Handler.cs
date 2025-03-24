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
    }
}
