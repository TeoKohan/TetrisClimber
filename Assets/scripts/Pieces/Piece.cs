using System.Collections;
using System.Collections.Generic;
using UnityScript;
using UnityEngine;

public class Piece : MonoBehaviour
{

    const int healthSteps = 5;

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
    protected PieceTypes pieceType;
    [SerializeField]
    protected GameObject block;

    //[SerializeField] GameObject property;
    [SerializeField]
    int maxHealth;

    protected TetrisMachine parentMachine;
    protected GameObject[] blocks;
    protected bool onTower;
    protected bool[,,] pieceValues;
    protected int id;
    protected int health;
    protected float radius;
    protected float nextHealthStep;
    protected int blockAmount;
    int3 pieceSize;


    //VIRTUAL METHODS
    public virtual void steppedOn()
    {

    }

    public virtual void onPlace()
    {

    }

    public virtual void onTowerUpdate()
    {

    }

    //PUBLIC
    public void generate(int[,,] values, float radius)
    {

        this.radius = radius;

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
                        GameObject GOBlock = Instantiate(block, transform.position + (new Vector3(-u, v, w) * radius * 2) + new Vector3(1, 0.5f, -1) * radius * 2, Quaternion.identity);
                        GOBlock.transform.parent = this.transform;
                        blockAmount++;
                        //PASS DURATION TO BLOCK COMPONENT
                    }
                    else { pieceValues[u, v, w] = false; }
                }
            }
        }

        debugArray(pieceValues);

        blocks = new GameObject[blockAmount];
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject block = transform.GetChild(i).gameObject;
            blocks[i] = block;
        }
    }

    public void initialize()
    {
        onTower = false;
        health = maxHealth;
        nextHealthStep = maxHealth;
        setNextHealthStep();
    }

    public void tick()
    {
        if (onTower)
        {
            health--;
            checkForTimeout();
            updateDamage();
        }
    }

    private void rotateX3x3_45()
    {
        Debug.Log("Rotate X");

        bool store;

        debugArray(pieceValues);

        for (int i = 0; i < 3; i++)
        {
            store = pieceValues[i, 0, 0];
            pieceValues[i, 0, 0] = pieceValues[i, 0, 1];
            pieceValues[i, 0, 1] = pieceValues[i, 0, 2];
            pieceValues[i, 0, 2] = pieceValues[i, 1, 2];
            pieceValues[i, 1, 2] = pieceValues[i, 2, 2];
            pieceValues[i, 2, 2] = pieceValues[i, 2, 1];
            pieceValues[i, 2, 1] = pieceValues[i, 2, 0];
            pieceValues[i, 2, 0] = pieceValues[i, 1, 0];
            pieceValues[i, 1, 0] = store;
        }

        debugArray(pieceValues);
    }

    private void rotateY3x3_45()
    {
        Debug.Log("Rotate Y");

        bool store;

        debugArray(pieceValues);

        for (int i = 0; i < 3; i++)
        {
            store = pieceValues[0, i, 0];
            pieceValues[0, i, 0] = pieceValues[0, i, 1];
            pieceValues[0, i, 1] = pieceValues[0, i, 2];
            pieceValues[0, i, 2] = pieceValues[1, i, 2];
            pieceValues[1, i, 2] = pieceValues[2, i, 2];
            pieceValues[2, i, 2] = pieceValues[2, i, 1];
            pieceValues[2, i, 1] = pieceValues[2, i, 0];
            pieceValues[2, i, 0] = pieceValues[1, i, 0];
            pieceValues[1, i, 0] = store;
        }

        debugArray(pieceValues);
    }

    private void rotateZ3x3_45()
    {
        Debug.Log("Rotate Z");

        bool store;

        debugArray(pieceValues);

        for (int i = 0; i < 3; i++)
        {
            store = pieceValues[0, 0, i];
            pieceValues[0, 0, i] = pieceValues[1, 0, i];
            pieceValues[1, 0, i] = pieceValues[2, 0, i];
            pieceValues[2, 0, i] = pieceValues[2, 1, i];
            pieceValues[2, 1, i] = pieceValues[2, 2, i];
            pieceValues[2, 2, i] = pieceValues[1, 2, i];
            pieceValues[1, 2, i] = pieceValues[0, 2, i];
            pieceValues[0, 2, i] = pieceValues[0, 1, i];
            pieceValues[0, 1, i] = store;
        }

        debugArray(pieceValues);
    }

    private void debugArray(bool[,,] a)
    {
        string dString = "";

        int x = pieceSize.x;
        int y = pieceSize.y;
        int z = pieceSize.z;

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (a[u,v,w] == true)
                    {
                        dString += "1 ";
                    }
                    else { dString += "0 "; }
                }
            }
        }

        Debug.Log(dString);
    }

    public void rotateY3x3()
    {

    }

    public void rotateZ3x3()
    {

    }

    protected void relocate()
    {

        int x = pieceSize.x;
        int y = pieceSize.y;
        int z = pieceSize.z;

        //Vector3 offset = new Vector3(-1f, 1f, 1f);

        int blockCount = 0;

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (pieceValues[u, v, w] == true)
                    {
                        //DEINVERT U BECAUSE PIVOT IS SHIFTED IN PICKUP
                        blocks[blockCount].transform.position = transform.position + (new Vector3(u, v, w) * radius * 2) + new Vector3(1, 0.5f, -1) * radius * 2;
                        blockCount++;
                        //PASS DURATION TO BLOCK COMPONENT
                    }
                }
            }
        }
    }

    protected void updateDamage()
    {
        float currentHealth = (float)health / (float)maxHealth;
        if (currentHealth <= nextHealthStep)
        {
            setNextHealthStep();
            foreach (GameObject G in blocks)
            {
                G.GetComponent<Renderer>().material.SetFloat("_HealthPerc", currentHealth);
            }
        }
    }

    protected void setNextHealthStep()
    {
        nextHealthStep = nextHealthStep - ((float)maxHealth / (float)healthSteps);
    }

    protected void checkForTimeout()
    {
        if (health <= 0)
        {
            TowerController.instance.RemovePiece(id);
            Destroy(transform.gameObject);
        }
    }

    public void goDown()
    {
        //here we execute de property just in case it's a magnet. --Pending--
        //Notify about the end of the animation
    }

    public int3 getPieceSize()
    {
        return pieceSize;
    }

    public void RotateInX()
    {
        rotateX3x3_45();
        rotateX3x3_45();
        relocate();
    }

    public void RotateInY()
    {
        rotateY3x3_45();
        rotateY3x3_45();
        relocate();
    }

    public void RotateInZ()
    {
        rotateZ3x3_45();
        rotateZ3x3_45();
        relocate();
    }

    public void validPlaceFound()
    {

    }

    public void validPlaceNotFound()
    {

    }

    public void pickUp()
    {
        parentMachine.removePiece(id);
        relocate();

        InvokeRepeating("rotateX3x3_90", 0f, 0.5f);
    }

    public void placeOnTower()
    {
        float currentHealth = health / maxHealth;
        onTower = true;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public int getID()
    {
        return id;
    }

    public void setID(int id)
    {
        this.id = id;
    }

    public int getBlockAmount()
    {
        return blockAmount;
    }

    public bool[,,] getMatrix()
    {
        return pieceValues;
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
