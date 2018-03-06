using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{

    public GameObject circle;
    public GameObject floor;
    public GameObject city;
    //public GameObject car;
    public GameObject hand;
    private Transform target;
    private Vector3 normal;
    private bool toggleDraw;
    private Color carColor;
    private GameObject selectedCar;
    
    // Use this for initialization
    void Start()
    {
        //target = hand.GetComponent<SteamVR_LaserPointer>().hitpt;
        normal = new Vector3(0, 1, 0);
        toggleDraw = false;
        carColor = new Color(0, 0, 0);
    }

    private void OnEnable()
    {
        hand.GetComponent<SteamVR_LaserPointer>().PointerIn -= HandlePointerIn;
        hand.GetComponent<SteamVR_LaserPointer>().PointerIn += HandlePointerIn;
        hand.GetComponent<SteamVR_LaserPointer>().PointerIn -= HandlePointerOut;
        hand.GetComponent<SteamVR_LaserPointer>().PointerIn += HandlePointerOut;
    }

    // Update is called once per frame
    void Update()
    {
        float hauteur = 1.5f;
        float distancemax = 80f;
        if (hand.GetComponent<SteamVR_LaserPointer>().thickness > 0 && toggleDraw)
        {
            if (Dot(normal, hand.transform.forward) != 0)
            {
                float t = -(Dot(normal, hand.transform.position) - floor.transform.position.y) / Dot(normal, hand.transform.forward);
                Vector3 intersection = hand.transform.position + hand.transform.forward * t;
                float distance = (hand.transform.position - intersection).magnitude;
                IsRoadArguments args = new IsRoadArguments(intersection);
                city.SendMessage("isRoad", args);
                if (distance <= distancemax && args.answer)
                {
                    GameObject drawing = Instantiate<GameObject>(circle);
                    drawing.GetComponentInChildren<MeshRenderer>().material.color = carColor;
                    drawing.transform.position = new Vector3(intersection.x, intersection.y + hauteur, intersection.z);
                    drawing.SetActive(true);
                    selectedCar.GetComponent<CarMovement>().AddBaowl(drawing);
                    selectedCar.GetComponent<CarMovement>().chemin.Add(drawing.transform.position);
                }
            }
        }
    }

    float Dot(Vector3 v1, Vector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    void HandlePointerIn(object sender, PointerEventArgs e){

        if (e.target.gameObject.name.Contains("SkyCar") && !e.target.parent && hand.GetComponent<SteamVR_LaserPointer>().thickness > 0 && !toggleDraw)
        {
            selectedCar = e.target.gameObject;
            carColor = e.target.gameObject.GetComponentInChildren<MeshRenderer>().material.color;
            hand.GetComponent<SteamVR_LaserPointer>().pointer.GetComponentInChildren<MeshRenderer>().material.color = carColor;     
        }
    }


    void HandlePointerOut(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.name.Contains("SkyCar") && hand.GetComponent<SteamVR_LaserPointer>().thickness > 0)
        {
            toggleDraw = true;
        }
    }

    void toggleDrawF()
    {
        toggleDraw = false;
    }
}
