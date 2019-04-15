using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseAgent_SimpleReward : CourseAgent {

    public override void UpdateRewards(Vector3 movement)
    {
        Vector2 goal = new Vector2(Target.GetComponent<Transform>().localPosition.x, Target.GetComponent<Transform>().localPosition.z);
        Vector2 move = new Vector2(movement.x, movement.z);
        float angle = Vector2.Angle(goal, move);
        float reward = Mathf.Cos(angle);
        reward *= 0.1f;

        AddReward(reward / agentParameters.maxStep);
    }
}
