using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.ComponentModel;

public class Reinforce_Grid : MonoBehaviour
{
    public enum Type
    {
        Crystal,
        Laser,
        Shotgun,
        Double,
        
    }

    public Type type;
    public int price;
    Text txt;
    Button b;
    
    Text money;
    Text description;
    Image image;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private GameObject cancelBuyButton;
    string keyPadStr;
    void Start()
    {
        b = GetComponent<Button>(); 
        b.onClick.AddListener(() => Reinforce());
        
        money = GameObject.Find("CostAmount").GetComponent<Text>();
        money.text = Datas.GameData.GameDataList[0].intValue.ToString();
        txt = transform.GetChild(0).GetComponent<Text>();
        txt.text = price.ToString();
        image = transform.parent.GetChild(0).GetComponent<Image>();
        image.sprite = sprite;
       // cancelBuyButton = transform.parent.parent.parent.GetChild(2).gameObject;

        if (type == Type.Crystal)
        {
            keyPadStr = "1C1";
        }
        else if (type == Type.Laser)
        {
            keyPadStr = "1L1";
        }
        else if (type == Type.Shotgun)
        {
            keyPadStr = "1S1";
        }
        else if (type == Type.Double)
        {
            keyPadStr = "1D2";
        }

        description = transform.parent.GetChild(1).GetComponent<Text>();
        description.text = Datas.KeyPadData.KeyPadDataMap[keyPadStr].Description;
        image.sprite = Resources.Load<Sprite>(Datas.KeyPadData.KeyPadDataMap[keyPadStr].sprite);
    }

    public void Reinforce()
    {
        if (price <= Datas.GameData.GameDataList[0].intValue)
        {
            // SoundManager.instance.PlaySE(SoundManager.instance.reinforce);

            Datas.GameData.GameDataList[1].intValue = price;
            money.text = Datas.GameData.GameDataList[0].intValue.ToString();
            cancelBuyButton.SetActive(true);

            Datas.GameData.GameDataList[2].strValue = keyPadStr;
            //Debug.Log("str값 " + Datas.GameData.GameDataList[2].strValue);

            print($"데이터 ={Datas.KeyPadData.KeyPadDataMap[keyPadStr].Description} {gameObject} " );

            //Debug.Log("데이터 ="  + Datas.KeyPadData.KeyPadDataMap[keyPadStr].Description);
            // Datas.GameData.GameDataList[2].strValue 참조해 능력 부여

        }
    }
}
