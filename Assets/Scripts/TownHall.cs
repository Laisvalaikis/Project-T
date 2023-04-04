using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownHall : MonoBehaviour
{
   [HideInInspector] public GameObject SelectedUpgrade;
   public Data _data;
    void Start()
    {
        //UpdateButtons();
    }
        public void UpdateButtons()
    {
        GameObject buttons = transform.Find("UpgradeButtons").gameObject;
        for(int i = 0; i < buttons.transform.childCount; i++)
        {
            buttons.transform.GetChild(i).gameObject.GetComponent<UpgradeButton>().UpdateUpgradeButton();
        }
        if(SelectedUpgrade != null)
        {
            transform.Find("UpgradeName").gameObject.SetActive(true);
            transform.Find("UpgradeDescription").gameObject.SetActive(true);
            transform.Find("UpgradeCost").gameObject.SetActive(true);
            transform.Find("BuyButton").gameObject.SetActive(true);
            //
            transform.Find("UpgradeName").GetComponent<TextMeshProUGUI>().text = SelectedUpgrade.GetComponent<UpgradeButton>().upgradeName;
            transform.Find("UpgradeDescription").GetComponent<TextMeshProUGUI>().text = SelectedUpgrade.GetComponent<UpgradeButton>().upgradeDescription;
            transform.Find("UpgradeCost").GetComponent<Text>().text = "-" + SelectedUpgrade.GetComponent<UpgradeButton>().upgradeCost.ToString() + "g";
            transform.Find("BuyButton").GetComponent<Button>().interactable = _data.townData.townGold >= SelectedUpgrade.GetComponent<UpgradeButton>().upgradeCost;

        }
        else
        {
            transform.Find("UpgradeName").gameObject.SetActive(false);
            transform.Find("UpgradeDescription").gameObject.SetActive(false);
            transform.Find("UpgradeCost").gameObject.SetActive(false);
            transform.Find("BuyButton").gameObject.SetActive(false);
        }
    }
    public void BuyUpgrade()
    {
        SelectedUpgrade.GetComponent<UpgradeButton>().BuyUpgrade();
        SelectedUpgrade = null;
        UpdateButtons();
    }
    public void CloseTownHall()
    {
        SelectedUpgrade = null;
        UpdateButtons();
        gameObject.SetActive(false);
        GameObject.Find("CanvasCamera").transform.Find("TownHallTable").Find("BackgroundForText").gameObject.SetActive(false);
    }
}
