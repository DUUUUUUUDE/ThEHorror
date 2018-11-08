using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAChainSpirit : TActionChain {

    public Spirit ActionSpirit;

    protected override void Action()
    {
        PlayerManager.Instace.CurrentSpirit = ActionSpirit;
        PlayerManager.Instace.CurrentSpirit.SetUp();
    }

}
