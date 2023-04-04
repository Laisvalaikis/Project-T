using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotCard : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private Button addButton;
    [SerializeField] private GameObject slotMenu;
    [SerializeField] private TextMeshProUGUI slotTitle;
    // Start is called before the first frame update
    void Start()
    {
        addButton.gameObject.SetActive(!SaveSystem.DoesSaveFileExist(slotIndex));
        slotMenu.SetActive(SaveSystem.DoesSaveFileExist(slotIndex));
        if(SaveSystem.DoesSaveFileExist(slotIndex))
        {
            slotTitle.text = SaveSystem.LoadTownData(slotIndex).slotName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
