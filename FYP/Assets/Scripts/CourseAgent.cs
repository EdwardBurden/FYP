using MLAgents;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CourseAgent : Agent
{
    public float JumpForce;
    public float MoveForce;
    private BoxCollider AgentCollider;
    private RayPerception AgentRayPerception;
    private Rigidbody AgentRigidbody;

    public List<GameObject> Obstacles;

    public GameObject Target;
    public CourseAcademy Academy;


    public float GoalCount;
    public float DeathCount;
    public float Reward;

    bool Colliding;
    bool Falling;


    private Vector3 StartPosition = new Vector3(0, 0.5f, 20);

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        Colliding = false;
        Falling = false;
        GoalCount = 0;
        DeathCount = 0;
        UpdateUI();

        AgentRayPerception = GetComponent<RayPerception>();
        AgentRigidbody = GetComponent<Rigidbody>();
        AgentCollider = GetComponent<BoxCollider>();
        Academy = GameObject.FindObjectOfType<CourseAcademy>();
        CreateObstacles(Academy.SpawnLimit, Academy.Obstacle);
    }

    public void UpdateUI()
    {
        Reward = GetCumulativeReward();
    }

    public override void CollectObservations()
    {
        float rayDistance = 50f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        string[] detectableObjects = { "Goal", "Obstacle", "Wall" };

        AddVectorObs(AgentRayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(transform.localPosition);
        AddVectorObs(Target.transform.localPosition);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (!Physics.Raycast(AgentRigidbody.position, Vector3.down, 20) && !Colliding && !Falling)
        {
            Falling = true;
            SetReward(-1);
            DeathCount++;
            Done();
        }
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1f, 1f);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1f, 1f);
        AgentRigidbody.AddForce(controlSignal * MoveForce, ForceMode.VelocityChange);
        if (Mathf.Clamp(vectorAction[2], 0, 1f) == 1 && IsGrounded())
        {
            AgentRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
        UpdateRewards(controlSignal);
        UpdateUI();
    }


    public void UpdateRewards(Vector3 movement)
    {

        Vector2 goal = new Vector2(Target.GetComponent<Transform>().localPosition.x, Target.GetComponent<Transform>().localPosition.z);
        Vector2 move = new Vector2(movement.x, movement.z);
        float angle = Vector2.Angle(goal, move);
        float reward = Mathf.Cos(angle);
       if (reward > 0)
            reward *= 10;

        AddReward(reward / agentParameters.maxStep);
       // AddReward(-1.0f / agentParameters.maxStep);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (Colliding)
            return;
        Colliding = true;
        switch (other.tag)
        {
            case "Wall":
                AddReward(-1);
                DeathCount++;
                Done();
                break;
            case "Obstacle":
                AddReward(-1);
                DeathCount++;
                Done();
                break;
            case "Goal":
                AddReward(1);
                GoalCount++;
                Done();
                break;
            default:
                break;
        }

    }

    private bool IsGrounded()
    {

        return Physics.Raycast(transform.localPosition, Vector3.down, 0.5f);
    }

    public override void AgentReset()
    {
        Colliding = false;
        Falling = false;
        transform.localPosition = new Vector3(0, 0.6f, 20);
        AgentRigidbody.velocity = Vector3.zero;
        ResetObstacles(Academy.SpawnLimit, Academy.Obstacle);
    }
    public void CreateObstacles(float maximum, GameObject blocks)
    {

        for (int i = 0; i < maximum; i++)
        {
            GameObject ob = Instantiate(blocks, this.transform.parent);
            Obstacles.Add(ob);
            ob.transform.localPosition = new Vector3(Random.Range(Academy.ObstacleSpawnXRange.x, Academy.ObstacleSpawnXRange.y), 0.5f, Random.Range(Academy.ObstacleSpawnZRange.x, Academy.ObstacleSpawnZRange.y));
        }
    }

    public void ResetObstacles(float maximum, GameObject blocks)
    {
        foreach (GameObject obstacle in Obstacles)
        {
            obstacle.transform.localPosition = new Vector3(Random.Range(Academy.ObstacleSpawnXRange.x, Academy.ObstacleSpawnXRange.y), 0.5f, Random.Range(Academy.ObstacleSpawnZRange.x, Academy.ObstacleSpawnZRange.y));
        }
    }
}
