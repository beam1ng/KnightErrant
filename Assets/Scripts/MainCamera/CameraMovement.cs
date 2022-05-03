using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool followPlayerXPosition = true;
    
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _player.GetComponent<PlayerMovement>().SideHitEvent += OnPlayerSideHit;
    }

    private void Update()
    {
        if (_player == null) return;
        var playerPosition = _player.transform.position;
        transform.position =
            new Vector3(followPlayerXPosition? playerPosition.x:transform.position.x, Math.Max(transform.position.y, playerPosition.y), -10);
    }

    private void OnPlayerSideHit()
    {
        followPlayerXPosition = false;
    }
}
