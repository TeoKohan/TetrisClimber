using System.Collections;
using System.Collections.Generic;
using UnityScript;
using UnityEngine;

public class Piece : MonoBehaviour
{

    const int healthSteps = 10;

    [SerializeField]
    int maxHealth;
    [SerializeField]
    protected PieceTypes pieceType;
    [SerializeField]
    protected GameObject block;
    [SerializeField]
    protected Material defaultMaterial;
    [SerializeField]
    protected Material placingMaterial;

    Helper.int3 pieceSize;

    protected int id;
    protected int health;
    protected int[,,] pieceValues;
    protected int blockAmount;

    protected float radius;

    protected TetrisMachine parentMachine;
    protected GameObject[] blocks;

    protected bool placing;
    protected bool onTower;


    //VIRTUAL METHODS
    public virtual void steppedOn() {

    }

    public virtual void onPlace() {

    }

    public virtual void onTowerUpdate() {

    }

    //PUBLIC
    public void generateBlocks(int[,,] values, float radius)
    {
        this.radius = radius;

        pieceSize = new Helper.int3(values.GetLength(0), values.GetLength(1), values.GetLength(2));

        int x = pieceSize.x;
        int y = pieceSize.y;
        int z = pieceSize.z;

        pieceValues = values;

        blockAmount = 0;

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (values[u, v, w] >= 1)
                    {
                        GameObject newBlock = Instantiate(block, transform.position + (new Vector3(u, v, w) * radius * 2) - Vector3.one * radius * 2, Quaternion.identity);
                        newBlock.transform.parent = this.transform;
                        blockAmount++;
                    }
                }
            }
        }

        blocks = new GameObject[blockAmount];
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject block = transform.GetChild(i).gameObject;
            blocks[i] = block;
        }

        setNormalMaterial();
    }

    public void initialize()
    {
        placing = false;
        onTower = false;
        health = maxHealth;
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
    }

    protected void updateBlocks()
    {
        int x = pieceSize.x;
        int y = pieceSize.y;
        int z = pieceSize.z;

        int blockCount = 0;

        for (int u = 0; u < x; u++)
        {
            for (int v = 0; v < y; v++)
            {
                for (int w = 0; w < z; w++)
                {
                    if (pieceValues[u, v, w] >= 1)
                    {
                        blocks[blockCount].transform.position = transform.position + (new Vector3(u, v, w) * radius * 2) - Vector3.one * radius * 2;
                        blockCount++;
                    }
                }
            }
        }
    }

    protected void updateDamage() {
        float currentHealth = health / maxHealth;
        foreach (GameObject G in blocks) {
            G.GetComponent<Renderer>().material.SetFloat("_Health", currentHealth);
        }
    }

    protected void checkForTimeout() {
        if (health <= 0) {
            TowerController.instance.RemovePiece(id);
            Destroy(transform.gameObject);
        }
    }

    public void goDown() {
        /* Maybe Someday, when grass is greener and water purer.
         * The trouts play among pruny fingers and their shadows dance in the flickering surface
         * no one knows why they left but in the wake of the new era we must toil without them
         * for that is what we do, our sole and irredeemable purpouse is to be.
         * also, do remember to close the door, it gets chilly in the night dear.*/
    }

    public Helper.int3 getPieceSize() {
        return pieceSize;
    }

    public void rotate(Helper.int3 axis) {
        //Helper.Rotate
        updateBlocks();
    }

    public void validPlaceFound() {
        foreach (GameObject G in blocks) {
        }
    }

    public void validPlaceNotFound() {
        foreach (GameObject G in blocks) {
        }
    }

    protected void setDefaultMaterial() {
        foreach (GameObject G in blocks) {
            G.GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    public void pickUp() {
        parentMachine.removePiece(id);
        updateBlocks();
    }

    public void placeOnTower() {
        float currentHealth = health / maxHealth;
        setDefaultMaterial();
        onTower = true;
    }

    public Vector3 getPosition() {
        return transform.position;
    }

    public int getID() {
        return id;
    }

    public void setID(int id) {
        this.id = id;
    }

    public int getBlockAmount() {
        return blockAmount;
    }

    public int[,,] getMatrix() {
        return pieceValues;
    }

    public TetrisMachine getTetrisMachine() {
        return parentMachine;
    }

    public void setTetrisMachine(TetrisMachine tetrisMachine) {
        parentMachine = tetrisMachine;
    }
}
