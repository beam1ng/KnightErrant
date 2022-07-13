using Unity.VisualScripting;
using UnityEngine;

public class OffScreenable : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler OffScreenEvent;
    
    private Vector2 _objectSize;
    private bool _appearedInScreenOnce;
    private bool _offScreenEventInvoked;
    private GameObject _mainCamera;
    private const float OffScreenBuffer = 0f;

    [SerializeField] private bool invokeEventOnXOffScreen = true;

    void Start()
    {
        _mainCamera = GameObject.FindWithTag("MainCamera");
        _objectSize = Vector2.Scale(transform.localScale, GetComponent<SpriteRenderer>().sprite.bounds.size);
    }
    void Update()
    {
        if (_offScreenEventInvoked) return;
        if (
            invokeEventOnXOffScreen &&
                (transform.position.x + _objectSize.x / 2 < -1 * (ScreenDimensions.SD.GetScreenWidth() / 2) +_mainCamera.transform.position.x-OffScreenBuffer||
                transform.position.x - _objectSize.x / 2 > ScreenDimensions.SD.GetScreenWidth() / 2 +_mainCamera.transform.position.x+OffScreenBuffer)
            ||
            transform.position.y + _objectSize.y / 2 < -1 * (ScreenDimensions.SD.GetScreenHeight() / 2) +_mainCamera.transform.position.y-OffScreenBuffer||
            transform.position.y - _objectSize.y / 2 > ScreenDimensions.SD.GetScreenHeight() / 2 +_mainCamera.transform.position.y+OffScreenBuffer)
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
