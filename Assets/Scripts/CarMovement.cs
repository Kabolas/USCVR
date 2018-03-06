using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CarMovement : MonoBehaviour
{


    public List<Vector3> chemin;
    private List<GameObject> BallWay = new List<GameObject>();
    public Player player;
    public bool togglemove;
    private int i = 0;
    // Use this for initialization
    void Start()
    {
        chemin = new List<Vector3>();
    }
    public void AddBaowl(GameObject boule)
    {
        BallWay.Add(boule);
    }
    // Update is called once per frame
    void Update()
    {

        if (player.hands[0])
        {
            if (player.hands[0].controller.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu))
                togglemove = !togglemove;

            if (togglemove && chemin.Count > 0 && i < chemin.Count)
            {
                float angle = 0;
                if (i > 0)
                {
                    Vector3 pt1 = chemin[i - 1] - gameObject.transform.position, pt2 = chemin[i]- gameObject.transform.position;
                    float XDiff = pt2.x - pt1.x, YDiff = pt1.z - pt2.z;
                    angle = -Mathf.Atan2(YDiff, XDiff);
                    gameObject.transform.Rotate(new Vector3(0,1,0), angle);
                }
                this.gameObject.transform.position = new Vector3(chemin[i].x, gameObject.transform.position.y, chemin[i].z);
                DestroyObject(BallWay[i++]);
            }
        }
    }
}
