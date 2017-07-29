using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisMachine : MonoBehaviour {

    public struct PieceValues
    {
        public int[,,] blocks;

        public PieceValues(int[,,] values)
        {
            blocks = values;
        }
    }

    [SerializeField]
    protected TextAsset[] blocks;
    [SerializeField]
    protected float[] spawnrate;
    [SerializeField]
    protected float conveyorSpeed;
    [SerializeField]
    protected int maxPieces;
    [SerializeField]
    protected int currentPieces;

    void Start() {
        generateDebugPiece();
    }

    void Update() {

    }

    //METHODS

    //DEBUG
    private void generateDebugPiece() {
        int randomPiece = getRandomPiece();
        drawDebugPiece(parsePiece(randomPiece));
    }

    private void drawDebugPiece(PieceValues pieceValues) {

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
        //TODO:
        //INSTANTIATE PIECE GAMEOBJECT
        //PASS PIECEVALUES TO INSTANTIATED GAMEOBJECT
        //RUN GENERATE ON INSTANTIATED PIECE COMPONENT
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
    protected int getRandomPiece() {
        float randomValue = Random.Range(0f, 100f);

        float percent = 0f;
        for (int i = 0; i < blocks.Length; i++) {
            percent += spawnrate[i];
            if (randomValue <= percent) { return i; }
        }
        return 0;
    }

    protected PieceValues parsePiece(int index)
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

        PieceValues pieceValues = new PieceValues(values);
        return pieceValues;
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
