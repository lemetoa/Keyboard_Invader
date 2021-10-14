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

    public static bool highUpdated = false; //이번에 최고기록 갱신했는지

    [SerializeField]
    public TextMeshProUGUI scoreText,scoreNumber,highScoreText;
    [SerializeField]
    Animator anim;

    public static void AddScore(float _score)
    {
        curScore += _score;
        instance.scoreNumber.text = curScore.ToString("0");
        
        if (IsAboveBest(curScore))
        {
            highScore = curScore;
            // SaveHighScore();         //최고기록은 게임 끝났을때 저장함

            if (!highUpdated)
            {
                Debug.Log("최고기록 갱신!");
                highUpdated = true;
                instance.anim.SetTrigger("scoreBreak");
            }
            
        }
    }

    public static void ResetCurScore()
    {
        curScore = 0f;
        instance.scoreNumber.text = "0";
        highUpdated = false;
        //instance.scoreText.color = Color.white;
        //instance.scoreNumber.color = Color.white;
        instance.anim.SetTrigger("backtoNormal");
    }

    public static void SaveHighScore()
    {
        float _score = Mathf.Max(curScore, highScore); highScore = curScore;
        highScore = _score;
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
            AddScore(1 * Time.deltaTime * Time.timeScale);
        }
        highScoreText.text = "HighScore\n" + highScore.ToString("0");
    }
}
