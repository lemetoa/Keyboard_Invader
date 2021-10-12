using System.Collections;
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
    public static void ChangeState(GameStateType _state)
    {
        switch (_state)
        {
            case GameStateType.MainMenu:
                Time.timeScale = 1f;
                Score.ResetCurScore();
                instance.mainMenuAnim.ResetTrigger("Start Trigger");
                instance.mainMenuAnim.SetTrigger("MainMenu Trigger");

                break;
            case GameStateType.Playing:
                Time.timeScale = 1f;
                break;
            case GameStateType.Shopping:
                Time.timeScale = 0f;
                break;
            case GameStateType.GameOver:
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

    public Animator mainMenuAnim;

    public static void StartGame()  //게임 시작
    {
        Score.curScore = 0;
        instance.mainMenuAnim.SetTrigger("Start Trigger");
        ChangeState(GameStateType.Playing);
        GameResult.timeBonus = true;
        PlayerController.instance.transform.position = PlayerController.startPosition;
        PlayerController.instance.playerUnit.AddKeyCap(Vector2Int.zero, "0", KeyCode.Space); 

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

    private void Update()
    {
        if (current== GameStateType.MainMenu && Input.GetKeyDown(KeyCode.Space))   //게임 시작
        {/*
            mainMenuAnim.SetTrigger("Start Trigger");
            GameState.ChangeState(GameStateType.Playing);
            GameResult.timeBonus = true;*/
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscClicked_();
        }
    }
}
