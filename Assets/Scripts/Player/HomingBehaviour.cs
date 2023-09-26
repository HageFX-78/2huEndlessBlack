using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBehaviour : MonoBehaviour
{
    public Transform target;
    public float bspeed = 5f, rotateSpeed = 200f, delayTimer = 1;
    private float localTimer;
    Rigidbody2D rb;

    IEnumerator rt;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Boss").transform;
        if(target == null)
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
        
        rb = GetComponent<Rigidbody2D>();
        //rt = rotate();    
    }

    void FixedUpdate()
    {
        
        if (gameObject.activeSelf && localTimer == 0)
        {

            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmt = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = rotateAmt * -rotateSpeed;

            rb.velocity = transform.up * bspeed;

            localTimer = delayTimer;
        }
        else { localTimer--; }
        
    }
    /*
    private void OnEnable()
    {
        StartCoroutine(rt);
    }
    private void OnDisable()
    {
        StopCoroutine(rt);
    }
    // Update is called once per frame
    IEnumerator rotate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();

        float rotateAmt = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = rotateAmt * -rotateSpeed;

        rb.velocity = transform.up * bspeed;

        yield return new WaitForSeconds(0.2f);
    }//*/
}
