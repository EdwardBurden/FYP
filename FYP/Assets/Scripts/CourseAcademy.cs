using MLAgents;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CourseAcademy : Academy
{
    public float GoalDistance;
    public float SpawnLimit;

    public Vector2 ObstacleSpawnXRange;
    public Vector2 ObstacleSpawnZRange;

    public GameObject Obstacle;

    public Text GoalText;
    public Text DeathText;
    public Text RewardText;
    public Text TimeText;

    private float GoalCount;
    private float DeathCount;
    private float Reward;
    private float StepCount = 0;

    public string path;

    public override void AcademyReset()
    {
        GoalDistance = resetParameters["GoalDistance"];
        SpawnLimit = resetParameters["SpawnLimit"];

        GameObject[] goal = GameObject.FindGameObjectsWithTag("Goal");
        foreach (GameObject g in goal)
        {
            g.transform.localPosition = new Vector3(g.transform.localPosition.x, g.transform.localPosition.y, GoalDistance);
        }
    }
    public override void InitializeAcademy()
    {
        GameObject[] goal = GameObject.FindGameObjectsWithTag("Goal");
        foreach (GameObject g in goal)
        {
            g.transform.localPosition = new Vector3(g.transform.localPosition.x, g.transform.localPosition.y, GoalDistance);
        }
        StepCount = 0;
        CreateFile();
    }

    public void CreateFile()
    {
        string day = DateTime.Now.ToLongDateString() + DateTime.Now.ToString(" hh mm ss");
        path = Application.dataPath + "/Resources/Data_" + day + ".txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Goals,Deaths,Reward,Step \n");
        }
    }


    public void LogReset(float goal, float death, float reward)
    {
        string entry = goal.ToString() +","+ death.ToString()+ ","+ reward.ToString() +"," + StepCount +"\n";
        File.AppendAllText(path, entry);
    }


    public override void AcademyStep()
    {
        base.AcademyStep();
        float minutes = (int)(Time.time / 60.0f);
        float seconds = (int)(Time.time % 60.0f);
        StepCount++;

        GoalCount = 0;
        DeathCount = 0;
        Reward = 0;
        GameObject[] agent = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject a in agent)
        {
            CourseAgent c = a.GetComponent<CourseAgent>();
            GoalCount += c.GoalCount;
            DeathCount += c.DeathCount;
            Reward += c.GetReward();
        }
        GoalText.text = "Goals: " + GoalCount;
        DeathText.text = "Deaths: " + DeathCount;
        RewardText.text = "Reward: " + Reward / agent.Length;
        TimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

}
