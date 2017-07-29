using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    [SerializeField] GameObject property;
    [SerializeField] int destructionTime;

    bool[,,] matrix;
    int timer;
    Vector3 position;

    private void Awake()
    {
        //initializing
        timer = destructionTime;
        position = transform.position;
        //here we execute de property just in case it's a magnet. --Pending--
    }

    public void goDown()
    {
        //here we execute de property just in case it's a magnet. --Pending--
        position -= new Vector3(0, 1, 0);
        //Notify about the end of the animation
    }

    public void reduceTimer()
    {
        timer--;
    }

    public void generatePiece(int xSize, int ySize, int zSize)
    {
        matrix = new bool[xSize, ySize, zSize];
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public bool[,,] getMatrix()
    {
        return matrix;
    }
}
