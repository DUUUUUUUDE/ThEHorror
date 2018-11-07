using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TActionInfo : TerminalAction
{
    public List<KeyInfo> InfoNotes = new List<KeyInfo>();
    
    public override string LookForKey(string key)
    {
        //index of InfoNotes
        if (key == "index")
        {
            int noteIndex = 0;
            string returnString = "index \n" + "----- \n";
            foreach (KeyInfo note in InfoNotes)
            {
                noteIndex++;
                returnString += "\n" + "_" + noteIndex + ": " + note.Input_Key + ".";
            }

            return returnString;
        }

        foreach (KeyInfo info in InfoNotes)
        {
            if (info.Input_Key == key)
                return info.Output;
        }

        return null;
    }

}
