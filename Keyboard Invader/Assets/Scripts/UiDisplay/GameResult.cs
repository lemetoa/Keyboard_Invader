using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResult : MonoBehaviour
{
    private static GameResult instance;
    public static GameResult Instance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameResult>();
        }
        if (instance == null)
        {
            var newObj = new GameObject("ResultScreen");
            newObj.AddComponent<GameResult>();
        }
        return instance;
    }
    public GameObject resultScreen;

    [SerializeField]
    TextMeshProUGUI timeTmpro, enemyTmpro, bossTmpro, scoreTmpro, highscoreTmpro;
    
    
    public static int enemyDestroyed = 0,
        bossDestroyed = 0;

    public static bool timeBonus = false;      //시간점수가 반영되고있는지
    public static float playTime = 0f;

    public static void ShowResult()
    {
        EnemySpawner.StopSpawn();
        EnemySpawner.ResetSpawner();

        Score.SaveHighScore();
        instance.timeTmpro.text = playTime.ToString("00:00");
        instance.enemyTmpro.text = enemyDestroyed.ToString("0");
        instance.bossTmpro.text = bossDestroyed.ToString("0");
        instance.scoreTmpro.text = Score.curScore.ToString("0");
        if (Score.curScore >= Score.highScore)
        {
            instance.scoreTmpro.color = Color.red;
        }
        else
        {
            instance.scoreTmpro.color = Color.white;
        }

        instance.highscoreTmpro.text = Score.highScore.ToString("0");

        GameState.ChangeState(GameStateType.GameOver);
        instance.resultScreen.SetActive(true);
    }
    //메뉴로가기 눌렸을때
    public void MenuClicked()
    {
        CloseResult();
        GameState.ChangeState(GameStateType.MainMenu);
    }

    public void RetryClicked()    // 리트라이 눌렸을때
    {
        CloseResult();
        Score.ResetCurScore();
        GameState.StartGame();
    }


    public void CloseResult()   //결과창 닫기
    {
        resultScreen.SetActive(false);
    }

    
    public void ClearResult()   //게임결과 초기화
    {
        enemyDestroyed = 0;
        bossDestroyed = 0;
        playTime = 0f;
        timeBonus = false;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }resultScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBonus == true)
        {
            playTime += Time.deltaTime* Time.timeScale;
        }

    }
}
