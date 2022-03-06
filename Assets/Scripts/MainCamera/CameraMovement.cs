using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    void Update()
    {
        if (player == null) return;
        var playerPosition = player.transform.position;
        transform.position =
            new Vector3(playerPosition.x, Math.Max(transform.position.y, playerPosition.y), -10);
    }
    // Update is called once per frame
}
