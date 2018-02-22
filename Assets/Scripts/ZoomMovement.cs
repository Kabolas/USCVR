using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class ZoomMovement : MonoBehaviour
    {

        public Player player;
        public GameObject parking;
        private Vector3 initialPosition;
        private Valve.VR.InteractionSystem.Hand hand1, hand2;
        private SteamVR_Controller.Device controller1, controller2;
        private int idOverwatchHand;
        private bool bothHands = false;
        private Hand test;
        private Vector3 rotinit, Vectinit;
        // Use this for initialization
        void Start()
        {
            idOverwatchHand = 0;
            if (player)
            {
                initialPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                test = GameObject.FindGameObjectWithTag("/Player/SteamVRObjects/Hand1").GetComponent<Hand>();
                hand2 = player.GetHand(1);
                controller1 = hand1.controller;
                controller2 = hand2.controller;
            }
            if (parking)
                parking.SetActive(false);

            rotinit = new Vector3(0, 0);
            Vectinit = new Vector3(0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (player)
            {
                Hand h2 = player.hands[1];
                if (parking && h2.isActiveAndEnabled)
                    if (h2.controller.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
                        parking.SetActive(true);
                    else if (h2.controller.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger))
                        parking.SetActive(false);


                Hand h = player.hands[0];
                if (h.isActiveAndEnabled)
                    if (h.controller.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
                        h.GetComponent<SteamVR_LaserPointer>().thickness = (float)0.002;
                    else if (h.controller.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger))
                        h.GetComponent<SteamVR_LaserPointer>().thickness = 0;

                if((h.controller.GetPress(EVRButtonId.k_EButton_Grip) && player.hands[1].controller.GetPressDown(EVRButtonId.k_EButton_Grip)) || (h.controller.GetPressDown(EVRButtonId.k_EButton_Grip) && player.hands[1].controller.GetPress(EVRButtonId.k_EButton_Grip)))
                {
                    bothHands = true;
                    Vectinit = h.transform.position - h2.transform.position;
                }
                if (!h.controller.GetPress(EVRButtonId.k_EButton_Grip) || !h2.controller.GetPress(EVRButtonId.k_EButton_Grip))
                    bothHands = false;
                //if (h.controller.GetPressDown(EVRButtonId.k_EButton_Grip) && player.hands[1].controller.GetPressDown(EVRButtonId.k_EButton_Grip))
                    //player.transform.Rotate(rotinit);
                //bothHands = h.controller.GetPress(EVRButtonId.k_EButton_Grip) && player.hands[1].controller.GetPress(EVRButtonId.k_EButton_Grip);
                if (!bothHands)
                    if (idOverwatchHand == 0)
                    {
                        if (h.controller.GetPressDown(EVRButtonId.k_EButton_Grip) && h.AttachedObjects.Count == 1)
                        {
                            idOverwatchHand = 1;
                            initialPosition = new Vector3(h.transform.position.x, h.transform.position.y, h.transform.position.z);

                        }
                        else if (player.hands[1].controller.GetPressDown(EVRButtonId.k_EButton_Grip) && player.hands[1].AttachedObjects.Count == 1)
                        {
                            initialPosition = new Vector3(player.hands[1].transform.position.x, player.hands[1].transform.position.y, player.hands[1].transform.position.z);
                            idOverwatchHand = 2;
                        }
                    }
                    else
                    {
                        if (idOverwatchHand == 1 && h.controller.GetPress(EVRButtonId.k_EButton_Grip))
                        {
                           player.transform.Translate(-h.controller.velocity * 3 / 5);//(player.hands[0].transform.position - initialPosition)*3;
                        }

                        else if (idOverwatchHand == 2 && h2.controller.GetPress(EVRButtonId.k_EButton_Grip))
                        {
                            player.transform.Translate(-h2.controller.velocity * 3 / 5);//(player.hands[1].transform.position - initialPosition)*3;
                        }
                        if (player.transform.position.y < -0.5) player.transform.position = new Vector3 { x = player.transform.position.x, y = (float)-0.5, z = player.transform.position.z };
                        if ((idOverwatchHand == 1 && h.controller.GetPressUp(EVRButtonId.k_EButton_Grip)) || (idOverwatchHand == 2 && player.hands[1].controller.GetPressUp(EVRButtonId.k_EButton_Grip)))
                        {
                            idOverwatchHand = 0;
                        }
                    }
                else
                {
                    //Vector3 v = h.transform.position - h2.transform.position;
                    //player.transform.rotation.y = h2.controller.angularVelocity.y;
                   // Vector3 hp1 = h.transform.position - player.transform.position, hp2 = h2.transform.position - player.transform.position, difh = h.transform.position - h2.transform.position;
                    player.transform.Rotate(new Vector3(0, 1, 0), -h2.controller.angularVelocity.y);
                   // rotinit = player.transform.rotation.eulerAngles;
                }
            }
        }
    }
}