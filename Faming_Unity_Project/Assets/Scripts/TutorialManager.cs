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
        nextButton.gameObject.SetActive(false);

        switch (step)
        {
            case 1:
                messageText.text =
                    "Dear Child,\n\n" +
                    "I know this land has seen better days, and I can’t face those Texas tornadoes anymore. " +
                    "I’m leaving everything to you. Follow these instructions carefully and you’ll make enough selling crops to keep things afloat.\n\n";
                nextButton.gameObject.SetActive(true);
                break;

            case 2:
                messageText.text =
                    "First, hop into your trusty tractor and drive up to the Store entrance to open the Store menu.\n\n";
                nextButton.gameObject.SetActive(true);
                break;

            case 3:
                messageText.text =
                    "Inside the Store you can:\n" +
                    "  • Upgrade your tractor’s speed\n" +
                    "  • Buy crop seeds\n" +
                    "  • Purchase new plots of land\n\n" +
                    "Choose wisely.";
                nextButton.gameObject.SetActive(true);
                break;

            case 4:
                messageText.text =
                    "Let’s start by giving your tractor a speed boost.Each upgrade costs $200, don’t spend your whole budget at once.\n\n";
                nextButton.gameObject.SetActive(true);
                break;

            case 5:
                messageText.text =
                    "Now, still in the Store, select the Seeds tab and purchase a bag of crop seeds for $150.\n\n" +
                    "That will cover one plot.";
                nextButton.gameObject.SetActive(true);
                break;


            case 6:
                messageText.text =
                    "Next, buy your first plot of land. Once you’ve got it, head out to the field and get ready to plant.\n\n";
                nextButton.gameObject.SetActive(true);
                break;

            case 7:
                messageText.text =
                    "Tornado season is brutal. If one catches you off guard, your tractor will downgrade one level.\n\n";
                nextButton.gameObject.SetActive(true);
                break;

            case 8:
                messageText.text =
                    "A second tornado strike will strip you back to the base model, don’t let that happen.\n\n";
                nextButton.gameObject.SetActive(true);
                break;

            case 9:
                messageText.text =
                    "That’s all I’ve got for now. Follow these steps, dodge the twisters, and turn a profit. " +
                    "Make me proud out there.\n\n" +
                    "Sincerely,\nDad";
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

