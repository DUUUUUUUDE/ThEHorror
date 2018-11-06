using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TActionChain : TerminalAction
{
    [Space(5)]
    public List<KeyChain> Chain = new List<KeyChain> ();
    [HideInInspector] public KeyChain CurrentChain = null;
    [HideInInspector] public KeyChain LastChain = null;

    public override string LookForKey (string key)
    {
        string output = null;

        if (CurrentChain.Input_Key == null)
        {
            CurrentChain = Chain[0];
        }

        if (CurrentChain.Input_Key == key)
        {
            output = CurrentChain.Output;

            //Final Key
            if (CurrentChain.SuccesKey == -1)
                Action();
            //Normal Key
            else
            {
                LastChain = CurrentChain;
                CurrentChain = Chain[CurrentChain.SuccesKey];
            }

            //Return the output od the key
            return output;

        }
        else
        {
            //Fail on finding key
            if (LastChain != null)
            {
                CurrentChain = Chain[CurrentChain.FailKey];
                return "? syntax error" + "\n" + LastChain.ReturnOutput;
            }
            else
            {
                return null;
            }
        }

    }

}
