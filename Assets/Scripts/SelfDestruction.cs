using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour {
    public GameObject floor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (floor)
            if (this.gameObject.transform.position.y < floor.transform.position.y)
                DestroyObject(gameObject);

    }
}
