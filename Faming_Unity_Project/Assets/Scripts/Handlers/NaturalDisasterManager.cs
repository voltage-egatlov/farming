using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalDisasterManager : MonoBehaviour
{
    // Reference to the tornado prefab (assign your tornado GameObject prefab in the Inspector)
    public GameObject tornadoPrefab;

    // Spawn point (Transform) for the tornado
    public Transform tornadoSpawnPoint;

    // Minimum and maximum interval in seconds between tornado events (e.g., 120s = 2 minutes, 300s = 5 minutes)
    public float minInterval = 120f;
    public float maxInterval = 300f;

    // Duration (in seconds) after which the tornado object will be destroyed (when its animation finishes)
    public float tornadoDuration = 10f;

    void Start()
    {
        // Start the tornado event coroutine which triggers events indefinitely
        StartCoroutine(TornadoEventRoutine());
    }

    /// <summary>
    /// A coroutine that waits for a random interval (between minInterval and maxInterval)
    /// then triggers a tornado event, then repeats.
    /// </summary>
    IEnumerator TornadoEventRoutine()
    {
        while (true)
        {
            // Wait for a random time interval before triggering the event
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Now trigger the tornado event
            TriggerTornado();
        }
    }

    /// <summary>
    /// Instantiates the tornado prefab at the designated spawn point and then destroys half of the crops.
    /// </summary>
    void TriggerTornado()
    {
        Debug.Log("Tornado event triggered!");

        // Instantiate the tornado prefab if both the prefab and spawn point have been assigned
        if (tornadoPrefab != null && tornadoSpawnPoint != null)
        {
            // Instantiate the tornado and automatically destroy it after tornadoDuration seconds
            GameObject tornado = Instantiate(tornadoPrefab, tornadoSpawnPoint.position, tornadoSpawnPoint.rotation);
            Destroy(tornado, tornadoDuration);
        }
        else
        {
            Debug.LogWarning("Tornado prefab or spawn point is not assigned in NaturalDisasterManager!");
        }

        // Call a method that goes through each planting box and destroys approximately half the crops
        DestroyHalfTheCropsInPlantingBoxes();
    }

    /// <summary>
    /// Finds all PlantingBoxScript components in the scene and calls their DestroyHalfOfCrops() method.
    /// </summary>
    void DestroyHalfTheCropsInPlantingBoxes()
    {
        PlantingBoxScript[] plantingBoxes = FindObjectsOfType<PlantingBoxScript>();

        foreach (PlantingBoxScript box in plantingBoxes)
        {
            // Call the method that destroys roughly half of the crops in the box.
            box.DestroyHalfOfCrops();
        }

        Debug.Log("Tornado has destroyed half the crops in each planting box.");
    }
}
