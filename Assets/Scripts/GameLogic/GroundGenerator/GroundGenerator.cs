using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GroundGenerator : MonoBehaviour
{
    public float interGroundDistance = 3f;
    public GameObject groundPrefab;
    public GameObject firstGround;

    private GameObject _oldGround;
    private GameObject _newGround;
    private Queue<GameObject> _groundQueue = new Queue<GameObject>();

    void Start()
    {
        ScoreSystem.SS.SuccessfulJumpEvent += InstantiateGround;
        _groundQueue.Enqueue(firstGround);
    }

    private void InstantiateGround(int e)
    {
        if (_oldGround != null)
        {
            Destroy(_oldGround);
        }
        _oldGround = _groundQueue.Dequeue();
        _newGround = Instantiate(groundPrefab);
        _groundQueue.Enqueue(_newGround);
        _newGround.transform.position = _oldGround.transform.position+Vector3.up*interGroundDistance;
        _newGround.GetComponent<GroundMovement>().SetInitialMovement();
    }
}
