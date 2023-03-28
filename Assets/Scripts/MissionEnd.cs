using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEnd : MonoBehaviour
{
    void Start()
    {
        var gameProgress = GameObject.Find("GameProgress").GetComponent<GameProgress>();
        gameProgress.LoadTownData();
        GameObject.Find("CanvasCamera").transform.Find("XPProgressTable").GetComponent<XPProgressManager>().UpdateButtons();
    }
}
