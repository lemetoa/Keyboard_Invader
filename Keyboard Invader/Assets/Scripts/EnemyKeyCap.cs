﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyKeyCap : MonoBehaviour
{
    public Datas.KeyPadData keypad { get; protected set; }

    [SerializeField]
    protected TextMeshPro tmpro;

    [SerializeField]
    private enum Type
    {
        Normal,
        Rotate
    }

    [SerializeField]
    Type Enemy;

    Rigidbody2D rb;
    Transform target;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private float life;
    private float currentLife;
    [SerializeField]
    private SpriteRenderer outLine;

    //생명 게이지 프리팹을 저장할 변수
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    //부모가 될 Canvas 객체
    private Canvas uiCanvas;
    //생명 수치에 따라 fillAmount 속성을 변경할 Image
    private Image hpBarImage;
    GameObject hpBar;
    void Start()
    {
        GameState.onReset += Disable;
        //생명 게이지의 생성 및 초기화
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        //overlayCanvas = GameObject.Find("UICanvas_Overlay").GetComponent<Canvas>();
        SetHpBar(uiCanvas);

        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        KeyStand keystand = (KeyStand)Random.Range(1, 26);
        tmpro.text = keystand.ToString();

        keypad = Datas.KeyPadData.KeyPadDataMap["0"];
        currentLife = life;
    }
    private void OnEnable()
    {
        //타입 무작위로 설정
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            outLine.color = Color.blue;
            Enemy = Type.Normal;
        }
        else
        {
            outLine.color = Color.red;
            Enemy = Type.Rotate;
        }
        if (uiCanvas != null)
            RestartHpBar(uiCanvas);
    }
    private void OnDisable()
    {

       // GameState.onReset -= delegate { Disable(); };
        // ObjectPooler.ReturnToPool(gameObject, ObjectPooler.PoolingType.Enemy);
    }

    protected virtual void Disable()
    {
        currentLife = life;
        hpBar.SetActive(false);
        GameState.onReset -= Disable;
        if (gameObject !=null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);    //오브젝트풀링하면 자꾸 게임 멈춘다
        }

    }

    void SetHpBar(Canvas canvas)
    {
        hpBar = Instantiate(hpBarPrefab, canvas.transform);

        HpFollow();
    }

    void RestartHpBar(Canvas canvas)
    {
        hpBar = hpBarPrefab;
        for (int i = 0; i < canvas.transform.childCount - 1; i++)
        {
            if (!canvas.transform.GetChild(i).gameObject.activeInHierarchy && canvas.transform.GetChild(i).gameObject.name.Substring(0, 1) == this.gameObject.tag.Substring(0, 1))
            {
                hpBar = canvas.transform.GetChild(i).gameObject;
                hpBarImage.fillAmount = 1;
                
                break;
            }
            else if (i + 1 >= canvas.transform.childCount - 1)
            {
                hpBar = Instantiate(hpBarPrefab, canvas.transform);

            }
        }
        HpFollow();
        hpBar.SetActive(true);
    }

    void HpFollow()
    {
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[0];

        var _hpBar = hpBar.GetComponent<HpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;

    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy == Type.Normal)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.AngleAxis(angle - 90, Vector3.forward), Time.deltaTime * rotSpeed);


            Vector3 toTarg = (target.position - transform.position).normalized;
            rb.velocity = toTarg;
        }
        else
        {
            transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
            Vector3 toTarg = (target.position - transform.position).normalized;
            rb.velocity = toTarg;
        }

        OnUseStart();

    }
    private float nextShotTime = 0; //다음 발사 가능한 시간
    private bool Shootable()
    {
        if (Time.time >= nextShotTime)
        {
            return true;
        }
        return false;
    }

    //누르기 시작할때
    public virtual void OnUseStart()
    {
        if (Shootable())
        {
            if (keypad != null)
            {
                ProjectileManager.Shoot(transform, keypad.onFirstShot, gameObject.layer);
                nextShotTime = Time.time + 1f / keypad.fireRate;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("PlayerProjectile"))
        {
            var _projectile = collision.GetComponent<Projectile>();
            _projectile.life -= 1f;
            if (_projectile.life<=0)
            {
                collision.gameObject.SetActive(false);
            }

            currentLife -= _projectile.damage;

            hpBarImage.fillAmount = currentLife / life;

            if (currentLife <= 0)
            {
                KeyCap key = ObjectPooler.SpawnFromPool<KeyCap>(ObjectPooler.PoolingType.KeyCap, transform.position, true);
                key.SetKeyPad("1L1", KeyCode.L);
                key.gameObject.layer = 13;
                Score.AddScore(100f);
                GameResult.enemyDestroyed++;
                hpBarImage.transform.parent.gameObject.SetActive(false);
                Disable();
            }
        }
    }
    

}
