using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    public float startSize = 5;
    [HideInInspector]
    public float currSize;
    public Vector3 storeCameraOffset;
    public Vector3 storeCameraOffset_OnlyOne;

    public List<BackgroundScroll> backgrounds = new List<BackgroundScroll>();

    private Camera _main;
    // Start is called before the first frame update
    void Start()
    {
        _main = Camera.main;
        _main.orthographicSize = startSize;

        foreach (var item in backgrounds)
        {
             item.bgScale = _main.orthographicSize * 0.5f;
            //item.render.material.mainTexture.
             item.FitSize();
        }
    }

    private void FixedUpdate()
    {
        if(GameState.current == GameStateType.Playing)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y, this.transform.position.z);

        }

        if(currSize < startSize)
            _main.orthographicSize = startSize;
        else
            _main.orthographicSize = currSize;
    }
    private void Update()
    {

        foreach (var item in backgrounds)
        {
            //item.bgScale = _main.orthographicSize * 0.5f;
           // item.FitSize();
        }
    }
}
