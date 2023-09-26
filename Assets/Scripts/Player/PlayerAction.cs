using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerAction : MonoBehaviour
{
    //Reference to bulletpool
    BulletPoolGeneral mainpool;
    Queue<GameObject> bulletPool;
    Queue<GameObject> bulletPool2;
    DropPool dp;
    Queue<GameObject> dlist;
    GameObject pup;

    ShakeEffect cameraEff;

    [Header("Stats")]
    //Player stat
    public int HP;
    public static long score;
    private bool immuneState;
    [Header("Bullet")]
    [SerializeField] private int bulletLevel;
    [SerializeField] private int bulletGauge;
    //Variables for fired bullets
    public float speed, bulletSpeed, bulletSpeed2, fireRate;
    private bool b2switch;//Boolean to switch between left right for homing bullet
    public Transform bulletFPOS;//Fire position
    public Transform LbulletFPOS;//Fire position Left
    public Transform RbulletFPOS;//Fire position Right
    public int fireDelayTurn_b2;

    [SerializeField] private float baseAngle;
    [SerializeField] private float angleDifference;//Only affects bullet type 1
    [SerializeField] private float subAngleDifference;//Angle difference


    [Header("UI")]
    public TextMeshProUGUI bulletLVL;
    public TextMeshProUGUI scoreTXT;
    public TextMeshProUGUI scoreGO;
    public TextMeshProUGUI hpTXT;
    public TextMeshProUGUI winScore;
    public Image bgauge;

    [Header("Others")]
    public GameObject gameOverMenu;
    public GameObject winMenu;
    private Vector3 screenBorder;

    //Search range stuff
    public searchRange searchRange;
    private List<Transform> enemylist;

    public Animator anim;

    //Coroutine 
    IEnumerator cr;

    void Start()
    {

        bulletLevel = 1;

        score = 0;

        bulletGauge = 0;
        bgauge.rectTransform.sizeDelta = new Vector2(0, 32);
        immuneState = false;
        mainpool = BulletPoolGeneral.poolInstance;
        dp = DropPool.droppool;
        screenBorder = Camera.main.ScreenToWorldPoint(transform.position);
        cameraEff = ShakeEffect.caminstance;
        cr = shoot();
    }

    void Update()
    {
        if (bulletLevel == 9)
        {
            bulletLVL.text = "MAX";
        }
        else
        {
            bulletLVL.text = bulletLevel.ToString();
        }

        scoreTXT.text = score.ToString();
        scoreGO.text = "Score: " + score.ToString();
        hpTXT.text = "x" + HP.ToString();
        int localGauge = bulletGauge;
        if (localGauge > 100) { localGauge -= 100 * (bulletLevel - 1); }
        bgauge.rectTransform.sizeDelta = new Vector2(localGauge * 256 / 100, 32);




        //Input----------------------------------------------------------------------------------------
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }


        //shoot code for now
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(cr);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopCoroutine(cr);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed /= 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed *= 2;
        }
    }
    void LateUpdate()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, screenBorder.x, screenBorder.x * -1);
        currentPos.y = Mathf.Clamp(currentPos.y, screenBorder.y, screenBorder.y * -1);
        transform.position = currentPos;
    }

    public void damageSelf()
    {
        if (!immuneState)
        {
            FindObjectOfType<AudioManager>().plyAudio("plyerDeath");
            cameraEff.shakeScreen(0.2f);
            HP--;

            Debug.Log(HP);
            if (HP <= 0)
            {
                Time.timeScale = 0f;
                gameOverMenu.SetActive(true);
            }
            else
            {
                if (bulletLevel > 1)
                {
                    bulletGauge -= 100;
                    for (int x = 1; x < 6; x++)
                    {
                        dlist = dp.getDropQueue("powerUp");
                        pup = dlist.Dequeue();
                        pup.transform.position = transform.position + new Vector3(Random.Range(-3, 3) * x, Random.Range(-2, 2) * x, 0);
                        pup.SetActive(true);
                        Rigidbody2D rb = pup.GetComponent<Rigidbody2D>();
                        rb.AddForce(transform.up * 15, ForceMode2D.Impulse);
                        dlist.Enqueue(pup);
                    }
                    bulletLevel--;
                }



                anim.SetBool("isImmune", true);
                Invoke("removeImmune", 5f);
            }

        }

    }
    void removeImmune()
    {
        immuneState = false;
        anim.SetBool("isImmune", false);
    }
    public void addScore(int amt)
    {
        score += Mathf.Abs(amt);

    }
    public void addBulletGauge(int amt)
    {
        if (bulletLevel < 9)
        {
            bulletGauge += amt;
            upgradeBullet();
        }
        else if (bulletLevel == 9)
        {
            bulletGauge = 900;
            bgauge.rectTransform.sizeDelta = new Vector2(256, 32);
        }

    }

    private void upgradeBullet()
    {
        if (bulletGauge >= 100 * bulletLevel)
        {
            bulletLevel++;
        }
    }

    public void winCondition()
    {
        winScore.text = score + "\n" + "Boss Defeated: +" + 10000 + "\n HP Bonus: x" + HP + "\n Total: " + (score + 10000) * 5;
        winMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {

            damageSelf();
        }

    }
    Transform getClosestEnemy(List<Transform> enemies)
    {

        Transform closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform target in enemies)
        {
            Vector3 directionToTarget = target.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = target;
            }
        }

        return closest;
    }
    IEnumerator shoot()//Shoot action
    {
        bulletPool = mainpool.getBulletQueue("playerBullet");
        bulletPool2 = mainpool.getBulletQueue("playerBullet2");
        int tempSkip = 0;
        while (true)
        {
            FindObjectOfType<AudioManager>().plyAudio("shoot");
            bool skipThisTurn = tempSkip > 0 ? true : false;
            float offsetAngle = angleDifference / 2;
            int offsetCount;
            if (bulletLevel % 2 == 0)
            {
                offsetCount = (bulletLevel - 2) / 2;
            }
            else
            {
                offsetCount = (bulletLevel - 1) / 2;
            }
            float baseTemp = baseAngle - (offsetAngle * offsetCount);

            //For secondary/directed bullet
            float tempAngle = subAngleDifference + 90;
            float tempAngle2 = tempAngle - (subAngleDifference * 2);
            b2switch = false;

            for (int x = 0; x < bulletLevel; x++)
            {

                GameObject thisBullet;
                GameObject thisBullet2;
                if ((x + 1) % 2 != 0)
                {
                    //fire angle left right switch back, left 
                    baseTemp += x * angleDifference;


                    thisBullet = bulletPool.Dequeue();
                    thisBullet.transform.position = bulletFPOS.transform.position;

                    thisBullet.SetActive(true);
                    Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();

                    float xdir = Mathf.Cos(baseTemp * Mathf.PI / 180);
                    float ydir = Mathf.Sin(baseTemp * Mathf.PI / 180);

                    rb.AddForce(new Vector2(xdir, ydir) * bulletSpeed, ForceMode2D.Impulse);
                    bulletPool.Enqueue(thisBullet);
                }
                else//----------------------------Fire homing bullet code/secondary fire
                {
                    //fire angle left right switch back, right
                    baseTemp -= x * angleDifference;
                    enemylist = searchRange.getEnemyList();
                    if (!skipThisTurn)
                    {
                        thisBullet = bulletPool2.Dequeue();//Get bullet from queue reminder
                        thisBullet.transform.position = LbulletFPOS.transform.position;
                        thisBullet.SetActive(true);
                        Rigidbody2D rb = thisBullet.GetComponent<Rigidbody2D>();

                        thisBullet2 = bulletPool2.Dequeue();
                        thisBullet2.transform.position = RbulletFPOS.transform.position;
                        thisBullet2.SetActive(true);
                        Rigidbody2D rb2 = thisBullet2.GetComponent<Rigidbody2D>();
                        if (b2switch)
                        {
                            tempAngle += subAngleDifference;
                            tempAngle2 -= subAngleDifference;
                        }
                        else
                        {
                            b2switch = true;
                        }

                        if (enemylist.Count != 0)
                        {

                            rb.AddForce((getClosestEnemy(enemylist).position - transform.position).normalized * bulletSpeed2, ForceMode2D.Impulse);
                            rb2.AddForce((getClosestEnemy(enemylist).position - transform.position).normalized * bulletSpeed2, ForceMode2D.Impulse);
                        }
                        else
                        {
                            float xdir = Mathf.Cos(tempAngle * Mathf.PI / 180);
                            float ydir = Mathf.Sin(tempAngle * Mathf.PI / 180);
                            rb.AddForce(new Vector2(xdir, ydir) * bulletSpeed2, ForceMode2D.Impulse);
                            float xdir2 = Mathf.Cos(tempAngle2 * Mathf.PI / 180);
                            float ydir2 = Mathf.Sin(tempAngle2 * Mathf.PI / 180);
                            rb2.AddForce(new Vector2(xdir2, ydir2) * bulletSpeed2, ForceMode2D.Impulse);
                        }

                        bulletPool2.Enqueue(thisBullet);
                        bulletPool2.Enqueue(thisBullet2);
                    }
                }
            }

            if (tempSkip > 0)
            {
                tempSkip--;
            }
            else if (fireDelayTurn_b2 > 0)
            {
                tempSkip = fireDelayTurn_b2;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }
}
