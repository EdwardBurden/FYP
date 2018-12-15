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
    public GameObject area;
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
        float rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        string[] detectableObjects = { "Goal", "Obstacle", "Wall" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));

        Bounds are = area.transform.GetComponent<Renderer>().bounds;
        Vector3 size = are.max - are.min;
        AddVectorObs(agentRB.velocity.x / size.x);
        AddVectorObs(agentRB.velocity.z / size.z);

        float distancemoved = Vector3.Distance(transform.position, StartPosition);
        float maxdistance = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, -25f), StartPosition);
        AddVectorObs(distancemoved / maxdistance);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(-0.05f);

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
                AddReward(-1); Done();
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
