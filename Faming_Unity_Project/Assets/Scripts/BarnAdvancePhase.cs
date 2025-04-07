using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarnAdvancePhase : MonoBehaviour
{
    public GameObject Crop; // Reference to the seed prefab
    public TextMeshProUGUI PhaseText; // Reference to the UI TextMeshPro component for displaying the current phase

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the tractor
        if (other.CompareTag("TractorModel"))
        {
            PlantingBoxScript[] plantingBoxes = FindObjectsOfType<PlantingBoxScript>();
            int numHarvested = 0;
            foreach (PlantingBoxScript plantingBox in plantingBoxes)
            {
                if (plantingBox.currentState == PlantingBoxScript.BoxState.Watered)
                {
                    clearPlot(plantingBox.gameObject);
                    float plantingWidth = plantingBox.transform.localScale.x; // Get the width of the planting box
                    float plantingLength = plantingBox.transform.localScale.z; // Get the length of the planting box

                    float numCropsWidth = Mathf.Round(plantingWidth*25); // Calculate the number of crops that can fit in the width of the planting box
                    float numCropsLength = Mathf.Round(plantingLength*25); // Calculate the number of crops that can fit in the length of the planting box

                    for (int i = 0; i < numCropsWidth; i++) // Loop through the width of the planting box
                    {
                        for (int j = 0; j < numCropsLength; j++) // Loop through the length of the planting box
                        {
                            GameObject newCrop = Instantiate(Crop, new Vector3(0, 0, 0), Quaternion.identity, plantingBox.transform);
                            newCrop.transform.localScale = new Vector3(.75f, 7.5f, .75f); // Set the scale of the crop object
                            newCrop.transform.localPosition = new Vector3(-.5f + (i + .5f) * (1 / numCropsWidth), 0, -.5f + (j + .5f) * (1 / numCropsLength)); // Position the crop within the planting box

                            plantingBox.currentState = PlantingBoxScript.BoxState.Ready; // Change the state of the planting box to Empty
                        }
                    }
                    numHarvested++;
                }
                PhaseText.text = numHarvested + " plots ready to harvest";
                if(plantingBox.currentState!= PlantingBoxScript.BoxState.Empty && plantingBox.currentState!= PlantingBoxScript.BoxState.Ready)
                {
                    clearPlot(plantingBox.gameObject); // Clear the planting box of any existing crops
                    plantingBox.currentState = PlantingBoxScript.BoxState.Empty; // Change the state of the planting box to Empty
                }
            }
        }
    }

    void clearPlot(GameObject plot) // Function to clear the children of a planting box
    {
        for(int i = plot.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(plot.transform.GetChild(i).gameObject); // Destroy each child object of the planting box
        }
    }
}
