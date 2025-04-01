using TMPro;
using UnityEngine;

public class LandPlotSelector : MonoBehaviour
{
    public LandPlot plotToSelect;
    public TextMeshProUGUI buttonText;

    void Start()
    {
        if (buttonText != null && plotToSelect != null)
        {
            buttonText.text = $"Buy Plot (${plotToSelect.price})";
        }
    }

    public void SelectThisPlot()
    {
        StoreManager.Instance.SetSelectedPlot(plotToSelect);
    }
}
