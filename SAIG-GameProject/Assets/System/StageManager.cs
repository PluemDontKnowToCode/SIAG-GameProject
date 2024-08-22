using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    [Header("InGameUI")]
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject InGameUI;
    [SerializeField] TMP_Text scoreDisplay;
    [SerializeField] TMP_Text killCountText;
    Animator killCountAnim;
    [SerializeField] GameObject UnimportantUI;
    [SerializeField] GameObject InGameBGM;
    public Slider healthBar;
    [Header("StartUI")]
    [SerializeField] GameObject StartUI;
    [SerializeField] TMP_Text HighScore;
    [SerializeField] GameObject creditPage;
    [SerializeField] GameObject mainMenuBGM;
    int highScore;
    [Header("Transition")]
    [SerializeField] PlayableDirector StartTimeline;
    [SerializeField] PlayableDirector BackTimeline;
    [Header("ETC ")]
    [SerializeField] GameObject[] obPrefab;
    public GameObject damageFloatingTextPrefab;
    public GameObject bulletPrefab;
    public GameObject enemyDieFXPrefab;

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
                

            }
        }
    }
    int _killCount = 0;
    public int killCount
    {
        get
        {
            return _killCount;
        }
        set
        {
            _killCount = value;
            killCountText.text = killCount + "x";
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-5f,5f));

            killCountAnim.Play("KillCount");
        }
    }
    float spawnDelay = 0;
    float maxDelay;
    void Start()
    {
        creditPage.SetActive(false);
        killCountAnim = killCountText.GetComponent<Animator>();
        highScore = PlayerPrefs.GetInt("HighScore");
        HighScore.text = $"HighScore : <color=#ff0000><b>{highScore}<b></color>";

        mainMenuBGM.SetActive(true);
        InGameBGM.SetActive(false);

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
        if(Player.Instance.isDie || Input.GetKeyDown(KeyCode.Escape))
        {
            ResetGame();
        }
        if(Input.GetKeyDown(KeyCode.Tab) && IsGameAvalible)
        {
            UnimportantUI.SetActive(!UnimportantUI.activeInHierarchy);
        }
    }
    void SpawnEnemies()
    {
        if(spawnDelay <= 0)
        {
            if(EnemyCount < 30)
            {
                int randomNum = Random.Range(0, obPrefab.Length - 1);
                Vector3 spawnPosition = (randomNum == 4) ? GetRandomPositionAroundPlayer(2f) : GetRandomPositionAroundPlayer(10f);
                GameObject objectToSpawn = Instantiate(
                    obPrefab[randomNum],
                    spawnPosition, 
                    Quaternion.identity
                );
                objectToSpawn.gameObject.SetActive(true);
            }
            
            maxDelay = 2 - (killCount / 10);

            if(maxDelay < 0.8f)
            {
                maxDelay = 0.8f;
            }

            spawnDelay = maxDelay;
        }
        spawnDelay -= Time.deltaTime;
    }

    Vector3 GetRandomPositionAroundPlayer(float outside)
    {
        Transform player = Player.Instance.transform;
        // Generate a random point inside a circle
        Vector2 randomPoint = Random.insideUnitCircle * 15;

        // Create the spawn position by adding the random point to the player's position
        Vector3 spawnPosition = new Vector3(
            player.position.x + randomPoint.x + outside, 
            player.position.y + randomPoint.y + outside, 
            player.position.z);

        return spawnPosition;
    }
    public void StartGame()
    {
        healthBar.maxValue = Player.Instance.HP.MaxStat;
        healthBar.value = Player.Instance.HP.MaxStat;

        StartTimeline.Play();

        Player.Instance.Respawn();

        StartCoroutine(GameAvailable(true));
    }
    IEnumerator GameAvailable(bool isAvalible)
    {
        yield return new WaitForSeconds(2f);
        mainMenuBGM.SetActive(!isAvalible);
        InGameBGM.SetActive(isAvalible);
        IsGameAvalible = isAvalible;
        if(isAvalible)
        {

        }
        else
        {
            if(Score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", Score);
                StartCoroutine(AnimateScoreChange(Score, highScore, 0.9f));
                highScore = Score;
            }
        }
    }
    public void ResetGame()
    {
        BackTimeline.Play();

        StartCoroutine(GameAvailable(false));
    }
}