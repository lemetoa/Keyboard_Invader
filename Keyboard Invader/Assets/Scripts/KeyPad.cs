using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    public Datas.KeyPadData keypad { get; protected set; }     //키패드 속성
    public void SetKeyPad(string code)//키패드 속성 변경
    {

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
            ProjectileManager.Shoot(transform, keypad.onFirstShot);
        }
    }
    //누르고있을때
    public void OnUse()
    {

    }
    //손가락 땠을 때 
    public void OnUseEnd()
    {

    }

}
