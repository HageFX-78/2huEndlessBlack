using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0Behaviour : MonoBehaviour
{

    public float moveSpeed, moveOffset;
    private char pattern;


    void Start()
    {

        moveOffset = Random.Range(-10, 10);
    }
    void OnEnable()
    {
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

    public void movePattern(char p)
    {
        pattern = p;
    }

}
