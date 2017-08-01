using System;
using UnityEngine;

[Flags]
public enum PieceType : byte { Empty = 0, Neutral = 1, Trampoline = 2, Repair = 3, Damage = 4, Sticky = 5, Portal = 6, MagnetP = 7, MagnetN = 8 };

public struct int3
{
    public int x, y, z;

    public int3(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static int3 operator + (int3 a, int3 b) {
        int3 result = new int3(a.x + b.x, a.y + b.y, a.z + b.z);
        return result;
    }

    public static bool operator > (int3 a, int3 b) {
        if (a.x > b.x) {
            return true;
        }

        else if (a.y > b.y) {
            return true;
        }

        else if (a.z > b.z) {
            return true;
        }

        return false;
    }

    public static bool operator < (int3 a, int3 b) {
        if (a.x < b.x) {
            return true;
        }

        else if (a.y < b.y) {
            return true;
        }

        else if (a.z < b.z) {
            return true;
        }

        return false;
    }
}

public static class Helper {

    //ONLY WORKING WITH RADIUS 1 AND PERFECTLY ALIGNED MATRICES
    public static Vector3 snapVector3(Vector3 inVector) {
        int3 output;
        output.x = (int)Mathf.Round(inVector.x);
        output.y = (int)Mathf.Round(inVector.y);
        output.z = (int)Mathf.Round(inVector.z);

        return new Vector3(output.x, output.y, output.z);
    }

    public static int[,,] multiplyMatrix (int[,,] matrix, int n)
    {
        for (int x = 0; x < matrix.GetLength(0); x++) {
            for (int y = 0; y < matrix.GetLength(1); y++) {
                for (int z = 0; z < matrix.GetLength(2); z++) {
                    matrix[x, y, z] *= n;
                }
            }
        }

        return matrix;
    }

    public static int[,,] rotate3DMatrix(int[,,] oldMatrix) {
        //SHUFFLE AXIS AROUND TO GET DESIRED TRANSFORMS THEN ITERATE THROUGH
        return new int[oldMatrix.GetLength(0), oldMatrix.GetLength(1), oldMatrix.GetLength(2)];
    }

    public static int[,] rotate2DMatrix(int[,] oldMatrix) {
        int[,] newMatrix = new int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }

}

