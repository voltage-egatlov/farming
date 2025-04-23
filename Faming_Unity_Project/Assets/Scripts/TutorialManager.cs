using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System; // for Action
using System.Collections;


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public bool InTutorial { get; private set; }

    [Header("UI References")]
    public GameObject tutorialCanvas;     // root Canvas (start disabled)
    public TextMeshProUGUI messageText;   // your text field
    public Button nextButton;             // “Next” button

    int step = 0;
    Coroutine hideCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartTutorial();
    }

    void StartTutorial()
    {
        InTutorial = true;
        tutorialCanvas.SetActive(true);
        step = 0;

        // Wire the Next button to AdvanceStep
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(AdvanceStep);

        AdvanceStep();
    }

    void AdvanceStep()
    {
        step++;
        // hide Next until we decide it’s needed
        nextButton.gameObject.SetActive(false);

        switch (step)
        {
            case 1:
                messageText.text = "Welcome to FARMING! Let’s get started.";
                // manual “click Next” step:
                nextButton.gameObject.SetActive(true);
                break;

            case 2:
                messageText.text = "Drive your tractor up to the store entrance to open the Store menu.";
                nextButton.gameObject.SetActive(true);
                break;

            case 3:
                messageText.text = "Inside the Store, you can upgrade your tractor, buy crop seeds, or purchase new land.";
                nextButton.gameObject.SetActive(true);
                break;

            case 4:
                messageText.text = "First, upgrade your tractor to boost its speed—but don’t spend all your money at once.";
                nextButton.gameObject.SetActive(true);
                break;

            case 5:
                messageText.text = "Next, purchase a plot of land. Then head to your new field to begin planting.";
                nextButton.gameObject.SetActive(true);
                break;

            case 6:
                messageText.text = "Press '1' on your keyboard to equip your seeds, then drive onto the field to plant them.";
                nextButton.gameObject.SetActive(true);
                break;

            case 7:
                messageText.text = "Tutorial complete! Good luck on Farming";
                nextButton.gameObject.SetActive(true);
                break;

            default:
                EndTutorial();
                break;
        }

        //hideCoroutine = StartCoroutine(HideAfterSeconds(100f));
    }

    //IEnumerator HideAfterSeconds(float seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    tutorialCanvas.SetActive(false);
    //}

    //void OnStoreOpened()
    //{
    //    StoreManager.Instance.OnStoreOpened -= OnStoreOpened;
    //    AdvanceStep();
    //}

    //void OnLandPurchased()
    //{
    //    StoreManager.Instance.OnLandPurchased -= OnLandPurchased;
    //    AdvanceStep();
    //}

    //void OnSeedPurchased()
    //{
    //    StoreManager.Instance.OnSeedPurchased -= OnSeedPurchased;
    //    AdvanceStep();
    //}

    //void OnTractorUpgraded()
    //{
    //    StoreManager.Instance.OnTractorUpgraded -= OnTractorUpgraded;
    //    AdvanceStep();
    //}

    void EndTutorial()
    {
        tutorialCanvas.SetActive(false);
        InTutorial = false;
    }
}

