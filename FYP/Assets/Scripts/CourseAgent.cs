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
    public Vector2 ObstacleSpawnXRange;
    public Vector2 ObstacleSpawnZRange;
    public GameObject Obstacle;
    public GameObject Target;
    public int SpawnLimit;

    private int GoalCount;
    private int DeathCount;
    public Text GoalText;
    public Text DeathText;
    public Text RewardText;

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
    }

    public void UpdateUI()
    {
        GoalText.text = "Goals: " + GoalCount;
        DeathText.text = "Deaths: " + DeathCount;
        RewardText.text = "Reward: " + GetCumulativeReward();
    }

    public override void CollectObservations()
    {
        float rayDistance = 50f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        string[] detectableObjects = { "Goal", "Obstacle", "Wall" };

        AddVectorObs(AgentRayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(transform.position);
        AddVectorObs(Target.transform.position);
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
        AddReward(-1.0f / agentParameters.maxStep);
        if (Vector3.Distance(Target.transform.position, StartPosition) > Vector3.Distance(Target.transform.position, transform.position))
            AddReward(2.0f / agentParameters.maxStep);
        else
            AddReward(-0.5f / agentParameters.maxStep);

        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1f, 1f);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1f, 1f);
        AgentRigidbody.AddForce(controlSignal * MoveForce, ForceMode.VelocityChange);
        if (Mathf.Clamp(vectorAction[2], 0, 1f) == 1 && IsGrounded())
        {
            AgentRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
        
        UpdateUI();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Colliding)
            return;
        Colliding = true;
        switch (other.tag)
        {
            case "Wall":
            case "Obstacle":
                SetReward(-1);
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

        return Physics.Raycast(transform.position, Vector3.down, 0.5f);
    }

    public override void AgentReset()
    {
        Colliding = false;
        Falling = false;
        transform.position = new Vector3(0, 0.6f, 20);
        AgentRigidbody.velocity = Vector3.zero;
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
            GameObject ob = Instantiate(blocks, new Vector3(Random.Range(ObstacleSpawnXRange.x, ObstacleSpawnXRange.y), 0.5f, Random.Range(ObstacleSpawnZRange.x, ObstacleSpawnZRange.y)), Quaternion.Euler(new Vector3(0f, Random.Range(0f, 360f), 90f)));
        }
    }
}
