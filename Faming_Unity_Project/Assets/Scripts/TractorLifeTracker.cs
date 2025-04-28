using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorLifeTracker : MonoBehaviour
{
    public Tractor_Handler tractorHandler;  
    public GameObject[] TractorLives; 
    private int maxLife = 3;  
    private int currentLife;  

    // Start is called before the first frame update
    void Start()
    {
        tractorHandler = FindObjectOfType<Tractor_Handler>();
        currentLife = maxLife;
        UpdateLifeUI();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the tractor life changed from downgrade
        int newLife = maxLife - tractorHandler.currentTractorStage;

        // If the tractor has lost a life (downgraded)
        if (newLife != currentLife)
        {
            currentLife = newLife;
            UpdateLifeUI();  
        }
    }

    void UpdateLifeUI()
    {
        for (int i = 0; i < maxLife; i++)
        {
            if (i < currentLife)
            {
                TractorLives[i].SetActive(true);  
            }
            else
            {
                TractorLives[i].SetActive(false);  
            }
        }
    }
}
