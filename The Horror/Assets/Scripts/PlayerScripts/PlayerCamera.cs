using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    Transform _Camera;
    public float _SensitivityNormal;
    public float _SensitivityAim;
    public float _MouseSensitivity;

    [Space(10)]
    public Camera NormalCamera;

    public LayerMask NormalMasks;
    public LayerMask ScanerMasks;

    protected float _XAxisClamp = 0.0f;


    private void Start()
    {
        _Camera = PlayerManager.Instace.CameraHolder;
    }

    public void ScanON()
    {
        NormalCamera.cullingMask = NormalMasks;
    }

    public void ScanOFF()
    {
        NormalCamera.cullingMask = ScanerMasks;
    }


    void Update ()
    {
        RotateCamera();
    }

    public void NormalSensitivity ()
    {
        _MouseSensitivity = _SensitivityNormal;
    }

    public void AimSensitivity ()
    {
        _MouseSensitivity = _SensitivityAim;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * _MouseSensitivity;
        float rotAmountY = mouseY * _MouseSensitivity;

        _XAxisClamp -= rotAmountY;

        Vector3 targetRotCam = _Camera.rotation.eulerAngles;
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

        _Camera.rotation = Quaternion.Euler(new Vector3(targetRotCam.x, targetRotCam.y, transform.rotation.eulerAngles.z));
        transform.rotation = Quaternion.Euler(new Vector3(targetRotBody.x, targetRotBody.y, transform.rotation.eulerAngles.z));

    }
}
