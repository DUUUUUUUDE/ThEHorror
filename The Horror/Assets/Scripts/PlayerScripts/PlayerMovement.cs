using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region PUBLIC VARIABLES
    public float _MaxJumpHeight;
    public float _MinJumpHeight;
    public float _TimeToJumpMaxHeight;
    public float _ChangeDirAir;
    public float _ChangeDirGround;
    public float _NormalMoveSpeed;
    public float _RunMoveSpeed;
    public float _MouseSensitivity;
    #endregion

    #region VARS JUMP
    [HideInInspector]
    public bool CanJump;
    [HideInInspector]
    public bool Jump;
    #endregion

    #region DescendSlope
    bool LateGrounded;
    bool OnSlope;
    #endregion

    #region HIDDEN PUBLIC VARIABLES
    [HideInInspector]
    public float _MoveSpeed;
    [HideInInspector]
    public Vector3 _Velocity;
    [HideInInspector]
    public Vector3 _Move;
    #endregion

    #region PRIVATE VARIABLES
    protected float     _Gravity;
    protected float     _NormalGravity;
    protected float     _MaxJumpVelocity;
    protected float     _MinJumpVelocity;
    protected float     _VelocityGroundSmoothingX;
    protected float     _VelocityGroundSmoothingZ;
    protected float     _VelocityAirSmoothing;
    protected float     _ChangeDirectionTimeAir;
    protected float     _ChangeDirectionTimeGround;
    protected float     _VerticalAxis;
    protected float     _HorizontalAxis;
    protected float     _XAxisClamp = 0.0f;
    CharacterController _CharacterController;
    protected Camera    _CharacterCamera;
    #endregion

    protected void Start()
    {

        _NormalGravity = -(2 * _MaxJumpHeight) / Mathf.Pow(_TimeToJumpMaxHeight, 2);
        _Gravity = _NormalGravity;
        _MaxJumpVelocity = Mathf.Abs(_Gravity) * _TimeToJumpMaxHeight;
        _MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_Gravity) + _MaxJumpHeight);
        _ChangeDirectionTimeAir = _ChangeDirAir;
        _ChangeDirectionTimeGround = _ChangeDirGround;
        _MoveSpeed = _NormalMoveSpeed;
        _CharacterController = GetComponent<CharacterController>();
        _CharacterCamera = GetComponentInChildren<Camera>();
    }

    public virtual void Walk()
    {
        _MoveSpeed = _NormalMoveSpeed;
    }

    public virtual void Run()
    {
        _MoveSpeed = _RunMoveSpeed;
    }

    public virtual void StartJump()
    {
        if (CanJump)    //Jump
        {
            _Velocity.y = _MaxJumpVelocity;
            CanJump = false;
            //Slope
            if (OnSlope)
                OnSlope = false;
        }
    }
    //JUMP END
    public virtual void EndJump()
    {
        if (_Velocity.y > _MinJumpVelocity)
        {
            _Velocity.y = _MinJumpVelocity;
        }
    }

    public void SetMoveDirection(Vector3 Direction)
    {
        _Move.x = Direction.x;
        _Move.z = Direction.z;
    }

    public void Update()
    {
        RotateCamera();
        NormalMovement();
    }

    public virtual void NormalMovement()
    {
        if (_CharacterController.collisionFlags == CollisionFlags.Above && _Velocity.y > 0)
        {
            _Velocity.y = 0;
        }

        // setup target velocity
        float targetVelocityX;
        float targetVelocityZ;
        targetVelocityX = _Move.x * _MoveSpeed;
        targetVelocityZ = _Move.z * _MoveSpeed;

        // setup velocity
        _Velocity.x = Mathf.SmoothDamp(_Velocity.x, targetVelocityX, ref _VelocityGroundSmoothingX, (_CharacterController.isGrounded) ? _ChangeDirectionTimeGround : _ChangeDirectionTimeAir);
        _Velocity.z = Mathf.SmoothDamp(_Velocity.z, targetVelocityZ, ref _VelocityGroundSmoothingZ, (_CharacterController.isGrounded) ? _ChangeDirectionTimeGround : _ChangeDirectionTimeAir);
        _Velocity.y += _Gravity * Time.deltaTime;

        //move
        _CharacterController.Move(_Velocity * Time.deltaTime);



        // reset fall
        if ((_CharacterController.isGrounded))
        {
            if (!LateGrounded)
            {
                LateGrounded = true;
                ChecKSlope();
            }

            _Velocity.y = 0;
            CanJump = true;

        }
        else
        {
            //Slope
            if (LateGrounded)
                LateGrounded = false;
            if (OnSlope)
                DescendingSlope();
        }
    }


    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * _MouseSensitivity;
        float rotAmountY = mouseY * _MouseSensitivity;

        _XAxisClamp -= rotAmountY;

        Vector3 targetRotCam = _CharacterCamera.transform.rotation.eulerAngles;
        Vector3 targetRotBody = transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;
        targetRotCam.z = 0;
        targetRotBody.y += rotAmountX;

        if (_XAxisClamp > 90)
        {
            _XAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (_XAxisClamp < -90)
        {
            _XAxisClamp = -90;
            targetRotCam.x = 270;
        }

        _CharacterCamera.transform.rotation = Quaternion.Euler(new Vector3(targetRotCam.x, targetRotCam.y, transform.rotation.eulerAngles.z));
        transform.rotation = Quaternion.Euler(new Vector3(targetRotBody.x, targetRotBody.y, transform.rotation.eulerAngles.z));

    }

    #region SLOPES
    void ChecKSlope()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(transform.up, hit.normal) < 45)
            {
                OnSlope = true;
            }
        }
    }
    //DescendingSLope
    void DescendingSlope()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(transform.up, hit.normal) < 45)
            {
                _CharacterController.Move(Vector3.down);
            }
        }
    }
    #endregion
}
