using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseAcademy : Academy
{
    public  float GoalDistance;
     public float PositiveReward;
     public float NegativeReward;

    public override void AcademyReset()
    {
        GoalDistance = resetParameters["GoalDistance"];
        PositiveReward = resetParameters["PositiveReward"];
        NegativeReward = resetParameters["NegativeReward"];

        GameObject goal = GameObject.FindGameObjectWithTag("Goal");
        goal.transform.position = new Vector3(goal.transform.position.x, goal.transform.position.y, GoalDistance);
    }
    public override void InitializeAcademy()
    {
        GameObject goal = GameObject.FindGameObjectWithTag("Goal");
        goal.transform.position = new Vector3(goal.transform.position.x, goal.transform.position.y, GoalDistance);
    }
}
