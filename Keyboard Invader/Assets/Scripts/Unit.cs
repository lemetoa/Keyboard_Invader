﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private bool isMoving = false;
    public void SetMove(bool moving)
    {
        isMoving = moving;
    }

    private Vector2 targetPos;
    
    public void SetTarget(Vector2 _vector)
    {
        targetPos = _vector;
    }


    //보유중인 키패드와 위치
    public Dictionary<Vector2Int, KeyCap> keyPads = new Dictionary<Vector2Int, KeyCap>();
    
    public void AddKeyCap(Vector2Int _pos, string keyCode)
    {
        //해당위치에 키캡이 없으면 추가
        if (!keyPads.ContainsKey(_pos))
        {
            var newkey = ObjectPooler.SpawnFromPool<KeyCap>(ObjectPooler.PoolingType.KeyCap, transform.position);
            newkey.transform.SetParent(transform);
            newkey.transform.localPosition = (Vector2)_pos;
            newkey.transform.localRotation = Quaternion.identity;

            newkey.gameObject.layer = gameObject.layer;
            keyPads.Add(_pos, newkey);
            newkey.m_Master = this;
        }

        keyPads[_pos].SetKeyPad(keyCode);
        UpdateStands();
    }

    public delegate void OnKeyPadRemove();
    public OnKeyPadRemove onKeyPadRemove;


    //키패드 제거
    public void RemoveKeyPad(Vector2Int position)
    {
       // Debug.Log(position);
        if (keyPads.ContainsKey(position))
        {
            keyPads[position].gameObject.SetActive(false);
            keyPads[position].transform.SetParent(null);
            keyPads[position].m_Master = null;
            
            keyPads.Remove(position);
            onKeyPadRemove.Invoke();

            Vector2Int connected = position + Vector2Int.up;
            if (!IsConectedtoCore(connected))
            {
                RemoveKeyPad(position + Vector2Int.up);
            }
            connected = position + Vector2Int.down;
            if (!IsConectedtoCore(connected))
            {
                RemoveKeyPad(connected);
            }
            connected = position + Vector2Int.left;
            if (!IsConectedtoCore(connected))
            {
                RemoveKeyPad(connected);
            }
            connected = position + Vector2Int.right;
            if (!IsConectedtoCore(connected))
            {
                RemoveKeyPad(connected);
            }
        }
    }

    //코어랑 붙어있는가?
    private bool IsConectedtoCore(Vector2Int _pos)
    {
        if (_pos == Vector2Int.zero)
        {
            return true;
        }
        List<Vector2Int> closedPos = new List<Vector2Int>();
        closedPos.Add(Vector2Int.zero);
        List<Vector2Int> OpenList = new List<Vector2Int>();
        Vector2Int curPos = Vector2Int.zero;
        OpenList.Add(curPos);

        Vector2Int checking;
        do
        {

            curPos = OpenList[0];
            if (curPos == _pos)
                return true;
            closedPos.Add(curPos);
            OpenList.Remove(curPos);

            checking = curPos + Vector2Int.up;
            if (!closedPos.Contains(checking) && keyPads.ContainsKey(checking))    //확인 안했고 길 맞으면
            {
                if (checking == _pos)
                    return true;
                OpenList.Add(checking);
                //Debug.Log(checking);
            }
            else
                closedPos.Add(checking);
            checking = curPos + Vector2Int.down;
            if (!closedPos.Contains(checking) && keyPads.ContainsKey(checking))    //확인 안했고 길 맞으면
            {
                if (checking == _pos) 
                    return true;
                OpenList.Add(checking);// Debug.Log(checking);
            }
            else
                closedPos.Add(checking);
            checking = curPos + Vector2Int.left;
            if (!closedPos.Contains(checking) && keyPads.ContainsKey(checking))    //확인 안했고 길 맞으면
            {
                if (checking == _pos)   
                    return true;
                OpenList.Add(checking); //Debug.Log(checking);
            }
            else
                closedPos.Add(checking);
            checking = curPos + Vector2Int.right;
            if (!closedPos.Contains(checking) && keyPads.ContainsKey(checking))    //확인 안했고 길 맞으면
            {
                if (checking == _pos) 
                    return true;
                OpenList.Add(checking); //Debug.Log(checking);
            }
            else
                closedPos.Add(checking);

            string asdf = "";
            foreach (var item in closedPos)
            {
                asdf += item;
            }
           // Debug.Log(asdf);
        } while (OpenList.Count != 0);

        return false;
    }

       //상호작용 가능한 버튼들
    public List<KeyCode> stands = new List<KeyCode>();

    //보유중인 키패드와 상호작용 가능한 버튼들 찾기
    private void UpdateStands()
    {
        foreach (var item in keyPads.Values.ToArray())
        {
            if (!stands.Contains(item.Stand))
            {
                stands.Add(item.Stand);
            } 
        }
    }


    //특정 알파벳에 해당하는 키패드들 반환하기
    public List<KeyCap> GetKeybyStand(KeyCode _key)
    {
        var result = new List<KeyCap>();
        foreach (var item in keyPads.Values.ToArray())
        {
            if (item.Stand == _key)
            {
                result.Add(item);
            }
        }
        return result;

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
