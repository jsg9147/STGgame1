using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public int life;
    public int score;

    public float speed;
    public int power;
    public int maxPower;
    public int maxBoom;
    public int boom;

    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletObj_A;
    public GameObject bulletObj_B;
    public GameObject bulletObj_C;
    public GameObject bulletObj_D;
    public GameObject boomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public bool isHit;
    public bool isBoomTime;

    public bool isRespawnTime;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    private float time = 0;

    Animator anim;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;
        if(isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }
    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
        TimeToScore();
    }
    public void JoyPanel(int type)
    {
        for (int index = 0; index < 9; index++)
        {
            joyControl[index] = index == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }
    public void JoyUp()
    {
        isControl = false;
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");        
        float v = Input.GetAxisRaw("Vertical");

        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1) || !isControl)
        {
            h = 0;
        }
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl)
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        //Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; 부드럽게 이동하는 방법
        Vector3 nextPos = new Vector3(h, 0, 0);
        Vector3 movePos = curPos + nextPos;
        if (movePos.x < -2 || movePos.x > 2)
            return;

        transform.position = curPos + nextPos;

        isControl = false;
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }
    public void ButtonAUp()
    {
        isButtonA = false;
    }
    public void ButtonBDown()
    {
        isButtonB = true;
    }

    void Fire()
    {

        if (curShotDelay < maxShotDelay) //딜레이 전에는 발사 못하게
            return;

        switch(power)
        {
            case 0:
                //Power one
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 1:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;

                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.3f;

                GameObject bulletCC = objectManager.MakeObj("BulletPlayerB");
                bulletCC.transform.position = transform.position;

                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.3f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 3:
                GameObject bulletC = objectManager.MakeObj("BulletPlayerC");
                bulletC.transform.position = transform.position;

                Rigidbody2D rigidC = bulletC.GetComponent<Rigidbody2D>();
                rigidC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 4:
            case 5:
                GameObject bulletCR = objectManager.MakeObj("BulletPlayerC");
                bulletCR.transform.position = transform.position + Vector3.right * 0.1f;

                GameObject bulletCL = objectManager.MakeObj("BulletPlayerC");
                bulletCL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidCR = bulletCR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCL = bulletCL.GetComponent<Rigidbody2D>();
                rigidCR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 6:
                GameObject bulletCRR = objectManager.MakeObj("BulletPlayerC");
                bulletCRR.transform.position = transform.position + Vector3.right * 0.3f;

                GameObject bulletDCC = objectManager.MakeObj("BulletPlayerD");
                bulletDCC.transform.position = transform.position;

                GameObject bulletCLL = objectManager.MakeObj("BulletPlayerC");
                bulletCLL.transform.position = transform.position + Vector3.left * 0.3f;

                Rigidbody2D rigidCRR = bulletCRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidDCC = bulletDCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCLL = bulletCLL.GetComponent<Rigidbody2D>();
                rigidCRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidDCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }       

        curShotDelay = 0; //총을 다 쏜후 장전을 위한 초기화
    }

    void Boom()
    {
        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isButtonB = false;
        isBoomTime = true;
        //gameManager.UpdateBoomIcon(boom);
        
        //#1. Effect visible
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 4f);

        //#2.Remove Enemy
        GameObject[] enemiesB = objectManager.GetPool("EnemyB");
        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for (int index = 0; index < enemiesB.Length; index++)
        {
            if (enemiesB[index].activeSelf)
            {
                Enemy enemyLogic = enemiesB[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesL.Length; index++)
        {
            if(enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }            
        }
        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        

        //#3.Remove Enemy Bullet
        GameObject[] enemyBulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] enemyBulletsB = objectManager.GetPool("BulletEnemyB");
        for (int index = 0; index < enemyBulletsA.Length; index++)
        {
            enemyBulletsA[index].SetActive(false);
        }
        for (int index = 0; index < enemyBulletsB.Length; index++)
        {
            enemyBulletsB[index].SetActive(false);
        }
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //외곽선
        if(collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
        //적 충돌
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime)
                return;
            if (isHit)
                return;

            isHit = true;
            life--;
            //gameManager.UpdateLifeIcon(life);

            gameManager.CallExplosion(transform.position, "P");

            if(life ==0)
            {
                if(PlayerPrefs.HasKey("rank9"))
                {
                    if (score > Convert.ToInt32(PlayerPrefs.GetString("rank9")))
                    {
                        gameManager.RankEnrollSet();
                        PlayerPrefs.SetString("rank9", score.ToString());
                    }
                    else
                        gameManager.GameOver();
                }
                else
                {
                    gameManager.GameOver();
                }
                
            }
            else
            {
                gameManager.RespawnPlayer();
            }            
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
        //아이템
        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch(item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                        power++;
                    break;
                case "Boom":
                    boom++;
                    //gameManager.UpdateBoomIcon(boom); 폭탄 갱신 함수, 즉발로 바뀌며 필요 없어짐
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }
    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void TimeToScore()
    {
        time += Time.deltaTime;
        if(time > 0.1)
        {
            score += 10;
            time = 0;
        }        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}