using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject pfToPool;
        public int poolSize;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDic;
    public static DropPool droppool;

    private void Awake()
    {
        droppool = this;
    }
    void Start()
    {
        //normal bullet pool instantiate
        poolDic = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int x = 0; x < pool.poolSize; x++)
            {
                GameObject bulletObj = Instantiate(pool.pfToPool);
                bulletObj.SetActive(false);
                bulletObj.transform.parent = gameObject.transform;
                objPool.Enqueue(bulletObj);
            }

            poolDic.Add(pool.tag, objPool);
        }
    }

    public Queue<GameObject> getDropQueue(string tagname)
    {
        if (poolDic.ContainsKey(tagname))
        {

            return poolDic[tagname];

        }
        else
        {
            return null;
        }

    }
}
