using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;


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
    Rigidbody2D rb;
    int x;
    int y;

    private float enableTime;

    InfoPos infoPos;
    public void SetKeyPad(string code, KeyCode _stand = KeyCode.None)//키패드 속성 변경
    {
        keyPadCode = code;
        // Debug.Log("키패드 " + keyPadCode);
        if (Datas.KeyPadData.KeyPadDataMap.ContainsKey(code))
        {
            keypad = Datas.KeyPadData.KeyPadDataMap[code];

            //Debug.Log(_stand);
            if (_stand != KeyCode.None)
            {
                if (_stand == KeyCode.Space)
                {
                    tmpro.text = "";
                    Stand = KeyCode.Space;
                }
                else
                {
                    //Debug.Log(_stand);
                    tmpro.text = _stand.ToString();
                    Stand = _stand;
                    //Stand = (KeyCode)97 - 'A' + keypad.stand[0]; //알파벳을 키코드로 바꿈
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
        infoPos = keycapInfo.GetComponent<InfoPos>();

        SetKeyPad(keyPadCode, Stand);

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        //아이템 키일 경우 게임 재시작 시 비활성화
        GameState.onReset += delegate {
            if (gameObject.layer ==13)
            {
                gameObject.SetActive(false);
            } };
    }

    private void OnEnable()
    {
        enableTime = Time.time;

        x = Random.Range(-1, 2);
        y = Random.Range(-1, 2);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (EditorApplication.isPlaying)
        {
            SetKeyPad(keyPadCode, Stand);
        }
#endif
        //30초지나면 아이템 사라짐
        if (gameObject.layer == 13 && Time.time > enableTime + 30f)
        {
            gameObject.SetActive(false);
        }
        if (m_Master == null)
        {

            rb.velocity = new Vector2(x, y);
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
        if (keypad != null)
        {
            if (keypad.autoFire && Shootable())
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
        if (1 << collision.gameObject.layer == LayerMask.GetMask("DroppedKey"))
        {
            if (m_Master.TryGetComponent(out PlayerController _pc))
            {

                SoundManager.PlaySfx(SoundManager.GetSoundFx("KeyboardCollect"));
                if (collision.gameObject.TryGetComponent(out KeyCap _cap))
                {
                    _pc.virtualKey.GainTmpKey(_cap);
                    _pc.virtualKey.SetVirtualKeys(m_Master.keyPads.Keys.ToList());
                    _pc.virtualKey.SetKeysActive(true);
                }
                collision.gameObject.SetActive(false);
            }
        }
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("DroppedKey"))
        {
            if (m_Master.TryGetComponent(out PlayerController _pc))
            {

                SoundManager.PlaySfx(SoundManager.GetSoundFx("KeyboardCollect"));
                if (collision.gameObject.TryGetComponent(out KeyCap _cap))
                {
                    _pc.virtualKey.GainTmpKey(_cap);
                    _pc.virtualKey.SetVirtualKeys(m_Master.keyPads.Keys.ToList());
                    _pc.virtualKey.SetKeysActive(true);
                }
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        
        
        if (GameState.current != GameStateType.Shopping)
        {
            return;
        }
        if (!Store.isSelling)
        {
            return;
        }
           
        if (m_Master != null)
        {
            if(Datas.GameData.GameDataList[1].intValue > 0) // 구매 아이템 선택시
            {
                SetKeyPad(Datas.GameData.GameDataList[2].strValue);
                print("강화");
                Datas.GameData.GameDataList[0].intValue -= Datas.GameData.GameDataList[1].intValue;
                Datas.GameData.GameDataList[1].intValue = 0;
                GameObject cancelBuyButton = GameObject.Find("CancelBuyButton");
                cancelBuyButton.SetActive(false);

                keycapInfo.transform.GetChild(0).gameObject.SetActive(true);
                {
                    keycapInfo.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = Datas.KeyPadData.KeyPadDataMap[keyPadCode].Description;
                    keycapInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(Datas.KeyPadData.KeyPadDataMap[keyPadCode].sprite);
                }
            }
            else if (Stand != KeyCode.Space)
            {
                Debug.Log("분해");
                //Debug.Log("crashed!");
                Datas.GameData.GameDataList[0].intValue += getCost;
                Vector2Int _pos = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
                m_Master.RemoveKeyPad(_pos);
                keycapInfo.transform.GetChild(0).gameObject.SetActive(false);

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
                keycapInfo.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = Datas.KeyPadData.KeyPadDataMap[keyPadCode].Description;
                keycapInfo.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(Datas.KeyPadData.KeyPadDataMap[keyPadCode].sprite);
            }
        }
    }

    private void OnMouseOver()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // keycapInfo.transform.position = worldPosition;
        infoPos.SetPos(worldPosition);
    }

    private void OnMouseExit()
    {
        keycapInfo.transform.GetChild(0).gameObject.SetActive(false);
    }
}
