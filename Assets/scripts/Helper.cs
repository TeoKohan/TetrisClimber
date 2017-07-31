using System.Collections;
using System.Collections.Generic;

public static class Helper {

    public struct int3 {
        public int x, y, z;

        public int3(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
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
