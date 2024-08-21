using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    [Header("InGameUI")]
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject InGameUI;
    [SerializeField] TMP_Text scoreDisplay;
    public Slider healthBar;
    [Header("StartUI")]
    [SerializeField] GameObject StartUI;
    [SerializeField] TMP_Text HighScore;
    [SerializeField] GameObject creditPage;
    int highScore;
    [Header("ETC ")]
    [SerializeField] AudioSource BGM;
    [SerializeField] Enemy[] enemiesPrefab;
    public GameObject damageFloatingTextPrefab;
    public GameObject bulletPrefab;
    public bool IsGameAvalible;
    int _enemyCount = 0;
    int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            StartCoroutine(AnimateScoreChange(_score, value,0.5f));
            _score = value;
        }
    }

    private IEnumerator AnimateScoreChange(int oldScore, int newScore,float duration)
    {
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
        highScore = PlayerPrefs.GetInt("HighScore");
        HighScore.text = $"HighScore : <color=#ff0000><b>{highScore}<b></color>";
        InGameUI.SetActive(false);
        StartUI.SetActive(true);
    }
    void Update()
    {
        if (!IsGameAvalible)
        {
            return;
        }
        if (healthBar != null)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, Player.Instance.HP.CurrentStat , 3f * Time.deltaTime);
        }

        PauseGame();
        SpawnEnemies();
        
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
            StartUI.SetActive(true);
        }
    }
    void SpawnEnemies()
    {
        if(spawnDelay <= 0)
        {
            if(EnemyCount < 30)
            {
                Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                Enemy enemy = Instantiate(
                    enemiesPrefab[Random.Range(0,enemiesPrefab.Length-1)],
                    spawnPosition, 
                    Quaternion.identity
                );
                enemy.gameObject.SetActive(true);
            }
            
            maxDelay = 2 - (killCount / 50);

            if(maxDelay < 0.8f)
            {
                maxDelay = 0.8f;
            }

            spawnDelay = maxDelay;
        }
        spawnDelay -= Time.deltaTime;
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        Transform player = Player.Instance.transform;
        // Generate a random point inside a circle
        Vector2 randomPoint = Random.insideUnitCircle * 15;

        // Create the spawn position by adding the random point to the player's position
        Vector3 spawnPosition = new Vector3(
            player.position.x + randomPoint.x + 15f, 
            player.position.y + randomPoint.y + 15f, 
            player.position.z);

        return spawnPosition;
    }
    public void StartGame()
    {
        healthBar.maxValue = Player.Instance.HP.MaxStat;
        healthBar.value = Player.Instance.HP.MaxStat;
        
        IsGameAvalible = true;

        InGameUI.SetActive(true);
        StartUI.SetActive(false);

        healthBar.gameObject.SetActive(true);
        scoreDisplay.gameObject.SetActive(true);
    }
    public void ResetGame()
    {
        

        PauseUI.SetActive(false);

        healthBar.gameObject.SetActive(false);
        scoreDisplay.gameObject.SetActive(false);

        InGameUI.SetActive(false);
        StartUI.SetActive(true);

        if(Score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", Score);
            StartCoroutine(AnimateScoreChange(Score, highScore, 0.9f));
            highScore = Score;
        }
    }
}