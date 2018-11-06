using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalAction : MonoBehaviour
{
    [Space (10)]
    //TRIGGER KEY
    [Tooltip ("The key that triggers the action") ]
    public string TriggerKey;
    
    public virtual string LookForKey (string key)
    {
        return null;

    }

    protected virtual void Action ()
    {

    }

}
	
