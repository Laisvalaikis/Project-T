using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;
    private Animator targetAnimator;

    private void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("Target Image is not assigned.");
            return;
        }

        targetAnimator = targetImage.GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAnimator.ResetTrigger("FadeOut");
        targetAnimator.SetTrigger("FadeIn");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetAnimator.ResetTrigger("FadeIn");
        targetAnimator.SetTrigger("FadeOut");
    }
}
