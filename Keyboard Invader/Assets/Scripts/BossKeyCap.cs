﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossKeyCap : MonoBehaviour
{
    public Datas.KeyPadData keypad { get; protected set; }

    Rigidbody2D rb;
    Transform target;
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private float life;
    private float currentLife;

    private Slider slider;

    [SerializeField]
    private Transform[] arms;

    [SerializeField]
    private Transform nextPhaseArms;

    private GameObject store;

    bool nextPhase;
    void Start()
    {
        GameState.onReset += Disable;
        //생명 게이지의 생성 및 초기화
        store = GameObject.Find("StoreObj");
        store.gameObject.SetActive(false);
        slider = GameObject.Find("MainCanvas").transform.GetChild(7).GetComponent<Slider>();
        slider.value = 1;
        slider.gameObject.SetActive(true);
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        KeyStand keystand = (KeyStand)Random.Range(1, 26);

        keypad = Datas.KeyPadData.KeyPadDataMap["0"];
        currentLife = life;
    }

    private void OnDisable()
    {

       // GameState.onReset -= delegate { Disable(); };
        // ObjectPooler.ReturnToPool(gameObject, ObjectPooler.PoolingType.Enemy);
    }

    protected virtual void Disable()
    {
        currentLife = life;
        slider.gameObject.SetActive(false);
        store.transform.position = this.transform.position;
        store.gameObject.SetActive(true);
        GameState.onReset -= Disable;
        if (gameObject !=null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);    //오브젝트풀링하면 자꾸 게임 멈춘다
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!nextPhase)
        {
            Phase_One();
        }
        else
        {
            Phase_Two();
        }
        
        Vector3 toTarg = (target.position - transform.position).normalized;
        rb.velocity = toTarg;
        
        
        OnUseStart();

    }

    void Phase_One()
    {
        for (int i = 0; i < arms.Length; i++)
        {
            Vector3 dir = target.position - arms[i].position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            arms[i].rotation = Quaternion.Slerp(arms[i].localRotation, Quaternion.AngleAxis(angle - 270, Vector3.forward), Time.deltaTime * rotSpeed);

        }
    }

    void Phase_Two()
    {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
    }

    void Phase_One_Shoot()
    {
        for (int i = 0; i < arms.Length; i++)
        {
            ProjectileManager.Shoot(arms[i].GetChild(1), keypad.onFirstShot, gameObject.layer);
            nextShotTime = Time.time + 1f / keypad.fireRate;
        }
    }

    void Phase_Two_Shoot()
    {
        for (int i = 0; i < nextPhaseArms.childCount; i++)
        {
            ProjectileManager.Shoot(nextPhaseArms.GetChild(i), keypad.onFirstShot, gameObject.layer);
            nextShotTime = Time.time + 1f / keypad.fireRate;
        }
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
            if (!nextPhase)
                Phase_One_Shoot();
            else
                Phase_Two_Shoot();
            
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
            slider.value = currentLife / life;

            if(slider.value <= 0.5)
            {
                nextPhase = true;
                for (int i = 0; i < arms.Length; i++)
                {
                    arms[i].gameObject.SetActive(false);
                }
            }
            // hpBarImage.fillAmount = currentLife / life;

            if (currentLife <= 0)
            {

                Score.AddScore(1000f);
                GameResult.enemyDestroyed++;

                Disable();
            }
        }
    }
    

}