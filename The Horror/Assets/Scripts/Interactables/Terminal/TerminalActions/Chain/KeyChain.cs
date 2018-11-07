using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyChain
{
    [Space (5)]
    public string Input_Key;
    [TextArea (0,10)]
    public string Output;
    public string ReturnOutput;

    [Space(5)]
    public int FailKey;
    public int SuccesKey;

}
