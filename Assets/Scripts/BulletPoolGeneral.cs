using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolGeneral : MonoBehaviour
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
    public static BulletPoolGeneral poolInstance;

    private void Awake()
    {
        poolInstance = this;
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

    public Queue<GameObject> getBulletQueue(string tagname)
    {
        if (poolDic.ContainsKey(tagname))
        {
          
            return poolDic[tagname];
          
        }
        else
        {
            Debug.Log("Null queue returned from pool");
            return null;
        }
        
    }
}
