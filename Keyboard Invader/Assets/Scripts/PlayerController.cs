using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainMenu,
    Playing,
    Shopping,
    Ranking,
    Setting,    
}
public class PlayerController : MonoBehaviour
{
    public static GameState gameState { get; protected set; }
    public GameState GetGameState()
    {
        return GameState.MainMenu;
    }
    public void SetGameState(GameState _state)
    {
        gameState = _state;
    }

    [SerializeField]
    private Animator anim;

    public Transform player;        //플레이어 기체

    public Unit playerUnit;

    private Camera camera;

    [SerializeField]
    private Transform[] keypads; //키패드 트랜스폼 (임시)


    public List<KeyCode> keys = new List<KeyCode>();
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                OnMainMenu();
                break;
            case GameState.Playing:
                OnPlaying();
                break;
            case GameState.Shopping:
                break;
            case GameState.Ranking:
                break;
            case GameState.Setting:
                break;
            default:
                break;
        }

    }
    public void OnMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Start Trigger");
            gameState = GameState.Playing;
            playerUnit.AddKeyCap(Vector2Int.zero);
        }
    }

    public void OnPlaying()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        player.up = mousePos - (Vector2)transform.position;

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("d");
            //player.transform.position += ((Vector3)mousePos - player.position).normalized * Time.deltaTime;
            playerUnit.SetMove(true);
            playerUnit.SetTarget(mousePos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            playerUnit.SetMove(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProjectileManager.Shoot(keypads[3], "0");
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            ProjectileManager.Shoot(keypads[0], "1C11");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ProjectileManager.Shoot(keypads[1], "1D11");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ProjectileManager.Shoot(keypads[2], "1L11");
        }

        //현재 가지고있는 알파벳 종류만큼 foreach문돌아서  키 눌렸는지 확인

        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {

            }
            else if (Input.GetKey(key))
            {

            }
            else if (Input.GetKeyUp(key))
            {

            }
        }

    }
}
