using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    [Header("UI")]
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject RespawnUI;
    [SerializeField] TMP_Text scoreDisplay;
    [SerializeField] AudioSource currentBGM;
    [SerializeField] Enemy[] enemiesPrefab;
    public GameObject bulletPrefab;
    bool _isGameAvalible;
    public bool IsGameAvalible
    {
        get
        {
            return _isGameAvalible;
        }
        set
        {
            _isGameAvalible = value;
            if(value)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
    int _enemyCount = 0;
    int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            StartCoroutine(AnimateScoreChange(_score, value));
            _score = value;
        }
    }

    private IEnumerator AnimateScoreChange(int oldScore, int newScore)
    {
        float duration = 0.5f; // Animation duration
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            int currentScore = (int)Mathf.Lerp((float)oldScore, (float)newScore, t);
            scoreDisplay.text = $"Score : {currentScore}";
            yield return null;
        }

        // Ensure the final value is set correctly
        scoreDisplay.text = $"Score : {newScore}";
    }
    public int EnemyCount
    {
        get
        {
            return _enemyCount;
        }
        set
        {
            _enemyCount = value;
            if(value > 0)
            {

            }
            else
            {
                killCount++;
            }
        }
    }
    public int killCount = 0;
    float spawnDelay = 0;
    float maxDelay;
    void Start()
    {
        PauseUI.SetActive(false);
        RespawnUI.SetActive(false);
    }
    void Update()
    {
        PauseGame();

        if(spawnDelay <= 0)
        {
            SpawnEnemies();
            maxDelay = 3 - (killCount / 50);

            if(maxDelay < 0.8f)
            {
                maxDelay = 0.8f;
            }

            spawnDelay = maxDelay;
        }
        spawnDelay -= Time.deltaTime;
    }
    void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsGameAvalible = false;
            PauseUI.SetActive(true);
        }
        else if(Player.Instance.isDie)
        {
            IsGameAvalible = false;
            RespawnUI.SetActive(true);
        }
    }
    void SpawnEnemies()
    {
        if(EnemyCount < 20)
        {
            Vector3 spawnPosition = GetRandomPositionAroundPlayer();
            Enemy enemy = Instantiate(
                enemiesPrefab[Random.Range(0,enemiesPrefab.Length-1)],
                spawnPosition, 
                Quaternion.identity
            );
            enemy.gameObject.SetActive(true);
        }
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        Transform player = Player.Instance.transform;
        // Generate a random point inside a circle
        Vector2 randomPoint = Random.insideUnitCircle * 10;


        // Create the spawn position by adding the random point to the player's position
        Vector3 spawnPosition = new Vector3(
            player.position.x + randomPoint.x + 5f, 
            player.position.y + randomPoint.y + 5f, 
            player.position.z);

        return spawnPosition;
    }
    
}