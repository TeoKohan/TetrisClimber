using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RuntimeManager : MonoBehaviour {

    [SerializeField]
    protected float tickInterval;

	void Awake () {
        GameManager.initialize();
        Invoke("tick", tickInterval);
	}

    private void Update() {
        GameManager.update();
    }

    private void tick() {
        GameManager.tick();
        Invoke("tick", tickInterval);
    }
}
