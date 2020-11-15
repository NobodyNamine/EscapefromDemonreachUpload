using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Button Hover Reference: https://answers.unity.com/questions/1199251/onmouseover-ui-button-c.html
public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool mouse_over = false;
    public GameObject selectorGraphic;

    void Update()
    {
        if (mouse_over)
        {
            selectorGraphic.SetActive(true);

            selectorGraphic.transform.position = 
                new Vector3(selectorGraphic.transform.position.x,
                this.transform.position.y,
                selectorGraphic.transform.position.z);
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectorGraphic.SetActive(false);
        mouse_over = false;
        Debug.Log("Mouse exit");
    }
}
