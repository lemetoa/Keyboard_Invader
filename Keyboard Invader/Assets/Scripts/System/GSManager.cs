using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSManager : MonoBehaviour
{

    private void Awake()
    {
        //UnityGoogleSheet.Load<SkillDataTable.defaultData>();
        //UnityGoogleSheet.Load<SkillDataTable.RangeData>();
        UnityGoogleSheet.LoadAllData();
    }

}
