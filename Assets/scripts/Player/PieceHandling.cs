using System;
using UnityEngine;

public class PieceHandling : MonoBehaviour {

    [SerializeField]
    protected Transform piecePivot;
    [SerializeField]
    protected float maxInteractDistance;
    [SerializeField]
    protected float maxDistanceToSeePossiblePlacement;
    [SerializeField]
    protected float minDistanceToSeePossiblePlacement;

    protected int pieceLayer;
    protected int towerLayer;
    protected int terrainLayer;
    protected int ignoreLayer;

    protected TowerController currentTowerController;

    protected Piece currentPiece;
    protected float currentPieceRadius;

    protected bool holding;

    //JUST TEMPORARY, VALID WHEN A SINGLE TOWERCONTROLLER IS PRESENT
    public void initialize() {
        currentTowerController = GameManager.getTowerController();

        pieceLayer = LayerMask.NameToLayer("Piece");
        towerLayer = LayerMask.NameToLayer("Tower");
        terrainLayer = LayerMask.NameToLayer("Terrain");
        ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");

        currentPiece = null;
        holding = false;
    }

    public void update() {

        bool click = Input.GetMouseButtonDown(0);

        if (holding) {

            Vector3 placePosition = getPivotPosition();
            int3 matrixPosition = currentTowerController.worldToMatrixPosition(placePosition);

            bool placeState = canPlace(matrixPosition);

            checkForRotation();
            displayPiece(placePosition, placeState);

            if (click) {
                if (canPlace(matrixPosition)) {
                    placePiece(matrixPosition);
                }
            }
        }

        else if (click) {

            Transform pieceTransform = getPiece();
            if (pieceTransform != null) {
                Piece piece = pieceTransform.GetComponent<Piece>();
                if (canPickUp(piece)) {
                    pickUp(pieceTransform);
                }
            }
        }
    }

    protected Vector3 getPivotPosition() {

        int layermask = (1 << pieceLayer) | (1 << towerLayer) | (1 << terrainLayer);

        RaycastHit hit;
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 facingDirection = new Vector3(cameraRay.direction.x, 0f, cameraRay.direction.z);
        Ray verticalRay = new Ray(cameraRay.origin + facingDirection * maxDistanceToSeePossiblePlacement, Vector3.down);

        if (Physics.Raycast(cameraRay, out hit, maxInteractDistance, layermask)) {
            return Helper.snapVector3(hit.point);
        }
        //HARDCODED 3, SHOULD DERIVE FROM ANOTHER VARIABLE
        else if (Physics.Raycast(verticalRay, out hit, 3f, layermask)) {
            return Helper.snapVector3(hit.point);
        }

        return Vector3.zero;
    }

    protected void displayPiece(Vector3 position, bool state) {
        currentPiece.setPlaceState(state);
        piecePivot.position = position;
    }

    protected Transform getPiece() {

        int layermask = 1 << pieceLayer;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxInteractDistance, layermask)) {
            return hit.transform.parent;
        }

        return null;
    }

    protected bool canPickUp(Piece piece) {
        return !piece.isOnTower();
    }

    void pickUp(Transform pieceTransform) {
        Piece piece = pieceTransform.GetComponent<Piece>();
        piece.pickUp();

        pieceTransform.SetParent(piecePivot);
        pieceTransform.localPosition = Vector3.zero;

        currentPiece = piece;
        currentPieceRadius = currentPiece.getRadius();
        foreach (GameObject B in currentPiece.getBlocks()) {
            B.layer = ignoreLayer;
        }

        holding = true;
    }

    //TRY TO PLACE PIECE
    // Verifies if the player is clicking a valid spot for the piece
    bool canPlace(int3 matrixPosition) {
        return currentTowerController.canPlace(currentPiece, matrixPosition);
    }

    //PLACE PIECE
    // Locates a piece in a spot
    void placePiece(int3 matrixPosition) {

        currentPiece.transform.parent = null;

        currentPiece.setTowerController(currentTowerController);
        currentTowerController.place(currentPiece, matrixPosition);
        currentPiece.placeOnTower();

        foreach (GameObject B in currentPiece.getBlocks()) {
            B.layer = pieceLayer;
        }

        //DEBUG
        currentTowerController.debugMarkPlace(matrixPosition);

        holding = false;
    }

    //CHECK FOR ROTATION
    // Verifies if the player is trying to rotate the piece
    void checkForRotation()
    {
        //TODO: DO
    }
}
