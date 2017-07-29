using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAssist : MonoBehaviour {

	public static void drawCube(Vector3 position, float radius, float duration) {

        Vector3 cubeRadiusX = new Vector3(radius, 0f, 0f);
        Vector3 cubeRadiusY = new Vector3(0f, radius, 0f);
        Vector3 cubeRadiusZ = new Vector3(0f, 0f, radius);

        //Base
        Debug.DrawLine(position - cubeRadiusX - cubeRadiusY - cubeRadiusZ, position + cubeRadiusX - cubeRadiusY - cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position - cubeRadiusX - cubeRadiusY + cubeRadiusZ, position + cubeRadiusX - cubeRadiusY + cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position - cubeRadiusX - cubeRadiusY - cubeRadiusZ, position - cubeRadiusX - cubeRadiusY + cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position + cubeRadiusX - cubeRadiusY - cubeRadiusZ, position + cubeRadiusX - cubeRadiusY + cubeRadiusZ, Color.red, duration);

        //Sides
        Debug.DrawLine(position - cubeRadiusX - cubeRadiusY - cubeRadiusZ, position - cubeRadiusX + cubeRadiusY - cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position + cubeRadiusX - cubeRadiusY - cubeRadiusZ, position + cubeRadiusX + cubeRadiusY - cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position - cubeRadiusX - cubeRadiusY + cubeRadiusZ, position - cubeRadiusX + cubeRadiusY + cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position + cubeRadiusX - cubeRadiusY + cubeRadiusZ, position + cubeRadiusX + cubeRadiusY + cubeRadiusZ, Color.red, duration);

        //Top
        Debug.DrawLine(position - cubeRadiusX + cubeRadiusY - cubeRadiusZ, position + cubeRadiusX + cubeRadiusY - cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position - cubeRadiusX + cubeRadiusY + cubeRadiusZ, position + cubeRadiusX + cubeRadiusY + cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position - cubeRadiusX + cubeRadiusY - cubeRadiusZ, position - cubeRadiusX + cubeRadiusY + cubeRadiusZ, Color.red, duration);
        Debug.DrawLine(position + cubeRadiusX + cubeRadiusY - cubeRadiusZ, position + cubeRadiusX + cubeRadiusY + cubeRadiusZ, Color.red, duration);
    }
}
