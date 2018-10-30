using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [HideInInspector]
    public Vector3 _MovementForward;

    PlayerManager Manager;

    private void Start()
    {
        Manager = GetComponent<PlayerManager>();
    }

    Vector3 GetForwardMovement()
    {
        float HMovement = Input.GetAxis("Horizontal");
        float VMovement = Input.GetAxis("Vertical");

        Vector3 moveDirSide = Manager._Movement.transform.right * HMovement;
        Vector3 moveDirForward = Manager._Movement.transform.forward * VMovement;

        Vector3 dir = new Vector3(moveDirSide.x + moveDirForward.x, 0, moveDirSide.z + moveDirForward.z);

        if (dir.magnitude > 1)
            return dir.normalized;
        else
            return dir;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Manager._Gun.ShootGun();
        }

        #region MOVEMENT

        //MOVEMENT VECTOR
        _MovementForward = GetForwardMovement();
        //2D MOVEMENT
        Manager._Movement.SetMoveDirection(_MovementForward);

        // JUMP START
        if (Input.GetButtonDown("Jump"))
        {
            Manager._Movement.StartJump();
        }
        // JUMP END
        if (Input.GetButtonUp("Jump"))
        {
            Manager._Movement.EndJump();
        }

        //RUN START
        if (Input.GetButtonDown("Fire3"))
        {
            Manager._Movement.Run();
        }
        //RUN END
        if (Input.GetButtonUp("Fire3"))
        {
            Manager._Movement.Walk();
        }

        #endregion
    }
    
}
