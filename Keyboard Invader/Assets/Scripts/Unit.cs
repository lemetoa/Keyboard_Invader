using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private bool isMoving = false;

    private Vector2 targetPos;

    public void SetTarget(Transform transform) {
        SetTarget(transform.position);
    }
    public void SetTarget(Vector2 _vector)
    {
        targetPos = _vector;
    }

    public void SetMove(bool moving)
    {
        isMoving = moving;
    }

    //보유중인 키패드와 위치
    public Dictionary<Vector2Int, KeyCap> keyPads = new Dictionary<Vector2Int, KeyCap>();

    //위치에 키패드 추가 및 변경
    public void AddKeyPad(Vector2Int position, Datas.KeyPadData keyPad)
    {

    }
    public void AddKeyCap(Vector2Int _pos)
    {
        var newkey = ObjectPooler.SpawnFromPool<KeyCap>(ObjectPooler.PoolingType.KeyCap,transform.position,false);
        newkey.transform.SetParent(transform);
        newkey.transform.localPosition = (Vector2)_pos;
        newkey.gameObject.SetActive(true);
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
    private List<KeyCap> GetKeybyStand(char Stand)
    {
        return new List<KeyCap>();
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

    private void FixedUpdate()
    {
        if (isMoving)
        {

            transform.position += (Vector3)(targetPos - (Vector2)transform.position).normalized * moveSpeed * Time.fixedDeltaTime;
        }
    }
}
