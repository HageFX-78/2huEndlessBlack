using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private string selfTag;
    private bool playerBulletType;
    // Start is called before the first frame update

    private void Start()
    {
        playerBulletType = (gameObject.CompareTag("EnemyBullet")) ? false : true;
    }
    void OnBecameInvisible()
    {
        resetAtt();
        gameObject.SetActive(false);
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerBulletType)
        {
            if (collision.CompareTag("Enemy"))
            {
                resetAtt();
                gameObject.SetActive(false);
                collision.GetComponent<EnemyHealthManager>().damageSelf();
            }
            else if(collision.CompareTag("Boss"))
            {
                resetAtt();
                gameObject.SetActive(false);
                collision.GetComponent<BossBehaviour>().damageSelf();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                resetAtt();
                gameObject.SetActive(false);
                collision.GetComponent<PlayerAction>().damageSelf();
            }
        }
        
    }

    private void resetAtt()
    {
        transform.position = new Vector3(0,0,0);
    }
    // Update is called once per frame

}
