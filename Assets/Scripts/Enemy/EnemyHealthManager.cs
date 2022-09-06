using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private int health;

    DropPool dp;
    Queue<GameObject> dlist;
    GameObject pup;
    Queue<GameObject> slist;
    GameObject scoreup;

    private string thisEnemy;
    private void Start()
    {
        dp = DropPool.droppool;
        thisEnemy = gameObject.name;
    }
    public void damageSelf()
    {
        int dropCount=1;
        health--;
        if (health <= 0)
        {
            FindObjectOfType<AudioManager>().plyAudio("enemyDeath");
            if (thisEnemy == "Enemy1")
            {
                dropCount = 3;
            }
            else if(thisEnemy == "Enemy2")
            {
                dropCount = 6;
            }
            else
            {
                dropCount = 1;
            }
            for(int x=1; x<=dropCount;x++)
            {
                gameObject.SetActive(false);
                dlist = dp.getDropQueue("powerUp");
                pup = dlist.Dequeue();
                pup.transform.position = transform.position + new Vector3(Random.Range(-3, 3)*x, Random.Range(-2, 2)*x, 0);
                pup.SetActive(true);
                Rigidbody2D rb = pup.GetComponent<Rigidbody2D>();
                rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
                dlist.Enqueue(pup);

                slist = dp.getDropQueue("scoreUp");
                scoreup = slist.Dequeue();
                scoreup.transform.position = transform.position + new Vector3(Random.Range(-3, 3)*x, Random.Range(-2, 2)*x, 0);
                scoreup.SetActive(true);
                Rigidbody2D rb2 = scoreup.GetComponent<Rigidbody2D>();
                rb2.AddForce(transform.up * 10, ForceMode2D.Impulse);
                slist.Enqueue(scoreup);
            }
            
        }
    }
}
