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

    private int3 towerSize;
    private PieceType[,,] values;
    private int[,,] IDs;
    private Piece[] pieces;

    //INITIALIZE VALUES
    public void initialize() {

        GameManager.addTower(this);

        idManager = new IDManager();
        idManager.initialize();

        towerSize = new int3(_xSize, _ySize, _zSize);

        values = new PieceType[towerSize.x, towerSize.y, towerSize.z];
        IDs = new int[towerSize.x, towerSize.y, towerSize.z];

        //ASSIGN PROGRAMATICALLY AND ADD DOUBLING FUNCTION
        pieces = new Piece[1024];

        Invoke("tickPieces", tickInterval);

        activate();

        //REMOVE AFTER INITIAL RELEASE
    }

    //ACTIVATE TOWER
    public void activate() {
        active = true;
        Invoke("tickPieces", tickInterval);
    }

    //DEACTIVATE TOWER
    public void deactivate() {
        active = false;
    }

    //TICK FOR ALL PIECES
    void tickPieces() {
        if (active) {
            foreach (Piece P in pieces) {
                if (P != null) {
                    P.tick();
                }
            }

            Invoke("tickPieces", tickInterval);
        }
    }

    //PLACES A PIECE IF IT CAN
    public bool place(Piece piece, int3 position) {
        if (!canPlace(piece, position) || !active) { return false; }
        piece.setID(idManager.getID());
        placePiece(piece, position);
        return true;
    }

    //CHECKS IF A PIECE CAN BE PLACED
    protected bool canPlace(Piece piece, int3 position) {

        if (!checkExtents(piece.getPieceSize(), position)) { return false; }

        bool available = true;

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

    return available;
    }

    //CHECKS IF PIECE IS WITHIN BOUNDARIES
    protected bool checkExtents(int3 pieceSize, int3 position) {

        if ((pieceSize + position) > towerSize) { return false; }
        return true;
    }


    //WRITES TO VALUES OF THIS TOWERCONTROLLER
    protected void placePiece(Piece piece, int3 position) {

        int ID = piece.getID();
        PieceType[,,] pieceValues = piece.getValues();

        int3 pieceSize = piece.getPieceSize();

        for (int x = 0; x < pieceSize.x; x++) {
            for (int y = 0; y < pieceSize.y; y++) {
                for (int z = 0; z < pieceSize.x; z++) {
                    if (piece.getValues()[x, y, z] != PieceType.Empty) {
                        values[position.x + x, position.y + y, position.z + z] = pieceValues[x, y, z];
                        IDs[position.x + x, position.y + y, position.z + z] = ID;

                        pieces[piece.getID()] = piece;
                        //DEBUG BLOCKS HERE
                    }
                }
            }
        }
    }

    //GETS THE REAL WORLD POSITION OF A MATRIX POSITION
    public static int3 worldToMatrixPosition(Vector3 position)
    {
        int3 blockPosition = new int3(0, 0, 0);
        //Tower center - tower radius * vector.one + position * radius * 2
        return blockPosition;
    }

    //GETS THE REAL WORLD POSITION OF A MATRIX POSITION
    public static Vector3 matrixToWorldPosition(int3 position) {
        Vector3 blockPosition = Vector3.zero;
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

        Piece piece = pieces[id];

        foreach (int3 i in piece.getBlockPositions()) {
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
