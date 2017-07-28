using UnityEngine;
using System.Collections;

public class DrawMethods : MonoBehaviour {

	public struct DrawData {
		public Vector3 _mazeLocalCenter;
		public Vector3 _offset;

		public float _wallHeight;

		public DrawData(Vector3 offset, Vector2 values, float sizeM, float wallH) {
			_offset = offset;
			values *= sizeM;
			_mazeLocalCenter = - (values / 2 ) + Vector2.one * sizeM / 2;
			_wallHeight = wallH;
		}
	}
		
	static private Vector3 offset;
	static private Material mat;

	static private float wallHeight;

	public static void initialize(DrawData dD) {
		offset = new Vector3(dD._mazeLocalCenter.x, 0, dD._mazeLocalCenter.y) + dD._offset;
		wallHeight = dD._wallHeight;
	}

	public static void drawEdge(Vector3 i, Vector3 o) {
		Debug.DrawLine (i + offset, o + offset, Color.red, Mathf.Infinity);
	}

	public static void drawWall(Vector3 i, Vector3 o, float wS, float wH, Orientation.orientation ori) {
	}

	public static void drawSolidWall(Vector3 i, Vector3 o, float wS, float wH, Material m) {
		GameObject g = new GameObject ();
		g.name = "wall";
		Instantiate (g);
		g.AddComponent<MeshFilter> ();
		g.AddComponent<MeshRenderer> ();
		g.GetComponent<MeshFilter>().mesh =	WallCreation.createWall (i + offset, o + offset, wS, wH);
		g.GetComponent<MeshRenderer> ().material = m;
		g.transform.position = Vector3.Lerp(i + offset, o + offset, 0.5f);
	}

	public static void drawCube(Vector3 i, float s, Color c) {
		Vector3 x = new Vector3 (s/2, 0, 0);
		Vector3 y = new Vector3 (0, s/2, 0);
		Vector3 z = new Vector3 (0, 0, s/2);

		Debug.DrawLine (i + x + y + z + offset, i - x + y + z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i + x - y + z + offset, i - x - y + z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i + x + y + z + offset, i + x - y + z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i - x + y + z + offset, i - x + y + z + offset, c, Mathf.Infinity);

		Debug.DrawLine (i + x + y - z + offset, i - x + y - z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i + x - y - z + offset, i - x - y - z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i + x + y - z + offset, i + x - y - z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i - x + y - z + offset, i - x + y - z + offset, c, Mathf.Infinity);

		Debug.DrawLine (i + x + y - z + offset, i + x + y + z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i + x - y - z + offset, i + x - y + z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i - x + y - z + offset, i - x + y + z + offset, c, Mathf.Infinity);
		Debug.DrawLine (i - x + y - z + offset, i - x - y + z + offset, c, Mathf.Infinity);
	}

	public static void drawPillar(Vector3 i, float s, float h, Material m) {
		GameObject g = new GameObject ();
		g.name = "wall";
		Instantiate (g);
		g.AddComponent<MeshFilter> ();
		g.AddComponent<MeshRenderer> ();
		g.GetComponent<MeshFilter>().mesh =	WallCreation.createPillar (i + offset, s, h);
		g.GetComponent<MeshRenderer> ().material = m;
		g.transform.position = i + offset;
	}
}
