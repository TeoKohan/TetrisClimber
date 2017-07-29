using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    private TowerController instance;
    private bool[,,] floorSpaces;

    [SerializeField]
    private int xSize;
    [SerializeField]
    private int ySize;
    [SerializeField]
    private int zSize;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            floorSpaces = new bool[xSize,ySize,zSize];
        }
    }



    // CHECKFORPLACE

    //This method will tell you if there's enough space in the tower for the piece you want to place
    // piece: the matrix for any given piece
    // position: the position within the tower that was clicked by the user

    public bool CheckForPlace(bool[,,] piece, int[] position)
    {
        //assuming the desired reference point in the piece is 0,0,0
        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    // if there's a cube in the piece AND there's a cube in the floor
                    if (piece[x,y,z] && 
                        floorSpaces[position[0]+z, position[1]+y, position[2]+z])
                    {
                        //break!
                        return false;
                    }
                }
            }
        }

        return true;
    }




    //PLACEPIECE

    //This method effectively sets a piece in the tower
    // piece: the matrix for any given piece
    // position: the position within the tower that was clicked by the user

    public void PlacePiece(bool[,,] piece, int[] position)
    {
        //assuming the desired reference point in the piece is 0,0,0
        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    // if there's a cube in the piece, set it in the matrix
                    if (piece[x, y, z])
                    {
                        floorSpaces[position[0] + z, position[1] + y, position[2] + z] = true;
                    }
                }
            }
        }
    }



}
