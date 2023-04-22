using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutsideMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] menuPanels;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (GameObject menuPanel in menuPanels)
        {
            if (menuPanel.activeInHierarchy &&
                !RectTransformUtility.RectangleContainsScreenPoint(
                    menuPanel.GetComponent<RectTransform>(),
                    eventData.position,
                    eventData.pressEventCamera))
            {
                menuPanel.SetActive(false);
            }
        }
    }
}
