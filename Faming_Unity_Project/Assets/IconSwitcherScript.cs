using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using currentEquippedItem = Tractor_Handler.currentEquippedItem;

public class IconSwitcherScript : MonoBehaviour
{
    public GameObject[] icons;
    public GameObject Tractor;

    // Start is called before the first frame update
    void Start()
    {
        Tractor = GameObject.FindGameObjectsWithTag("Tractor")[0];
        icons = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            icons[i] = transform.GetChild(i).gameObject;
        }
        foreach (GameObject icon in icons)
        {
            icon.SetActive(false);
        }

        // Activate the first icon
        if (icons.Length > 0)
        {
            icons[0].SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        currentEquippedItem tractorEquipped = Tractor.GetComponent<Tractor_Handler>().currentItem;
        for (int i = 0; i < icons.Length; i++)
        {
            if(icons[i].name == tractorEquipped.ToString())
            {
                icons[i].SetActive(true);
            }
            else
            {
                icons[i].SetActive(false);
            }
        }
        
    }
}
