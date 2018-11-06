using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region PUBLIC VARIABLES
    [Space(10)]
    public float _MaxJumpHeight;
    public float _MinJumpHeight;
    public float _TimeToMaxHeight;
    [Space(10)]
    public float _ChangeDirAir;
    public float _ChangeDirGround;
    [Space(10)]
    public float _NormalMoveSpeed;
    public float _RunMoveSpeed;
    [Space(10)]
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
    CharacterController _CharacterController;
    PlayerManager       _Manager;
    #endregion

    protected void Start()
    {
        _NormalGravity = -(2 * _MaxJumpHeight) / Mathf.Pow(_TimeToMaxHeight, 2);
        _Gravity = _NormalGravity;
        _MaxJumpVelocity = Mathf.Abs(_Gravity) * _TimeToMaxHeight;
        _MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_Gravity) + _MaxJumpHeight);
        _ChangeDirectionTimeAir = _ChangeDirAir;
        _ChangeDirectionTimeGround = _ChangeDirGround;
        _MoveSpeed = _NormalMoveSpeed;
        _CharacterController = GetComponent<CharacterController>();

        _Manager = GetComponentInParent<PlayerManager>();
    }

    public void Update()
    {
        NormalMovement();
    }

    #region Change MoveSpeed
    public virtual void Walk()
    {
        _MoveSpeed = _NormalMoveSpeed;
    }

    public virtual void Run()
    {
        if (_Manager.CurrentSpirit == null)
            _MoveSpeed = _RunMoveSpeed;
    }
    #endregion

    #region Jump
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
    public virtual void EndJump()
    {
        if (_Velocity.y > _MinJumpVelocity)
        {
            _Velocity.y = _MinJumpVelocity;
        }
    }
    #endregion

    public void SetMoveDirection(Vector3 Direction)
    {
        _Move.x = Direction.x;
        _Move.z = Direction.z;
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
