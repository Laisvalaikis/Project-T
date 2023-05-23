using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;

public class CrowProp : Consumable
{
    public Animator animator;
    public override void PickUp(GameObject WhoStepped)
    {

        if (WhoStepped.gameObject.tag == "Player")
        {
            animator.SetTrigger("death");
        }
    }
}
