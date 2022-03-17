using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public float interGroundDistance = 3f;
    public GameObject groundPrefab;
    public GameObject firstGround;

    private Queue<GameObject> _groundQueue = new Queue<GameObject>();

    void Start()
    {
        _groundQueue.Enqueue(firstGround);
        ScoreSystem.SS.SuccessfulJumpEvent += InstantiateGround;
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
