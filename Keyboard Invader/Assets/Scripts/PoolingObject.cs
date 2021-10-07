using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    public ObjectPooler.PoolingType poolingType;

    private void OnDisable()
    {

        ObjectPooler.ReturnToPool(gameObject,poolingType);    // 한 객체에 한번만 
         
    }
}
