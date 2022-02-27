using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenable : MonoBehaviour
{
    private Vector2 _objectSize;
    private bool _appearedInScreenOnce = false;
    private bool _offScreenEventInvoked = false;
    
    public delegate void EventHandler();

    public event EventHandler OffScreenEvent;
    
    
    void Start()
    {
        _objectSize = Vector2.Scale(transform.localScale, GetComponent<SpriteRenderer>().sprite.bounds.size);
    }
    void Update()
    {
        if (_offScreenEventInvoked) return;
        if (transform.position.x + _objectSize.x / 2 < -1 * ScreenDimensions.SD.GetScreenWidth() / 2 ||
            transform.position.x - _objectSize.x / 2 > ScreenDimensions.SD.GetScreenWidth() / 2 ||
            transform.position.y - _objectSize.y / 2 < -1 * ScreenDimensions.SD.GetScreenHeight() / 2 ||
            transform.position.y + _objectSize.y / 2 > ScreenDimensions.SD.GetScreenHeight() / 2)
        {
            if (_appearedInScreenOnce)
            {
                OffScreen();
            }
        }
        else
        {
            if (!_appearedInScreenOnce)
            {
                _appearedInScreenOnce = true;
            }
        }
    }

    protected virtual void OffScreen()
    {
        OffScreenEvent?.Invoke();
        _offScreenEventInvoked = true;
    }

}
