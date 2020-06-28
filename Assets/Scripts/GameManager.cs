using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoint;

    public float maxSpawnDelay;
    public float curSpawnDelay;

    public GameObject player; // 플레이어 비행기 좌표를 위한것
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;

    public GameObject gameOverSet;
    public GameObject rankEnrollSet;
    public InputField IdField;

    public ObjectManager objectManager;

    public GameObject Menu;

    string enemyName;
    int bossCallPoint;
    bool bossSpawn = false;
    private void Awake()
    {
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL", "EnemyB" };
        bossCallPoint = 0;
        
    }
    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(1f, 2f);
            curSpawnDelay = 0;
        }

        //#.UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

        InputBack();
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 5);
        enemyName = enemyObjs[ranEnemy];        

        Player playerLogic = player.GetComponent<Player>();

        //boss를 일정 점수 이상 올라갈때마다 불러낼 식
        if (playerLogic.score > bossCallPoint + 20000)
        {
            bossCallPoint = playerLogic.score;
            bossSpawn = true;
        }
        if (bossSpawn)
        {
            enemyName = "EnemyB";
            ranPoint = 2;
        }
        GameObject enemy = objectManager.MakeObj(enemyName);
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        
        if(!bossSpawn)
        {
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = enemyLogic.sprites[0];
        }
        enemy.transform.position = spawnPoint[ranPoint].position;
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        bossSpawn = false;
    }

    public void UpdateLifeIcon(int life)
    {
        //#UI. Life Init Disable
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        //#UI. Life Active
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomIcon(int boom)
    {
        //#UI. Life Init Disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        //#UI. Life Active
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }
    public void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplotion(type);
    }

    public void RankEnrollSet()
    {
        rankEnrollSet.SetActive(true);
    }

    public void RankEnroll()
    {
        PlayerPrefs.SetString("Name9", IdField.text);
        rankEnrollSet.SetActive(false);
        GameOver();
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(1);
    }

    public void CallMenu()
    {
        Time.timeScale = 0;
        Menu.SetActive(true);
    }

    public void InputBack()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 1;
            Menu.SetActive(false);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
    }
}
