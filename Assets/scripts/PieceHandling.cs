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
            PickUpPiece(hit.collider.gameObject);
        }
    }


    //PICK UP PIECE
    // Picks the piece and adds it to the holding spot
    void PickUpPiece(GameObject piece)
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

        if (Physics.Raycast(ray, out hit, maxDistanceToInteract, towerLayer))
        {
            PlacePiece(hit.point);
        }
    }

    //PLACE PIECE
    // Locates a piece in a spot
    void PlacePiece(Vector3 point)
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();
        int[] position = new int[] {
            Mathf.FloorToInt(point.x),
            Mathf.FloorToInt(point.y),
            Mathf.FloorToInt(point.z)};
        if(TowerController.instance.CheckForPlace(p.getMatrix(), position))
        {
            Vector3 newPosition = TowerController.instance.PlacePiece(p, position);
            piece.transform.parent = TowerController.instance.transform;
            piece.transform.position = newPosition;
        } 
    }


    //CHECK FOR PLACING SPACE
    // Verifies if the player is aiming to a valid placing spot for the piece
    void CheckForPlacingSpace()
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, maxDistanceToInteract, towerLayer))
        {
            int[] position = new int[] {
            Mathf.FloorToInt(hit.point.x),
            Mathf.FloorToInt(hit.point.y),
            Mathf.FloorToInt(hit.point.z)};
            if (TowerController.instance.CheckForPlace(p.getMatrix(), position))
            {
                p.validPlaceFound();
            } else
            {
                p.validPlaceNotFound();
            }
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
