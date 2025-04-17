using UnityEngine;

public class TornadoMovement : MonoBehaviour
{
    public float movementSpeed = 5f; // How fast the tornado moves
    public Vector3 movementAreaSize = new Vector3(5, 0, 5); // Define the area in which the tornado wanders
    
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        // Save the starting position (optional: this could be used as the center for random movement)
        initialPosition = transform.position;
        SetNewRandomTarget();
    }

    void Update()
    {
        // Move toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        // When close enough to the target, choose a new target
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewRandomTarget();
        }
    }

    void SetNewRandomTarget()
    {
        // Calculate a new random point within the defined movement area around the initial position.
        float randomX = Random.Range(-movementAreaSize.x, movementAreaSize.x);
        float randomZ = Random.Range(-movementAreaSize.z, movementAreaSize.z);
        // Y can remain the same for a horizontal movement effect (or adjust if you want vertical variation).
        targetPosition = initialPosition + new Vector3(randomX, 0, randomZ);
    }
}
