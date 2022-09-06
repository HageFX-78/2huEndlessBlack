using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class searchRange : MonoBehaviour
{
    public List<Transform> enemies;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            enemies.Add(collision.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            enemies.Remove(collision.transform);
        }
    }
    
    public List<Transform> getEnemyList()
    {
        return enemies;
    }
}
