using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingVisuals : MonoBehaviour
{
    public GameObject PlantingVisual; // Reference to the planting visual object
    public GameObject TotalPlantingArea;

    private float plantingAreaWidth; // Width of the total planting area
    private float plantingAreaLength; // Height of the total planting area

    public float sideCount=4; // Number of plots in the planting area

    private float plotWidth; // Width of each plot
    private float plotLength; // Length of each plot

    // Start is called before the first frame update
    void Start()
    {
        plantingAreaWidth = TotalPlantingArea.transform.localScale.x; // Get the width of the planting area from its scale
        plantingAreaLength = TotalPlantingArea.transform.localScale.z; // Get the length of the planting area from its scale 

        // Calculate the width and length of each plot based on the total area and number of plots
        plotWidth = plantingAreaWidth / sideCount; // Assuming square plots for simplicity
        plotLength = plantingAreaLength / sideCount; // Assuming square plots for simplicity

        // Loop through each plot in the planting area to position the planting visuals
        for (int i = 0; i < sideCount; i++)
        {
            for (int j = 0; j < sideCount; j++)
            {                
                GameObject newPlot = Instantiate(PlantingVisual, new Vector3(0, 0, 0), Quaternion.identity, TotalPlantingArea.transform);
                newPlot.transform.localScale = new Vector3(1/sideCount, 1, 1/(sideCount+1));
                newPlot.transform.localPosition = new Vector3(-.5f+ (i+.5f)*1/sideCount, 0.3f, -.5f + (j+.5f)*1/sideCount); 
                newPlot.name = "Plot_" + i + "_" + j;
                }
        }       
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
