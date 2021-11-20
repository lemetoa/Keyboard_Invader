using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class Unit : MonoBehaviour
{
    FollowCamera cam;

    [SerializeField]
    private float moveSpeed;

    private bool isMoving = false;
    public void SetMove(bool moving)
    {
        isMoving = moving;
    }

    private Vector2 targetPos;
    
    [SerializeField]
    GameObject keyParticle;

    public void SetTarget(Vector2 _vector)
    {
        targetPos = _vector;
    }

    public bool dying;

    //보유중인 키패드와 위치
    public Dictionary<Vector2Int, KeyCap> keyPads = new Dictionary<Vector2Int, KeyCap>();
    
    public void AddKeyCap(Vector2Int _pos, string keyCode,KeyCode _stand = KeyCode.None)
    {
        //해당위치에 키캡이 없으면 추가
        if (!keyPads.ContainsKey(_pos))
        {
            var newkey = ObjectPooler.SpawnFromPool<KeyCap>(ObjectPooler.PoolingType.KeyCap, transform.position);
          
            newkey.transform.SetParent(transform);
            newkey.transform.localPosition = (Vector2)_pos;
            newkey.transform.localRotation = Quaternion.identity;
            newkey.ChangeStand(_stand);

            newkey.gameObject.layer = gameObject.layer;
            keyPads.Add(_pos, newkey);
            newkey.m_Master = this;
        }

        keyPads[_pos].SetKeyPad(keyCode,_stand);
        //keyPads[_pos].sta
        UpdateStands();
    }

    public delegate void OnKeyPadRemove();
    public OnKeyPadRemove onKeyPadRemove;

    public delegate void OnDeath();
    public OnDeath onDeath = new OnDeath(delegate {
       // Debug.Log("Core destroyed!");
    });
    

    //키패드 제거
    public void RemoveKeyPad(Vector2Int position)
    {
       // Debug.Log(position);
        if (keyPads.ContainsKey(position))
        {
            KeyCode _stand = keyPads[position].Stand;
            keyPads[position].gameObject.SetActive(false);
            keyPads[position].transform.SetParent(null);
            keyPads[position].m_Master = null;
            
            keyPads.Remove(position);
            onKeyPadRemove.Invoke();

            if (position == Vector2Int.zero)//파괴된 곳이 코어면
            {
                StartCoroutine(DeadEffect());   
            }
            else
            {
                var Obj = Instantiate(keyParticle, this.transform.position, Quaternion.identity);
                
            }
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
            UpdateStands();
        }
    }



    //코어랑 붙어있는가?
    private bool IsConectedtoCore(Vector2Int _pos)
    {
        if (_pos == Vector2Int.zero)
        {
            return true;
        }    
        //0,0에 키가 없으면 무조건 코어와 떨어진걸로 판정
        if (!keyPads.ContainsKey(Vector2Int.zero)) 
        {
            return false;
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

        } while (OpenList.Count != 0);

        return false;
    }

       //상호작용 가능한 버튼들
    public List<KeyCode> stands = new List<KeyCode>();

    //보유중인 키패드와 상호작용 가능한 버튼들 찾기
    private void UpdateStands()
    {
        //string sum = "";
        stands.Clear();
        foreach (var item in keyPads.Values.ToArray())
        {
            //sum += item.Stand;
            if (!stands.Contains(item.Stand))
            {
                stands.Add(item.Stand);
            }
        }

        if (cam != null)
        {
            float camSize = stands.Count + cam.startSize - 1;
            if (camSize <= cam.maxSize)
                cam.currSize = camSize;

        }
        //Debug.Log(sum);
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

    private void OnEnable()
    {
        dying = false;
    }

    IEnumerator DeadEffect()
    {
        //Debug.Log("죽음");
        dying = true;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        var Obj = Instantiate(keyParticle, this.transform.position, Quaternion.identity);
        SoundManager.PlaySfx(SoundManager.GetSoundFx("PlayerExp"));
        yield return new WaitForSeconds(3f);
        
        onDeath.Invoke();
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            UpdateStands();
        }
#endif
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position += (Vector3)(targetPos - (Vector2)transform.position).normalized * moveSpeed * Time.fixedDeltaTime;
        }
    }
}
