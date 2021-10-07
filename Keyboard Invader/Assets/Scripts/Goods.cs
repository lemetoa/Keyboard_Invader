using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Goods : MonoBehaviour
{
    //상점의 상품 하나
    public Button button;
    private TextMeshPro text;

    //키캡 구매후 드로우
    public static void ClickedCap()
    {
        Debug.Log("cap buyed");
    }

    public static Goods selectedGoods;

    public Datas.KeyPadData keypad;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(delegate {
            selectedGoods = this;
            Debug.Log("goods clicked");
        });
    }

    //상품 설정
    public void SetGoods(string Stand, string _keyPad, int price)
    {
        if (Datas.KeyPadData.KeyPadDataMap.ContainsKey(_keyPad))
        {
            keypad = Datas.KeyPadData.KeyPadDataMap[_keyPad];
        }

        text.text = price.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
