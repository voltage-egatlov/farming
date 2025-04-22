using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantingIconsOverAreaScript : MonoBehaviour
{
    public List<GameObject> plantingIcons; 

    public BoxState currentBoxState; // Current state of the planting box
    // Start is called before the first frame update

    void Start()
    {
        // Add all the children to the plantingIcons list
        plantingIcons = new List<GameObject>();
        foreach (Transform child in transform)
        {
            plantingIcons.Add(child.gameObject);
        }

        currentBoxState = BoxState.Empty; // Assuming the initial state is Empty
    }

    // Update is called once per frame
    void Update()
    {
        // Check the current state of the planting box and update icons to show what it needs
        if (currentBoxState == BoxState.Empty)
        {
            SetIconVisibility(true, true, true);
        }
        else if (currentBoxState == BoxState.Planted)
        {
            SetIconVisibility(false, true, true); // Show water and fertilizer icons
        }
        else if (currentBoxState == BoxState.Fertilized)
        {
            SetIconVisibility(false, false, true); // Show fertilizer icon only
        }
        else if (currentBoxState == BoxState.Watered)
        {
            SetIconVisibility(false, false, false); // No icons needed
        }
        else if (currentBoxState == BoxState.Ready)
        {
            SetIconVisibility(false, false, false); // No icons needed
        }
    }

    private void SetIconVisibility(bool plant, bool water, bool fertilizer)
    {
        plantingIcons[0].SetActive(plant); // Assuming index 0 is for planting icon
        plantingIcons[1].SetActive(water); // Assuming index 1 is for watering icon
        plantingIcons[2].SetActive(fertilizer); // Assuming index 2 is for fertilizing icon
    }
}
