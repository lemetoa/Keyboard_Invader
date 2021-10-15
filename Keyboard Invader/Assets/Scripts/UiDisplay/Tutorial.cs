using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    private int page;


    public Image img;
    public List<Sprite> sprites = new List<Sprite>();

    public Button next;

    public void buttonClicked()
    {
        page++;
        if (page < sprites.Count)
        {
            img.sprite = sprites[page];
        }
        else
        {
            img.gameObject.SetActive(false);
            GameState.ChangeState(GameStateType.MainMenu);
        }
    }

    public void OpenTutorial()
    {
        page = 0;
        img.sprite = sprites[0];
        img.gameObject.SetActive(true);

        GameState.ChangeState(GameStateType.Tutorial);
    }

    public void CloseTutorial()
    {
        img.gameObject.SetActive(false);
        GameState.ChangeState(GameStateType.MainMenu);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
