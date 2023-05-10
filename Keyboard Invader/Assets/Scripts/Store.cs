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

    [SerializeField]
    private float camSize = 5f;
    public OnClick uiMgr;
    FollowCamera cam;
    [SerializeField]
    Transform backgroundParent;
    [SerializeField]
    private Material[] material;
    [SerializeField]
    private GameObject tmpKey;
    int level;
    Unit unit;
    BoxCollider2D box;

    [SerializeField]
    private GameObject sellingUI;
    public static bool isSelling = false;//판매중


    private void Awake()
    {
        if (_go ==null)
        {
            _go = this.gameObject;
            GameState.onReset += delegate { _go.SetActive(false); };
            _go.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<FollowCamera>();
        GameObject player = GameObject.Find("Player").gameObject;
        unit = player.GetComponent<Unit>();
        box = player.GetComponent<BoxCollider2D>();
        GetComponent<SpriteRenderer>().sortingOrder = -2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrySell(bool sell)
    {
        isSelling = sell;
        sellingUI.SetActive(sell);
    }
    public void ChangeBackground()
    {
        for (int i = 0; i < backgroundParent.childCount; i++)
        {
            if (backgroundParent.GetChild(i).TryGetComponent(out Renderer _render))
            {
                //Debug.Log("메터리얼 " + level % material.Length);
                _render.material = material[level % material.Length];
            }  
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
        unit.gameObject.transform.localScale = unit.gameObject.transform.localScale / Mathf.Sqrt(unit.stands.Count);
        box.enabled = false;
        Time.timeScale = 0;
        Camera.main.orthographicSize = camSize;
        tmpKey.SetActive(false);

        if (unit.stands.Count > 1)
            cam.transform.position = cam.transform.position + cam.storeCameraOffset;
        else
            cam.transform.position = cam.transform.position + cam.storeCameraOffset_OnlyOne;

    }

    public void StoreOut()
    {
        Time.timeScale = 1;
        box.enabled = true;
        Datas.GameData.GameDataList[1].intValue = 0;
        GameState.ChangeState(GameStateType.Playing);
        unit.gameObject.transform.localScale = Vector3.one;
        tmpKey.SetActive(true);
        this.gameObject.SetActive(false);
        ChangeBackground();
        EnemySpawner.bossKilled = false;
        EnemySpawner.isBossSpawn = false;
    }
}
