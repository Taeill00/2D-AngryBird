using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("StartGroup")]
    [SerializeField] private GameObject startBackgrounds;

    [Header("StartGrounp UI")]
    [SerializeField] private GameObject startUI;
    [SerializeField] private Button btn_start;
    [SerializeField] private Button btn_quit;

    [Header("InGame")]
    [SerializeField] private GameObject inGameUI;
    public BirdSpawner birdSpawner;
    public int score = 0;

    [Header("Stage")]
    public int stageIndex = 0;
    public List<GameObject> stagePrefabs;
    public GameObject curstage;

    [Header("InGame UI")]
    [SerializeField] private TextMeshProUGUI text_Score;


    [Header("GameOver UI")]
    [SerializeField] private GameObject panel_gameover;
    [SerializeField] private TextMeshProUGUI text_gameoverScore;
    [SerializeField] private TextMeshProUGUI text_gameoverHighScore;
    [SerializeField] private Button btn_restart;
    [SerializeField] private Button btn_menu;
    [SerializeField] private Button btn_nextStage;
    private int highScore = 0;


    private void Start()
    {
        Instance = this;
        curstage = Instantiate(stagePrefabs[stageIndex]);
        stageIndex++;

        highScore = PlayerPrefs.GetInt("HighScore");
        panel_gameover.SetActive(false);
    }

    private void Update()
    {
        ScoreUpdate();

        if(birdSpawner.birdCount == 0)
        {
            // BirdCount 
            birdSpawner.birdCount = 3;

            StartCoroutine(GameOverCo());
        }
    }

    private void ScoreUpdate()
    {
        text_Score.text = "Score : " + score.ToString();
    }

    private IEnumerator GameOverCo()
    {
        yield return new WaitForSeconds(1f);


        // BGM
        BGMManager.instance.StopBGM();

        // SFX
        SFXManager.instance.PlaySFX("Clear");

        // Panel
        panel_gameover.SetActive(true);
        text_gameoverScore.text = "Score : " + score.ToString();

        if(score >= highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", score);
        }

        text_gameoverHighScore.text = "HighScore : " + highScore.ToString();
    }

    public void OnBtnRestart()
    {
        InitData(1);
    }

    public void OnBtnNextStage()
    {
        InitData(0);
    }

    private void InitData(int index)
    {
        if (stagePrefabs[stageIndex] == null)
            return;

        // Panel
        panel_gameover.SetActive(false);

        // Stage
        Destroy(curstage);
        curstage = Instantiate(stagePrefabs[stageIndex - index]);
        stageIndex++;

        birdSpawner.InitBird();

        // Score
        score = 0;

        // Btn
        birdSpawner.InitBtn();

        // BGM
        BGMManager.instance.PlayBGM();
    }

    public void OnBtnMenu()
    {
        inGameUI.SetActive(false);
        panel_gameover.SetActive(false);

        startBackgrounds.SetActive(true);
        startUI.SetActive(true);
    }

    public void OnBtnStart()
    {
        startBackgrounds.SetActive(false);
        startUI.SetActive(false);

        inGameUI.SetActive(true);
    }

    public void OnBtnQuit()
    {
        Application.Quit();
    }

    
}
