using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPower : Piece {

    public override void steppedOn()
    {
        //Vector3 position = TowerController.instance.GetHighestBlockOfType(typeof(PortalPower));
        //Debug.Log("La ubicacion de la pieza es " + position);
        //GameManager.instance.GetPlayer().GetComponent<PlayerMovement>().Teleport(position);
    }

    public override void onPlace()
    {

    }

    public override void onTowerUpdate()
    {

    }
}
