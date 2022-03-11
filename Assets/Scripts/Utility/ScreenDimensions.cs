using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ScreenDimensions : MonoBehaviour
{
    public static ScreenDimensions SD;
    
    private float _screenWidth;
    private float _screenHeight;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (SD != null)
        {
            GameObject.Destroy(SD);
        }
        else
        {
            SD = this;
        }
        // DontDestroyOnLoad(this);

        if (Camera.main != null)
        {
            var orthographicSize = Camera.main.orthographicSize;
            _screenHeight = orthographicSize * 2;
            _screenWidth = orthographicSize * 2 / Screen.height * Screen.width;
        }
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
