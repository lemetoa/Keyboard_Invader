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
    
    public Vector3 currCameraOffset;
    public List<BackgroundScroll> backgrounds = new List<BackgroundScroll>();

    private Camera _main;
    // Start is called before the first frame update
    void Start()
    {
        _main = Camera.main;
        _main.orthographicSize = startSize;
        currCameraOffset = storeCameraOffset;

        foreach (var item in backgrounds)
        {
             item.bgScale = _main.orthographicSize * 0.5f;
             item.FitSize();
        }
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, this.transform.position.z);

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
