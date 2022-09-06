using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletPoolP : MonoBehaviour
{
    public static BulletPoolP SharedInstance;
    public List<GameObject> pooledObj;
    public GameObject objectToPool;
    public int MaxPoolAmt;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObj = new List<GameObject>();
        GameObject tmp;
        for(int x=0;x<MaxPoolAmt; x++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObj.Add(tmp);
        }
    }
}
