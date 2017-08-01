using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

    [SerializeField]
    protected int goal;
    [SerializeField]
    private float tickInterval;

    [SerializeField]
    private int _xSize;
    [SerializeField]
    private int _ySize;
    [SerializeField]
    private int _zSize;

    [SerializeField]
    protected int[] milestones;

    private IDManager idManager;

    private bool active;

    private Vector3 localPosition;
    private int3 towerSize;
    private PieceType[,,] values;
    private int[,,] IDs;
    private Piece[] pieces;

    //INITIALIZE VALUES
    public void initialize() {

        GameManager.addTower(this);

        localPosition = Helper.snapVector3(transform.position);

        idManager = new IDManager();
        idManager.initialize();

        towerSize = new int3(_xSize, _ySize, _zSize);

        values = new PieceType[towerSize.x, towerSize.y, towerSize.z];
        IDs = new int[towerSize.x, towerSize.y, towerSize.z];

        //ASSIGN PROGRAMATICALLY AND ADD DOUBLING FUNCTION
        pieces = new Piece[1024];

        activate();

        //REMOVE AFTER INITIAL RELEASE
    }

    public void update()
    {
        debugExtents();
    }

    //TICK FOR ALL PIECES
    public void tick() {
        if (active) {
            foreach (Piece P in pieces) {
                if (P != null) {
                    P.tick();
                }
            }
        }
    }

    public void debugMarkPlace(int3 matrix) {
        Vector3 origin = matrixToWorldPosition(matrix);
        Vector3 destination = matrixToWorldPosition(new int3(matrix.x, towerSize.y - 1, matrix.z));
        Debug.DrawLine(origin, destination, Color.green, 2048f);
    }

    //DEBUG
    protected void debugExtents() {
        Vector3 origin = matrixToWorldPosition(new int3(0, 0, 0));
        Vector3 destination = matrixToWorldPosition(new int3(0, towerSize.y, 0));
        Debug.DrawLine(origin, destination, Color.red);

        Debug.DrawLine(matrixToWorldPosition(new int3(towerSize.x, 0, 0)), matrixToWorldPosition(new int3(towerSize.x, towerSize.y, 0)), Color.red);
        Debug.DrawLine(matrixToWorldPosition(new int3(0, 0, towerSize.z)), matrixToWorldPosition(new int3(0, towerSize.y, towerSize.z)), Color.red);
        Debug.DrawLine(matrixToWorldPosition(new int3(towerSize.x, 0, towerSize.z - 1)), matrixToWorldPosition(new int3(towerSize.x, towerSize.y, towerSize.z)), Color.red);
    }

    //ACTIVATE TOWER
    public void activate() {
        active = true;
    }

    //DEACTIVATE TOWER
    public void deactivate() {
        active = false;
    }

    //PLACES A PIECE IF IT CAN
    public bool place(Piece piece, int3 position) {
        piece.setID(idManager.getID());
        placePiece(piece, position);
        return true;
    }

    //CHECKS IF A PIECE CAN BE PLACED
    public bool canPlace(Piece piece, int3 position) {

        if (!checkExtents(piece.getPieceSize(), position)) { return false; }

        int3 pieceSize = piece.getPieceSize();

        for (int x = 0; x < pieceSize.x; x++) {
            for (int y = 0; y < pieceSize.y; y++) {
                for (int z = 0; z < pieceSize.x; z++) {

                    //IF THE SPACE IN TOWER IS EMPTY OR THE SPACE IN THE BLOCK IS EMPTY THEN THERE IS SPACE,
                    //IF NOT SPACE ISNT AVAILABLE
                    if (!(values[position.x + x, position.y + y, position.z + z] == PieceType.Empty ||
                        piece.getValues()[x, y, z] == PieceType.Empty)) {
                        return false;
                    }

                }
            }
        }

    return true;
    }

    //CHECKS IF PIECE IS WITHIN BOUNDARIES
    protected bool checkExtents(int3 pieceSize, int3 position) {
        if (((pieceSize + position) > towerSize) || (position < new int3(0, 0, 0))) { return false; }
        return true;
    }


    //WRITES TO VALUES OF THIS TOWERCONTROLLER
    public void placePiece(Piece piece, int3 position) {

        Debug.Log("placing Piece");

        int ID = piece.getID();
        PieceType[,,] pieceValues = piece.getValues();

        int3 pieceSize = piece.getPieceSize();

        int blockCount = 0;
        int3[] blockPositions = new int3[piece.getBlockAmount()];

        for (int x = 0; x < pieceSize.x; x++) {
            for (int y = 0; y < pieceSize.y; y++) {
                for (int z = 0; z < pieceSize.x; z++) {
                    if (piece.getValues()[x, y, z] != PieceType.Empty) {
                        int3 matrixPosition = new int3(position.x + x, position.y + y, position.z + z);
                        values[matrixPosition.x, matrixPosition.y, matrixPosition.z] = pieceValues[x, y, z];
                        IDs[matrixPosition.x, matrixPosition.y, matrixPosition.z] = ID;
                        blockPositions[blockCount] = matrixPosition;
                        blockCount++;
                        //DEBUG BLOCKS HERE
                    }
                }
            }
        }

        piece.setBlockPositions(blockPositions);
        pieces[piece.getID()] = piece;
    }

    //GETS THE MATRIX POSITION OF A REAL WORLD POSITION
    public int3 worldToMatrixPosition(Vector3 position) {
        Vector3 centerOffset = new Vector3(towerSize.x, towerSize.y, towerSize.z) / 2f;
        Vector3 vectorMatrixPosition = (position - localPosition + centerOffset) /* / radius*/ ;
        int3 matrixPosition = new int3((int)vectorMatrixPosition.x, (int)vectorMatrixPosition.y, (int)vectorMatrixPosition.z);

        return matrixPosition;
    }

    //GETS THE REAL WORLD POSITION OF A MATRIX POSITION
    public Vector3 matrixToWorldPosition(int3 matrixPosition) {
        Vector3 centerOffset = new Vector3(towerSize.x, towerSize.y, towerSize.z) / 2f;
        Vector3 blockPosition = (new Vector3(matrixPosition.x, matrixPosition.y, matrixPosition.z) - centerOffset) /* * radius*/ + localPosition;
        //Tower center - tower radius * vector.one + position * radius * 2
        return blockPosition;
    }

    // CHECK BLOCK STATUS
    // This method is meant to be run anytime that someone wants to see if pieces must fall in the tower
    // WARNING! This method is recursive and will run until no pieces can fall!
    public void checkBlockStatus() {
        //TODO: DO
    }

    //REMOVE PIECE
    // Takes out a piece from the tower
    public void removePiece(int id) {

        Debug.Log("removing");

        Piece piece = pieces[id];

        foreach (int3 i in piece.getBlockPositions()) {
            Debug.Log(i.x + "  " + i.y + "  " + i.z);
            values[i.x, i.y, i.z] = PieceType.Empty;
            IDs[i.x, i.y, i.z] = 0;
        }

        idManager.returnID(id);
    }

    // GET NEIGHBOURING PIECES
    // A method that returns a list of all neighbouring pieces of a certain piece
    // id: the ID for the certain piece

    public Piece[] getNeigbouringPieces (int id) {
        return pieces;
    }

    public Vector3 getHighestBlockOfType(System.Type T) {
        return Vector3.zero;
    }

    public Piece[] getPiecesUpToHeight(int height) {
        return pieces;
    }

    public int[] getMilestones() {
        return milestones;
    }
}
