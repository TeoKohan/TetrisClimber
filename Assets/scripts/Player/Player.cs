using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    protected PlayerMovement playerMovement;
    [SerializeField]
    protected PieceHandling pieceHandling;

    public void initialize() {
        GameManager.addPlayer(this);
        playerMovement.initialize();
        pieceHandling.initialize();
    }

    public void update() {
        playerMovement.update();
        pieceHandling.update();
    }
}
