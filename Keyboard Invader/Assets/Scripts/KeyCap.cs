using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class KeyCap : MonoBehaviour
{
    public Datas.KeyPadData keypad { get; protected set; }     //키패드 속성

    [SerializeField]
    protected string keyPadCode;
    public KeyCode Stand = KeyCode.Space;

    public Unit m_Master;

    [SerializeField]
    protected TextMeshPro tmpro;

    [SerializeField]
    private int getCost = 200;
    protected GameObject keycapInfo;

    Collider2D coll;
    

    public void SetKeyPad(string code, KeyCode _stand = KeyCode.None)//키패드 속성 변경
    {
        keyPadCode = code;
        //Debug.Log("키패드 " + keyPadCode);
        if (Datas.KeyPadData.KeyPadDataMap.ContainsKey(code))
        {
            keypad = Datas.KeyPadData.KeyPadDataMap[code];

            if (_stand != KeyCode.None)
            {
                if (keypad.stand == "Space" || keypad.stand == "space")
                {
                    tmpro.text = "";
                    Stand = KeyCode.Space;
                }
                else
                {
                    tmpro.text = keypad.stand;
                    Stand = (KeyCode)97 - 'A' + keypad.stand[0]; //알파벳을 키코드로 바꿈
                                                                 // Debug.Log(Stand);
                }
            }
        }
    } 

    public void ChangeStand(KeyCode _newCode)
    {

        if (_newCode == KeyCode.Space)
        {
            tmpro.text = "";
        }
        {
            tmpro.text = _newCode.ToString();
            //Stand = (KeyCode)97 - 'A' + keypad.stand[0]; //알파벳을 키코드로 바꿈
                                                         // Debug.Log(Stand);
        }
        Stand = _newCode;
    }
    public void ChangeStand(char _newCode)
    {
        Stand = (KeyCode)97 - 'A' + _newCode;
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
    protected virtual void Start()
    {
        keycapInfo = GameObject.Find("KeyCapInfoHolder");

        SetKeyPad(keyPadCode, Stand);

        //아이템 키일 경우 게임 재시작 시 비활성화
        GameState.onReset += delegate {
            if (gameObject.layer ==13)
            {
                gameObject.SetActive(false);
            } };
    }

    // Update is called once per frame
    void Update()
    {
        if (EditorApplication.isPlaying)
        {
            SetKeyPad(keyPadCode, Stand);
        }
    }

    //누르기 시작할때
    public virtual void OnUseStart()
    {
        if (Shootable())
        {
            if (keypad !=null)
            {
                ProjectileManager.Shoot(transform, keypad.onFirstShot,gameObject.layer);
                nextShotTime = Time.time + 1f / keypad.fireRate;
            }
        }
    }
    //누르고있을때
    public void OnUsing()
    {
        if (keypad.autoFire && Shootable())
        {
            if (keypad != null)
            {
                ProjectileManager.Shoot(transform, keypad.onFirstShot, gameObject.layer);
                nextShotTime = Time.time + 1f / keypad.fireRate;
            }
        }

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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (1<<collision.gameObject.layer == LayerMask.GetMask("EnemyProjectile"))
        {
            
            var _projectile = collision.GetComponent<Projectile>();
            _projectile.life -= 1f;
            if (_projectile.life <= 0)
            {
                collision.gameObject.SetActive(false);
            }
            if (m_Master !=null)
            {
                //Debug.Log("crashed!");
                Vector2Int _pos = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
                m_Master.RemoveKeyPad(_pos);
                
            }
        }
    }

    private void OnMouseDown()
    {
        
        Debug.Log("분해");
        
        if (GameState.current != GameStateType.Shopping)
        {
            return;
        }
           
        if (m_Master != null && Stand != KeyCode.Space)
        {
            if(Datas.GameData.GameDataList[1].intValue > 0) // 구매 아이템 선택시
            {
                SetKeyPad(Datas.GameData.GameDataList[2].strValue);
                Datas.GameData.GameDataList[0].intValue -= Datas.GameData.GameDataList[1].intValue;
                Datas.GameData.GameDataList[1].intValue = 0;
                GameObject cancelBuyButton = GameObject.Find("CancelBuyButton");
                cancelBuyButton.SetActive(false);
            }
            else
            {
                //Debug.Log("crashed!");
                Datas.GameData.GameDataList[0].intValue += getCost;
                Vector2Int _pos = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
                m_Master.RemoveKeyPad(_pos);


            }
                Text money = GameObject.Find("CostAmount").GetComponent<Text>();
                money.text = Datas.GameData.GameDataList[0].intValue.ToString();

        }

    }

    private void OnMouseEnter()
    {
        if (GameState.current == GameStateType.Shopping)
        {
            keycapInfo.transform.GetChild(0).gameObject.SetActive(true);

            
            {
                keycapInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("laser1");
                keycapInfo.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = Datas.KeyPadData.KeyPadDataMap[keyPadCode].Description; ;

            }
        }
    }

    private void OnMouseOver()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        keycapInfo.transform.position = worldPosition;
    }

    private void OnMouseExit()
    {
        keycapInfo.transform.GetChild(0).gameObject.SetActive(false);
    }
}
