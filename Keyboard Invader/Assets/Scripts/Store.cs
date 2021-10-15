using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    private static GameObject _go;
    public static GameObject Portal()
    {
        return _go;
    }
    public OnClick uiMgr;
    FollowCamera cam;
    [SerializeField]
    Transform backgroundParent;
    [SerializeField]
    private Material[] material;
    int level;

    private void Awake()
    {
        if (_go ==null)
        {
            _go = this.gameObject;
            _go.SetActive(false);
        }
        Debug.Log(_go);
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeBackground()
    {
        for (int i = 0; i < backgroundParent.childCount - 1; i++)
        {
            backgroundParent.GetChild(i).GetComponent<Renderer>().material = material[level];
        }
        level++;
        uiMgr.dontDoAgain = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)
        {
            StoreIn();
            // player = col.GetComponent<PlayerController>();
            // player.SetGameState(GameState.Shopping);
            GameState.ChangeState(GameStateType.Shopping);
        }
    }


    public void StoreIn()
    {
        uiMgr.OpenPopUp(1);
        uiMgr.RandomProjectile();
        Time.timeScale = 0;
        cam.transform.position = cam.transform.position + cam.currCameraOffset;
    }

    public void StoreOut()
    {
        Time.timeScale = 1;
        Datas.GameData.GameDataList[1].intValue = 0;
        GameState.ChangeState(GameStateType.Playing);
        this.gameObject.SetActive(false);
        ChangeBackground();
        EnemySpawner.bossKilled = false;
    }
}
