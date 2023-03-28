using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTableAbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.Find("ActionButtonFrame").GetComponent<Animator>().SetBool("hover", false);
    }
}
