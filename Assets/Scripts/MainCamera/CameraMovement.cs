using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (_player == null) return;
        var playerPosition = _player.transform.position;
        transform.position =
            new Vector3(playerPosition.x, Math.Max(transform.position.y, playerPosition.y), -10);
    }
}
