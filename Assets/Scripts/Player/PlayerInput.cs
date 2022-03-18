using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        HandlePlayerInput();
    }
    
    private void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.GetComponent<PlayerMovement>().Jump();
        }
        for (var index = 0; index < Input.touchCount; index++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[index].fingerId)) continue;
            if (Input.touches[index].phase != TouchPhase.Began) continue;
            _player.GetComponent<PlayerMovement>().Jump();
            break;

        }
    }
}
