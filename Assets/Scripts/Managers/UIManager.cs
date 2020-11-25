using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Stack<Canvas> canvasTracker = new Stack<Canvas>();
    public static UIManager instance = null;

    void Start()
    {
        if (instance != null)
        {
            instance.canvasTracker.Clear();
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }

    public void ForwardCanvas(Canvas canvasToAdd)
    {
        if(canvasTracker.Count > 0)
            canvasTracker.Peek().gameObject.SetActive(false);
        canvasTracker.Push(canvasToAdd);
        canvasTracker.Peek().gameObject.SetActive(true);
    }

    public void BackButton()
    {
        canvasTracker.Peek().gameObject.SetActive(false);
        canvasTracker.Pop();
        if(canvasTracker.Count > 0)
            canvasTracker.Peek().gameObject.SetActive(true);
    }
}
