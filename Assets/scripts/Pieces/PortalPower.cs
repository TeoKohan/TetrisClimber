using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPower : Piece {

    public override void steppedOn()
    {
        GameManager.instance.GetPlayer().GetComponent<PlayerMovement>().Teleport();
    }

    public override void onPlace()
    {

    }

    public override void onTowerUpdate()
    {

    }
}
