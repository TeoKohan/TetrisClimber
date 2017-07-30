using System;
using UnityEngine;

public class PieceHandling : MonoBehaviour {

    [SerializeField]
    private Transform piecePivot;
    [SerializeField]
    private float maxDistanceToInteract;
    [SerializeField]
    private LayerMask pieceLayer;
    [SerializeField]
    private LayerMask towerLayer;




    // Update is called once per frame
    void Update() {

        //if the player's holding something
        if (piecePivot.childCount > 0)
        {
            CheckForPlacingSpace();
            CheckForRotation();
        }
        
        //On click
        if (Input.GetMouseButtonDown(0))
        {
            // if the player hasn't picked up a piece, let them pick up a piece
            if (piecePivot.transform.childCount == 0) {
                TryToPickUpPiece();
            } else
            //if the player picked up something, try to place it
            {
                TryToPlacePiece();
            }
        }
    }


    //TRY TO PICK UP PIECE
    // Verifies if there was a piece in range where the player clicked
    void TryToPickUpPiece()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistanceToInteract, pieceLayer))
        {
            if (hit.transform.parent.parent == null) {
                PickUpPiece(hit.transform.parent);
            } else if (hit.transform.parent.parent.gameObject.layer != towerLayer)
            {
                PickUpPiece(hit.transform.parent);
            }
        }
    }


    //PICK UP PIECE
    // Picks the piece and adds it to the holding spot
    void PickUpPiece(Transform piece)
    {
        Piece p = piece.GetComponent<Piece>();
        p.pickUp();
        piece.transform.SetParent(piecePivot);
        piece.transform.localPosition = new Vector3(0,0,0);
    }


    //TRY TO PLACE PIECE
    // Verifies if the player is clicking a valid spot for the piece
    void TryToPlacePiece()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistanceToInteract))
        {
            if (TowerController.instance.IsWithinTower(hit.point)) { 
                PlacePiece(TowerController.instance.transform.position - hit.point);
            }
        }
    }

    //PLACE PIECE
    // Locates a piece in a spot
    void PlacePiece(Vector3 point)
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();
        int[] position = new int[] {
            Mathf.FloorToInt(Mathf.Abs(point.x)),
            Mathf.FloorToInt(Mathf.Abs(point.y)),
            Mathf.FloorToInt(Mathf.Abs(point.z))
        };
        
        if(TowerController.instance.CheckForPlace(p, position))
        {
            piece.transform.position = TowerController.instance.PlacePiece(p, position);
            piece.transform.parent = TowerController.instance.transform;
        } 
    }


    //CHECK FOR PLACING SPACE
    // Verifies if the player is aiming to a valid placing spot for the piece
    void CheckForPlacingSpace()
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (TowerController.instance.IsWithinTower(hit.point)) { 
                int[] position = new int[] {
                    Mathf.FloorToInt(hit.point.x),
                    Mathf.FloorToInt(hit.point.y),
                    Mathf.FloorToInt(hit.point.z)
                };
           
                if (TowerController.instance.CheckForPlace(p, position) && hit.distance <= maxDistanceToInteract)
                {
                    p.validPlaceFound();
                } else
                {
                    p.validPlaceNotFound();
                }
            }

            piece.transform.position = new Vector3(
                Mathf.Floor(hit.point.x) - p.getPieceSize().x/2, 
                Mathf.Floor(hit.point.y), 
                Mathf.Floor(hit.point.z) - p.getPieceSize().z / 2);

            piece.transform.rotation = Quaternion.Euler(new Vector3(
                piece.transform.rotation.eulerAngles.x,
                Mathf.Round(piece.transform.rotation.eulerAngles.y / 90) * 90,
                piece.transform.rotation.eulerAngles.z));
        }
    }

    //CHECK FOR ROTATION
    // Verifies if the player is trying to rotate the piece
    void CheckForRotation()
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            p.RotateInX();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            p.RotateInY();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            p.RotateInZ();
        }
    }

}
