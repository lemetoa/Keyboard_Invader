using System.Collections;
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

    // private GameObject store;

    [SerializeField]
    GameObject keyParticle;

    bool nextPhase;
    void Start()
    {
        GameState.onReset += Disable;
        //생명 게이지의 생성 및 초기화
        /*
        store = GameObject.Find("StoreObj");
        if(store !=null)
            store.gameObject.SetActive(false);*/
        slider = EnemySpawner.GetSlider();
        slider.value = 1;
        slider.gameObject.SetActive(true);
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        KeyStand keystand = (KeyStand)Random.Range(1, 26);

        keypad = Datas.KeyPadData.KeyPadDataMap["enemy"];
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
        if (Store.Portal() !=null)
        {
            Store.Portal().transform.position = this.transform.position;
            Store.Portal().gameObject.SetActive(true);
        }
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
        if (currentLife<=0f)
        {
            return;
        }
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
    
    IEnumerator DeathEffect()
    {
        for (int i = 0; i < 10; i++)
        {
            int x = Random.Range(-1, 2);
            int y = Random.Range(-1, 2);
            var Obj = Instantiate(keyParticle, transform.position + new Vector3(x, y), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);

        }
        Disable();
    }

    void ArmBreak()
    {
        for (int i = 0; i < arms.Length; i++)
        {
            var Obj = Instantiate(keyParticle, arms[i].transform.position, Quaternion.identity);

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
                if (!nextPhase)
                {
                    ArmBreak();
                }
                nextPhase = true;
                for (int i = 0; i < arms.Length; i++)
                {
                    arms[i].gameObject.SetActive(false);
                }
            }
            // hpBarImage.fillAmount = currentLife / life;

            if (currentLife <= 0 && !EnemySpawner.bossKilled)
            {

                Score.AddScore(1000f);
                GameResult.bossDestroyed++;
                EnemySpawner.bossKilled = true;
                StartCoroutine(DeathEffect());
                
            }
        }
    }
    

}
