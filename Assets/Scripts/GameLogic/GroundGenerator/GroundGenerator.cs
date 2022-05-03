using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public static GroundGenerator GG;
    [SerializeField] private float interGroundDistance = 3f;
    [SerializeField] private int activeGroundInstances = 3;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private GameObject firstGround;

    private readonly Queue<GameObject> _groundQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (GG != null)
        {
            Destroy(this);
        }
        else
        {
            GG = this;
        }
    }
    
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

    public float GetInterGroundDistance()
    {
        return interGroundDistance;
    }
}
