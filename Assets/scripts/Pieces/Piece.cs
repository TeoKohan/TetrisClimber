using System.Collections;
using System.Collections.Generic;
using UnityScript;
using UnityEngine;

public class Piece : MonoBehaviour {

    const int healthSteps = 5;

    public struct int3 {
        public int x, y, z;

        public int3(int x, int y, int z) {
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
                        GameObject GOBlock = Instantiate(block, transform.position + (new Vector3(-u, v, w) * radius * 2) + new Vector3(1, 0.5f, -1) * radius * 2, Quaternion.identity);
                        GOBlock.transform.parent = this.transform;
                        blockAmount++;
                        //PASS DURATION TO BLOCK COMPONENT
                    }
                    else { pieceValues[u, v, w] = false; }
                }
            }
        }

        blocks = new GameObject[blockAmount];
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject block = transform.GetChild(i).gameObject;
            blocks[i] = block;
        }
    }

    public void initialize() {
        onTower = false;
        health = maxHealth;
        nextHealthStep = maxHealth;
        setNextHealthStep();
    }

    public void tick() {
        if (onTower) {
            health--;
            checkForTimeout();
            updateDamage();
        }
    }

    protected void updateDamage()
    {
        float currentHealth = (float)health / (float)maxHealth;
        if (currentHealth <= nextHealthStep) {
            setNextHealthStep();
            foreach (GameObject G in blocks) {
                G.GetComponent<Renderer>().material.SetFloat("_HealthPerc", currentHealth);
            }
        }
    }

    protected void setNextHealthStep() {
        nextHealthStep = nextHealthStep - ((float)maxHealth / (float)healthSteps);
    }

    protected void checkForTimeout() {
        if (health <= 0) {
            TowerController.instance.RemovePiece(id);
            Debug.Log(maxHealth + "  " + health);
            Destroy(transform.gameObject);
            Debug.Break();
        }
    }

    public void goDown() {
        //here we execute de property just in case it's a magnet. --Pending--
        //Notify about the end of the animation
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
        parentMachine.removePiece(id);
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
