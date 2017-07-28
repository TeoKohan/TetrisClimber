using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BinaryDivisionMaze : MonoBehaviour 
{
	public MazeOptions.wallType wallOption;
	public MazeOptions.mode modeOption;
	public float stepTime;

	public Vector3 MazeCenter = Vector3.zero;

	public float sizeMultiplier = 1;

	public int floorXDivisions = 5;
	public int floorYDivisions = 5;
	public int floorQuantity = 1;
	public int floorHeight = 1;

	public int roomMinSize = 1;

	public float startingHeight = 0;
	public float wallSize = 1;
	public float wallHeight = 1;

	public int cutCount;

	public Material wallMaterial;

	private int[,,] map;
	private Node[,,] nodeMap;
	private Stack<Room> rooms;

	void Start ()
	{
		nodeMap = new Node[floorXDivisions, floorQuantity, floorYDivisions];
		map = new int[floorXDivisions * 2 - 1, floorQuantity, floorYDivisions * 2 - 1];

		for (int i = 0; i < floorQuantity; i++) {
			for (int x = 0; x < floorXDivisions * 2 - 1; x++) {
				for (int y = 0; y < floorYDivisions * 2 - 1; y++) {
					if (x == 0 || y == 0 || x == floorXDivisions * 2 - 2 || y == floorYDivisions * 2 - 2 || (x % 2 == 0 && y % 2 == 0)) {
						map [x, i, y] = 1;
					}
				}
			}

			map [floorXDivisions * 2 - 5, i, 0] = 0;
			map [3, i, floorYDivisions * 2 - 2] = 0;
		}

		rooms = new Stack<Room> ();

		cutCount = 0;

		DrawMethods.initialize (new DrawMethods.DrawData(MazeCenter, new Vector2(floorXDivisions, floorYDivisions), sizeMultiplier, wallHeight));

		visualizePoints ();

		placeWalls ();
	}

	void placeWalls()
	{
		for (int i = 0; i < floorQuantity; i++)
		{
			rooms.Push(new Room(0, 0, floorXDivisions - 1, floorYDivisions - 1, i));
			placeBorder(i);
			//PrefabLibrary.instance.createCornerPlane((nodeMap[0, i, 0].getPosition(sizeMultiplier) + nodeMap[floorDivisions - 1, i, floorDivisions - 1].getPosition(sizeMultiplier)) / 2, floorDivisions);
		}

		if (modeOption == MazeOptions.mode.automatic)
		{
			binaryDivision(rooms.Pop());
		}
		if (modeOption == MazeOptions.mode.step)
		{
			StartCoroutine(step());
		}
	}

	void placeBorder(int floor)
	{
		if (wallOption == MazeOptions.wallType.line)
		{
			DrawMethods.drawEdge(nodeMap[0, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 3, floor, 0].getPosition(sizeMultiplier));
			DrawMethods.drawEdge(nodeMap[floorXDivisions - 2, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, 0].getPosition(sizeMultiplier));

			DrawMethods.drawEdge(nodeMap[0, floor, 0].getPosition(sizeMultiplier), nodeMap[0, floor, floorYDivisions - 1].getPosition(sizeMultiplier));
			DrawMethods.drawEdge(nodeMap[floorXDivisions - 1, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, floorYDivisions - 1].getPosition(sizeMultiplier));

			DrawMethods.drawEdge(nodeMap[0, floor, floorYDivisions - 1].getPosition(sizeMultiplier), nodeMap[1, floor, floorYDivisions - 1].getPosition(sizeMultiplier));
			DrawMethods.drawEdge(nodeMap[2, floor, floorYDivisions - 1].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, floorYDivisions - 1].getPosition(sizeMultiplier));
		}

		else if (wallOption == MazeOptions.wallType.ghostWall)
		{
			Orientation.orientation o = Orientation.orientation.horizontal;
			DrawMethods.drawWall(nodeMap[0, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 3, floor, 0].getPosition(sizeMultiplier), wallSize, wallHeight, o);
			DrawMethods.drawWall(nodeMap[floorXDivisions - 2, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, 0].getPosition(sizeMultiplier), wallSize, wallHeight, o);

			o = Orientation.orientation.vertical;
			DrawMethods.drawWall(nodeMap[0, floor, 0].getPosition(sizeMultiplier), nodeMap[0, floor, floorYDivisions - 1].getPosition(sizeMultiplier), wallSize, wallHeight, o);
			DrawMethods.drawWall(nodeMap[floorXDivisions - 1, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, floorYDivisions - 1].getPosition(sizeMultiplier), wallSize, wallHeight, o);

			o = Orientation.orientation.horizontal;
			DrawMethods.drawWall(nodeMap[0, floor, floorYDivisions - 1].getPosition(sizeMultiplier), nodeMap[1, floor, floorYDivisions - 1].getPosition(sizeMultiplier), wallSize, wallHeight, o);
			DrawMethods.drawWall(nodeMap[2, floor, floorYDivisions - 1].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, floorYDivisions - 1].getPosition(sizeMultiplier), wallSize, wallHeight, o);
		}

		else if (wallOption == MazeOptions.wallType.solidWall)
		{
			for (int i = 0; i < floorXDivisions - 3; i++)
			{
				DrawMethods.drawSolidWall(nodeMap[i, floor, 0].getPosition(sizeMultiplier), nodeMap[i + 1, floor, 0].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
			}
			DrawMethods.drawSolidWall(nodeMap[floorXDivisions - 2, floor, 0].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, 0].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);

			for (int i = 0; i < floorXDivisions - 1; i++)
			{
				DrawMethods.drawSolidWall(nodeMap[0, floor, i].getPosition(sizeMultiplier), nodeMap[0, floor, i + 1].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
			}

			for (int i = 0; i < floorXDivisions - 1; i++)
			{
				DrawMethods.drawSolidWall(nodeMap[floorXDivisions - 1, floor, i].getPosition(sizeMultiplier), nodeMap[floorXDivisions - 1, floor, i + 1].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
			}

			for (int i = 0; i < floorYDivisions - 3; i++)
			{
				DrawMethods.drawSolidWall(nodeMap[i + 2, floor, floorYDivisions - 1].getPosition(sizeMultiplier), nodeMap[i + 3, floor, floorYDivisions - 1].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
			}
			DrawMethods.drawSolidWall(nodeMap[0, floor, floorYDivisions - 1].getPosition(sizeMultiplier), nodeMap[1, floor, floorYDivisions - 1].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
		}
	}

	void resolveFloor(int floor)
	{
		binaryDivision(rooms.Pop());
	}

	void binaryDivision(Room room)
	{
		
		if (room.width - 2 * (roomMinSize) < 0 ||room.height - 2 * (roomMinSize) < 0)
		{
			if (rooms.Count != 0)
			{
				binaryDivision(rooms.Pop());
			}
			
			return;
		}

		int xBound = room.x + room.width;
		int yBound = room.y + room.height;

		Orientation.orientation o;
		o = Orientation.getOrientation(room.width, room.height);

		//ok lets do this
		int cut;

		if (o == Orientation.orientation.horizontal)
		{
			cut = Random.Range(room.y + roomMinSize, yBound - roomMinSize);
			performCut(cut, room.x, xBound, room.floor, o);

			divide(room, cut, o);
		}

		else
		{
			cut = Random.Range(room.x + roomMinSize, xBound - roomMinSize);
			performCut(cut, room.y, yBound, room.floor, o);

			divide(room, cut, o);
		}

		cutCount ++;

		if (modeOption == MazeOptions.mode.automatic)
		{
			if (rooms.Count != 0)
			{
				binaryDivision(rooms.Pop());
			}
		}

		return;
	}

	void performCut(int cut, int min, int max, int floor, Orientation.orientation o)
	{
		int hole = Random.Range(min, max);

		if (o == Orientation.orientation.horizontal)
		{
			for (int i = min; i < max; i++)
			{
				if (i != hole)
				{
					map[i * 2, floor, cut * 2] = 1;
					map[i * 2 + 1, floor, cut * 2] = 1;
					if (wallOption == MazeOptions.wallType.line)
					{
						DrawMethods.drawEdge(nodeMap[i, floor, cut].getPosition(sizeMultiplier), nodeMap[i + 1, floor, cut].getPosition(sizeMultiplier));
					}
					else if (wallOption == MazeOptions.wallType.ghostWall)
					{
						DrawMethods.drawWall(nodeMap[i, floor, cut].getPosition(sizeMultiplier), nodeMap[i + 1, floor, cut].getPosition(sizeMultiplier), wallSize, wallHeight, o);
					}
					else
					{
						DrawMethods.drawSolidWall(nodeMap[i, floor, cut].getPosition(sizeMultiplier), nodeMap[i + 1, floor, cut].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
					}
				}
			}
		}

		else
		{
			for (int i = min; i < max; i++)
			{
				if (i != hole)
				{
					map[cut * 2, floor, i * 2] = 1;
					map[cut * 2, floor, i * 2 + 1] = 1;
					if (wallOption == MazeOptions.wallType.line)
					{
						DrawMethods.drawEdge(nodeMap[cut, floor, i].getPosition(sizeMultiplier), nodeMap[cut, floor, i + 1].getPosition(sizeMultiplier));
					}
					else if (wallOption == MazeOptions.wallType.ghostWall)
					{
						DrawMethods.drawWall(nodeMap[cut, floor, i].getPosition(sizeMultiplier), nodeMap[cut, floor, i + 1].getPosition(sizeMultiplier), wallSize, wallHeight, o);
					}
					else
					{
						DrawMethods.drawSolidWall(nodeMap[cut, floor, i].getPosition(sizeMultiplier), nodeMap[cut, floor, i + 1].getPosition(sizeMultiplier), wallSize, wallHeight, wallMaterial);
					}
				}
			}
		}

	}

	void divide(Room room, int cut, Orientation.orientation o)
	{


		if (o == Orientation.orientation.horizontal)
		{
			//Bot
			rooms.Push(new Room(
			room.x, room.y, room.width, cut - room.y, room.floor
			));

			//Top
			rooms.Push(new Room(
			room.x, cut, room.width, room.height - (cut - room.y), room.floor
			));
		}

		else
		{
			//Left
			rooms.Push(new Room(
			room.x, room.y, cut - room.x, room.height, room.floor
			));

			//Right
			rooms.Push(new Room(
			cut, room.y, room.width - (cut - room.x), room.height, room.floor
			));
		}
	}

	Color getRandomColor()
	{
		Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		return c;
	}

	void assignVertical(int floor)
	{
		//From i to i++
	}

	void visualizePoints()
	{
		for (int x = 0; x < floorXDivisions; x++)
		{
			for (int y = 0; y < floorYDivisions; y++)
			{
				for (int z = 0; z < floorQuantity; z++)
				{
					Vector3 point = new Vector3(x, z * floorHeight, y);

					if (wallOption == MazeOptions.wallType.line)
					{
						DrawMethods.drawCube(point * sizeMultiplier, wallSize, Color.red);
					}
					else if (wallOption == MazeOptions.wallType.ghostWall)
					{

					}
					else
					{
						DrawMethods.drawPillar(point * sizeMultiplier, wallSize, wallHeight, wallMaterial);
					}

					nodeMap[x, z, y] = new Node(x, z * floorHeight, y);
				}	
			}
		}
	}

	private class Room
	{
		public int x, y;
		public int width, height;
		public int floor;
		
		public Room (int Ix, int Iy, int Iwidth, int Iheight, int Ifloor)
		{
			x = Ix;
			y = Iy;
			width = Iwidth;
			height = Iheight;
			floor = Ifloor;
		}
	}

	private class Node
	{
		public Vector2 pos;
		public int floor;
		
		public Node (int x, int Ifloor, int y)
		{
			pos = new Vector2(x, y);
			floor = Ifloor;
		}

		public Vector3 getPosition(float size)
		{
			return new Vector3(pos.x, floor, pos.y) * size;
		}
	}

	public void Animate()
	{
		if (rooms.Count != 0)
		{
			binaryDivision(rooms.Pop());
		}
	}

	public int[,,] getMap()
	{
		return map;
	}

	IEnumerator step()
	{
		yield return new WaitForSeconds(stepTime);

		if (rooms.Count != 0)
		{
			binaryDivision(rooms.Pop());
			StartCoroutine(step());
		}
	}
}
