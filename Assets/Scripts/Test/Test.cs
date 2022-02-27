using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
   private void Start()
   {
      GetComponent<OffScreenable>().OffScreenEvent += OffScreenImplementation;
   }

   private void Update()
   {
      transform.position+=Vector3.left*Time.deltaTime;
   }

   void OffScreenImplementation()
   {
      Debug.Log("changedImplementation");
   }
}
