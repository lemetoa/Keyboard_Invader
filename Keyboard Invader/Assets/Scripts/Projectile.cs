using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Datas.Projectile projCode;
    private float velocity;
    private float duration;
    public float damage { get; private set; }
    public float life;//탄 내구도

    private string onEnd;

    public SpriteRenderer sr;
    [SerializeField]
    private BoxCollider2D box2d;

    public void SetCode(string code)
    {
        if (!Datas.Projectile.ProjectileMap.ContainsKey(code))
        {
            Debug.Log(code);
            return;
        }
        projCode = Datas.Projectile.ProjectileMap[code];
        velocity = projCode.velocity;
        duration = projCode.duration;
        damage = projCode.damege;
        life = projCode.penetration;

        onEnd = projCode.onEnd;
        float angle = Mathf.Atan2(transform.position.x, transform.position.y) * Mathf.Rad2Deg - 90f;
        sr.sprite = ProjectileManager.GetSprite(code);
        //Debug.Log(code +" = "+ProjectileManager.GetSprite(code));
        //히트박스 크기변경
        box2d.size = projCode.size;

        //이미지 크기변경
        if (projCode.renderSize == Vector2.one)
        {
            sr.drawMode = SpriteDrawMode.Simple;
            sr.transform.localScale = Vector2.one;
        }
        else
        {

            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = projCode.renderSize;
            sr.transform.localScale = Vector2.one;
        }
    }

    private void EndShoot()
    {
       // if (projCode != null) Debug.Log(projCode.onEnd[0]);

        if (projCode !=null && projCode.onEnd[0] =='1')     //종료시에 호출할 슈터가 있다면
        { 
            ProjectileManager.Shoot(transform, projCode.onEnd, gameObject.layer);
        }
    }

    private void Awake()
    {
        GameState.onReset += delegate { gameObject.SetActive(false); };
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up *Time.fixedDeltaTime* velocity);
        duration -= Time.fixedDeltaTime;
        if (duration <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        EndShoot();
        ObjectPooler.ReturnToPool(gameObject, ObjectPooler.PoolingType.Projectile);    // 한 객체에 한번만 
        CancelInvoke();    // Monobehaviour에 Invoke가 있다면 
    }


}
