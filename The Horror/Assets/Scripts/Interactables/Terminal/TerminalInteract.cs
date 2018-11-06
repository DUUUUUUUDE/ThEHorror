using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalInteract : Interactable {

    Terminal _Terminal;

    private void Start()
    {
        _Terminal = GetComponent<Terminal>();
    }

    public override void OnInteract()
    {
        _Terminal.EnterTerminal();
    }
}
