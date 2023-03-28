using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public List<Character> characters { get; set; }
    public string teamName { get; set; }
    public GameObject teamManager { get; set; }

    public Team(string teamName, GameObject teamManager)
    {
        characters = new List<Character>();
        this.teamName = teamName;
        this.teamManager = teamManager;
    }


}
