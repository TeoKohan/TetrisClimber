using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RuntimeManager : MonoBehaviour {

    [SerializeField]
    protected float tickInterval;

	void Awake () {
        GameManager.initialize();
        //DEBUG OR MORE LIKE PIECING SOMETHING RESEMBLING A GAME IN 20MIN
        //Invoke("tick", tickInterval);
	}

    private void Update() {
        GameManager.update();
    }

    private void tick() {
        GameManager.tick();
        Invoke("tick", tickInterval);
    }
}
