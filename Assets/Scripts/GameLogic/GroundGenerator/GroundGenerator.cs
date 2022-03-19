using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public float interGroundDistance = 3f;
    public int activeGroundInstances = 3;
    public GameObject groundPrefab;
    public GameObject firstGround;

    private Queue<GameObject> _groundQueue = new Queue<GameObject>();

    private void Start()
    {
        _groundQueue.Enqueue(firstGround);
        ScoreSystem.SS.SuccessfulJumpEvent += InstantiateGround;
        for (var i = 0; i < activeGroundInstances; i++)
        {
            InstantiateGround(0);
        }
    }

    private void InstantiateGround(int e)
    {
        var oldGround = _groundQueue.Dequeue();
        var newGround = Instantiate(groundPrefab);
        _groundQueue.Enqueue(newGround);
        newGround.transform.position = oldGround.transform.position+Vector3.up*interGroundDistance;
        newGround.GetComponent<GroundMovement>().SetInitialMovement();
    }
}
