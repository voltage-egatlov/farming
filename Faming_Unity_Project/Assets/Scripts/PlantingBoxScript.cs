using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingBoxScript : MonoBehaviour
{
    public GameObject PlantingVisual; // Reference to the planting visual object
    private Renderer canSee; // Renderer component of the planting visual object

    public enum BoxState
    {
        Empty,
        Planted,
        Watered,
        Fertilized
    } // Enum to represent the state of the planting box
    [SerializeField] public BoxState currentState = BoxState.Empty; // Current state of the planting box, default is Empty

    // Start is called before the first frame update
    void Start()
    {
        PlantingVisual = this.gameObject; // Assign the current game object to the PlantingVisual variable
        canSee = PlantingVisual.GetComponent<Renderer>();

        canSee.enabled = false;
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
            // turn renderer on when the tractor enters the trigger
            canSee.enabled = true; 
        }
    }
    void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is the tractor
        if (other.CompareTag("TractorModel"))
        {
            // turn renderer off when the tractor exits the trigger
            canSee.enabled = false; 
        }
    }
}
