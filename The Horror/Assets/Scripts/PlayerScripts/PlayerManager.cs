using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public PlayerMovement _Movement;
    public PlayerGun _Gun;
    public Camera _Camera;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _Movement = GetComponent<PlayerMovement>();
        _Gun = GetComponent<PlayerGun>();
        _Camera = Camera.main;
    }

    private void Start()
    {
        
    }
}
