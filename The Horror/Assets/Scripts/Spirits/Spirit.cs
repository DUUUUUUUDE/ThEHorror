using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spirits/Base")]
public class Spirit : ScriptableObject {

    public float TotalEnergy = 100;
    public float TotalTimeToRecover = 30;

    float Energy;
    float Timer;

    // USE SPIRIT
	public void Drain (float cost)
    {
        Energy -= cost;
        if (Energy < 0)
        {
            PlayerManager.Instace.CurrentSpirit = null;
        }
    }

    //SPIRIT UPDATE
    public void Recover()
    {
        Timer += Time.deltaTime;
        Energy = TotalEnergy * (Timer / TotalTimeToRecover);
    }

    //INTERACT WITH SPIRIT
    public virtual void Interact ()
    {

    }

    //GIVE PLAYER SPIRIT
    public void SetUp ()
    {
        PlayerManager.Instace.CurrentSpirit = this;
    }
}
