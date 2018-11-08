using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGadgets : MonoBehaviour {

    PlayerManager Manager;

    [Space(10)]
    public ParticleSystem DotScanParticles;
    [Tooltip("The time for the scan to recover from 0% - 100%")]
    public float DotScanRecoverTotalTime;
    public int DotScanMaxAmmo;
    [Tooltip("The multyplier on the recovery time of the scan")]
    public float SpiritMod;

    [Space(10)]
    public GameObject WayPoint;
    public float WayPointCost;

    [Space (10)]
    [HideInInspector] public bool Aiming;
    public float GunTimeToCharge;
    public float GunDamageMax;
    public float GunDamageMin;

    public void Shoot()
    {
        if (Aiming)
        {
            Debug.Log(GunDamage);
            ResetAim();
        }
        else
        {
            ShootDotScan();
        }
    }


    #region GUN

    float GunTimer;
    float GunDamage;

    public void StartAim ()
    {
        if (PlayerManager.Instace.CurrentSpirit)
        {
            PlayerManager._Movement.Aim();
            Aiming = true;
            ResetAim();
            PlayerManager._CameraMovement.AimSensitivity();
        }
    }

    public void StopAim ()
    {
        if (Aiming)
        {
            PlayerManager._Movement.Walk();
            Aiming = false;
            ResetAim();
            PlayerManager._CameraMovement.NormalSensitivity();
        }
    }

    void Aim ()
    {
        if (GunDamage < GunDamageMax)
        {
            GunTimer += Time.deltaTime;
            GunDamage = GunDamageMin + (GunDamageMax - GunDamageMin) * (GunTimer/ GunTimeToCharge);
        }
    }

    void ResetAim()
    {
        GunTimer = 0;
        GunDamage = GunDamageMin;
    }
    

    #endregion

    #region DOT SCAN

    float   DotScanRecoverTime;
    float   DotScanTimer;
    int     DotScanAmmo;
    
    //Shoot
    void ShootDotScan()
    {
        if (DotScanAmmo > 0)
        {
            DotScanParticles.Play();
            DotScanAmmo--;
        }
    }

    //Update
    void DotScanUpdate ()
    {
        if (Manager.CurrentSpirit == null)
            DotScanTimer += Time.deltaTime;
        else
            DotScanTimer += Time.deltaTime * SpiritMod;

        if (DotScanTimer > DotScanRecoverTime)
        {
            ReloadDotScan();
            DotScanTimer = 0;
        }
    }

    //Reload
    void ReloadDotScan ()
    {
        if (DotScanAmmo < DotScanMaxAmmo)
            DotScanAmmo++;
    }

    #endregion


    #region WAYPOINT

    //WAYPOINT
    public void SetWayPoint ()
    {
        if (Manager.CurrentSpirit != null)
        {
            PlayerManager.Instace.CurrentSpirit.Drain(WayPointCost);
            GameObject newWayPoint = Instantiate(WayPoint, transform.position, Quaternion.identity, Manager.GarbageCollector);
        }
    }

    #endregion

    //START
    private void Start()
    {
        Manager = PlayerManager.Instace;

        //SprayScan
        DotScanRecoverTime = DotScanRecoverTotalTime / DotScanMaxAmmo;
        DotScanAmmo = DotScanMaxAmmo;

    }

    //UPDATE
    private void Update()
    {
        DotScanUpdate();

        if (Manager.CurrentSpirit)
            Manager.CurrentSpirit.Recover();

        if (Aiming)
            Aim();
    }

}
