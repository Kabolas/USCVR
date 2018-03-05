using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour {
    public GameObject floor;
    public GameObject CityGenerator;
	// Use this for initialization
	void Start ()
    {
        CityGenerator = GameObject.Find("CityGenerator");
    }
	
	// Update is called once per frame
	void Update () {
        if (floor)
            if (this.gameObject.transform.position.y < floor.transform.position.y)
                DestroyObject(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsRoadArguments args = new IsRoadArguments(this.transform.position);
        CityGenerator.SendMessage("isRoad",args);
        if (!args.answer)
        {
            Destroy(this.gameObject);
        }
    }
}
