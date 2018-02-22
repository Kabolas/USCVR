using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakingRenew : MonoBehaviour
{
    public GameObject[] cars;
    //public GameObject
    private Vector3[] carspositions;
    // Use this for initialization
    void Start()
    {
        foreach (GameObject carType in cars)
        {
            if (this.gameObject.transform.Find(carType.name) == null && this.gameObject.transform.Find(carType.name + "(Clone)") == null)
            {
                GameObject gamecar = Instantiate<GameObject>(carType, gameObject.transform);
                gamecar.SetActive(true);
                gamecar.transform.position = new Vector3(carType.transform.position.x, carType.transform.position.y, carType.transform.position.z);
                gamecar.transform.parent = gameObject.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject carType in cars)
        {
            if (this.gameObject.transform.Find(carType.name + "(Clone)") == null && this.gameObject.transform.Find(carType.name) == null)
            {
                GameObject gamecar = Instantiate<GameObject>(carType, gameObject.transform);
                gamecar.SetActive(true);
                gamecar.transform.position = new Vector3(carType.transform.position.x, carType.transform.position.y, carType.transform.position.z);
                gamecar.transform.parent = gameObject.transform;
            }
        }
    }
}
