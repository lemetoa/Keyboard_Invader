using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public Transform player;        //플레이어 기체

    public Unit playerUnit;

    public VirtualKey virtualKey;   //키 배치할수 있는 공간
    

    private Camera camera;

    [SerializeField]
    private Transform[] keypads; //키패드 트랜스폼 (임시)

    public List<KeyCode> keys = new List<KeyCode>();
    // Start is called before the first frame update
    void Start()
    {
        playerUnit.AddKeyCap(Vector2Int.zero, "0");
        camera = Camera.main;
        if (virtualKey ==null)
        {
            virtualKey = GetComponent<VirtualKey>();
            virtualKey.SetUnit(playerUnit);
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (GameState.current)
        {
            case GameStateType.MainMenu:
                OnMainMenu();
                break;
            case GameStateType.Playing:
                OnPlaying();
                break;
            case GameStateType.Shopping:
                gameObject.transform.rotation = Quaternion.identity;
                if (Input.GetMouseButtonDown(0))
                {

                }
                break;
            case GameStateType.GameOver:
                break;
            case GameStateType.Setting:
                break;
            default:
                break;
        }

    }
    public void OnMainMenu()
    {/*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Start Trigger");
            GameState.ChangeState(GameStateType.Playing);
        }*/
    }

    public void OnPlaying()
    {
        if (Time.timeScale <= 0)
        {
            Debug.Log("멈춰있음");
            return;
        }
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        if (!Input.GetKey(KeyCode.LeftControl))
        {

            player.up = mousePos - (Vector2)transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            playerUnit.SetMove(true);
            playerUnit.SetTarget(mousePos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            playerUnit.SetMove(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProjectileManager.Shoot(keypads[0], "0");
        }
        
        //현재 가지고있는 알파벳 종류만큼 foreach문 돌아서  키 눌렸는지 확인
        foreach (var _stand in playerUnit.stands)
        {
            if (Input.GetKeyDown(_stand))
            {
                foreach (var _keyCap in playerUnit.GetKeybyStand(_stand))
                {
                    _keyCap.OnUseStart();
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (1<< collision.gameObject.layer == LayerMask.GetMask("DroppedKey"))
        {
            if (collision.gameObject.TryGetComponent(out KeyCap _cap))
            {
                virtualKey.GainTmpKey(_cap);
                virtualKey.SetVirtualKeys(playerUnit.keyPads.Keys.ToList());
                virtualKey.SetKeysActive(true);
            }
        }
    }
}
