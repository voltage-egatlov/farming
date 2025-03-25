using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorPlant : MonoBehaviour
{
    public GameObject cropPrefab;
    public float plantOffset;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlantCrop();
        }
    }

    void PlantCrop()
    {
        Vector3 plantPosition = transform.position - transform.forward * plantOffset;
        plantPosition.y = 0f;
        Instantiate(cropPrefab, plantPosition, Quaternion.identity);
    }
}
