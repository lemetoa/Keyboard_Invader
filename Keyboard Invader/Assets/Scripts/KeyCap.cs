using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyCap : MonoBehaviour
{
    public Datas.KeyPadData keypad { get; protected set; }     //키패드 속성

    [SerializeField]
    private string keyPadCode;
    public KeyCode Stand = KeyCode.Space;

    public Unit m_Master;

    [SerializeField]
    private TextMeshPro tmpro;

    public void SetKeyPad(string code)//키패드 속성 변경
    {
        keyPadCode = code;
        if (Datas.KeyPadData.KeyPadDataMap.ContainsKey(code))
        {
            keypad = Datas.KeyPadData.KeyPadDataMap[code];

            tmpro.text = keypad.stand;
            Stand = (KeyCode)97 -'A' + keypad.stand[0]; //알파벳을 키코드로 바꿈
           // Debug.Log(Stand);
        }
    } 
    
    //체력

    private float nextShotTime = 0; //다음 발사 가능한 시간
    private bool Shootable()
    {
        if ( Time.time >= nextShotTime)
        {
            return true;
        }
        return false;
    }
    private bool isCharged;         //차지완료됐는지
    private float chargeTime;       //차지 완료되는 시간 

    // Start is called before the first frame update
    void Start()
    {
        SetKeyPad(keyPadCode);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //누르기 시작할때
    public void OnUseStart()
    {
        if (Shootable())
        {
            if (keypad !=null)
            {
                ProjectileManager.Shoot(transform, keypad.onFirstShot);
            }
        }
    }
    //누르고있을때
    public void OnUsing()
    {

    }
    //손가락 땠을 때 
    public void OnUseEnd()
    {

    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject, ObjectPooler.PoolingType.KeyCap);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (1<<collision.gameObject.layer == LayerMask.GetMask("EnemyProjectile"))
        {
            if (m_Master !=null)
            {
                //Debug.Log("crashed!");
                Vector2Int _pos = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
                m_Master.RemoveKeyPad(_pos);
                
            }
        }
    }

}
