using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurplePower : Piece {

    public override void steppedOn()
    {
        player.GetComponent<PlayerMovement>().Teleport();
    }

    public override void onPlace()
    {

    }

    public override void onTowerUpdate()
    {

    }
}
