using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    private bool updown;
    public GameObject treeup, treeupper;
    public SpriteRenderer test, test2, test3;
    // Update is called once per frame
    private void Start()
    {
        updown = true;
        test2 = treeup.GetComponent<SpriteRenderer>();
        test3 = treeupper.GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {

        if (transform.localPosition.y >= -1.2)
        {
            updown = true;
            
        }
        else if (transform.localPosition.y<= -2)
        {
            updown = false;
            test.sortingOrder = 0;
            test2.sortingOrder++;
            test3.sortingOrder++;
        }
        if(updown)
        {
            transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
        }
        else if(!updown)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
    }
}
