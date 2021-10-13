using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "new sounds", menuName = "Scriptable/SoundData")]
public class SoundDatabase : ScriptableObject
{
    [SerializeField]
    private StringSound sounds;

    public AudioClip GetSound(string code)
    {
        if (sounds.ContainsKey(code))
        {
            return sounds[code];
        }
        //없으면 그냥 첫번째꺼
        return sounds.Values.ToList()[0];
    }
}

[System.Serializable]
public class StringSound : SerializableDictionary<string, AudioClip> { }