using MLAgents;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float GoalCount;
    private float DeathCount;
    private float Reward;

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
    }
    public override void AcademyStep()
    {
        base.AcademyStep();
        GoalCount = 0;
        DeathCount = 0;
        Reward = 0;
        GameObject[] agent = GameObject.FindGameObjectsWithTag("Agent");
        foreach (GameObject a in agent)
        {
            CourseAgent c = a.GetComponent<CourseAgent>();
            GoalCount += c.GoalCount;
            DeathCount += c.DeathCount;
            Reward += c.Reward;
        }
        GoalText.text = "Goals: " + GoalCount;
        DeathText.text = "Deaths: " + DeathCount;
        RewardText.text = "Reward: " + Reward/ agent.Length;
    }

}
