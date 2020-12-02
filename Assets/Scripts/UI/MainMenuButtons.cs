using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuButtons : BasicButtons
{
    private Stack<Canvas> canvasStack = new Stack<Canvas>();
    public Canvas mainMenuCanvas;
    void Start()
    {
        canvasStack.Push(mainMenuCanvas);
    }
    public void GoForwardCanvas(Canvas can)
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<ButtonHover>().mouse_over = false;
        EventSystem.current.currentSelectedGameObject.GetComponent<ButtonHover>().alphaVal = 0.0f;

        canvasStack.Peek().gameObject.SetActive(false);
        canvasStack.Push(can);
        canvasStack.Peek().gameObject.SetActive(true);
    }

    public void BackButton()
    {
        canvasStack.Peek().gameObject.SetActive(false);
        canvasStack.Pop();
        canvasStack.Peek().gameObject.SetActive(true);
    }
}
