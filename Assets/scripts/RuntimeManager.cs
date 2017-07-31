using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeManager : MonoBehaviour {

	void Awake () {
        GameManager.initialize();
	}

    private void Update() {
        GameManager.update();
    }
}
