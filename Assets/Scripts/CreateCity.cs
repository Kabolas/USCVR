﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CityObject { R, B }
public enum RoadType { DEAD_END_UP = 0, DEAD_END_DOWN, DEAD_END_RIGHT, DEAD_END_LEFT, STRAIGHT_H, STRAIGHT_V, CURVE_UL, CURVE_UR, CURVE_DL, CURVE_DR, TRI_UP, TRI_DOWN, TRI_LEFT, TRI_RIGHT, CROSS }

public class CreateCity : MonoBehaviour
{
    private CityObject[] cityMap;
    private int mapHeight = 5;
    private int mapWidth = 8;
    public UnityEngine.Object[] houses;
    public UnityEngine.GameObject[] roads;

	// Use this for initialization
	void Start ()
    {
        string[] cityMapL = System.IO.File.ReadAllText("Assets/map.csv").Split(new char[] {';','\n'});
        for (int i = 0; i < cityMapL.Length; i++)
        { 
            Debug.Log(cityMapL[i]);
        }
        cityMap = new CityObject[cityMapL.Length];
        for (int i = 0; i < cityMapL.Length; i++)
        {
            cityMap[i] = (CityObject)Enum.Parse(typeof(CityObject), cityMapL[i]);
        }

        for (int i=0; i < mapHeight * mapWidth; i++)
        {
            CityObject cityObj = cityMap[i];
            int x = i % mapWidth;
            int y = i / mapWidth;
            switch (cityObj)
            {
                case CityObject.R: Instantiate(roads[(int)getRoadType(x, y)], new Vector3(-x*20,3,y*20)+ roads[(int)getRoadType(x, y)].transform.position, roads[(int)getRoadType(x,y)].transform.rotation ); break;
                default: Instantiate(houses[UnityEngine.Random.Range(0, 6)], new Vector3(-x*20, 0, y*20), Quaternion.Euler(-90, 0, 0)); break;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    RoadType getRoadType(int x, int y)
    {
        int nbRoad = 0;
        bool u = false, d = false, l = false, r = false;

        if (y + 1 < mapHeight && cityMap[x + (y + 1) * mapWidth] == CityObject.R)
        {
            nbRoad += 1;
            d = true;
        }
        if (y - 1 >= 0 && cityMap[x + (y - 1) * mapWidth] == CityObject.R)
        {
            nbRoad += 1;
            u = true;
        }
        if (x + 1 < mapWidth && cityMap[(x + 1) + y * mapWidth] == CityObject.R)
        {
            nbRoad += 1;
            r = true;
        }
        if (x - 1 >= 0 && cityMap[(x - 1) + y * mapWidth] == CityObject.R)
        {
            nbRoad += 1;
            l = true;
        }
        Debug.Log(nbRoad);
        if (nbRoad == 1)
        {
            if (u == true) { return RoadType.DEAD_END_DOWN; }
            else if (r == true) { return RoadType.DEAD_END_LEFT; }
            else if (d == true) { return RoadType.DEAD_END_UP; }
            else { return RoadType.DEAD_END_RIGHT; }
        }
        if (nbRoad == 2)
        {
            /*Charger murs horizontaux / verticaux*/
            if ((u == true) && (d == true)) { return RoadType.STRAIGHT_V; }
            else if ((l == true) && (r == true)) { return RoadType.STRAIGHT_H; }

            /*Charger les coins*/
            else if ((u == true) && (r == true)) { return RoadType.CURVE_UR; }
            else if ((u == true) && (l == true)) { return RoadType.CURVE_UL; }
            else if ((d == true) && (r == true)) { return RoadType.CURVE_DR; }
            else { return RoadType.CURVE_DL; }

        }
        if (nbRoad == 3)
        {
            if (d == false) { return RoadType.TRI_UP; }
            else if (r == false) { return RoadType.TRI_LEFT; }
            else if (l == false) { return RoadType.TRI_RIGHT; }
            else { return RoadType.TRI_DOWN; }
        }
        else
        {
            return RoadType.CROSS;
        }
    }
}