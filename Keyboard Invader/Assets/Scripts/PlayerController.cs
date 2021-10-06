using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform player;        //플레이어 기체

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
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        player.up = mousePos - (Vector2)transform.position;

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
