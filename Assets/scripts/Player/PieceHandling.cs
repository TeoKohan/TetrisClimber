using System;
using UnityEngine;

public class PieceHandling : MonoBehaviour {

    [SerializeField]
    private Transform piecePivot;
    [SerializeField]
    private float maxInteractDistance;
    [SerializeField]
    private float maxDistanceToSeePossiblePlacement;
    [SerializeField]
    private float minDistanceToSeePossiblePlacement;

    [SerializeField]
    private LayerMask pieceLayer;
    [SerializeField]
    private LayerMask towerLayer;

    private TowerController currentTowerController;

    private Piece currentPiece;

    private bool holding;

    //JUST TEMPORARY, VALID WHEN A SINGLE TOWERCONTROLLER IS PRESENT
    public void initialize() {
         currentTowerController = GameManager.getTowerController();

        currentPiece = null;
        holding = false;
    }

    public void update() {

        bool click = Input.GetMouseButtonDown(0);

        if (holding) {
            Vector3 placePosition = getTargetPosition();
            checkForRotation();
            //displayPiece();

            if (click) {
                int3 matrixPosition = TowerController.worldToMatrixPosition(placePosition);
                if (canPlace(matrixPosition)) {
                    placePiece(placePosition);
                }
            }
        }
        else if (click) {
            Debug.Log("Click");
            Transform pieceTransform = getPiece();
            if (pieceTransform != null) {
                Piece piece = pieceTransform.GetComponent<Piece>();
                if (canPickUp(piece)) {
                    pickUp(pieceTransform);
                }
            }
        }
    }

    protected Transform getPiece() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxInteractDistance, pieceLayer)) {
            Debug.Log(hit.transform.parent.name);
            return hit.transform.parent;
        }

        return null;
    }

    protected bool canPickUp(Piece piece) {
        return piece.isOnTower();
    }

    void pickUp(Transform pieceTransform) {
        Piece piece = pieceTransform.GetComponent<Piece>();
        piece.pickUp();

        pieceTransform.SetParent(piecePivot);
        pieceTransform.localPosition = new Vector3(0,piece.getPieceSize().y / 2f, 0);

        currentPiece = piece;
    }


    protected Vector3 getTargetPosition() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxInteractDistance)) {
            return hit.point;
        }
        return Vector3.zero;
    }

    //TRY TO PLACE PIECE
    // Verifies if the player is clicking a valid spot for the piece
    bool canPlace(int3 matrixPosition) {
        return currentTowerController.place(currentPiece, matrixPosition);
    }

    //PLACE PIECE
    // Locates a piece in a spot
    void placePiece(Vector3 point) {
        currentPiece.transform.parent = null;
    }


    //CHECK FOR PLACING SPACE
    // Verifies if the player is aiming to a valid placing spot for the piece
    /*
    void checkForPlacingSpace()
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green, maxDistanceToInteract);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance >= minDistanceToSeePossiblePlacement) { 
                if (TowerController.instance.IsWithinTower(hit.point)) {
                    Vector3 relPosition =  hit.point - TowerController.instance.transform.position;
                    
                    if (relPosition.x < 0 || relPosition.y < 0 || relPosition.z < 0)
                    {
                        p.validPlaceNotFound();
                    } else { 
                        int[] position = new int[] {
                            Mathf.FloorToInt(relPosition.x),
                            Mathf.FloorToInt(relPosition.y),
                            Mathf.FloorToInt(relPosition.z)
                        };

                        if (currentTowerController.place(p, position) && hit.distance <= maxDistanceToInteract)
                        {
                            p.validPlaceFound();
                        }
                        else
                        {
                            p.validPlaceNotFound();
                        }
                    }
                                        
                }

                piece.transform.position = new Vector3(
                    Mathf.Floor(hit.point.x), 
                    Mathf.Floor(hit.point.y), 
                    Mathf.Floor(hit.point.z));

                piece.transform.rotation = Quaternion.Euler(new Vector3(
                    piece.transform.rotation.eulerAngles.x,
                    Mathf.Round(piece.transform.rotation.eulerAngles.y / 90) * 90,
                    piece.transform.rotation.eulerAngles.z));
            }
            else
            {
                p.validPlaceNotFound();
                Vector3 correctedPosition = transform.position + (transform.position - hit.point).normalized * minDistanceToSeePossiblePlacement;
                piece.transform.position = new Vector3(
                    Mathf.Floor(correctedPosition.x),
                    Mathf.Floor(correctedPosition.y),
                    Mathf.Floor(correctedPosition.z));

                piece.transform.rotation = Quaternion.Euler(new Vector3(
                    piece.transform.rotation.eulerAngles.x,
                    Mathf.Round(piece.transform.rotation.eulerAngles.y / 90) * 90,
                    piece.transform.rotation.eulerAngles.z));
            }
        }
    }*/

    //CHECK FOR ROTATION
    // Verifies if the player is trying to rotate the piece
    void checkForRotation()
    {
        //TODO: DO
    }
}
