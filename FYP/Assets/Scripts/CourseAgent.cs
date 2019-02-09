using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseAgent : Agent
{

    RayPerception rayPer;
    Rigidbody agentRB;
    public Vector2 Xrange;
    public Vector2 Zrange;
    public GameObject Obstacle;
    public GameObject Target;
    public int SpawnLimit;
    public float speed = 0.5f;
    private Vector3 StartPosition = new Vector3(0, 0.5f, 20);

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        rayPer = GetComponent<RayPerception>();
        agentRB = GetComponent<Rigidbody>();
    }


    public override void CollectObservations()
    {
        float rayDistance = 50f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        string[] detectableObjects = { "Goal", "Obstacle", "Wall" };

        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(transform.position);
        AddVectorObs(Target.transform.position);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // AddReward(-0.005f);
        AddReward(-1.0f / agentParameters.maxStep);
        //AddReward(-0.0005f);
        if (Vector3.Distance(Target.transform.position, StartPosition) > Vector3.Distance(Target.transform.position, transform.position))
            AddReward(2.0f / agentParameters.maxStep);
        // AddReward(0.005f);
        else
            AddReward(-0.5f / agentParameters.maxStep);

        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1f, 1f);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1f, 1f);
        agentRB.AddForce(controlSignal * speed, ForceMode.VelocityChange);
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Wall":
            case "Obstacle":
                SetReward(-1); Done();
                break;
            case "Goal":
                AddReward(1); Done();
                break;
            default:
                break;
        }

    }

    public override void AgentReset()
    {
        transform.position = new Vector3(0, 0.5f, 20);
        agentRB.velocity = Vector3.zero;
        CreateObstacles(SpawnLimit, Obstacle);
    }
    public void CreateObstacles(int maximum, GameObject blocks)
    {
        GameObject[] obstacles;

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        for (int i = 0; i < maximum; i++)
        {
            GameObject ob = Instantiate(blocks, new Vector3(Random.Range(Xrange.x, Xrange.y), 0.5f, Random.Range(Zrange.x, Zrange.y)), Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
        }
    }
}
