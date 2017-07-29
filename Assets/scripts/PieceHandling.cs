using System.Collections;
using System.Collections.Generic;
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

        if (piecePivot.childCount > 0)
        {
            CheckForPlacingSpace();
        }


        //On click
        if (Input.GetMouseButtonDown(0))
        {
            // if the player hasn't picked up a piece, let them pick up a piece
            if (piecePivot.transform.childCount == 0) { 
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistanceToInteract, pieceLayer))
                {
                    PickPiece(hit.collider.gameObject);
                }
            } else
            //if the player picked up something, try to place it
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, maxDistanceToInteract, towerLayer))
                {
                    SetPiece(hit.point);
                }
            }
        }
    }


    void PickPiece(GameObject piece)
    {
        Piece p = piece.GetComponent<Piece>();
        p.pickUp();
        piece.transform.SetParent(piecePivot);
        piece.transform.localPosition = new Vector3(0,0,0);
    }


    void SetPiece(Vector3 point)
    {
        GameObject piece = piecePivot.GetChild(0).gameObject;
        Piece p = piece.GetComponent<Piece>();
        int[] position = new int[] {
            Mathf.FloorToInt(point.x),
            Mathf.FloorToInt(point.y),
            Mathf.FloorToInt(point.z)};
        if(TowerController.instance.CheckForPlace(p.getMatrix(), position))
        {
            TowerController.instance.PlacePiece(p.getMatrix(), position);
            piece.transform.parent = null;
        } 
    }
    
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


}
