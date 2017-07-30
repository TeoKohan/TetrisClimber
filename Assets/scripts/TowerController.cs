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
    [SerializeField]
    private float tickDuration;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;

            //let's set up the slot matrix
            floorSpaces = new int[xSize,ySize,zSize];
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    for (int z = 0; z < zSize; z++)
                    {
                        floorSpaces[x, y, z] = -1;
                    }
                }
            }
            //and the list for all involved pieces
            pieces = new List<Piece>();
            
            //and make the graphic base larger
            floorGraphic.localPosition = new Vector3(xSize / 2, floorGraphic.localPosition.y, zSize / 2);
            floorGraphic.localScale = new Vector3(xSize, floorGraphic.localScale.y, zSize);

            // Start ticking!
            Invoke("NotifyTick",tickDuration);
        }
    }



    //TICK
    //Notify pieces that they should move in time (?)
    void NotifyTick()
    {
        foreach (Piece p in pieces)
        {
            p.tick();
        }
        Invoke("NotifyTick", tickDuration);
    }



    // CHECK FOR PLACE

    //This method will tell you if there's enough space in the tower for the piece you want to place
    // piece: the matrix for any given piece
    // position: the position within the tower that was clicked by the user

    public bool CheckForPlace(Piece piece, int[] position)
    {
        bool[,,] p = piece.getMatrix();
        // Exception logic: if the piece is too big, it shouldn't fit
        if (piece.getPieceSize().x + position[0] > floorSpaces.GetLength(0) ||
            piece.getPieceSize().y + position[1] > floorSpaces.GetLength(1) ||
            piece.getPieceSize().z + position[2] > floorSpaces.GetLength(2)) {

            return false;
        } else {
            //assuming the desired reference point in the piece is 0,0,0
            for (var x = 0; x < xSize && x < piece.getPieceSize().x; x++)
            {
                for (var y = 0; y < ySize && y < piece.getPieceSize().y; y++)
                {
                    for (var z = 0; z < zSize && z < piece.getPieceSize().z; z++)
                    {
                        // if there's a cube in the piece AND there's a cube in the floor
                        if (p[x,y,z] && 
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
        for (var x = 0; x < xSize && x < piece.getPieceSize().x; x++)
        {
            for (var y = 0; y < ySize && y < piece.getPieceSize().y; y++)
            {
                for (var z = 0; z < zSize && z < piece.getPieceSize().z; z++)
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

        Vector3 piecePosition = transform.position + new Vector3(position[0] + piece.getPieceSize().x / 2, position[1], position[2] + piece.getPieceSize().z / 2);

        return piecePosition;
    }


    

    // CHECK BLOCK STATUS

    // This method is meant to be run anytime that someone wants to see if pieces must fall in the tower
    // WARNING! This method is recursive and will run until no pieces can fall!

    public void CheckBlockStatus()
    {
        // we'll use some basic variables to optimize this beauty
        List<int> checkedPieces = new List<int>();
        bool somethingWentDown = false;

        for (var x = 0; x < xSize; x++)
        {
            for (var y = 0; y < ySize; y++)
            {
                for (var z = 0; z < zSize; z++)
                {
                    // if there's a cube in the spot and I haven't checked that piece yet, find its piece
                    if (floorSpaces[x, y, z] >= 0 && 
                        checkedPieces.FindAll(i => i == floorSpaces[x, y, z]).Count <= 0)
                    {
                        Piece piece = pieces.Find(p => p.getID() == floorSpaces[x, y, z]);
                        bool goesDown = true;

                        //i get the coordinates in my matrix for all the blocks in the piece
                        List<int3> blocksCoordinates = CoordinatesOf<int>(floorSpaces, piece.getID());

                        //and check if they can go down
                        foreach (int3 coord in blocksCoordinates)
                        {
                            //if the block's not at the bottom, it may need to go down
                            if (coord.y != 0) { 
                                int3 newCoord = new int3(coord.x, coord.y - 1, coord.z);
                                // if the coordinates BELOW that block is occupied, it can't go down
                                if (floorSpaces[newCoord.x, newCoord.y, newCoord.z] >=0) {
                                    goesDown = false;
                                    checkedPieces.Add(piece.getID());
                                    break;
                                }
                            }
                            else
                            {
                                //if the block's at the bottom, it can't possibly go down.
                                goesDown = false;
                                checkedPieces.Add(piece.getID());
                                break;
                            }
                        }
                        
                        //if the piece can go down, then it should
                        if (goesDown)
                        {
                            piece.goDown();
                            somethingWentDown = true;
                        }

                        //anyway, we've checked this one
                        checkedPieces.Add(piece.getID());

                    }
                }
            }
        }

        //if anything moved down, then we have to recheck everything just in case. Yikes!
        if (somethingWentDown)
        {
            CheckBlockStatus();
        }

    }


    //accessory method for the CheckBlockStatus method, pretty useful shit!
    public static List<int3> CoordinatesOf<T>(T[,,] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height
        int d = matrix.GetLength(2); // height
        List<int3> coordinates = new List<int3>();

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                for (int z = 0; z < d; ++z)
                {
                    if (matrix[x, y, z].Equals(value)) {
                        coordinates.Add(new int3(x, y, z));
                    }
                }
            }
        }

        return coordinates;
    }


    //REMOVE PIECE
    // Takes out a piece from the tower

    public void RemovePiece(int id)
    {
        pieces.Remove(pieces.Find(p => p.getID() == id));
        List<int3> pieceCoordinates = CoordinatesOf<int>(floorSpaces, id);
        foreach (int3 coord in pieceCoordinates)
        {
            floorSpaces[coord.x, coord.y,coord.z] = -1;
        }
    }


    //IS WITHIN TOWER
    // Checks if a point is within the tower
    public bool IsWithinTower(Vector3 point)
    {
        if (point.x > transform.position.x && point.x < transform.position.x + xSize &&
            point.y > transform.position.y && point.y < transform.position.y + ySize &&
            point.z > transform.position.z && point.z < transform.position.z + zSize)
        {
            return true;
        }
        return false;
    }

    // GET NEIGHBOURING PIECES
    // A method that returns a list of all neighbouring pieces of a certain piece
    // id: the ID for the certain piece

    public List<Piece> GetNeigbouringPieces (int id)
    {
        List<Piece> neighbours = new List<Piece>();

        List<int3> pieceCoordinates = CoordinatesOf<int>(floorSpaces, id);

        //foreach block in the piece
        foreach (int3 coord in pieceCoordinates)
        {
            //check out its neighbours
            for (var x = coord.x - 1; x <= coord.x + 1 && x < xSize; x++)
            {
                if (x < 0)
                    x = 0;
                for (var y = coord.y - 1; y <= coord.y + 1 && y < ySize; y++)
                {
                    if (y < 0)
                        y = 0;
                    for (var z = coord.z - 1; z <= coord.z + 1 && z < zSize; x++)
                    {
                        if (z < 0)
                            z = 0;
                        //if they're not part of our piece and we haven't found them before, add them to the list
                        if (floorSpaces[x,y,z] != id && 
                            neighbours.FindAll(p =>p.getID() == floorSpaces[x, y, z]).Count == 0)
                        {
                            neighbours.Add(pieces.Find(p => p.getID() == floorSpaces[x,y,z]));
                        }
                    }
                }
            }
        }

        return neighbours;
    }


}
