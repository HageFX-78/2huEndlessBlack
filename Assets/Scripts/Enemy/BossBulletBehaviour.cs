using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletBehaviour : MonoBehaviour
{
    private string selfTag;
    private bool playerBulletType;
    public GameObject target;
    public int directionVal;
    // Start is called before the first frame update

    private void Start()
    {
        playerBulletType = (gameObject.CompareTag("EnemyBullet")) ? false : true;
    }
    private void Update()
    {
        transform.RotateAround(target.transform.position, new Vector3(0,0, directionVal), 50 * Time.deltaTime);
    }
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
                
            gameObject.SetActive(false);
            collision.GetComponent<PlayerAction>().damageSelf();
        }

    }
    public void setDir(int x)
    {
        directionVal = x;
    }
}
