using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, this.transform.position.z);
    }
}
