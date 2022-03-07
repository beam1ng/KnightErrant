using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTouch : MonoBehaviour
{
    public delegate void EventHandler();

    public event EventHandler ScreenClickedEvent;
    
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(OnScreenClicked);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    protected virtual void OnScreenClicked()
    {
        ScreenClickedEvent?.Invoke();
    }
}
