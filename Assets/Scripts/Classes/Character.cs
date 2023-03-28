using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject OnBoard { get; set; }
    public GameObject Portrait { get; set; }
    public string Team { get; set; }
    public Character(GameObject onBoard, GameObject portrait, string team)
    {
        this.OnBoard = onBoard;
        this.Portrait = portrait;
        this.Team = team;
    }
}
