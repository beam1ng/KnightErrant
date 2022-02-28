using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHandler : MonoBehaviour
{
    public static RandomHandler RH;
    public System.Random Random;
    
    void Start()
    {
        if (RH != null)
        {
            GameObject.Destroy(this);
        }
        else
        {
            RH = this;
        }

        Random = new System.Random((int) Time.time);
    }
}
