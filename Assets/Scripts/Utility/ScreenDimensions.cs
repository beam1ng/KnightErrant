using UnityEngine;

public sealed class ScreenDimensions : MonoBehaviour
{
    public static ScreenDimensions SD;
    private float _screenWidth;
    private float _screenHeight;

    private void Awake()
    {
        if (SD != null)
        {
            Destroy(SD);
        }
        else
        {
            SD = this;
        }

        if (Camera.main == null) return;
        var orthographicSize = Camera.main.orthographicSize;
        _screenHeight = orthographicSize * 2;
        _screenWidth = orthographicSize * 2 / Screen.height * Screen.width;
    }

    public float GetScreenWidth()
    {
        return _screenWidth;
    }

    public float GetScreenHeight()
    {
        return _screenHeight;
    }
}
