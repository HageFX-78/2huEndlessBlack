using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpBehaviour : MonoBehaviour
{
    private float scoremultiplier;
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);

    }
    void Update()
    {
        scoremultiplier += Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().plyAudio("item");
            gameObject.SetActive(false);
            collision.GetComponent<PlayerAction>().addScore(1000-(5*Mathf.FloorToInt(scoremultiplier)));
        }

    }
}
