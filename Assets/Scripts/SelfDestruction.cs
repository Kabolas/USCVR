using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : MonoBehaviour {
    public GameObject floor;
    public GameObject CityGenerator;
    public GameObject parking;
	// Use this for initialization
	void Start ()
    {
        CityGenerator = GameObject.Find("CityGenerator");
    }

    // Update is called once per frame
    void Update() {
        if (floor)
            if (this.gameObject.transform.position.y < floor.transform.position.y)
            {
                Destroy(gameObject);
            }
        if (parking && gameObject.transform.parent != parking.transform && gameObject.transform.parent != null)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        else gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject.name);
        if (parking && gameObject.transform.parent != parking.transform && collision.collider.gameObject.name != "BodyCollider" && collision.collider.gameObject.name != "HeadCollider")
        {
            IsRoadArguments args = new IsRoadArguments(this.transform.position);
            CityGenerator.SendMessage("isRoad", args);
            if (!args.answer)
            {
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(2, 2, 2);
                gameObject.transform.position = gameObject.transform.position + new Vector3(0, 3, 0);
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            }
        }
    }
}
