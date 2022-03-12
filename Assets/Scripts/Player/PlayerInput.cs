using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
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
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[index].fingerId)) return;
            _player.GetComponent<PlayerMovement>().Jump();
        }
    }
}
