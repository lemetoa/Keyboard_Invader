using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new shooter", menuName = "Scriptable/Shooter")]
public class ShooterData : ScriptableObject
{
    public int indexCode; //자신코드
    public KeyStand stand;

    public int[] projectiles; //발사할 투사체들 코드

    public float fireSpeed; //발사속도
    public float spread; //탄퍼짐
    public float multiShot; //멀티샷
    public bool autoShot = true;
}
