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
        current = _state;
    }

    public GameObject uiObject;

    public Animator mainMenuAnim;

    private static GameStateType lastType = GameStateType.MainMenu; //마지막 상태가 어디였는지

    public void EscClicked()
    {
        EscClicked_();
    }

    private static void EscClicked_()
    {
        if (current == GameStateType.Setting)   //설정창에서 눌렀을때
        {
            Time.timeScale = 1f;
            instance.uiObject.SetActive(false);
            ChangeState(lastType);
        }
        else
        {
            Time.timeScale = 0f;
            instance.uiObject.SetActive(true);
            lastType = current;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mainMenuAnim.SetTrigger("Start Trigger");
            GameState.ChangeState(GameStateType.Playing);
            //playerUnit.AddKeyCap(Vector2Int.zero, "0");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscClicked_();
        }
    }
}
