using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUi : MonoBehaviour
{
    public TextMeshProUGUI townGold;
    public List<TextMeshProUGUI> townGoldChanges;
    public TextMeshProUGUI dayNumber;
    public TextMeshProUGUI difficulty;
    public Data _data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateTownCost()
    {
        townGold.text = _data.townData.townGold.ToString() + "g";
    }
    
    public void UpdateDayNumber()
    {
        dayNumber.text = "Day " + _data.townData.day.ToString();
    }
    
    public void UpdateDifficultyButton()
    {
        if (_data.townData.difficultyLevel == 0)
        {
            difficulty.text = "EASY";
        }
        else
        {
            difficulty.text = "HARD";
        }
    }
    
    public void EnableGoldChange(string text)
    {
      
            for (int i = 0; i < townGoldChanges.Count; i++)
            {
                if (!townGoldChanges[i].gameObject.activeSelf)
                {
                    townGoldChanges[i].text = text;
                    townGoldChanges[i].gameObject.SetActive(true);
                    break;
                }
            }
      
    }
    
}
