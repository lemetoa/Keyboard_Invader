using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CopyRight : MonoBehaviour, IPointerClickHandler,IPointerExitHandler, IPointerEnterHandler
{

    public TextMeshProUGUI self;
    public GameObject Credit;
    private Color baseColor;
    public void OnPointerClick(PointerEventData eventData)
    {
        Credit.SetActive(true);
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        self.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        self.color = baseColor;
        Credit.SetActive(false);
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<TextMeshProUGUI>();
        baseColor = self.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
