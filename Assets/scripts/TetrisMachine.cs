using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisMachine : MonoBehaviour {

    //CONSTANTS
    protected const float radius = 0.5f;

    public struct PieceState
    {
        public Piece piece;
        public Vector3 destination;
        public float percentage;
        public float goal;

        public PieceState(Piece piece, Vector3 destination, float goal) {
            this.piece = piece;
            this.destination = destination;
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
    protected ConveyorBeltPiece[] conveyorBelt;
    protected PieceState[] pieces;

    void Start() {
        initialize();
        InvokeRepeating("generatePiece", 2f, 2f);
        generateDebugPiece();
    }

    void Update() {
        for (int i = 0; i < currentPieces; i++) {
            if (pieces[i].piece != null && pieces[i].percentage < 1f) {
                pieces[i].percentage += conveyorSpeed * Time.deltaTime;
                pieces[i].piece.transform.position = Vector3.Lerp(spawnpoint.position, pieces[i].destination, Mathf.Clamp01(pieces[i].percentage / pieces[i].goal));
            }
        }
    }

    //METHODS

    //DEBUG
    private void generateDebugPiece() {
        int randomPiece = getRandomPieceIndex();
        drawDebugPiece(parsePiece(randomPiece));
    }

    private void drawDebugPiece(Piece.PieceValues pieceValues) {

        float cubeRadius = 0.5f;
        float duration = 2048f;
        int x = pieceValues.blocks.GetLength(0);
        int y = pieceValues.blocks.GetLength(1);
        int z = pieceValues.blocks.GetLength(2);

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (pieceValues.blocks[u, v, w] == 1)
                    {
                        //SHOULD INVERT U TO GO RIGHT TO LEFT
                        DrawAssist.drawCube(new Vector3(u, v, w), cubeRadius, duration);
                    }
                }
            }
        }
    }

    //PUBLIC

    //TODO:
    //ADD AND REMOVE BLOCKS AT RUNTIME

    public void generatePiece() {
        if (currentPieces < maxPieces) {
            currentPieces++;
            GameObject GOPiece = Instantiate(piece, spawnpoint.position, Quaternion.identity);
            Piece pieceScript = GOPiece.GetComponent<Piece>();
            pieceScript.setTetrisMachine(this);
            pieceScript.generate(selectRandomPiece(), radius);
            pieceScript.initialize();
            PieceState P = new PieceState(pieceScript, getPieceDestination(), 1f - ((1f / maxPieces) * (getPieceSlot() - 1)));
            pieces[currentPieces - 1] = P;
        }
    }

    public void removePiece(Piece p) {

    }

    public void addCurrentPiece()
    {
        currentPieces++;
    }

    public void removeCurrentPiece()
    {
        currentPieces--;
    }

    //PROTECTED

    protected void initialize() {
        currentPieces = 0;
        conveyorBelt = new ConveyorBeltPiece[maxPieces];
        pieces = new PieceState[maxPieces];
        deployConveyorBelt(maxPieces);
    }

    protected void deployConveyorBelt(int length) {
        for (int i = 0; i < length; i++) {
            GameObject GOConveyor = Instantiate(conveyorBeltPiece, transform.position + transform.forward * (i * radius * 6 + radius * 9), transform.rotation);
            conveyorBelt[i] = GOConveyor.GetComponent<ConveyorBeltPiece>();
        }
    }

    protected Piece.PieceValues selectRandomPiece() {
        return parsePiece(getRandomPieceIndex());
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

    protected Piece.PieceValues parsePiece(int index)
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

        Piece.PieceValues pieceValues = new Piece.PieceValues(values);
        return pieceValues;
    }

    protected Vector3 getPieceDestination() {
        return conveyorBelt[maxPieces - (currentPieces)].getRestPosition().position;
    }

    protected int getPieceSlot() {
        return currentPieces;
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
