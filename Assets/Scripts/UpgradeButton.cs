using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Sprite DefaultSprite;
    public Sprite UpgradedSprite;
    public int upgradeIndex;
    public int upgradeValue;
    public int upgradeCost;
    public string upgradeName;
    public string upgradeDescription;
    public Data _data;
    public GameUi gameUi;
    public void UpdateUpgradeButton()
    {
        string townHall = _data.townData.townHall;
        if (int.Parse(townHall[upgradeIndex].ToString()) + 1 < upgradeValue)//negalimi pirkti nes per auksti
        {
            GetComponent<Button>().interactable = false;
        }
        else if (int.Parse(townHall[upgradeIndex].ToString()) + 1 > upgradeValue) //nupirkti
        {
            GetComponent<Button>().interactable = false;
            transform.Find("Frame").GetComponent<Image>().sprite = UpgradedSprite;
            transform.Find("Text").GetComponent<Text>().color = Color.white;
        }
        else {
            GetComponent<Button>().interactable = true;
        }//galimas pirkti
    }
    public void BuyUpgrade()
    {
        string townHall = _data.townData.townHall;
        string newTownHall = "";
        for (int i = 0; i < townHall.Length;i++)
        {
            if (i != upgradeIndex)
            {
                newTownHall += townHall[i];
            }
            else
            {
                newTownHall += upgradeValue.ToString();
            }
        }
        _data.townData.townHall = newTownHall;
        GameObject.Find("GameProgress").GetComponent<GameProgress>().SpendGold(upgradeCost);

    }
    public void SelectUpgrade()
    {
        var TownHallTable = GameObject.Find("CanvasCamera").transform.Find("TownHallTable").GetComponent<TownHall>();
        if (TownHallTable.SelectedUpgrade == gameObject)
        {
            TownHallTable.SelectedUpgrade = null;
            GameObject.Find("CanvasCamera").transform.Find("TownHallTable").Find("BackgroundForText").gameObject.SetActive(false);
        }
        else
        {
            TownHallTable.SelectedUpgrade = gameObject;
            GameObject.Find("CanvasCamera").transform.Find("TownHallTable").Find("BackgroundForText").gameObject.SetActive(true);
        }
        TownHallTable.GetComponent<TownHall>().UpdateButtons();
    }

}
