using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Behaviour : MonoBehaviour
{
    BulletPoolGeneral mpool;
    Queue<GameObject> bpool;

    public float moveSpeed, bulletSpeed, fireRate, moveOffset;
    private char pattern;
    [SerializeField] private float startAngle, incrementAngle;

    void Start()
    {
        mpool = BulletPoolGeneral.poolInstance;
    }

    // Update is called once per frame
    void Update()
    {

            transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
        
        
    }
    void OnEnable()
    {
        Invoke("shootfirst", 0.5f);
    }
    void OnBecameInvisible()
    {
        if (transform.localPosition.y < -24)
        {
            gameObject.SetActive(false);
        }
    }
    void shootfirst()
    {
        bpool = mpool.getBulletQueue("enemyBullet");
        StartCoroutine(shoot());
    }
    IEnumerator shoot()//Shoot action
    {
        bool fireDirection = true;//true to right in a literal sense, false to left
        while (true)
        {
            GameObject thisBullet;

            thisBullet = bpool.Dequeue();
            thisBullet.transform.position = transform.position;

            thisBullet.SetActive(true);
            Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();

            float xdir = Mathf.Cos(startAngle * Mathf.PI / 180);
            float ydir = Mathf.Sin(startAngle * Mathf.PI / 180);
            rb.AddForce(new Vector2(xdir, ydir) * bulletSpeed, ForceMode2D.Impulse);
            bpool.Enqueue(thisBullet);

            if(startAngle>=0)
            {
                fireDirection = false;

            }
            else if(startAngle <= -180)
            {
                fireDirection = true;
            }
            startAngle += fireDirection ?incrementAngle:-incrementAngle;
            yield return new WaitForSeconds(fireRate);
        }
    }
}
