using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decomposition : MonoBehaviour
{
    private int getCost = 200;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (GameState.current != GameStateType.Shopping)
        {
            return;
        }

        Datas.GameData.GameDataList[0].intValue += getCost;
        this.gameObject.SetActive(false);
    }
}
