using UnityEngine;
using System.Collections;

public class MazeOptions : MonoBehaviour {
	public enum wallType {
		line,
		ghostWall,
		solidWall
	};

	public enum mode {
		automatic,
		step
	};
}
