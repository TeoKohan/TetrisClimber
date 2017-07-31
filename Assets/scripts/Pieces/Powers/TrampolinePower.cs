using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePower : Piece {

    public override void steppedOn()
    {
        GameManager.instance.GetPlayer().GetComponent<PlayerMovement>().IsOnTrampoline();
    }

    public override void onPlace()
    {

    }

    public override void onTowerUpdate()
    {

    }
}
