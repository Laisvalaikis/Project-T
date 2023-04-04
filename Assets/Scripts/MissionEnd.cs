using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEnd : MonoBehaviour
{
    public SaveData _saveData;
    void Start()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        _saveData.LoadTownData();
        GameObject.Find("CanvasCamera").transform.Find("XPProgressTable").GetComponent<XPProgressManager>().UpdateButtons();
    }
}
