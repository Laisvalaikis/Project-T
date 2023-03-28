//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;

//public class DropdownController : MonoBehaviour
//{
//    void Awake()
//    {
//        GetComponent<Dropdown>().options.Clear();
//        GetComponent<Dropdown>().options.Add(new Dropdown.OptionData("Select Mission"));
//        foreach (GameObject prefab in GameObject.Find("GameProgress").GetComponent<MapSetup>().MapPrefabs)
//        {
//            GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(prefab.name));
//        }
//        GetComponent<Dropdown>().onValueChanged.AddListener(new UnityAction<int>(SelectMission));
//    }

//    private void SelectMission(int optionIndex)
//    {
//        GameObject.Find("GameProgress").GetComponent<GameProgress>().ChangeSelectedMission(GetComponent<Dropdown>().options[optionIndex].text);
//    }

//}