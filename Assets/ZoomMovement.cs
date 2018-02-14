using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    public class ZoomMovement : MonoBehaviour
    {

        public Player player;
        private Vector3 initialPosition;
        private Valve.VR.InteractionSystem.Hand hand1, hand2;
        private SteamVR_Controller.Device controller1, controller2;
        private int idOverwatchHand;
        private Hand test;
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
        }

        // Update is called once per frame
        void Update()
        {
            if (player)
            {
                if (idOverwatchHand == 0)
                {
                    if (player.hands[0].controller.GetHairTriggerDown())
                    {
                        idOverwatchHand = 1;
                        initialPosition = new Vector3(player.hands[0].transform.position.x, player.hands[0].transform.position.y, player.hands[0].transform.position.z);

                    }
                    else if (player.hands[1].controller.GetHairTriggerDown())
                    {
                        initialPosition = new Vector3(player.hands[1].transform.position.x, player.hands[1].transform.position.y, player.hands[1].transform.position.z);
                        idOverwatchHand = 2;
                    }
                }
                else
                {
                    if (idOverwatchHand == 1 && player.hands[0].controller.GetHairTrigger())
                    {
                        player.transform.position = player.transform.position - player.hands[0].controller.velocity*4/5;//(player.hands[0].transform.position - initialPosition)*3;
                    }

                    else if (idOverwatchHand == 2 && player.hands[1].controller.GetHairTrigger())
                    {
                        player.transform.position = player.transform.position - player.hands[1].controller.velocity*4/5;//(player.hands[1].transform.position - initialPosition)*3;
                    }
                    if (player.transform.position.y < -0.5) player.transform.position = new Vector3 { x = player.transform.position.x, y =(float)-0.5, z = player.transform.position.z};
                    if ((idOverwatchHand == 1 && player.hands[0].controller.GetHairTriggerUp()) || (idOverwatchHand == 2 && player.hands[1].controller.GetHairTriggerUp()))
                    {
                        idOverwatchHand = 0;
                    }
                }
            }
        }
    }
}