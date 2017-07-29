using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public struct int3 {
        public int x, y, z;

        public int3(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }

    public struct PieceValues
    {
        public int[,,] blocks;

        public PieceValues(int[,,] values)
        {
            blocks = values;
        }
    }

    [SerializeField]
    GameObject block;

    //[SerializeField] GameObject property;
    [SerializeField]
    int destructionTime;

    protected TetrisMachine parentMachine;
    protected PieceValues[,,] pieceValues;
    protected int timer;
    int3 pieceSize;

        public void generate(PieceValues values, float radius)
    {

        int x = values.blocks.GetLength(0);
        int y = values.blocks.GetLength(1);
        int z = values.blocks.GetLength(2);

        pieceSize = new int3(x, y, z);

        //Vector3 offset = new Vector3(-1f, 1f, 1f);

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (values.blocks[u, v, w] == 1)
                    {
                        //INVERT U TO GO RIGHT TO LEFT
                        GameObject GOBlock = Instantiate(block, transform.position + (new Vector3(-u, v, w) * radius * 2), Quaternion.identity);
                        GOBlock.transform.parent = this.transform;
                        //PASS DURATION TO BLOCK COMPONENT
                    }
                }
            }
        }
    }

    public void initialize() {
        Invoke("pickUp", Random.Range(1f, 10f));
    }

    public void goDown()
    {
        //here we execute de property just in case it's a magnet. --Pending--
        //Notify about the end of the animation
    }

    public void reduceTimer()
    {
        timer--;
    }

    public int3 getPieceSize()
    {
        return pieceSize;
    }

    public void RotateInX() {
    }

    public void RotateInY() {
    }

    public void RotateInZ() {
    }

    public void validPlaceFound() {

    }

    public void validPlaceNotFound() {

    }

    public void pickUp() {
        //TODO MAKE OTHER STUFF
        Destroy(gameObject);
    }

    public TetrisMachine getTetrisMachine()
    {
        return parentMachine;
    }

    public void setTetrisMachine(TetrisMachine tetrisMachine)
    {
        parentMachine = tetrisMachine;
    }
}
