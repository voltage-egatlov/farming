// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraController : MonoBehaviour
// {
//     // The camera will follow the tractor from an above angle, and be orthographic
//     public Transform tractor; // Reference to the tractor's transform
//     public Vector3 offset; // Offset from the tractor's position
//     public float height = 5f; // Height of the camera above the tractor
//     public float distance = 10f; // Distance from the tractor
//     public float smoothSpeed = 0.125f; // Speed of the camera smoothing
//     public float rotationSpeed = 5f; // Speed of the camera rotation
    
//     // Start is called before the first frame update
//     void Start()
//     {
//         // Set the initial offset based on the tractor's position and the specified height and distance
//         offset = new Vector3(0, height, -distance);
//         tractor = GameObject.FindGameObjectWithTag("Tractor").transform; // Find the tractor by its tag
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Calculate the desired position of the camera based on the tractor's position and the offset
//         Vector3 desiredPosition = tractor.position + offset;

//         // Smoothly interpolate the camera's position to the desired position
//         Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//         transform.position = smoothedPosition;

//         // Rotate the camera to look at the tractor
//         Quaternion lookRotation = Quaternion.LookRotation(tractor.position - transform.position);
//         transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform tractor; // Tractor to follow
    public float height = 5f; // Camera height above tractor
    public float distance = 10f; // Camera distance behind tractor
    public float smoothSpeed = 0.125f; // How smooth the position follows
    public float lookDownAngle = 5f; // Angle to tilt the camera down

    private Vector3 offset;

    void Start()
    {
        if (tractor == null)
            tractor = GameObject.FindGameObjectWithTag("Tractor").transform;

        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        if (tractor == null) return;

        // Rotate the offset to stay behind the tractor
        Vector3 rotatedOffset = tractor.rotation * offset;
        Vector3 desiredPosition = tractor.position + rotatedOffset;

        // Smoothly move the camera to the new position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Immediately face the tractor with a slight downward tilt
        Quaternion lookRotation = Quaternion.LookRotation(tractor.position - transform.position);
        Quaternion tiltRotation = Quaternion.Euler(lookDownAngle, lookRotation.eulerAngles.y, 0);

        // Directly apply rotation (no lag)
        transform.rotation = tiltRotation;
    }
}

