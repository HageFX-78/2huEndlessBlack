using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    BulletPoolGeneral mainpool;
    Queue<GameObject> bosspool;
    Queue<GameObject> bosspool2;
    public Transform player;
    public GameObject playerRef;

    [Header("Phase Settings")]
    //phase
    private char currentPhase;
    private bool switchAvailable;
    private int originalHP;

    [Header("Boss Stats + Phase 1")]
    //boss stats
    public int health;
    public float bulletSpeed, bulletSpeed2, fireAngle, fireRate, fireRate2;
    public int bulletSplit;
    private bool dirSwitch;

    [Header("Phase 2")]
    //phase 2
    [SerializeField] float startAngle;
    [SerializeField] float incrementAngle, incrementP2b, fireRateP2, fireRateP2b, bulletSpeedP2, bulletSpeedP2b;
    [SerializeField] int bundleSize, delayBCount;


    IEnumerator p1, p1b, p2 ,p22, p2b;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        mainpool = BulletPoolGeneral.poolInstance;

        currentPhase = '1';
        switchAvailable = true;
        originalHP = health;

        dirSwitch = true;

        p1 = Phase1();
        p1b = Phase1b();
        p2 = Phase2(startAngle);
        p22 = Phase2(startAngle-270);
        p2b = Phase2b(bundleSize);
        Invoke("delayedStart", 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(health<=0)
        {
            if(switchAvailable)
            {
                switchAvailable = false;
                health = originalHP;
                switchPhase();
                
            }
        }

            transform.position = new Vector3(0, 16, 0);

    }

    void switchPhase()
    {
        if (currentPhase == '1')
        {
            currentPhase = '2';
            StopCoroutine(p1);
            StopCoroutine(p1b);
            StartCoroutine(p2);
            StartCoroutine(p22);
            StartCoroutine(p2b);
            switchAvailable = true;
        }
        else if (currentPhase == '2')
        {
            ShakeEffect.caminstance.shakeScreen(2.0f);
            
            FindObjectOfType<AudioManager>().plyAudio("confirm");
            FindObjectOfType<PlayerAction>().winCondition();
        }
    }
    void delayedStart()
    {
        bosspool = mainpool.getBulletQueue("bossBullet");
        bosspool2 = mainpool.getBulletQueue("enemyBullet");
        StartCoroutine(p1);
        StartCoroutine(p1b);
    }

    void bundleFire()
    {
        
    }
    public void damageSelf()
    {
        health--;
    }


    //coroutines
    IEnumerator Phase1()
    {
        while(true)
        {
            fireAngle = 0;
            
            for (int x = 0; x < bulletSplit; x++)
            {
                
                GameObject thisBullet;
                thisBullet = bosspool.Dequeue();
                thisBullet.transform.position = transform.position;
                thisBullet.SetActive(true);
                Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();
                
                if (dirSwitch)
                {
                    thisBullet.GetComponent<BossBulletBehaviour>().setDir(-1);
                }
                else
                {
                    thisBullet.GetComponent<BossBulletBehaviour>().setDir(1);
                }
                float xdir = Mathf.Cos(fireAngle * Mathf.PI / 180);
                float ydir = Mathf.Sin(fireAngle * Mathf.PI / 180);
                rb.AddForce(new Vector2(xdir, ydir) * bulletSpeed, ForceMode2D.Impulse);
                bosspool.Enqueue(thisBullet);

                fireAngle += 360 / bulletSplit;
            }
            dirSwitch = !dirSwitch ? true : false;
            yield return new WaitForSeconds(fireRate);
        }
        
    }
    IEnumerator Phase1b()
    {
        while (true)
        {         
            GameObject thisBullet;
            thisBullet = bosspool2.Dequeue();
            thisBullet.transform.position = transform.position;
            thisBullet.SetActive(true);
            Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();

            rb.AddForce((player.position-transform.position).normalized * bulletSpeed2, ForceMode2D.Impulse);
            bosspool2.Enqueue(thisBullet);

            yield return new WaitForSeconds(fireRate2);
        }

    }

    IEnumerator Phase2(float x)
    {
        bool fireDirection = true;//true to right in a literal sense, false to left
        float localAngle = x;
        while (true)
        {
            GameObject thisBullet;

            thisBullet = bosspool2.Dequeue();
            thisBullet.transform.position = transform.position;

            thisBullet.SetActive(true);
            Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();

            float xdir = Mathf.Cos(localAngle * Mathf.PI / 180);
            float ydir = Mathf.Sin(localAngle * Mathf.PI / 180);
            rb.AddForce(new Vector2(xdir, ydir) * bulletSpeedP2, ForceMode2D.Impulse);
            bosspool2.Enqueue(thisBullet);

            if (localAngle >= 45)
            {
                fireDirection = false;

            }
            else if (localAngle <= -225)
            {
                fireDirection = true;
            }
            localAngle += fireDirection ? incrementAngle : -incrementAngle;
            yield return new WaitForSeconds(fireRateP2);
        }
    }
    IEnumerator Phase2b(int bcount)
    {
        int localbcount = 1, ogdelayBCount = bcount*delayBCount;
        bool noDelay = true;
        float randOffset;
        while(true)
        {
            if(noDelay)
            {
                
                randOffset = Random.Range(-2f, 2f);
                for (int x = 1; x <= localbcount; x++)
                {
                    GameObject thisBullet;
                    thisBullet = bosspool2.Dequeue();
                    thisBullet.transform.position = transform.position;
                    if (x % 2 == 0)
                    {
                        thisBullet.transform.position += new Vector3(randOffset * x, 0, 0);
                    }
                    else if(x%2!=0 && x!=1)
                    {
                        thisBullet.transform.position += new Vector3(randOffset * x, 0, 0);
                    }

                    thisBullet.SetActive(true);
                    Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();
                    Vector2 fireDirection = (player.position - transform.position).normalized;
                    rb.AddForce(fireDirection * bulletSpeedP2b, ForceMode2D.Impulse);
                    bosspool2.Enqueue(thisBullet);
                }
                if (localbcount >= bcount)
                {
                    localbcount = 1;
                    noDelay = false;
                }
                else
                {
                    localbcount++;
                }
            }
            else
            {
                delayBCount--;
                if (delayBCount<=0)
                {
                    noDelay = true;
                    delayBCount = ogdelayBCount;
                }
            }
            
            yield return new WaitForSeconds(fireRateP2b);
        }
    }
}
