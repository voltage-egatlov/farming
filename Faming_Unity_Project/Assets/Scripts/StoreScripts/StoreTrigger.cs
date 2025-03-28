using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
{
    Debug.Log("Triggered by: " + other.gameObject.name + " with tag: " + other.tag);
    if(other.CompareTag("Tractor"))
    {
        Debug.Log("Tractor detected - Opening Store");
        StoreManager.Instance.OpenStore();
    }
}

}
