using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    private static Score instance;
    public static float highScore; //최고점수

    public static float curScore = 0;   //이번 점수

    private static bool highUpdated = false; //이번에 최고기록 갱신했는지

    [SerializeField]
    public TextMeshProUGUI scoreText,scoreNumber;
    [SerializeField]
    Animator anim;

    public static void AddScore(float _score)
    {
        curScore += _score;
        instance.scoreNumber.text = curScore.ToString("0");
        
        if (IsAboveBest(curScore))
        {
            highScore = curScore;
            SaveHighScore();

            if (!highUpdated)
            {
                Debug.Log("최고기록 갱신!");
                highUpdated = true;
                instance.anim.SetTrigger("scoreBreak");
            }
            
        }
    }

    public static void SaveHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
    }
    public static void ResetHighScore()
    {
        highScore = 0f;
        
        PlayerPrefs.SetFloat("HighScore", 0f);
        Debug.Log("최고기록이 0으로 초기화되었습니다");
    }
    

    private static bool IsAboveBest(float _score)
    {
        if (highScore < _score)
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        if (instance ==null)
        {
            instance = this;
        }
        if (scoreNumber ==null && TryGetComponent(out TextMeshProUGUI _text))
        {
            scoreNumber = _text;
        }
    }
    private void Update()
    {
        if (GameState.current == GameStateType.Playing)
        {
            AddScore(10 * Time.deltaTime * Time.timeScale);
        }
    }
}
