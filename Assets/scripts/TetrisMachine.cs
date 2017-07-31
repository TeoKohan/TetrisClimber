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
    protected float pieceInterval = 2f;
    [SerializeField]
    protected float conveyorSpeed;
    [SerializeField]
    protected int maxPieces = 3;

    [SerializeField]
    protected GameObject conveyorBeltPiece;

    [SerializeField]
    protected Transform spawnpoint;

    [SerializeField]
    protected GameObject[] pieceTypes;
    [SerializeField]
    protected float[] spawnrate;


    protected TextAsset[] blocks;
    protected int currentPieces;
    protected bool[] pieceSlots;
    protected ConveyorBeltPiece[] conveyorBelt;
    protected PieceState[] pieces;
    protected int id;

    void Start() {
        initialize();
        InvokeRepeating("generatePiece", pieceInterval, pieceInterval);
    }

    void Update() {
        if (pieces != null) { 
            for (int i = 0; i < maxPieces; i++) {
                if (pieces[i].piece != null && pieces[i].percentage < 1f) {
                    pieces[i].percentage += conveyorSpeed * Time.deltaTime;
                    pieces[i].piece.transform.position = Vector3.Lerp(pieces[i].origin, pieces[i].destination, Mathf.Clamp01(pieces[i].percentage / pieces[i].goal));
                }
            }
        }
    }

    //METHODS

    //DEBUG
    private void generateDebugPiece() {
        int randomPiece = getRandomBlock();
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

        bool debugBool = true;
        for (int i = 0; i < maxPieces; i++) {
            if (pieces[i].piece != null && pieces[i].percentage <= 0.25f)
            {
                debugBool = false;
                break;
            }
        }

        if (pieceSlots[maxPieces-1] == false && debugBool) {
        //if (debugBool) {
            addCurrentPiece();
            int slot = getPieceSlot();
            pieceSlots[slot] = true;
            GameObject GOPiece = Instantiate(pieceTypes[getRandomBlockType()], spawnpoint.position, Quaternion.identity);
            GOPiece.name = "Slot: " + slot;
            Piece pieceScript = GOPiece.GetComponent<Piece>();
            pieceScript.setTetrisMachine(this);
            pieceScript.generate(selectRandomBlock(), radius);
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
            if (removePieceID == pieces[i].id) {
                pieceSlots[pieces[i].slot] = false;
                Debug.Log("Freeing slot: " + pieces[i].slot);
                pieces[i].piece = null;
                removeCurrentPiece();
                pushBackPieces(pieces[i].slot);
                debugSlots();
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

        Object[] tempBlocks = Resources.LoadAll("blocks");
        blocks = new TextAsset[tempBlocks.Length];
        for (int i = 0; i < tempBlocks.Length; i++) {
            blocks[i] = tempBlocks[i] as TextAsset;
        }
    }

    protected void deployConveyorBelt(int length) {
        for (int i = 0; i < length; i++) {
            GameObject GOConveyor = Instantiate(conveyorBeltPiece, transform.position + transform.forward * (i * radius * 6 + radius * 9), transform.rotation);
            GOConveyor.name = "conveyor_" + (length - 1 - i);
            conveyorBelt[length - 1 - i] = GOConveyor.GetComponent<ConveyorBeltPiece>();
        }
    }

    protected int generateID() {
        int tempID = id;
        id++;
        return tempID;
    }

    protected int[,,] selectRandomBlock() {
        return parsePiece(getRandomBlock());
    }

    protected void pushBackPieces(int removeSlot) {
        for (int i = removeSlot ; i < maxPieces; i++) {
            Debug.Log("Slot: " + i + " " + pieceSlots[i]);
            if (pieceSlots[i] == true)
            {
                int slotIndex = -1;

                for (int j = 0; j < maxPieces; j++)
                {
                    if (pieces[j].slot == i) { slotIndex = j; break; }
                }

                int moveSlots = isClearBehind(pieces[slotIndex].slot);

                Debug.Log(i + " Has " + moveSlots + " spaces behind.");

                if (pieces[slotIndex].piece != null && moveSlots > 0)
                {

                    int newSlot = pieces[slotIndex].slot - moveSlots;

                    pieces[slotIndex].origin = pieces[slotIndex].piece.getPosition();
                    pieces[slotIndex].destination = getPieceDestination(newSlot);
                    pieces[slotIndex].goal = 1f - ((1f / maxPieces) * (newSlot));
                    pieces[slotIndex].percentage = pieces[slotIndex].goal - 1f - ((1f / maxPieces) * (pieces[slotIndex].slot));

                    pieceSlots[pieces[slotIndex].slot] = false;
                    pieces[slotIndex].slot = newSlot;
                    pieceSlots[newSlot] = true;

                    pieces[slotIndex].piece.name = "Slot: " + newSlot;
                }
            }
        }
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

    protected int getRandomBlock() {
        int randomValue = Random.Range(0, blocks.Length);
        return randomValue;
    }

    protected int getRandomBlockType() {
        float totalPercentage = 0f;
        float[] newPercentages = new float[spawnrate.Length];
        for (int i = 0; i < spawnrate.Length; i++) {
            totalPercentage += spawnrate[i];
            newPercentages[i] = totalPercentage;
        }

        float random = Random.Range(0f, totalPercentage);

        for (int i = 0; i < spawnrate.Length; i++)
        {
            if (random < newPercentages[i]) {
                return i;
            }
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
            for (int w = 0; w < z; w++)
            {
                for (int v = 0; v < y; v++)
                {
                    //Debug.Log(values[u, v, w]);
                    values[u, v, w] = (int)char.GetNumericValue(fields[2][u + w * x + v * x * z]);
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
        int slot = -1;

        for (int i = maxPieces - 1; i >= 0; i--)
        {
            if (pieceSlots[i] == false && isClearAhead(i))
            {
                slot = i;
            }
        }
        return slot;
    }

    protected bool isClearAhead(int slot) {
        for (int i = slot; i < maxPieces; i++) {
            if (pieceSlots[i] == true) {
                return false;
            }
        }
        return true;
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
