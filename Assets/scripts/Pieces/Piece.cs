using UnityEngine;

public class Piece : MonoBehaviour
{

    const int healthSteps = 10;

    [SerializeField]
    int maxHealth;
    [SerializeField]
    protected PieceType pieceType;
    [SerializeField]
    protected GameObject block;
    [SerializeField]
    protected Material defaultMaterial;
    [SerializeField]
    protected Material placingMaterial;

    int3[] blockPositions;
    int3 pieceSize;

    protected int id;
    protected int health;
    protected PieceType[,,] pieceValues;
    protected int blockAmount;

    protected float radius;

    protected TetrisMachine parentMachine;
    protected TowerController parentTower;
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
    public void generateBlocks(PieceType[,,] values, float radius)
    {
        this.radius = radius;

        pieceSize = new int3(values.GetLength(0), values.GetLength(1), values.GetLength(2));

        int x = pieceSize.x;
        int y = pieceSize.y;
        int z = pieceSize.z;

        pieceValues = values;

        blockAmount = 0;

        for (int u = 0; u < x; u++) {
            for (int v = 0; v < y; v++) {
                for (int w = 0; w < z; w++) {
                    if (values[u, v, w] != PieceType.Empty) {
                        pieceValues[u, v, w] = values[u, v, w];

                        Vector3 centerOffset = new Vector3(pieceSize.x / 2, pieceSize.y / 2, pieceSize.z / 2) * radius;
                        Vector3 smallPieceOffset = new Vector3( Mathf.Clamp01(2 - pieceSize.x), Mathf.Clamp01(2 - pieceSize.y), Mathf.Clamp01(2 - pieceSize.z)) * radius * 1;

                        //Debug.Log(evenPieceOffset);

                        GameObject newBlock = Instantiate(block, transform.position + (new Vector3(u, v, w) * radius * 2 + centerOffset + smallPieceOffset), Quaternion.identity);
                        newBlock.transform.parent = this.transform;
                        blockAmount++;
                    }
                }
            }
        }

        blocks = new GameObject[blockAmount];
        for (int i = 0; i < transform.childCount; i++) {
            GameObject block = transform.GetChild(i).gameObject;
            blocks[i] = block;
        }

        setDefaultMaterial();
    }

    public void initialize() {
        placing = false;
        onTower = false;
        health = maxHealth;
    }

    public void tick() {
        if (onTower) {
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

    protected void updateBlocks() {
        int x = pieceSize.x;
        int y = pieceSize.y;
        int z = pieceSize.z;

        int blockCount = 0;

        for (int u = 0; u < x; u++) {
            for (int v = 0; v < y; v++) {
                for (int w = 0; w < z; w++) {
                    if (pieceValues[u, v, w] != PieceType.Empty) {
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
            parentTower.removePiece(id);
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

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public GameObject[] getBlocks() {
        return blocks;
    }

    public int3[] getBlockPositions()
    {
        return blockPositions;
    }

    public void setBlockPositions(int3[] blockPositions)
    {
        this.blockPositions = blockPositions;
    }

    public int3 getPieceSize() {
        return pieceSize;
    }

    public void rotate(int3 axis) {
        //Helper.Rotate
        updateBlocks();
    }

    public void setPlaceState(bool available) {
        if (available) {
            foreach (GameObject G in blocks) {
                G.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }
        }
        else {
            foreach (GameObject G in blocks) {
                G.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }
    }

    protected void setDefaultMaterial() {
        foreach (GameObject G in blocks) {
            G.GetComponent<Renderer>().material = defaultMaterial;
        }
    }

    protected void setPlaceMaterial() {
        foreach (GameObject G in blocks) {
            G.GetComponent<Renderer>().material = placingMaterial;
        }
    }

    public void pickUp() {
        parentMachine.removePiece(id);
        setPlaceMaterial();
        //updateBlocks();
    }

    public void placeOnTower() {
        float currentHealth = health / maxHealth;
        setDefaultMaterial();
        onTower = true;
    }

    public float getRadius() {
        return radius;
    }

    public bool isOnTower() {
        return onTower;
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

    public PieceType[,,] getValues() {
        return pieceValues;
    }

    public TetrisMachine getTetrisMachine() {
        return parentMachine;
    }

    public void setTetrisMachine(TetrisMachine tetrisMachine) {
        parentMachine = tetrisMachine;
    }

    public TowerController getTowerController()
    {
        return parentTower;
    }

    public void setTowerController(TowerController towerController)
    {
        parentTower = towerController;
    }
}
