using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;
    public static ProjectileManager Instance()
    {
        if (instance ==null)
        {
            instance = FindObjectOfType<ProjectileManager>();
        }
        if (instance == null)
        {
            var obj = new GameObject("Projectile Manager");
            obj.AddComponent<ProjectileManager>();
        }
        return instance;
    }

    public SpriteData sprites;
    public static Sprite GetSprite(string code)
    {
        return instance.sprites.Getsprite(code);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    /*
    public static void Shoot(Transform _transform, string shootCode)
    {
        Shoot(_transform, shootCode, 0);
    }*/
    public static void Shoot(Transform _transform, string shootCode,  int _layer)
    {
        var shooter = Datas.Shooter.ShooterMap[shootCode];
        if (shooter ==null)
        {
            return;
        }

        //사운드
        SoundManager.PlaySfx(SoundManager.GetSoundFx(shooter.audio));
        for (int i = 0; i < shooter.multiShot; i++)
        {
            var proj = ObjectPooler.SpawnFromPool<Projectile>(ObjectPooler.PoolingType.Projectile, _transform.position,true);

            //8,9면 9 10,11이면 11
            if (_layer == 8 || _layer == 9)
            {
                proj.gameObject.layer = 9;
            }
            else if(_layer == 10 || _layer == 11)
            {
                proj.gameObject.layer = 11;
            }
            //투사체 선택
            int _count = 0;
            if (shooter.randomShot)
            {
                //랜덤이면
                _count = Random.Range(0, shooter.projectiles.Count - 1);

            }
            else
            {
                // _count = shooter.projectiles.Count % 1;
                _count = 0;
            }
            
            proj.SetCode(shooter.projectiles[_count]);


            //각도 설정
            proj.transform.rotation = _transform.rotation;
            if (shooter.randomSpread)
            {
                proj.transform.rotation = _transform.rotation;
            }
            else
            {
                float angle = 0;
                if (shooter.multiShot != 1)
                {
                    angle = i * shooter.spread / (shooter.multiShot - 1) - shooter.spread / 2f + shooter.accuracy;
                }
                //Debug.Log(angle);

                proj.transform.Rotate(0, 0, proj.transform.rotation.z + angle);
            }

            //위치 설정
            proj.transform.Translate(shooter.positionOffset);

            //자식설정
            if (shooter.isChild)
            {
                proj.transform.SetParent(_transform);
            }
            else
            {
                proj.transform.SetParent(null);
            }
            //완료
            proj.gameObject.SetActive(true);
        }
        //Datas.Shooter.ShooterMap[shootCode].projectiles;
    }
}
