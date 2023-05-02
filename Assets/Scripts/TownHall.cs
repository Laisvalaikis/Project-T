using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
    [HideInInspector] public GameObject SelectedUpgrade;
    public Data _data;

    public List<UpgradeButton> upgradeButtons;
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI upgradeDescriptionText;
    public TextMeshProUGUI upgradeCostText;
    public Button buyButton;
    public GameObject backgroundForText;
    private Image backgroundImage;

    private void Start()
    {
        backgroundForText.SetActive(true);
        backgroundImage = backgroundForText.GetComponent<Image>();
    }
    public void UpdateButtons()
    {
        foreach (UpgradeButton button in upgradeButtons)
        {
            button.UpdateUpgradeButton();
        }

        if (SelectedUpgrade != null)
        {
            UpgradeButton selectedUpgradeButton = SelectedUpgrade.GetComponent<UpgradeButton>();

            upgradeNameText.gameObject.SetActive(true);
            upgradeDescriptionText.gameObject.SetActive(true);
            upgradeCostText.gameObject.SetActive(true);
            //buyButton.gameObject.SetActive(true);
            backgroundForText.gameObject.SetActive(true);

            upgradeNameText.text = selectedUpgradeButton.upgradeName;
            upgradeDescriptionText.text = selectedUpgradeButton.upgradeDescription;
            upgradeCostText.text = "-" + selectedUpgradeButton.upgradeCost.ToString() + "g";
            buyButton.interactable = _data.townData.townGold >= selectedUpgradeButton.upgradeCost;
            Debug.Log("Reikia perdaryti");
        }
        else
        {
            upgradeNameText.gameObject.SetActive(false);
            upgradeDescriptionText.gameObject.SetActive(false);
            upgradeCostText.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
        }
    }

    public void BuyUpgrade()
    {
        SelectedUpgrade.GetComponent<UpgradeButton>().BuyUpgrade();
        UpdateButtons();
    }

    public void CloseTownHall()
    {
        SelectedUpgrade = null;
        UpdateButtons();
        gameObject.SetActive(false);
        backgroundForText.SetActive(false);
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0f);
    }
}
