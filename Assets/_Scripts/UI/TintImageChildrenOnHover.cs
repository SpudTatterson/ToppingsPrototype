using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TintImageChildrenOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    List<Image> images = new List<Image>();
    Image original;
    Color highlightedColor;
    Color normalColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var img in images)
        {
            img.color = highlightedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var img in images)
        {
            img.color = normalColor;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        original = GetComponent<Image>();
        Button button = GetComponent<Button>();
        highlightedColor = button.colors.highlightedColor;
        normalColor = button.colors.normalColor;

        GetComponentsInChildren<Image>(images);
        images.Remove(original);
    }
}
