using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    [HideInInspector]
    public bool isOffsetOn;
    public float startSize = 5;
    [HideInInspector]
    public float currSize;
    public Vector3 storeCameraOffset;
    // Start is called before the first frame update
    void Start()
    {

        Camera.main.orthographicSize = startSize;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        if (isOffsetOn)
            transform.position = new Vector3(playerPos.x, playerPos.y, this.transform.position.z) + storeCameraOffset;
        else
            transform.position = new Vector3(playerPos.x, playerPos.y, this.transform.position.z);

        if(currSize < startSize)
            Camera.main.orthographicSize = startSize;
        else
            Camera.main.orthographicSize = currSize;
    }
}
