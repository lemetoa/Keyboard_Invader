using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.ComponentModel;

public class Reinforce_Grid : MonoBehaviour
{
    public int[] upPer;
    public enum Type
    {
        Bubble,
        Crystal,
        Dice,
        Laser,
        Zip,
        Shotgun,
        Double,
        
    }

    public Type type;
    // public GameObject upObj;
    public int price;
    Text txt;
    // [HideInInspector]
    // public Text costTxt;
    Button b;
    
    Text money;
    Text description;
    [SerializeField]
    private string descTxt;
    Image image;
    [SerializeField]
    private Sprite sprite;
    GameObject cancelBuyButton;
    void Start()
    {
        b = GetComponent<Button>(); 
        b.onClick.AddListener(() => Reinforce());
        
        money = GameObject.Find("CostAmount").GetComponent<Text>();
        money.text = Datas.GameData.GameDataList[0].intValue.ToString();
        txt = transform.GetChild(0).GetComponent<Text>();
        txt.text = price.ToString();
        description = transform.parent.GetChild(1).GetComponent<Text>();
        description.text = descTxt;
        image = transform.parent.GetChild(0).GetComponent<Image>();
        image.sprite = sprite;
        cancelBuyButton = transform.parent.parent.parent.GetChild(2).gameObject;
        // Refresh();
        /*
        if (type == Type.Skill1)
            amt = upObj.GetComponent<SkillMgr>().Time_shield;
        else if (type == Type.Skill2)
        {
            amt = upObj.GetComponent<SkillDam>().DamageAmt;

        }
        else if (type == Type.Skill3)
        {
            amt = upObj.transform.GetChild(0).GetComponent<SkillDam>().DamageAmt;
        }
        */


    }

    public void Reinforce()
    {
        if (price <= Datas.GameData.GameDataList[0].intValue)
        {
            // SoundManager.instance.PlaySE(SoundManager.instance.reinforce);

            Datas.GameData.GameDataList[1].intValue = price;
            money.text = Datas.GameData.GameDataList[0].intValue.ToString();
            cancelBuyButton.SetActive(true);

            string str = null;

            if (type == Type.Bubble)
            {
                str = "1B1";
            }
            else if (type == Type.Crystal)
            {
                str = "1C1";
            }
            else if (type == Type.Dice)
            {
                str = "1D1";
            }
            else if (type == Type.Laser)
            {
                str = "1L1";
            }
            else if (type == Type.Zip)
            {
                str = "1Z1";
            }
            else if (type == Type.Shotgun)
            {
                str = "1S1";
            }
            else if (type == Type.Double)
            {
                str = "1D1";
            }
            Datas.GameData.GameDataList[2].strValue = str;
            Debug.Log("str값 " + Datas.GameData.GameDataList[2].strValue);
            // Datas.GameData.GameDataList[2].strValue 참조해 능력 부여

            // if (type == Type.Unit)
            //     upObj.GetComponent<Unit>().Upgrade(upPer[currTier]);

            /*
            if (type == Type.Skill1)
                upObj.GetComponent<SkillMgr>().Time_shield = amt;
            else if (type == Type.Skill2)
            {
                upObj.GetComponent<SkillDam>().DamageAmt = (int)amt;

            }
            else if (type == Type.Skill3)
            {
                upObj.transform.GetChild(0).GetComponent<SkillDam>().DamageAmt = (int)amt;
            }
            
            if (price.Length - 1 < currTier)
            {
                txt.text = "Max";
                b.onClick.RemoveAllListeners();
                return;
            } 
            Refresh();
            */
        }
    }

    /*
    public void Refresh()
    {
        txt.text = price[currTier].ToString();
        
    }

    public void Save()
    {
        PlayerPrefs.SetInt(this.gameObject.name, currTier);
    }
    */
}
