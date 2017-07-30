using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurplePower : Piece {

    //Despues de mergerar descomentar
    //public override void steppedOn()
    public void steppedOn()
    {
        player.GetComponent<PlayerMovement>().Teleport();
    }

}
