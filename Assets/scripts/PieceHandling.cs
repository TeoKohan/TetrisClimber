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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, maxDistanceToInteract, pieceLayer))
            {
                PickPiece(hit.collider.gameObject);
            }

        }
    }


    void PickPiece(GameObject piece)
    {
        piece.transform.SetParent(piecePivot);
        piece.transform.localPosition = new Vector3(0,0,0);
    }


}
