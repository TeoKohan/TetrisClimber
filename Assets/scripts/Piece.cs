﻿using System.Collections;
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

    [SerializeField]
    GameObject block;

    //[SerializeField] GameObject property;
    [SerializeField]
    int destructionTime;

    protected TetrisMachine parentMachine;
    protected bool[,,] pieceValues;
    protected int id;
    protected int blockAmount;
    protected int timer;
    int3 pieceSize;

        public void generate(int[,,] values, float radius) {

        int x = values.GetLength(0);
        int y = values.GetLength(1);
        int z = values.GetLength(2);

        pieceSize = new int3(x, y, z);
        pieceValues = new bool[x, y, z];

        //Vector3 offset = new Vector3(-1f, 1f, 1f);

        blockAmount = 0;

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (values[u, v, w] >= 1)
                    {
                        //INVERT U TO GO RIGHT TO LEFT
                        pieceValues[u, v, w] = true;
                        GameObject GOBlock = Instantiate(block, transform.position + (new Vector3(-u, v, w) * radius * 2), Quaternion.identity);
                        GOBlock.transform.parent = this.transform;
                        blockAmount++;
                        //PASS DURATION TO BLOCK COMPONENT
                    }
                    else { pieceValues[u, v, w] = false; }
                }
            }
        }
    }

    public void initialize() {
        Invoke("pickUp", Random.Range(1f, 5f));
    }

    public void goDown() {
        //here we execute de property just in case it's a magnet. --Pending--
        //Notify about the end of the animation
    }

    public void reduceTimer() {
        timer--;
    }

    public int3 getPieceSize() {
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
        parentMachine.removePiece(this);
        Destroy(gameObject);
    }

    public int getID()
    {
        return id;
    }

    public int getBlockAmount()
    {
        return blockAmount;
    }

    public bool[,,] getMatrix() {
        return pieceValues;
    }

    public TetrisMachine getTetrisMachine() {
        return parentMachine;
    }

    public void setTetrisMachine(TetrisMachine tetrisMachine) {
        parentMachine = tetrisMachine;
    }
}
