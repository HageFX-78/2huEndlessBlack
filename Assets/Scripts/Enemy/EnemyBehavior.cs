using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    BulletPoolGeneral mpool;
    Queue<GameObject> bpool;
    public Transform player;

    public float moveSpeed, bulletSpeed, fireRate, shotDelay, moveOffset;
    private char pattern;

    IEnumerator sht;

    void Start()
    {
        sht = shoot();
        player = GameObject.FindWithTag("Player").transform;
        mpool = BulletPoolGeneral.poolInstance;
        moveOffset = Random.Range(-10, 10);
    }
    void OnEnable()
    {
        Invoke("shootfirst", 0.3f);
        moveOffset = Random.Range(-10, 10);
    }
    // Update is called once per frame
    void OnBecameInvisible()
    {
        if (transform.position.y < -24)
        {
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {

        transform.position += new Vector3(0, (-moveSpeed - moveOffset) * Time.deltaTime, 0);

    }
    void shootfirst()
    {
        bpool = mpool.getBulletQueue("enemyBullet");
        StartCoroutine(sht);
    }
    IEnumerator shoot()//Shoot action
    {
        while (true)
        {     
            GameObject thisBullet;
            
            thisBullet = bpool.Dequeue();
            thisBullet.transform.position = transform.position+ new Vector3(0, -2, 0);

            thisBullet.SetActive(true);
            Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();

            rb.AddForce((player.position-transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
            bpool.Enqueue(thisBullet);
            yield return new WaitForSeconds(fireRate);
        }
    }
}
