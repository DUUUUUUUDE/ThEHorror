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
    #endregion

    public Transform GarbageCollector;
    public Transform CameraHolder;


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

        FreeState();
    }

    #region SPIRIT

    public void SetUpSpirit (Spirit spirit)
    {
        CurrentSpirit = spirit;
        OnSpirit = true;
    }
    public void EraseSpirit ()
    {
        CurrentSpirit = null;
        OnSpirit = false;
    }

    #endregion

    #region STATES

    public bool OnSpirit;
    public bool OnConversation;

    public enum States
    {
        Free,
        OnTerminal,
        OnMenu
    }

    public static States _State;

    //Change State
    public void ChangeState (States newState)
    {
        switch (newState)
        {

            case States.Free:
                FreeState();
                break;

            case States.OnTerminal:
                OnTerminalState();
                break;

            case States.OnMenu:
                break;

        }
    }

    //STATE SETUPS
    void FreeState ()
    {
        _State = States.Free;

        _CameraMovement.enabled = true;
        _Movement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnTerminalState()
    {
        _State = States.OnTerminal;

        _CameraMovement.enabled = false;
        _Movement.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnMenuState()
    {
        _State = States.OnMenu;

        _CameraMovement.enabled = false;
        _Movement.enabled = false;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    

    #endregion
}
