using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public OnClick uiMgr;
    FollowCamera cam;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == 8)
        {
            cam.isOffsetOn = true;
            StartCoroutine(StoreIn());
            // player = col.GetComponent<PlayerController>();
            // player.SetGameState(GameState.Shopping);
            GameState.ChangeState(GameStateType.Shopping);
        }
    }


    IEnumerator StoreIn()
    {
        yield return new WaitForSeconds(0.2f);
        uiMgr.OpenPopUp(2);
        Time.timeScale = 0;
    }

    public void StoreOut()
    {
        cam.isOffsetOn = false;
        Time.timeScale = 1;
        Datas.GameData.GameDataList[1].intValue = 0;
        GameState.ChangeState(GameStateType.Playing);
        this.gameObject.SetActive(false);
    }
}
