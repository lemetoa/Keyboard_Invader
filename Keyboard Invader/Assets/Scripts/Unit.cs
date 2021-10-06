using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //보유중인 키패드와 위치
    public Dictionary<Vector2Int, KeyPad> keyPads = new Dictionary<Vector2Int, KeyPad>();

    //위치에 키패드 추가 및 변경
    public void AddKeyPad(Vector2Int position, Datas.KeyPadData keyPad)
    {

    }

    //키패드 제거
    public void RemoveKeyPad(Vector2Int position)
    {

    }
       //상호작용 가능한 버튼들
    public List<KeyCode> stands = new List<KeyCode>();

    //보유중인 키패드들로 상호작용 가능한 버튼들 찾기
    public void SetStands()
    {
        stands.Clear();
        foreach (var key in keyPads.Values)
        {

        }
    }

    //특정 알파벳에 해당하는 키패드들 반환하기
    private List<KeyPad> GetKeybyStand(char Stand)
    {
        return new List<KeyPad>();
    }

    //특정 키가 눌렸을때
    
    //특정 키를 누르고 있을 때

    //특정 키를 땠을 때


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
