using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Button Hover Reference: https://answers.unity.com/questions/1199251/onmouseover-ui-button-c.html
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public Image selectorGraphic;
    private float alphaVal = 0;

    void Update()
    {
        if (mouse_over)
        {/*
            selectorGraphic.SetActive(true);

            selectorGraphic.transform.position = 
                new Vector3(selectorGraphic.transform.position.x,
                this.transform.position.y,
                selectorGraphic.transform.position.z);*/
            alphaVal = Mathf.Lerp(alphaVal, 1.0f, .05f);
            selectorGraphic.GetComponent<Image>().color = new Color(1, 1, 1, alphaVal);
        }
        else
        {
            alphaVal = Mathf.Lerp(alphaVal, 0.0f, .05f);
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
