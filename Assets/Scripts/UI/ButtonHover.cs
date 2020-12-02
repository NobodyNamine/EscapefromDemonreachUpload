using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Button Hover Reference: https://answers.unity.com/questions/1199251/onmouseover-ui-button-c.html
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouse_over = false;
    public Image selectorGraphic;
    public float alphaVal = 0;

    void Update()
    {
        if (mouse_over)
        {
            alphaVal = Mathf.Lerp(alphaVal, 1.0f, .1f);
            selectorGraphic.GetComponent<Image>().color = new Color(1, 1, 1, alphaVal);
        }
        else
        {
            alphaVal = Mathf.Lerp(alphaVal, 0.0f, .1f);
            selectorGraphic.GetComponent<Image>().color = new Color(1, 1, 1, alphaVal);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //selectorGraphic.SetActive(false);
        mouse_over = false;
    }
}
