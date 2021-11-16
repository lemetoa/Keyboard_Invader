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
    [SerializeField]
    private GameObject tmpKey;
    int level;
    Unit unit;

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
        unit = GameObject.Find("Player").gameObject.GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeBackground()
    {
        for (int i = 0; i < backgroundParent.childCount; i++)
        {
            if (backgroundParent.GetChild(i).TryGetComponent(out Renderer _render))
            {
                Debug.Log("메터리얼 " + level % 3);
                _render.material = material[level % 3];
            }  
        }
        level++;
        uiMgr.dontDoAgain = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)
        {
            StoreIn(col.gameObject.transform);
            // player = col.GetComponent<PlayerController>();
            // player.SetGameState(GameState.Shopping);
            GameState.ChangeState(GameStateType.Shopping);
        }
    }


    public void StoreIn(Transform tr)
    {
        uiMgr.OpenPopUp(1);
        uiMgr.RandomProjectile();
        if(unit.stands.Count > 1)
            cam.transform.position = cam.transform.position + cam.storeCameraOffset;
        else
            cam.transform.position = cam.transform.position + cam.storeCameraOffset_OnlyOne;
        unit.gameObject.transform.localScale = unit.gameObject.transform.localScale / unit.stands.Count;
        Time.timeScale = 0;
        Camera.main.orthographicSize = 1.5f;
        tmpKey.SetActive(false);
    }

    public void StoreOut()
    {
        Time.timeScale = 1;
        Datas.GameData.GameDataList[1].intValue = 0;
        GameState.ChangeState(GameStateType.Playing);
        unit.gameObject.transform.localScale = Vector3.one;
        tmpKey.SetActive(true);
        this.gameObject.SetActive(false);
        ChangeBackground();
        EnemySpawner.bossKilled = false;
        EnemySpawner.isSpawn = false;
    }
}
