using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject toolTip;
    private bool hovering;
    private IEnumerator coroutine;
    public float delay = 0.5f;
    public float fadeSpeed = 2f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //print("pointer has entered");
        hovering = true;
        if(coroutine != null)
            StopCoroutine(coroutine);
        toolTip.GetComponent<CanvasGroup>().alpha = 1;
        StartCoroutine(ExecuteAfterTime(delay, () =>
        {
            if(hovering)
            {
                toolTip.SetActive(true);
            }
        }));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //print("pointer has exited");
        hovering = false;
        coroutine = FadeOut(fadeSpeed);
        StartCoroutine(coroutine);
    }

    void Awake()
    {
        toolTip = transform.Find("Tooltip").gameObject;
        hovering = false;
    }

    IEnumerator ExecuteAfterTime(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    IEnumerator FadeOut(float speed)
    {
        var group = toolTip.GetComponent<CanvasGroup>();
        while(group.alpha > 0)
        {
            group.alpha = Mathf.Clamp(group.alpha - Time.deltaTime * speed, 0, 1);
            yield return null;
        }
        toolTip.SetActive(false);
        group.alpha = 1;
    }
}
