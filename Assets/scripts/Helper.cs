using System;

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

    static int[,,] multiplyMatrix (int[,,] matrix, int n)
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

    static int[,,] rotate3DMatrix(int[,,] oldMatrix) {
        //SHUFFLE AXIS AROUND TO GET DESIRED TRANSFORMS THEN ITERATE THROUGH
        return new int[oldMatrix.GetLength(0), oldMatrix.GetLength(1), oldMatrix.GetLength(2)];
    }

    static int[,] rotate2DMatrix(int[,] oldMatrix) {
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
