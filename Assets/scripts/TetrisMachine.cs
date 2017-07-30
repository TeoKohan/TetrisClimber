using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisMachine : MonoBehaviour {

    //CONSTANTS
    protected const float radius = 0.5f;

    public struct PieceState
    {
        public Piece piece;
        public Vector3 origin;
        public Vector3 destination;
        public int id;
        public int slot;
        public float percentage;
        public float goal;

        public PieceState(Piece piece, Vector3 origin, Vector3 destination, int id, int slot, float goal) {
            this.piece = piece;
            this.origin = origin;
            this.destination = destination;
            this.id = id;
            this.slot = slot;
            this.goal = goal;
            percentage = 0f;
        }
    }


    [SerializeField]
    protected int maxPieces;

    [SerializeField]
    protected GameObject piece;
    [SerializeField]
    protected GameObject conveyorBeltPiece;

    [SerializeField]
    protected Transform spawnpoint;
    [SerializeField]
    protected TextAsset[] blocks;

    [SerializeField]
    protected float[] spawnrate;
    [SerializeField]
    protected float conveyorSpeed;


    protected int currentPieces;
    protected bool[] pieceSlots;
    protected ConveyorBeltPiece[] conveyorBelt;
    protected PieceState[] pieces;
    protected int id;

    void Start() {
        initialize();
        InvokeRepeating("generatePiece", 2f, 2f);
    }

    void Update() {
        for (int i = 0; i < maxPieces; i++) {
            if (pieces[i].piece != null && pieces[i].percentage < 1f) {
                pieces[i].percentage += conveyorSpeed * Time.deltaTime;
                pieces[i].piece.transform.position = Vector3.Lerp(pieces[i].origin, pieces[i].destination, Mathf.Clamp01(pieces[i].percentage / pieces[i].goal));
            }
        }
    }

    //METHODS

    //DEBUG
    private void generateDebugPiece() {
        int randomPiece = getRandomPieceIndex();
        drawDebugPiece(parsePiece(randomPiece));
    }

    private void drawDebugPiece(int[,,] pieceValues) {

        float cubeRadius = 0.5f;
        float duration = 2048f;
        int x = pieceValues.GetLength(0);
        int y = pieceValues.GetLength(1);
        int z = pieceValues.GetLength(2);

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (pieceValues[u, v, w] >= 1)
                    {
                        //SHOULD INVERT U TO GO RIGHT TO LEFT
                        DrawAssist.drawCube(new Vector3(u, v, w), cubeRadius, duration);
                    }
                }
            }
        }
    }

    private void debugSlots() {
        string debug = "";
        for (int i = 0; i < maxPieces; i++) {
           if (pieceSlots[i]) { debug += "1  ";}
           else { debug += "0  "; }
        }
        Debug.Log(debug);
    }

    //PUBLIC

    //TODO:
    //ADD AND REMOVE BLOCKS AT RUNTIME

    public void generatePiece() {
        if (pieceSlots[maxPieces-1] == false) {
            addCurrentPiece();
            int slot = getPieceSlot();
            pieceSlots[slot] = true;
            GameObject GOPiece = Instantiate(piece, spawnpoint.position, Quaternion.identity);
            Piece pieceScript = GOPiece.GetComponent<Piece>();
            pieceScript.setTetrisMachine(this);
            pieceScript.generate(selectRandomPiece(), radius);
            pieceScript.setID(generateID());
            pieceScript.initialize();
            PieceState P = new PieceState(pieceScript, spawnpoint.position, getPieceDestination(slot), pieceScript.getID(),  slot, 1f - ((1f / maxPieces) * (slot)));

            for (int i = 0; i < maxPieces; i++) {
                if (pieces[i].piece == null) {
                    pieces[i] = P;
                    //IMPORTANT
                    break;
                }
            }
            
            debugSlots();
        }
    }

    public void removePiece(int removePieceID) {
        for (int i = 0; i < maxPieces; i++) {
            Debug.Log("attempt: " + removePieceID + "   " + pieces[i].id);
            if (removePieceID == pieces[i].id) {
                Debug.Log("Removed from slot");
                pieceSlots[pieces[i].slot] = false;
                pieces[i].piece.destroyPiece();
                pieces[i].piece = null;
                debugSlots();
                removeCurrentPiece();
                pushBackPieces();
                return;
            }
        }
    }

    //PROTECTED

    protected void initialize() {
        currentPieces = 0;
        id = 0;
        pieceSlots = new bool[maxPieces];
        conveyorBelt = new ConveyorBeltPiece[maxPieces];
        pieces = new PieceState[maxPieces];

        for(int i = 0; i < maxPieces; i++) {
            pieces[i] = new PieceState(null, Vector3.zero, Vector3.zero, -1, -1, 0f);
        }

        deployConveyorBelt(maxPieces);
    }

    protected void deployConveyorBelt(int length) {
        for (int i = 0; i < length; i++) {
            GameObject GOConveyor = Instantiate(conveyorBeltPiece, transform.position + transform.forward * (i * radius * 6 + radius * 9), transform.rotation);
            conveyorBelt[length - 1 - i] = GOConveyor.GetComponent<ConveyorBeltPiece>();
        }
    }

    protected int generateID() {
        int tempID = id;
        id++;
        return tempID;
    }

    protected int[,,] selectRandomPiece() {
        return parsePiece(getRandomPieceIndex());
    }

    /*
     * int newSlot = pieces[j].slot - moveSlots;
                    pieces[j].destination = getPieceDestination(newSlot);
                    pieces[j].goal = 1f - ((1f / maxPieces) * (newSlot));
                    pieces[j].percentage = pieces[j].goal - 1f - ((1f / maxPieces) * (pieces[j].slot));
                    pieces[j].slot = newSlot;
     */

    protected void pushBackPieces() {
        for (int i = 1; i < maxPieces; i++) {
            if (pieces[i].piece != null && isClearBehind(pieces[i].slot) > 0) {
                int moveSlots = isClearBehind(pieces[i].slot);
                int newSlot = pieces[i].slot - moveSlots;
                pieces[i].origin = pieces[i].piece.getPosition();
                pieces[i].destination = getPieceDestination(newSlot);
                pieces[i].goal = 1f - ((1f / maxPieces) * (newSlot));
                pieces[i].percentage = pieces[i].goal - 1f - ((1f / maxPieces) * (pieces[i].slot));

                pieceSlots[pieces[i].slot] = false;
                pieces[i].slot = newSlot;
                pieceSlots[pieces[i].slot] = true;
            }
        }
    }

    protected int getRandomPieceIndex() {
        float randomValue = Random.Range(0f, 100f);

        float percent = 0f;
        for (int i = 0; i < blocks.Length; i++) {
            percent += spawnrate[i];
            if (randomValue <= percent) { return i; }
        }
        return 0;
    }

    protected int[,,] parsePiece(int index)
    {
        string text = blocks[index].text;
        string[] fields = text.Split(';');

        string name = fields[0];

        string[] dimensions = fields[1].Split(',');

        int x, y, z;
        if (!int.TryParse(dimensions[0], out x)) { x = 1; };
        if (!int.TryParse(dimensions[1], out y)) { y = 1; };
        if (!int.TryParse(dimensions[2], out z)) { z = 1; };

        fields[2] = fields[2].Replace("\r\n", string.Empty);

        int[,,] values = new int[x, y, z];
        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {

                    //INVERTING U, SWAPPING GLOBAL Y AND Z, LOCAL V AND W; 
                    values[x - u - 1, w, v] = (int)char.GetNumericValue(fields[2][u + w * x + v * x * y]);
                }
            }
        }

        return values;
    }

    protected Vector3 getPieceDestination() {
        for (int i = 0; i < maxPieces; i++) {
            if (pieceSlots[i] != true) {
                return conveyorBelt[i].getRestPosition().position;
            }
        }
        return Vector3.zero;
    }

    protected Vector3 getPieceDestination(int index)
    {
        return conveyorBelt[index].getRestPosition().position;
    }

    protected int getPieceSlot() {

        for (int i = 0; i < maxPieces; i++)
        {
            if (pieceSlots[i] == false && isClearAhead(i))
            {
                return i;
            }
        }
        return -1;
    }

    protected bool isClearAhead(int slot) {
        for (int i = slot; i < maxPieces; i++) {
            if (pieceSlots[i] == true) {
                return false;
            }
        }
        return true;
    }

    protected int isClearBehind(int slot)
    {
        int clearSlots = 0;
        for (int i = slot; i >= 0; i--)
        {
            if (pieceSlots[i] == true)
            {
                clearSlots++;
            }
            else { return clearSlots; }
        }
        return 0;
    }

    protected void addCurrentPiece()
    {
        currentPieces++;
    }

    protected void removeCurrentPiece()
    {
        currentPieces--;
    }

    //GETTERS & SETTERS
    public float[] getSpawnRates()
    {
        return spawnrate;
    }

    public void setSpawnRates(float[] rates)
    {
        spawnrate = rates;
    }

    public float getConveyorSpeed () {
        return conveyorSpeed;
    }

    public void setConveyorSpeed(float speed) {
        conveyorSpeed = speed;
    }

    public int getMaxPieces()
    {
        return maxPieces;
    }

    public void setMaxPieces(int max)
    {
        maxPieces = max;
    }

    public int getCurrentPieces()
    {
        return currentPieces;
    }

    public void setCurrentPieces(int current)
    {
        currentPieces = current;
    }
}
