using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGadgets : MonoBehaviour {

    PlayerManager Manager;

    [Space(10)]
    [Header("Scan")]
    public int DotCount;
    public int DotSpread;
    public int DotDistance;
    public LayerMask DotLayer;
    public Transform DecalPool;
    [Space(3)]
    [Tooltip("The time for the scan to recover from 0% - 100%")]
    public float DotScanRecoverTotalTime;
    public int DotScanMaxAmmo;
    [Tooltip("The multyplier on the recovery time of the scan")]
    public float SpiritMod;
    public float MaxTimeToDissapear;
    public float MinTimeToDissapear;

    [Space(10)]
    [Header("Waypoint")]
    public GameObject WayPoint;
    public float WayPointCost;

    
    [HideInInspector] public bool Aiming;
    [Space(10)]
    [Header("Gun")]
    public Transform LinePool;
    public float LineLifeTime;
    public float GunTimeToCharge;
    public float GunDamageMax;
    public float GunDamageMin;
    public float GunDistance;
    public LayerMask GunLayer;

    [Space(10)]
    public ParticleSystem GunChargeP;
    public ParticleSystem GunShootP;

    //Shoot
    public void Shoot()
    {


        if (Aiming)
        {
            GunShoot();
        }
        else
        {
            ShootDotScan();
        }
    }



    #region GUN

    float GunTimer;
    float GunDamage;

    List<LineRenderer> lines = new List<LineRenderer>();
    List<GunLine> LinesList = new List<GunLine>();

    //SetUp
    void SetupGun()
    {
        foreach (Transform t in LinePool.GetComponentsInChildren<Transform>(true))
        {
            if (t != LinePool)
                lines.Add(t.GetComponent<LineRenderer>());
        }

    }

    //Shoot
    void GunShoot ()
    {
        RaycastHit hit = new RaycastHit();
        Ray newRay = new Ray(PlayerManager._Camera.transform.position, PlayerManager._Camera.transform.forward);

        if (Physics.Raycast(newRay, out hit, GunDistance, GunLayer))
        {
            CastLines(hit.point);
        }

        StopAim();
        GunShootP.Play();
    }

    //Update
    void GunUpdate ()
    {

        List<GunLine> removeList = new List<GunLine>();
        foreach (GunLine line in LinesList)
        {
            line.Update(ref removeList);
        }

        foreach (GunLine line in removeList)
        {
            LinesList.Remove(line);
        }
    }

    //Cast Lines
    void CastLines (Vector3 pos)
    {
        foreach (LineRenderer line in lines)
        {
            if (!line.gameObject.activeSelf)
            {
                LinesList.Add(new GunLine(line, GunShootP.transform.position, pos, LineLifeTime));
                break;
            }

        }
    }

    //AIM start
    public void StartAim ()
    {
        if (PlayerManager.Instace.CurrentSpirit)
        {
            PlayerManager._Movement.Aim();
            Aiming = true;
            ResetAim();
            PlayerManager._CameraMovement.AimSensitivity();

            //Particles
            GunChargeP.Play();
        }
    }

    //AIM stop
    public void StopAim ()
    {
        if (Aiming)
        {
            PlayerManager._Movement.Walk();
            Aiming = false;
            ResetAim();
            PlayerManager._CameraMovement.NormalSensitivity();

            //particles
            GunChargeP.Stop();

        }
    }

    //AIM rest
    void ResetAim()
    {
        GunTimer = 0;
        GunDamage = GunDamageMin;
    }

    //AIM
    void Aim ()
    {
        if (GunDamage < GunDamageMax)
        {
            GunTimer += Time.deltaTime;
            GunDamage = GunDamageMin + (GunDamageMax - GunDamageMin) * (GunTimer/ GunTimeToCharge);
        }
    }

    #endregion



    #region DOT SCAN

    float   DotScanRecoverTime;
    float   DotScanTimer;
    int     DotScanAmmo;

    List<Transform> DecalListTransform = new List<Transform>();
    List<ScanDecal> DecalList = new List<ScanDecal>();

    //Set Up
    void SetUpScan ()
    {
        DotScanRecoverTime = DotScanRecoverTotalTime / DotScanMaxAmmo;
        DotScanAmmo = DotScanMaxAmmo;

        foreach (Transform t in DecalPool.GetComponentsInChildren<Transform>(true))
        {
            if (t != DecalPool)
                DecalListTransform.Add(t);
        }
    }

    //Shoot
    void ShootDotScan()
    {
        if (DotScanAmmo > 0)
        {
            CastDots();
            DotScanAmmo--;
        }
    }

    //Cast Dots
    void CastDots ()
    {
        RaycastHit hit = new RaycastHit();
        Ray newRay;

        Vector3 CameraForward = PlayerManager._Camera.transform.forward;
        Vector3 CameraPos = PlayerManager._Camera.transform.position;


        for (int i = 0; i < DotCount; i++)
        {
            newRay = new Ray(CameraPos,Random.insideUnitSphere + CameraForward * DotSpread);

            if (Physics.Raycast (newRay,out hit, DotDistance, DotLayer))
            {
                foreach (Transform t in DecalListTransform)
                {
                    if (!t.gameObject.activeSelf)
                    {
                        t.position = hit.point;
                        t.forward = hit.normal;
                        DecalList.Add(new ScanDecal(t,MaxTimeToDissapear,MinTimeToDissapear));
                        break;
                    }
                }
            }

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

        List<ScanDecal> removeList = new List<ScanDecal>();
        foreach (ScanDecal decal in DecalList)
        {
            decal.Update(ref removeList);
        }

        foreach (ScanDecal decal in removeList)
        {
            DecalList.Remove(decal);
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
            Manager.CurrentSpirit.Drain(WayPointCost);
            GameObject newWayPoint = Instantiate(WayPoint, transform.position, Quaternion.identity, Manager.GarbageCollector);
        }
    }

    #endregion

    //START
    private void Start()
    {
        Manager = PlayerManager.Instace;

        SetUpScan();
        SetupGun();
    }

    //UPDATE
    private void Update()
    {

        DotScanUpdate();
        GunUpdate();

        if (Manager.CurrentSpirit)
            Manager.CurrentSpirit.Recover();

        if (Aiming)
            Aim();

    }

}

class ScanDecal
{
    Transform Decal;
    float Timer;

    public ScanDecal(Transform decal, float max, float min)
    {
        Decal = decal;
        Timer = Random.Range (min, max);

        Decal.gameObject.SetActive(true);
    }

    public void Update (ref List <ScanDecal> list)
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Decal.gameObject.SetActive(false);
            list.Add(this);
        }
    }
}

class GunLine
{
    LineRenderer Line;
    float LifeTime;
    
    public GunLine(LineRenderer line, Vector3 start, Vector3 end, float lifeTime)
    {
        Line = line;
        Line.SetPosition(0, start);
        Line.SetPosition(1, end);

        LifeTime = lifeTime;

        Line.gameObject.SetActive(true);
    }

    public void Update(ref List <GunLine> list)
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
        {
            Line.gameObject.SetActive(false);
            list.Add(this);
        }
    }
}