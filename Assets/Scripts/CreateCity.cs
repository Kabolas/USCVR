using System;
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
    public UnityEngine.GameObject[] houses;
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
            GameObject obj;
            switch (cityObj)
            {
                case CityObject.R:
                    obj = roads[(int)getRoadType(x, y)];
                    break;
                default:
                    obj = houses[UnityEngine.Random.Range(0, houses.Length)];
                    break;
            }
            Instantiate(obj, new Vector3(-x * 20 -10, 0, y * 20 + 20) + obj.transform.position, obj.transform.rotation);
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

    void isRoad(IsRoadArguments args)
    {
        if(args.position.x > 0 || args.position.z < 0)
        {
            args.answer = false;
            return;
        }
        int x = - (int) args.position.x/20;
        int y = (int) args.position.z/20;
        if(cityMap[x+y*mapWidth] == CityObject.R)
        {
            args.answer = true;
        }
        else
        {
            args.answer = false;
        }
        Debug.Log(args.position.x + "," + args.position.z + " : " + x + ", " + y + " " + args.answer);
    }

    void getCarTransformOnRoad(CarTransformArgs args)
    {
        IsRoadArguments argsIs = new IsRoadArguments(args.transform.position);
        isRoad(argsIs);
        if(argsIs.answer)
        {
            int x = -(int)args.transform.position.x / 20;
            int y = (int)args.transform.position.z / 20;

            int relativeX = -(int)args.transform.position.x % 20;
            int relativeY = (int)args.transform.position.z % 20;

            Debug.Log(relativeX + " " + relativeY);

            RoadType r = getRoadType(x, y);
            switch(r)
            {
                case RoadType.DEAD_END_LEFT:
                case RoadType.DEAD_END_RIGHT:
                case RoadType.STRAIGHT_H:
                    if(relativeY <= 10)
                    {
                        args.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    else
                    {
                        args.transform.rotation = Quaternion.Euler(0, -90, 0);
                    }
                    break;
                case RoadType.DEAD_END_DOWN:
                case RoadType.DEAD_END_UP:
                case RoadType.STRAIGHT_V:
                    if (relativeX <= 10)
                    {
                        args.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        args.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;
                case RoadType.CROSS:
                    break;
                case RoadType.TRI_UP:
                    if(relativeY <= 3)
                    {
                        if (relativeX <= 10)
                        {
                            args.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            args.transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    else if (relativeX <= 10)
                    {
                        args.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        args.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;
                case RoadType.TRI_DOWN:
                    if (relativeY >= 17)
                    {
                        if (relativeX <= 10)
                        {
                            args.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            args.transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    else if (relativeX >= 10)
                    {
                        args.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        args.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;
            }
        }
    }
}