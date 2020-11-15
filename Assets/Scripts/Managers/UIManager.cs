using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static Stack<Canvas> canvasTracker;

    void Start()
    {
        canvasTracker = new Stack<Canvas>();
    }
    public static void ForwardCanvas(Canvas canvasToAdd)
    {
        if(canvasTracker.Count > 0)
            canvasTracker.Peek().gameObject.SetActive(false);
        canvasTracker.Push(canvasToAdd);
        canvasTracker.Peek().gameObject.SetActive(true);
    }

    public static void BackButton()
    {
        canvasTracker.Peek().gameObject.SetActive(false);
        canvasTracker.Pop();
        canvasTracker.Peek().gameObject.SetActive(true);
    }

}
