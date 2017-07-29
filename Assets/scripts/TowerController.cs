using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    static public TowerController instance;
    private int[,,] floorSpaces;
    private List<Piece> pieces;

    public struct int3
    {
        public int x, y, z;

        public int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }

    [SerializeField]
    private int xSize;
    [SerializeField]
    private int ySize;
    [SerializeField]
    private int zSize;
    [SerializeField]
    private Transform floorGraphic;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            floorSpaces = new int[xSize,ySize,zSize];
            pieces = new List<Piece>();

            //we modify the boxcollider to pick up clicks in the tower
            gameObject.GetComponent<BoxCollider>().center = new Vector3(xSize / 2, ySize/2, zSize/2);
            gameObject.GetComponent<BoxCollider>().size = new Vector3(xSize, ySize, zSize);

            //and make the graphic base larger
            floorGraphic.localPosition = new Vector3(xSize / 2, floorGraphic.localPosition.y, zSize / 2);
            floorGraphic.localScale = new Vector3(xSize, floorGraphic.localScale.y, zSize);
        }
    }



    // CHECK FOR PLACE

    //This method will tell you if there's enough space in the tower for the piece you want to place
    // piece: the matrix for any given piece
    // position: the position within the tower that was clicked by the user

    public bool CheckForPlace(bool[,,] piece, int[] position)
    {
        // Exception logic: if the piece is too big, it shouldn't fit
        if (piece.GetLength(0) + position[0] > floorSpaces.GetLength(0) ||
            piece.GetLength(1) + position[1] > floorSpaces.GetLength(1) ||
            piece.GetLength(2) + position[2] > floorSpaces.GetLength(2)) {

            return false;
        } else {
            //assuming the desired reference point in the piece is 0,0,0
            for (var x = 0; x < xSize; x++)
            {
                for (var y = 0; y < ySize; y++)
                {
                    for (var z = 0; z < zSize; z++)
                    {
                        // if there's a cube in the piece AND there's a cube in the floor
                        if (piece[x,y,z] && 
                            floorSpaces[position[0]+z, position[1]+y, position[2]+z] >= 0)
                        {
                            //break!
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }




    //PLACE PIECE

    //This method effectively sets a piece in the tower
    // piece: the matrix for any given piece
    // position: the position within the tower that was clicked by the user
    // returns the vector3 for the desired position of the piece

    public Vector3 PlacePiece(Piece piece, int[] position)
    {
        //assuming the desired reference point in the piece is 0,0,0
        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    // if there's a cube in the piece, set it in the matrix
                    if (piece.getMatrix()[x, y, z])
                    {
                        floorSpaces[position[0] + z, position[1] + y, position[2] + z] = piece.getID() ;
                    }
                }
            }
        }

        pieces.Add(piece);

        Vector3 piecePosition = new Vector3(piece.getPieceSize().x / 2, piece.getPieceSize().y / 2, piece.getPieceSize().z / 2);
        piecePosition += transform.position + new Vector3(position[0], position[1], position[2]);

        return piecePosition;
    }


    

    // CHECK BLOCK STATUS

    // This method is meant to be run anytime that someone wants to see if pieces must fall in the tower
    // WARNING! This method is recursive and will run until no pieces can fall!

    public void CheckBlockStatus()
    {

        List<int> checkedPieces = new List<int>();
        bool somethingWentDown = false;
        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    // if there's a cube in the spot and I haven't checked that piece yet, find its piece
                    if (floorSpaces[x, y, z] >= 0 && checkedPieces.FindAll(i => i == floorSpaces[x, y, z]).Count <= 0)
                    {
                        Piece piece = pieces.Find(p => p.getID() == floorSpaces[x, y, z]);
                        bool goesDown = true;
                        for ( var i = 0; i < piece.getBlockAmount(); i++)
                        {
                            int3 coord = CoordinatesOf<int>(floorSpaces, floorSpaces[x, y, z]);
                            if (coord.y != 0) { 
                                coord = new int3(coord.x, coord.y - 1, coord.z);
                                if (floorSpaces[coord.x, coord.y, coord.z] >=0) {
                                    goesDown = false;
                                    checkedPieces.Add(piece.getID());
                                    break;
                                }
                            }
                            else
                            {
                                goesDown = false;
                                checkedPieces.Add(piece.getID());
                                break;
                            }
                        }
                        
                        if (goesDown)
                        {
                            piece.goDown();
                            somethingWentDown = true;
                        }
                        checkedPieces.Add(piece.getID());

                    }
                }
            }
        }

        if (somethingWentDown)
        {
            CheckBlockStatus();
        }

    }



    public static int3 CoordinatesOf<T>(T[,,] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height
        int d = matrix.GetLength(2); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                for (int z = 0; z < d; ++z)
                {
                    if (matrix[x, y, z].Equals(value))
                        return new int3(x, y, z);
                }
            }
        }

        return new int3(-1, -1, -1);
    }


}
