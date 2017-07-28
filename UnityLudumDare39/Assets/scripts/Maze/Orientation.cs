using UnityEngine;
using System.Collections;

public class Orientation {
	public enum orientation {
		horizontal,
		vertical
	};

	static public orientation getOrientation(int width, int height) {
		if (width > height) {
			return orientation.vertical;
		} 
		else if (height > width) {
			return orientation.horizontal;
		} 
		else {
			//FIX!!!
			int i = Random.Range(0, 2); 
			if (i == 0) {
				return orientation.vertical;
			}
			else {
				return orientation.horizontal;
			}
		}
	}
}
