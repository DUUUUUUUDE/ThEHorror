using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spirits/Base")]
public class Spirit : ScriptableObject {

    public float TotalEnergy;
    public float TotalTimeToRecover;

    float Energy;
    float TimeMultiplier;

    // USE SPIRIT
	public void Drain (float cost)
    {
        Energy -= cost;
        if (Energy < 0)
        {
            PlayerManager.Instace.CurrentSpirit = null;
            PlayerManager._UI.HideSpirit ();
        }
    }

    //SPIRIT UPDATE
    public void Recover()
    {
        Energy = Mathf.MoveTowards(Energy, TotalEnergy, TimeMultiplier * Time.deltaTime);
        PlayerManager._UI.RefreshSpirit(Energy / TotalEnergy);
    }

    //INTERACT WITH SPIRIT
    public virtual void Interact ()
    {

    }

    //GIVE PLAYER SPIRIT
    public void SetUp ()
    {

        PlayerManager.Instace.CurrentSpirit = this;

        Energy = TotalEnergy;

        TimeMultiplier = TotalTimeToRecover / 60;

        PlayerManager.Instace.CurrentSpirit = this;
        PlayerManager._UI.ShowSpirit();
    }
}
