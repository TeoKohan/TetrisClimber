using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangePower : Piece {

    //Despues de mergerar descomentar
    //public override void steppedOn()
    public void steppedOn()
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
