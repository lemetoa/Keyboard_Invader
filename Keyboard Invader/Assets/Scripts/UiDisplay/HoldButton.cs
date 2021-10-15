using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float duration = 6;
    private float timer = 0;
    [SerializeField]
    Image image;
    bool isClicked;
    public Button btn;
    

    private void Update()
    {

        if (isClicked)
        {
            timer += Time.unscaledDeltaTime;
        }
        else // Do not forget to reset timer when the button is not pressed anymore
        {
            timer = 0;
        }
        
        if (timer > duration)
        {
            isClicked = false;
            timer = 0;
            btn.onClick.Invoke();
        }
        
        image.fillAmount = timer / duration;
        // Debug.Log("이벤트click " + isClicked);
        // Debug.Log("이벤트timer " + timer);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("이벤트down");
        isClicked = true;
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("이벤트up");
        isClicked = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("이벤트exit");
        isClicked = false;
    }
    
}
