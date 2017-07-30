using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePower : Piece {

    public override void steppedOn()
    {
        player.GetComponent<PlayerMovement>().SpecialJump();
    }

    public override void onPlace()
    {

    }

    public override void onTowerUpdate()
    {

    }
}
