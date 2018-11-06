using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instace;

    #region CLASSES
    [HideInInspector] public static PlayerMovement    _Movement { get; set; }
    [HideInInspector] public static PlayerGadgets     _Gadgets { get; set; }
    [HideInInspector] public static PlayerUI          _UI { get; set; }
    [HideInInspector] public static PlayerCamera      _CameraMovement { get; set; }
    [HideInInspector] public static PlayerInteract    _Interact { get; set; }
    [HideInInspector] public static Camera            _Camera { get; set; }
    #endregion

    public Transform GarbageCollector;

    [HideInInspector] public Spirit   CurrentSpirit;
    [HideInInspector] public Terminal CurrentTerminal;

    
    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;

            SetUp();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    void SetUp ()
    {
        _Movement = FindObjectOfType<PlayerMovement>();
        _Gadgets = FindObjectOfType<PlayerGadgets>();
        _UI = FindObjectOfType<PlayerUI>();
        _CameraMovement = FindObjectOfType<PlayerCamera>();
        _Interact = FindObjectOfType<PlayerInteract>();
        _Camera = Camera.main;

        FreeState();
    }

    #region STATES
    public enum PlayerStates
    {
        Free,
        OnTerminal,
        OnMenu
    }

    public static PlayerStates _State;

    //Change State
    public void ChangeState (PlayerStates newState)
    {
        switch (newState)
        {

            case PlayerStates.Free:
                FreeState();
                break;

            case PlayerStates.OnTerminal:
                OnTerminalState();
                break;

            case PlayerStates.OnMenu:
                break;

        }
    }

    //STATE SETUPS
    void FreeState ()
    {
        _State = PlayerStates.Free;

        _CameraMovement.enabled = true;
        _Movement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnTerminalState()
    {
        _State = PlayerStates.OnTerminal;

        _CameraMovement.enabled = false;
        _Movement.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnMenuState()
    {
        _State = PlayerStates.OnMenu;

        _CameraMovement.enabled = false;
        _Movement.enabled = false;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    #endregion
}
