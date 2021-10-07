using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VirtualKey : MonoBehaviour
{
    private static GameObject spot;
    public static GameObject Spot()
    {
        if (spot == null)
        {
            spot = new GameObject("spot");
        }
        return spot;
    }
    private static Camera _camera;

    private Transform children;

    //public List<GameObject> virtualKey = new List<GameObject>();
    public string tmpStand;
    public string tmpKeyCode;      //임시로 획득한 키

    private Dictionary<Vector2Int, GameObject> virtualKeys = new Dictionary<Vector2Int, GameObject>();

    [SerializeField]
    private Unit myUnit;
    public void SetUnit(Unit _unit) { myUnit = _unit; }
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        if (children ==null)
        {
            children = new GameObject("keys").transform;
        }
        children.SetParent(transform);
        children.localPosition = Vector2.zero;

        myUnit.onKeyPadRemove += delegate { SetVirtualKeys(myUnit.keyPads.Keys.ToList()); };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
    //키들 활성화/비활성화
    public void SetKeysActive(bool active) {
        children.gameObject.SetActive(active);
    }

    public void GainTmpKey(KeyCap _key)
    {
        tmpStand = _key.Stand.ToString();
        tmpKeyCode = _key.keypad.index;
    }

    public void AddTmpZone(Vector2Int _pos)
    {
        var newKey = ObjectPooler.SpawnFromPool(ObjectPooler.PoolingType.VirtualKeyCap, transform.position);
        newKey.transform.SetParent(children);
        newKey.transform.localPosition = (Vector2)_pos;
        newKey.transform.localRotation = Quaternion.identity;
        virtualKeys.Add(_pos, newKey);
    }

    public void SetVirtualKeys(List<Vector2Int> keys )
    {
        foreach (var item in virtualKeys)
        {
            item.Value.SetActive(false);
        }
        virtualKeys.Clear();

        foreach (var item in keys)
        {
            //위
            if (!keys.Contains(item + Vector2Int.up) && !virtualKeys.ContainsKey(item+Vector2Int.up))
            {
                AddTmpZone(item + Vector2Int.up);
            }
            //왼
            if (!keys.Contains(item + Vector2Int.left) && !virtualKeys.ContainsKey(item + Vector2Int.left))
            {
                AddTmpZone(item + Vector2Int.left);
            }
            //오
            if (!keys.Contains(item + Vector2Int.right) && !virtualKeys.ContainsKey(item + Vector2Int.right))
            {
                AddTmpZone(item + Vector2Int.right);
            }
            //아래
            if (!keys.Contains(item + Vector2Int.down) && !virtualKeys.ContainsKey(item + Vector2Int.down))
            {
                AddTmpZone(item + Vector2Int.down);
            }
        }
    }
    private void OnMouseDown()
    {
        //마우스위치 산출
        var realMouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        var mousepos = realMouse - transform.position;

        Spot().transform.SetParent(transform);

        spot.transform.localRotation = Quaternion.identity;
        spot.transform.position = realMouse;

        //회전에 의한 좌표
        Vector2Int intPos = new Vector2Int(Mathf.RoundToInt(spot.transform.localPosition.x), Mathf.RoundToInt(spot.transform.localPosition.y));

        SetKeysActive(false);
        /*
        foreach (var item in virtualKeys.Values)
        {
            item.SetActive(false);
        }*/
        myUnit.AddKeyCap(intPos, tmpKeyCode);
    }
}
