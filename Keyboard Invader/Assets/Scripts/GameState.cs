﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType
{
    MainMenu,
    Playing,
    Shopping,
    GameOver,
    Setting,
}
public class GameState : MonoBehaviour
{
    private static GameState instance;
    public static GameStateType current { get; protected set; }

    [SerializeField]
    private GameStateType gameState;

    static GameObject player;
    public static void ChangeState(GameStateType _state)
    {
        switch (_state)
        {
            case GameStateType.MainMenu:
                Time.timeScale = 0f;
                Score.ResetCurScore();
                instance.mainMenuAnim.ResetTrigger("Start Trigger");
                instance.mainMenuAnim.SetTrigger("MainMenu Trigger");
                SoundManager.PlayBgm(SoundManager.GetBgm("Main"));
                break;
            case GameStateType.Playing:
                SoundManager.PlayRandomBgm();
                EnemySpawner.ContinueSpawn();
                Time.timeScale = 1f;
                break;
            case GameStateType.Shopping:
                Time.timeScale = 0f;
                break;
            case GameStateType.GameOver:
                ResetGame();
                Time.timeScale = 0f;
                break;
            case GameStateType.Setting:
                Time.timeScale = 0f;
                break;
            default:
                break;
        }
        current = _state;
    }

    public GameObject uiObject;


    public delegate void OnReset();
    public static OnReset onReset = new OnReset(delegate {
        // Debug.Log("Core destroyed!");
    });


    public Animator mainMenuAnim;

    private IEnumerator animationWait;
    public static IEnumerator AnimationWait()
    {
        instance.mainMenuAnim.SetTrigger("Start Trigger");
        yield return new WaitForSecondsRealtime(1);
        Debug.Log("스타트");
        StartGame();
        yield return new WaitForSecondsRealtime(0.5f);
    }

    public static void StartGame()  //게임 시작
    {
        ResetGame();
        Score.curScore = 0;

        ChangeState(GameStateType.Playing);
        player.SetActive(true);
        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(true);
        }
        GameResult.timeBonus = true;
        PlayerController.instance.transform.position = PlayerController.startPosition;
        PlayerController.instance.playerUnit.AddKeyCap(Vector2Int.zero, "0", KeyCode.Space);
        PlayerController.instance.playerUnit.dying = false;
        EnemySpawner.StartSpawn();
        

    }

    public void StartAnimation()
    {
        if (instance.animationWait !=null)
        {
            StopCoroutine(instance.animationWait);
        }
        instance.animationWait = AnimationWait();
        instance.StartCoroutine(instance.animationWait);
    }
    //게임 리셋
    public static void ResetGame()
    {
        // 모든 투사체 비활성화
        onReset.Invoke();
    }

    public void ToggleFullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;
    }
    private static GameStateType lastType = GameStateType.MainMenu; //마지막 상태 기억
    
    public void EscClicked()
    {
        EscClicked_();
    }

    private static void EscClicked_()
    {
        if (current == GameStateType.Setting)   //설정창에서 눌렀을때
        {
            if (lastType == GameStateType.Shopping || lastType == GameStateType.GameOver)
            {
               // Time.timeScale = 0f;
            }
            else
            {

               // Time.timeScale = 1f;
            }
            instance.uiObject.SetActive(false);
            ChangeState(lastType);
        }
        else
        {
            Time.timeScale = 0f;
            instance.uiObject.SetActive(true);
            lastType = current;
            Debug.Log(current);
            ChangeState(GameStateType.Setting);
        }

    }
    public void QuitGame()       //게임종료
    {
        Debug.Log("종료");
        QuitGame_();
    }

    private static void QuitGame_()
    {

        Application.Quit();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        instance.uiObject.SetActive(false);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        player.SetActive(false);
    }

    private void Update()
    {
        gameState = current;
        if (current== GameStateType.MainMenu && Input.GetKeyDown(KeyCode.Space))   //게임 시작
        {/*
            mainMenuAnim.SetTrigger("Start Trigger");
            GameState.ChangeState(GameStateType.Playing);
            GameResult.timeBonus = true;*/
            StartAnimation();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscClicked_();
        }
    }
}
