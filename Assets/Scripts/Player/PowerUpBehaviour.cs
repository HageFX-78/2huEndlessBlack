using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().plyAudio("item");
            gameObject.SetActive(false);
            collision.GetComponent<PlayerAction>().addBulletGauge(25);
        }

    }

    // Update is called once per frame
}
