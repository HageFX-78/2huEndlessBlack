using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [System.Serializable]
    public class EPool
    {
        public string tag;
        public GameObject pfToPool;
        public int poolSize;
    }


    public List<EPool> epools;
    public Dictionary<string, Queue<GameObject>> enemyDic;
    public static EnemyPool epool;
    public GameObject bossprefab;
    public bool bossSpawned;

    //Timer and spawning
    IEnumerator sp0, sp1, sp2;
    private float timer;
    private bool allowSpawn0, allowSpawn1, allowSpawn2;

    [Header("Enemy Type 0")]
    public int maxSpawn0;
    public int minSpawn0;
    public float spawnRate0;

    [Header("Enemy Type 1")]
    public int maxSpawn1;
    public int minSpawn1;
    public float spawnRate1;

    [Header("Enemy Type 2")]
    public int maxSpawn2;
    public int minSpawn2;
    public float spawnRate2;
    private void Awake()
    {
        epool = this;
    }
    void Start()
    {
        
        //normal bullet pool instantiate
        enemyDic = new Dictionary<string, Queue<GameObject>>();

        foreach (EPool pool in epools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int x = 0; x < pool.poolSize; x++)
            {
                GameObject enemyObj = Instantiate(pool.pfToPool);
                enemyObj.SetActive(false);
                enemyObj.transform.parent = gameObject.transform;
                objPool.Enqueue(enemyObj);
            }

            enemyDic.Add(pool.tag, objPool);
        }

        allowSpawn0 = true; allowSpawn1 = true; allowSpawn2 = true;bossSpawned = false;
        timer = 0;
        sp0 = spawnType0();
        sp1 = spawnType1();
        sp2 = spawnType2();
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3 && allowSpawn0)
        {
            allowSpawn0 = false;
            StartCoroutine(sp0);
        }
        if (timer > 25 && allowSpawn1)
        {
            allowSpawn1 = false;
            StartCoroutine(sp1);
        }
        if (timer > 40 && allowSpawn2)
        {
            allowSpawn2 = false;
            StartCoroutine(sp2);
        }
        if (timer > 50)
        {
            StopCoroutine(sp0);
            StopCoroutine(sp1);
            StopCoroutine(sp2);
        }
        if (timer > 40 && !bossSpawned)
        {
            bossSpawned = true;
            spawnBoss();
        }
    }
    public Queue<GameObject> getEnemyQueue(string tagname)
    {
        if (enemyDic.ContainsKey(tagname))
        {
            return enemyDic[tagname];

        }
        else
        {
            Debug.Log("Null queue returned from pool");
            return null;
        }

    }

    void spawnBoss()
    {
        Instantiate(bossprefab, transform.position, Quaternion.identity);
    }
    IEnumerator spawnType0()
    {
        Queue<GameObject> elist;
        GameObject currentEnemy;
        while (true)
        {
            elist = getEnemyQueue("enemy0");
            int currentLimit = Random.Range(minSpawn0, maxSpawn0);
            for (int x = 0; x < currentLimit; x++)
            {
                currentEnemy = elist.Dequeue();
                currentEnemy.SetActive(true);
                currentEnemy.transform.position = transform.position;
                currentEnemy.transform.position += new Vector3(Random.Range(-30, 30), Random.Range(-5, 5), 0);

                elist.Enqueue(currentEnemy);
            }

            yield return new WaitForSeconds(spawnRate0);
        }


    }
    IEnumerator spawnType1()
    {
        Queue<GameObject> elist;
        GameObject currentEnemy;
        while (true)
        {
            elist = getEnemyQueue("enemy1");
            int currentLimit = Random.Range(minSpawn1, maxSpawn1);
            for(int x=0; x< currentLimit;x++)
            {
                currentEnemy = elist.Dequeue();
                currentEnemy.SetActive(true);
                currentEnemy.transform.position = transform.position;
                currentEnemy.transform.position += new Vector3(Random.Range(-30, 30) , Random.Range(-5, 5), 0);

                elist.Enqueue(currentEnemy);
            }

            yield return new WaitForSeconds(spawnRate1);
        }

        
    }
    IEnumerator spawnType2()
    {
        Queue<GameObject> elist;
        GameObject currentEnemy;
        while (true)
        {
            elist = getEnemyQueue("enemy2");
            int currentLimit = Random.Range(minSpawn2, maxSpawn2);
            for (int x = 0; x < currentLimit; x++)
            {
                currentEnemy = elist.Dequeue();
                currentEnemy.SetActive(true);
                currentEnemy.transform.position = transform.position;
                currentEnemy.transform.position += new Vector3(Random.Range(-30, 30), Random.Range(-5, 5), 0);

                elist.Enqueue(currentEnemy);
            }

            yield return new WaitForSeconds(spawnRate2);
        }


    }
}


