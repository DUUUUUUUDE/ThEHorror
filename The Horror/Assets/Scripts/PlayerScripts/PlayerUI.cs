using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Image SpiritImage;
    public Text InteractText;

    Spirit playerSpirit;

    // INTERACT TEXT
    public void ShowInteractText (string txt)
    {
        InteractText.text = txt;
        InteractText.enabled = true;
    }
    public void HideInteractText ()
    {
        InteractText.text = null;
        InteractText.enabled = false;
    }
    
    // SPIRIT
    public void RefreshSpirit(float ammount)
    {
        SpiritImage.fillAmount = ammount;
    }
    public void ShowSpirit ()
    {
        playerSpirit = PlayerManager.Instace.CurrentSpirit;
        SpiritImage.enabled = true;
    }
    public void HideSpirit ()
    {
        playerSpirit = null;
        SpiritImage.enabled = false;
    }

}
