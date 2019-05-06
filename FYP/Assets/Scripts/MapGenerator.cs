using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    private GameObject Goal;
    private GameObject[] Walls;
    private GameObject Floor;
    public float GoalDistance;
    public Vector2 XRange, ZRange;

    public void GenerateNewLayout(int distance)
    {
        Goal = GetObject("Goal");

        if (Goal != null)
        {
            Walls = GetWalls();
            if (Walls != null)
            {
                Floor = GetObject("Floor");
                if (Floor != null)
                {
                    int Length = Random.Range(10, distance); //Randomly generate Length of area
                    int Width = Random.Range(1, 4);     //Ranom generate Width of area
                    int Direction = Random.Range(1, 5); //Direction can be of 4 possible direction (-z,z,x,-x)

                    if (Direction == 1) //If direction ==1 (Positive x Direction)
                    {
                        Goal.transform.localPosition = new Vector3(Length, 0.5f, 0);     //Move the goal to new position at end of the area
                        Goal.transform.localScale = new Vector3(0.1f, 1, Width * 10);    //Scale it to fit the wdith of the area

                        Floor.transform.localPosition = new Vector3(Length / 2, 0, 0);              //Move the floor to center of area
                        Floor.transform.localScale = new Vector3((Length / 10.0f) + 1, 1, Width);   //scale it to fit the length

                        XRange = new Vector2(0, Length);                                            //Adjust the area in which obstacles can be spawned
                        ZRange = new Vector2(-(Width * 5.0f) * 0.9f, (Width * 5.0f) * 0.9f);        //Adjust the area in which obstacles can be spawned

                        Walls[0].transform.localPosition = new Vector3(-5, 0, 0);                   //Move wall to behind the Agent
                        Walls[0].transform.localScale = new Vector3(1, 1, 10 * Width);              //Scale by Width 

                        Walls[1].transform.localPosition = new Vector3(Length + 5, 0, 0);           //Move wall to behind the goal
                        Walls[1].transform.localScale = new Vector3(1, 1, 10 * Width);              //Scale by width

                        Walls[2].transform.localPosition = new Vector3(Length / 2, 0, Width*5);     //Move the wall to center of area, offset by width
                        Walls[2].transform.localScale = new Vector3(Length  + 10, 1, 1);            //Scale by length

                        Walls[3].transform.localPosition = new Vector3(Length / 2, 0, -Width*5);    //Move the wall to center of area, offset by width
                        Walls[3].transform.localScale = new Vector3(Length  + 10, 1, 1);            //Scale by length
                    }

                    if (Direction == 2)
                    {
                        Goal.transform.localPosition = new Vector3(-Length, 0.5f, 0);
                        Goal.transform.localScale = new Vector3(0.1f, 1, Width * 10);

                        Floor.transform.localPosition = new Vector3(-Length / 2, 0, 0);
                        Floor.transform.localScale = new Vector3((Length / 10.0f) + 1, 1, Width);

                        XRange = new Vector2(0, -Length);
                        ZRange = new Vector2(-(Width * 5.0f) * 0.9f, (Width * 5.0f) * 0.9f);

                        Walls[0].transform.localPosition = new Vector3(5, 0, 0);
                        Walls[0].transform.localScale = new Vector3(1, 1, 10 * Width);

                        Walls[1].transform.localPosition = new Vector3(-(Length + 5), 0, 0);
                        Walls[1].transform.localScale = new Vector3(1, 1, 10 * Width);

                        Walls[2].transform.localPosition = new Vector3(-Length / 2, 0, Width * 5);
                        Walls[2].transform.localScale = new Vector3(Length + 10, 1, 1);

                        Walls[3].transform.localPosition = new Vector3(-Length / 2, 0, -Width * 5);
                        Walls[3].transform.localScale = new Vector3(Length + 10, 1, 1);
                    }

                    if (Direction == 3)
                    {
                        Goal.transform.localPosition = new Vector3(0, 0.5f, Length);
                        Goal.transform.localScale = new Vector3(Width * 10, 1, 1.0f);

                        Floor.transform.localPosition = new Vector3(0, 0, Length / 2);
                        Floor.transform.localScale = new Vector3(Width, 1, (Length / 10.0f) + 1);

                        ZRange = new Vector2(0, Length);
                        XRange = new Vector2(-(Width * 5.0f) * 0.9f, (Width * 5.0f) * 0.9f);

                        Walls[0].transform.localPosition = new Vector3(0, 0, -5);
                        Walls[0].transform.localScale = new Vector3(10 * Width, 1, 1);

                        Walls[1].transform.localPosition = new Vector3( 0, 0,Length + 5);
                        Walls[1].transform.localScale = new Vector3(10 * Width, 1, 1);

                        Walls[2].transform.localPosition = new Vector3(Width * 5, 0, Length / 2);
                        Walls[2].transform.localScale = new Vector3(1, 1, Length + 10);

                        Walls[3].transform.localPosition = new Vector3(-Width * 5, 0, Length / 2);
                        Walls[3].transform.localScale = new Vector3(1, 1, Length + 10);
                    }


                    if (Direction == 4)
                    {
                        Goal.transform.localPosition = new Vector3(0, 0.5f, -Length);
                        Goal.transform.localScale = new Vector3(Width * 10, 1, 1.0f);

                        Floor.transform.localPosition = new Vector3(0, 0, -Length / 2);
                        Floor.transform.localScale = new Vector3(Width, 1, (Length / 10.0f) + 1);

                        ZRange = new Vector2(0, -Length);
                        XRange = new Vector2(-(Width * 5.0f) * 0.9f, (Width * 5.0f) * 0.9f);

                        Walls[0].transform.localPosition = new Vector3(0, 0, 5);
                        Walls[0].transform.localScale = new Vector3(10 * Width, 1, 1);

                        Walls[1].transform.localPosition = new Vector3(0, 0, -(Length + 5));
                        Walls[1].transform.localScale = new Vector3(10 * Width, 1, 1);

                        Walls[2].transform.localPosition = new Vector3(Width * 5, 0, -Length / 2);
                        Walls[2].transform.localScale = new Vector3(1, 1, Length + 10);

                        Walls[3].transform.localPosition = new Vector3(-Width * 5, 0, -Length / 2);
                        Walls[3].transform.localScale = new Vector3(1, 1, Length + 10);
                    }




                }
            }
        }
    }

    private GameObject GetObject(string name)
    {
        Transform goalT = this.transform.Find(name);
        if (goalT != null)
        {
            return goalT.gameObject;
        }
        else
        {
            return null;
        }
    }

    private GameObject[] GetWalls()
    {
        string wall;
        GameObject[] walls = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {

            wall = "Wall" + i;
            Transform result = this.transform.Find(wall);
            if (result != null)
            {
                walls[i] = result.gameObject;
            }
            else
            {
                return null;
            }
        }
        return walls;
    }
}
