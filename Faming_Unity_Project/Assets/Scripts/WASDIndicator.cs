using UnityEngine;

public class WASDIndicator : MonoBehaviour
{
    void Update()
    {
        // if any of the four keys is pressed, hide this icon
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            gameObject.SetActive(false);
            // you can also disable this script if you like:
            // enabled = false;
        }
    }
}
