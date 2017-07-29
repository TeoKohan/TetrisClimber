using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour {

    [SerializeField]
    GameObject block;

    public void generate(TetrisMachine.PieceValues values, float radius) {

        int x = values.blocks.GetLength(0);
        int y = values.blocks.GetLength(1);
        int z = values.blocks.GetLength(2);

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
}
