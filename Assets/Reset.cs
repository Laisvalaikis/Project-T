using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        SaveSystem.SaveCurrentSlot(0);
        SaveSystem.SaveTownData(TownData.NewGameData("Blue", 0, "Naujas"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
