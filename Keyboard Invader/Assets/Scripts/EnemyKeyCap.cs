using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyKeyCap : MonoBehaviour
{
    public Datas.KeyPadData keypad { get; protected set; }

    [SerializeField]
    protected TextMeshPro tmpro;

    Collider2D coll;

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

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        KeyStand keystand = (KeyStand)Random.Range(1, 26);
        tmpro.text = keystand.ToString();

        keypad = Datas.KeyPadData.KeyPadDataMap["0"];
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy == Type.Normal)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            Vector3 toTarg = (target.position - transform.position).normalized;
            rb.velocity = toTarg;
        }
        else
        {
            transform.Rotate(0, 0, 50 * Time.deltaTime);
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
                ProjectileManager.Shoot(transform, keypad.onFirstShot,gameObject.layer);
                nextShotTime = Time.time + 1f / keypad.fireRate;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("PlayerProjectile"))
        {
            
        }
    }
}
