using UnityEngine;
using System.Collections;

public static class WallCreation
{
	public static Mesh createWall(Vector3 start, Vector3 end, float wallWidth, float wallHeightF)
	{
		Vector3 wallHeight = wallHeightF * Vector3.up;
		Vector3 position = (start + end) / 2;

		start -= position;
		end -= position;

		int faces = 6;

		Vector3 direction = (end - start).normalized;
		Vector3 widthOffset = wallWidth * direction / 2;
		Vector3 widthDirection = Vector3.Cross(direction, Vector3.up) / 2;

		Mesh mesh = new Mesh();
		Vector4[] newTangents = new Vector4[24];
		Vector3[] newVertices = new Vector3[24];
		Vector3[] newNormals = new Vector3[24];
		Vector2[] newUVs = new Vector2[24];
		int[] newTriangles = new int[36];

		Vector2 wallTop01 = new Vector2(0f, 0.2f);
		Vector2 wallTop10 = new Vector2(1f, 0f);
		Vector2 wallTop11 = new Vector2(1f, 0.2f);

		Vector2 vector01 = new Vector2(0f, 1f);
		Vector2 vector10 = new Vector2(1f, 0f);


		newVertices[0] = start - widthDirection * wallWidth + widthOffset;
		newVertices[1] = end - widthDirection * wallWidth - widthOffset;
		newVertices[2] = end + widthDirection * wallWidth - widthOffset;
		newVertices[3] = start + widthDirection * wallWidth + widthOffset;

		newNormals[0] = Vector3.down;
		newNormals[1] = Vector3.down;
		newNormals[2] = Vector3.down;
		newNormals[3] = Vector3.down;

		newUVs[0] = Vector2.zero;
		newUVs[1] = wallTop10;
		newUVs[2] = wallTop11;
		newUVs[3] = wallTop01;

		newTangents[0] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[1] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[2] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[3] = new Vector4(direction.x, direction.y, direction.z, 1);

		newVertices[4] = start - widthDirection * wallWidth + wallHeight + widthOffset;
		newVertices[5] = end - widthDirection * wallWidth + wallHeight - widthOffset;
		newVertices[6] = end + widthDirection * wallWidth + wallHeight - widthOffset;
		newVertices[7] = start + widthDirection * wallWidth + wallHeight + widthOffset;

		newNormals[4] = Vector3.up;
		newNormals[5] = Vector3.up;
		newNormals[6] = Vector3.up;
		newNormals[7] = Vector3.up;

		newUVs[4] = Vector2.zero;
		newUVs[5] = wallTop10;
		newUVs[6] = wallTop11;
		newUVs[7] = wallTop01;

		newTangents[4] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[5] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[6] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[7] = new Vector4(direction.x, direction.y, direction.z, 1);

		newVertices[8] = start - widthDirection * wallWidth + widthOffset;
		newVertices[9] = start - widthDirection * wallWidth + wallHeight + widthOffset;
		newVertices[10] = end - widthDirection * wallWidth + wallHeight - widthOffset;
		newVertices[11] = end - widthDirection * wallWidth - widthOffset;

		newNormals[8] = -widthDirection;
		newNormals[9] =  -widthDirection;
		newNormals[10] = -widthDirection;
		newNormals[11] = -widthDirection;

		newUVs[8] = Vector2.zero;
		newUVs[9] = vector01;
		newUVs[10] = Vector2.one;
		newUVs[11] = vector10;

		newTangents[8] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[9] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[10] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[11] = new Vector4(direction.x, direction.y, direction.z, 1);

		newVertices[12] = start + widthDirection * wallWidth + widthOffset;
		newVertices[13] = start + widthDirection * wallWidth + wallHeight + widthOffset;
		newVertices[14] = end + widthDirection * wallWidth + wallHeight - widthOffset;
		newVertices[15] = end + widthDirection * wallWidth - widthOffset;

		newNormals[12] = widthDirection;
		newNormals[13] = widthDirection;
		newNormals[14] = widthDirection;
		newNormals[15] = widthDirection;

		newUVs[12] = Vector2.zero;
		newUVs[13] = vector01;
		newUVs[14] = Vector2.one;
		newUVs[15] = vector10;

		newTangents[12] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[13] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[14] = new Vector4(direction.x, direction.y, direction.z, 1);
		newTangents[15] = new Vector4(direction.x, direction.y, direction.z, 1);

		newVertices[16] = start - widthDirection * wallWidth + widthOffset;
		newVertices[17] = start + widthDirection * wallWidth + widthOffset;
		newVertices[18] = start + widthDirection * wallWidth + wallHeight + widthOffset;
		newVertices[19] = start - widthDirection * wallWidth + wallHeight + widthOffset;

		newNormals[16] = -direction;
		newNormals[17] = -direction;
		newNormals[18] = -direction;
		newNormals[19] = -direction;

		newUVs[16] = Vector2.zero;
		newUVs[17] = wallTop01;
		newUVs[18] = wallTop11;
		newUVs[19] = wallTop10;

		newTangents[16] = new Vector4(0, 1, 0, 1);
		newTangents[17] = new Vector4(0, 1, 0, 1);
		newTangents[18] = new Vector4(0, 1, 0, 1);
		newTangents[19] = new Vector4(0, 1, 0, 1);

		newVertices[20] = end - widthDirection * wallWidth - widthOffset;
		newVertices[21] = end + widthDirection * wallWidth - widthOffset;
		newVertices[22] = end + widthDirection * wallWidth + wallHeight - widthOffset;
		newVertices[23] = end - widthDirection * wallWidth + wallHeight - widthOffset;

		newNormals[20] = direction;
		newNormals[21] = direction;
		newNormals[22] = direction;
		newNormals[23] = direction;

		newUVs[20] = Vector2.zero;
		newUVs[21] = wallTop01;
		newUVs[22] = wallTop11;
		newUVs[23] = wallTop10;

		newTangents[20] = new Vector4(0, 1, 0, 1);
		newTangents[21] = new Vector4(0, 1, 0, 1);
		newTangents[22] = new Vector4(0, 1, 0, 1);
		newTangents[23] = new Vector4(0, 1, 0, 1);

		for (int i = 0; i < faces; i++)
		{
			bool alternate;

			if (i % 2 == 0)
			{
				alternate = true;
			}
			else
			{
				alternate = false;
			}

			if (alternate)
			{
				newTriangles[0 + 6 * i] = 0 + 4 * i;
				newTriangles[1 + 6 * i] = 1 + 4 * i;
				newTriangles[2 + 6 * i] = 2 + 4 * i;
				
				newTriangles[3 + 6 * i] = 0 + 4 * i;
				newTriangles[4 + 6 * i] = 2 + 4 * i;
				newTriangles[5 + 6 * i] = 3 + 4 * i;
			}
			else
			{
				newTriangles[0 + 6 * i] = 2 + 4 * i;
				newTriangles[1 + 6 * i] = 1 + 4 * i;
				newTriangles[2 + 6 * i] = 0 + 4 * i;
				
				newTriangles[3 + 6 * i] = 3 + 4 * i;
				newTriangles[4 + 6 * i] = 2 + 4 * i;
				newTriangles[5 + 6 * i] = 0 + 4 * i;
			}

		}

//		newTriangles[0] = 0;
//		newTriangles[1] = 1;
//		newTriangles[2] = 2;
//
//		newTriangles[3] = 0;
//		newTriangles[4] = 2;
//		newTriangles[5] = 3;
//
//		newTriangles[0 + 6] = 2 + 4;
//		newTriangles[1 + 6] = 1 + 4;
//		newTriangles[2 + 6] = 0 + 4;
//		
//		newTriangles[3 + 6] = 3 + 4;
//		newTriangles[4 + 6] = 2 + 4;
//		newTriangles[5 + 6] = 0 + 4;
//
//		newTriangles[0 + 12] = 0 + 8;
//		newTriangles[1 + 12] = 1 + 8;
//		newTriangles[2 + 12] = 2 + 8;
//		
//		newTriangles[3 + 12] = 0 + 8;
//		newTriangles[4 + 12] = 2 + 8;
//		newTriangles[5 + 12] = 3 + 8;
//
//		newTriangles[0 + 18] = 2 + 12;
//		newTriangles[1 + 18] = 1 + 12;
//		newTriangles[2 + 18] = 0 + 12;
//		
//		newTriangles[3 + 18] = 3 + 12;
//		newTriangles[4 + 18] = 2 + 12;
//		newTriangles[5 + 18] = 0 + 12;
//
//		newTriangles[0 + 24] = 0 + 16;
//		newTriangles[1 + 24] = 1 + 16;
//		newTriangles[2 + 24] = 2 + 16;
//		
//		newTriangles[3 + 24] = 0 + 16;
//		newTriangles[4 + 24] = 2 + 16;
//		newTriangles[5 + 24] = 3 + 16;
//
//		newTriangles[0 + 30] = 2 + 20;
//		newTriangles[1 + 30] = 1 + 20;
//		newTriangles[2 + 30] = 0 + 20;
//		
//		newTriangles[3 + 30] = 3 + 20;
//		newTriangles[4 + 30] = 2 + 20;
//		newTriangles[5 + 30] = 0 + 20;

		mesh.vertices = newVertices;
		mesh.uv = newUVs;
		mesh.triangles = newTriangles;
		mesh.normals = newNormals;
		mesh.tangents = newTangents;

		return mesh;
	}

