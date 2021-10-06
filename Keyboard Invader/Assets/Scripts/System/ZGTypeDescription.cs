using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hamster.ZG.Type;
using ZG.Core.Type;

//List<float>에 대한 정의
[Type(typeof(List<float>), new string[] { "List<float>", "list<float>" })]
public class FloatListType : IType
{
    public object DefaultValue => new List<float>();

    public object Read(string value)
    {
        var values = ReadUtil.GetBracketValueToArray(value);

        List<float> floats = new List<float>();
        for (int i = 0; i < values.Length; i++)
        {
            floats.Add(float.Parse(values[i]));
        }
        return floats;
    }

    public string Write(object value)
    {
        List<float> floats = (List<float>)value;

        string join = string.Join(",", floats);
        Debug.Log("write" + join);
        return $"[{join}]";
    }
}

//List<int>에 대한 정의
[Type(typeof(List<int>), new string[] { "List<int>", "list<int>" })]
public class IntListType : IType
{
    public object DefaultValue => new List<int>();

    public object Read(string value)
    {
        var values = ReadUtil.GetBracketValueToArray(value);

        List<int> floats = new List<int>();
        for (int i = 0; i < values.Length; i++)
        {
            floats.Add(int.Parse(values[i]));
        }
        return floats;
    }

    public string Write(object value)
    {
        List<int> ints = (List<int>)value;

        string join = string.Join(",", ints);
        Debug.Log("write" + join);
        return $"[{join}]";
    }
}
//List<string>에 대한 정의
[Type(typeof(List<string>), new string[] { "List<string>", "list<string>" })]
public class StringListType : IType
{
    public object DefaultValue => new List<string>();

    public object Read(string value)
    {
        var values = ReadUtil.GetBracketValueToArray(value);

        List<string> strings = new List<string>();
        for (int i = 0; i < values.Length; i++)
        {
            strings.Add(values[i]);
        }
        return strings;
    }

    public string Write(object value)
    {
        List<string> ints = (List<string>)value;

        string join = string.Join(",", ints);
        Debug.Log("write" + join);
        return $"[{join}]";
    }
}

//bool에 대한 정의
[Type(typeof(bool), new string[] { "bool", "Bool" })]
public class BoolType : IType
{
    public object DefaultValue => false;

    public object Read(string value)
    {
        var y = bool.TryParse(value, out bool x);

        if (y == false)
        {
            return DefaultValue;
        }
        return x;
    }

    public string Write(object value)
    {
        return value.ToString();
    }
}


//벡터2에 대한 정의
[Type(typeof(Vector2), new string[] { "vector2", "Vector2" })]
public class Vector2Type : IType
{
    public object DefaultValue => Vector2.zero;

    public object Read(string value)
    {
        var values = ReadUtil.GetBracketValueToArray(value);
        float x = float.Parse(values[0]);
        float y = float.Parse(values[1]);
        return new Vector2(x, y);
    }
    public string Write(object value)
    {
        Vector2 vec2 = (Vector2)value;
        return $"[{vec2.x},{vec2.y}]";
    }
}



[UGS(typeof(StatType))]
public enum StatType
{
    None = 0, //아무것도 아님
    MaxHp = 1,  //최대체력
    MaxMp = 2,  //최대마나
    MaxSp = 3,  //최대Sp
    Size = 4,   //크기
    Weight = 5,  //무게
    Strength = 6,   //힘
    Defense = 7,    //방어
    Intelligence = 8,   //지혜
    Dexterity = 9,  // 손재주
    Speed = 10,      //속도
    JumpPower = 11,  //점프력

    FireFriendly = 101,   //화염친화
    FireImmunity = 102,   //화염내성
    FireResistance = 103, //화염저항
    IceFriendly = 201,    //얼음친화
    IceImmunity = 202,    //얼음내성
    IceResistance = 203,  //얼음저항
    PoisonFriendly = 301, //독친화
    PoisonImmunity = 302, //독내성
    PoisonResistance = 303,   //독저항
    EletricFriendly = 401,    //번개친화
    EletricImmunity = 402,    //번개내성
    EletricResistance = 403,  //번개저항

}

[UGS(typeof(AreaType), new string[] { "AreaType", "areaType" })]
public enum AreaType
{
    Box,
    Circle,
}

[UGS(typeof(ElementType))]
public enum ElementType
{
    Cut, Pierce, Hit, Bomb, Fire, Ice, Poison, Eletric,
}

[UGS(typeof(ActiveType))]
public enum ActiveType
{
    Swing, Throw, Self, None,
}

[UGS(typeof(EquipType))]
public enum EquipType
{
    Armor,
    Skin,
    Tool,

}