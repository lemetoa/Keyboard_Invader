using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName= "newData",menuName ="Scriptable/SpriteData")]
public class SpriteData : ScriptableObject
{
    [SerializeField]
    private StringSprite sprites;

    public Sprite Getsprite(string code)
    {
        if (sprites.ContainsKey(code))
        {
            return sprites[code];
        }
        return sprites["0"];
    }
}

[System.Serializable]
public class StringSprite : SerializableDictionary<string, Sprite> { }