	public static Mesh createPillar(Vector3 start, float wallWidth, float wallHeight)
	{
		int faces = 6;
		
		Vector3 direction = Vector3.up;

		Vector3 end = start + direction * wallHeight * 1.5f;

		end -= start;
		start -= start;
			
		Vector3 xOffset = Vector3.right * wallWidth / 2f;
		Vector3 zOffset = Vector3.forward * wallWidth / 2f;
		
		Mesh mesh = new Mesh();
		Vector3[] newVertices = new Vector3[24];
		Vector3[] newNormals = new Vector3[24];
		int[] newTriangles = new int[36];
		
		newVertices[0] = start - xOffset + zOffset;
		newVertices[1] = start - xOffset - zOffset;
		newVertices[2] = start + xOffset - zOffset;
		newVertices[3] = start + xOffset + zOffset;
		
		newNormals[0] = Vector3.down;
		newNormals[1] = Vector3.down;
		newNormals[2] = Vector3.down;
		newNormals[3] = Vector3.down;
		
		newVertices[4] = end - xOffset + zOffset;
		newVertices[5] = end - xOffset - zOffset;
		newVertices[6] = end + xOffset - zOffset;
		newVertices[7] = end + xOffset + zOffset;
		
		newNormals[4] = Vector3.up;
		newNormals[5] = Vector3.up;
		newNormals[6] = Vector3.up;
		newNormals[7] = Vector3.up;
		
		newVertices[8] = start - xOffset + zOffset;
		newVertices[9] = end - xOffset + zOffset;
		newVertices[10] = end - xOffset - zOffset;
		newVertices[11] = start - xOffset - zOffset;
		
		newNormals[8] = Vector3.left;
		newNormals[9] =  Vector3.left;
		newNormals[10] = Vector3.left;
		newNormals[11] = Vector3.left;
		
		newVertices[12] = start + xOffset + zOffset;
		newVertices[13] = end + xOffset + zOffset;
		newVertices[14] = end + xOffset - zOffset;
		newVertices[15] = start + xOffset - zOffset;
		
		newNormals[12] = Vector3.right;
		newNormals[13] = Vector3.right;
		newNormals[14] = Vector3.right;
		newNormals[15] = Vector3.right;
		
		newVertices[16] = start - xOffset + zOffset;
		newVertices[17] = start + xOffset + zOffset;
		newVertices[18] = end + xOffset + zOffset;
		newVertices[19] = end - xOffset + zOffset;
		
		newNormals[16] = Vector3.forward;
		newNormals[17] = Vector3.forward;
		newNormals[18] = Vector3.forward;
		newNormals[19] = Vector3.forward;
		
		newVertices[20] = start - xOffset - zOffset;
		newVertices[21] = start + xOffset - zOffset;
		newVertices[22] = end + xOffset - zOffset;
		newVertices[23] = end - xOffset - zOffset;
		
		newNormals[20] = Vector3.back;
		newNormals[21] = Vector3.back;
		newNormals[22] = Vector3.back;
		newNormals[23] = Vector3.back;
		
		for (int i = 0; i < faces; i++)
		{
			bool alternate;
			
			if (i % 2 == 0)
			{
				alternate = true;
			}
			else
			{
				alternate = false;
			}
			
			if (alternate)
			{
				newTriangles[0 + 6 * i] = 0 + 4 * i;
				newTriangles[1 + 6 * i] = 1 + 4 * i;
				newTriangles[2 + 6 * i] = 2 + 4 * i;
				
				newTriangles[3 + 6 * i] = 0 + 4 * i;
				newTriangles[4 + 6 * i] = 2 + 4 * i;
				newTriangles[5 + 6 * i] = 3 + 4 * i;
			}
			else
			{
				newTriangles[0 + 6 * i] = 2 + 4 * i;
				newTriangles[1 + 6 * i] = 1 + 4 * i;
				newTriangles[2 + 6 * i] = 0 + 4 * i;
				
				newTriangles[3 + 6 * i] = 3 + 4 * i;
				newTriangles[4 + 6 * i] = 2 + 4 * i;
				newTriangles[5 + 6 * i] = 0 + 4 * i;
			}
			
		}
		
		//		newTriangles[0] = 0;
		//		newTriangles[1] = 1;
		//		newTriangles[2] = 2;
		//
		//		newTriangles[3] = 0;
		//		newTriangles[4] = 2;
		//		newTriangles[5] = 3;
		//
		//		newTriangles[0 + 6] = 2 + 4;
		//		newTriangles[1 + 6] = 1 + 4;
		//		newTriangles[2 + 6] = 0 + 4;
		//		
		//		newTriangles[3 + 6] = 3 + 4;
		//		newTriangles[4 + 6] = 2 + 4;
		//		newTriangles[5 + 6] = 0 + 4;
		//
		//		newTriangles[0 + 12] = 0 + 8;
		//		newTriangles[1 + 12] = 1 + 8;
		//		newTriangles[2 + 12] = 2 + 8;
		//		
		//		newTriangles[3 + 12] = 0 + 8;
		//		newTriangles[4 + 12] = 2 + 8;
		//		newTriangles[5 + 12] = 3 + 8;
		//
		//		newTriangles[0 + 18] = 2 + 12;
		//		newTriangles[1 + 18] = 1 + 12;
		//		newTriangles[2 + 18] = 0 + 12;
		//		
		//		newTriangles[3 + 18] = 3 + 12;
		//		newTriangles[4 + 18] = 2 + 12;
		//		newTriangles[5 + 18] = 0 + 12;
		//
		//		newTriangles[0 + 24] = 0 + 16;
		//		newTriangles[1 + 24] = 1 + 16;
		//		newTriangles[2 + 24] = 2 + 16;
		//		
		//		newTriangles[3 + 24] = 0 + 16;
		//		newTriangles[4 + 24] = 2 + 16;
		//		newTriangles[5 + 24] = 3 + 16;
		//
		//		newTriangles[0 + 30] = 2 + 20;
		//		newTriangles[1 + 30] = 1 + 20;
		//		newTriangles[2 + 30] = 0 + 20;
		//		
		//		newTriangles[3 + 30] = 3 + 20;
		//		newTriangles[4 + 30] = 2 + 20;
		//		newTriangles[5 + 30] = 0 + 20;
		
		mesh.vertices = newVertices;
		mesh.triangles = newTriangles;
		mesh.normals = newNormals;
		
		return mesh;
	}
}
