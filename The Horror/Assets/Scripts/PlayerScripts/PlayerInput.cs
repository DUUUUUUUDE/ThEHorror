using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [HideInInspector]
    public Vector3 _MovementForward;

    PlayerManager Manager;

    private void Start()
    {
        Manager = GetComponentInParent<PlayerManager>();
    }

    

    void Update()
    {

        // INPUT depending on PlayerState
        switch (PlayerManager._State)
        {
            case PlayerManager.States.Free:
                FreeInput();
                break;
            case PlayerManager.States.OnTerminal:
                TerminalInput();
                break;
            case PlayerManager.States.OnMenu:
                break;
        }
    }

    #region FREE

    void FreeInput ()
    {
        #region GADGETS

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerManager._Gadgets.ScanerOnOff();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerManager._Interact.Interact();
        }
        // GUN // SCAN
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerManager._Gadgets.Shoot ();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            PlayerManager._Gadgets.StartAim();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            PlayerManager._Gadgets.StopAim();
        }

        // Waypoint
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PlayerManager._Gadgets.SetWayPoint();
        }
        #endregion

        #region MOVEMENT
        //MOVEMENT VECTOR
        _MovementForward = GetForwardMovement();
        PlayerManager._Movement.SetMoveDirection(_MovementForward);

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            PlayerManager._Movement.StartJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            PlayerManager._Movement.EndJump();
        }
        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            PlayerManager._Movement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            PlayerManager._Movement.Walk();
        }
        #endregion

    }

    // GET 2D MOVEMENT
    Vector3 GetForwardMovement()
    {
        float HMovement = Input.GetAxis("Horizontal");
        float VMovement = Input.GetAxis("Vertical");

        Vector3 moveDirSide = PlayerManager._Movement.transform.right * HMovement;
        Vector3 moveDirForward = PlayerManager._Movement.transform.forward * VMovement;

        Vector3 dir = new Vector3(moveDirSide.x + moveDirForward.x, 0, moveDirSide.z + moveDirForward.z);

        if (dir.magnitude > 1)
            return dir.normalized;
        else
            return dir;
    }
    #endregion

    void TerminalInput ()
    {

        //Exit Terminal
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerManager.Instace.CurrentTerminal.ExitTerminal();
        }


    }
}
