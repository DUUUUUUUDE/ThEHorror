using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

    public bool ScanerOn;
    public int ScanerAperture;
    public LayerMask ScanerLayer;
    public Transform DotPool;
    public Transform ScanerPivot;

    int Level1Dots = 4;
    int Level2Dots = 8;
    int Level3Dots = 12;
    List<Transform> Dots = new List<Transform>();


    public ParticleSystem Gun1;

    PlayerManager Manager;


    private void Start()
    {
        Manager = GetComponent<PlayerManager>();

        foreach (Transform t in DotPool.GetComponentsInChildren<Transform>(true))
        {
            if (t != DotPool)
            {
                Dots.Add(t);
            }
        }
        
    }

    private void Update()
    {
        if (ScanerOn)
        {
            SetUpScanerPoints();
            PlaceDots();
        }
    }

    public void ShootGun ()
    {
        Gun1.Play ();
    }

    #region Scaner
    //SET UP
    List<Vector3> DotPositions = new List<Vector3> ();
    void SetUpScanerPoints ()
    {
        DotPositions.Clear();

        RaycastHit hit;
        Ray newRay = new Ray(ScanerPivot.position, Manager._Camera.transform.forward);
        if (Physics.Raycast (newRay, out hit, 100, ScanerLayer))
        {
            DotPositions.Add(hit.point);
        }
        else
        {
            DotPositions.Add(Vector3.zero);
        }

        newRay = new Ray(ScanerPivot.position, Manager.transform.up);
        if (Physics.Raycast(newRay, out hit, 100, ScanerLayer))
        {
            DotPositions.Add(hit.point);
        }
        else
        {
            DotPositions.Add(Vector3.zero);
        }
        newRay = new Ray(ScanerPivot.position, -Manager.transform.up);
        if (Physics.Raycast(newRay, out hit, 100, ScanerLayer))
        {
            DotPositions.Add(hit.point);
        }
        else
        {
            DotPositions.Add(Vector3.zero);
        }
        newRay = new Ray(ScanerPivot.position, Manager.transform.forward);
        if (Physics.Raycast(newRay, out hit, 100, ScanerLayer))
        {
            DotPositions.Add(hit.point);
        }
        else
        {
            DotPositions.Add(Vector3.zero);
        }

        if (ScanerAperture > 1)
        {
            List<Vector3> positions = GetPoints(Level1Dots, 10);
            foreach (Vector3 point in positions)
            {
                newRay = new Ray(ScanerPivot.position, point - ScanerPivot.position);
                if (Physics.Raycast(newRay, out hit, 100, ScanerLayer))
                {
                    DotPositions.Add(hit.point);
                }
                else
                {
                    DotPositions.Add(Vector3.zero);
                }
            }
        }

        if (ScanerAperture > 2)
        {
            List<Vector3> positions = GetPoints(Level2Dots, 20);
            foreach (Vector3 point in positions)
            {
                newRay = new Ray(ScanerPivot.position, point - ScanerPivot.position);
                if (Physics.Raycast(newRay, out hit, 100, ScanerLayer))
                {
                    DotPositions.Add(hit.point);
                }
                else
                {
                    DotPositions.Add(Vector3.zero);
                }
            }
        }

    }

    //CIRCLE FOR SCANER
    public Transform ScanerCircleSetter;
    List <Vector3> GetPoints (int num, float apperture)
    {
        float angle = 360 / num;

        List<Vector3> returnList = new List<Vector3> ();
        for (int i = 0; i < num; i++)
        {
            float a = i * angle;
            ScanerCircleSetter.localEulerAngles = new Vector3(0, 0, a);
            returnList.Add(ScanerCircleSetter.position + ScanerCircleSetter.up * Mathf.Tan (apperture * Mathf.Deg2Rad));
        }

        return returnList;
    }

    //PLACE DOTS
    void PlaceDots ()
    {
        for (int i = 0; i <= Level1Dots + Level2Dots; i++)
        {
            Dots[i].position = DotPositions[i];
        }
    }
    #endregion
}
