using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerNameCreatorScript : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public GameObject TowerName; // Reference to the tower name object
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        TowerName = new GameObject("TowerName");

        TextMeshPro textMeshPro = TowerName.AddComponent<TextMeshPro>();

        textMeshPro.text = this.name;
        textMeshPro.fontSize = 12;
        textMeshPro.color = Color.white;
        textMeshPro.alignment = TextAlignmentOptions.Center;


        TowerName.transform.SetParent(this.transform);
        TowerName.transform.localPosition = new Vector3(0, 7, 0);

        if (this.name=="Store"){
            TowerName.transform.localPosition = new Vector3(-1.5f, 1, .1f);
            textMeshPro.fontSize = 18;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            TowerName.transform.LookAt(mainCamera.transform.position);
            TowerName.transform.Rotate(0, 180, 0);
        }        
        
    }
}
