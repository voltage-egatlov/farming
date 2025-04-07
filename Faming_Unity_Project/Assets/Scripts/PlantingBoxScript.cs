using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingBoxScript : MonoBehaviour
{
    public GameObject PlantingVisual; // Reference to the planting visual object
    private Renderer canSee; // Renderer component of the planting visual object
    private GameObject floatingText; // Floating text object to show the current state of the planting box

    public enum BoxState
    {
        Empty,
        Planted,
        Watered,
        Fertilized,
        Ready
    } // Enum to represent the state of the planting box
    [SerializeField] public BoxState currentState = BoxState.Empty; // Current state of the planting box, default is Empty

    // Start is called before the first frame update
    void Start()
    {
        PlantingVisual = this.gameObject; // Assign the current game object to the PlantingVisual variable
        canSee = PlantingVisual.GetComponent<Renderer>();

        //Instantiate a floating text object to show the current state of the planting box
        floatingText = new GameObject("FloatingText");
        TextMesh textMesh = floatingText.AddComponent<TextMesh>();
        textMesh.text = currentState.ToString(); // Set the text to the current state of the planting box
        textMesh.fontSize = 12; // Set the font size of the text
        textMesh.color = Color.white; // Set the text color to white
        textMesh.transform.SetParent(PlantingVisual.transform); // Set the floating text as a child of the planting visual object
        textMesh.transform.localPosition = new Vector3(0, 3, 0); // Set the position of the floating text above the planting box
        textMesh.anchor = TextAnchor.MiddleCenter; // Center the text

        canSee.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        floatingText.GetComponent<TextMesh>().text = currentState.ToString();
        // rotate the floating text to always face the camera
        floatingText.transform.LookAt(Camera.main.transform.position);
        floatingText.transform.Rotate(0, 180, 0);
        floatingText.transform.rotation = Quaternion.Euler(0, floatingText.transform.rotation.eulerAngles.y, 0);
        
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
