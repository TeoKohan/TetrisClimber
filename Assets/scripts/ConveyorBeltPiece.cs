using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltPiece : MonoBehaviour {

	[SerializeField]
    protected Transform restPosition;

    public Transform getRestPosition() {
        return restPosition;
    }
}
