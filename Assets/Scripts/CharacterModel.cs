using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public void Die()
    {
        transform.parent.GetComponent<PlayerInformation>().Die();
    }
    public void RaiseRockAnimationEnd()
    {
        transform.parent.GetComponent<RaiseRock>().RaiseRockAnimationEnd();
    }
    public void WallSmashAnimationEnd()
    {
        transform.parent.GetComponent<WallSmash>().WallSmashAnimationEnd();
    }
    public void FreeCagedCharacter()
    {
        transform.parent.GetComponent<Cage>().FreeCagedCharacter();
    }
    public void SwordPushAnimationStart()
    {
        transform.parent.GetComponent<SwordPush>().SwordPushAnimationStart();
    }

    public void SwordPushAnimationEnd()
    {
        transform.parent.GetComponent<SwordPush>().SwordPushAnimationEnd();
    }
}
