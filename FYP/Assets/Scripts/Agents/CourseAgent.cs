using MLAgents;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CourseAgent : Agent
{
    public float JumpForce;
    public float MoveForce;
    private RayPerception AgentRayPerception;
    private Rigidbody AgentRigidbody;

    public List<GameObject> Obstacles;

    public GameObject Target;
    public CourseAcademy Academy;
    public MapGenerator MapGenerator;

    public int Counter;

    public float GoalCount;
    public float DeathCount;

    bool Colliding;
    bool Falling;


    private Vector3 StartPosition = new Vector3(0, 0.5f, 0);

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        Colliding = false;
        Falling = false;
        GoalCount = 0;
        DeathCount = 0;
        Counter = 0;
        AgentRayPerception = GetComponent<RayPerception>();
        AgentRigidbody = GetComponent<Rigidbody>();
        Academy = GameObject.FindObjectOfType<CourseAcademy>();
        CreateObstacles(Academy.SpawnLimit, Academy.Obstacle);
    }

    public float GetRewardForUi()
    {
        return GetCumulativeReward();
    }

    public override void CollectObservations()
    {
        float rayDistance = 50f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        string[] detectableObjects = { "Goal", "Obstacle", "Wall" };
        Vector3 relativePosition = transform.localPosition - Target.transform.localPosition;

        AddVectorObs(AgentRayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(relativePosition);
        AddVectorObs((float)(Counter / agentParameters.maxStep));
    }

    //public override void AgentAction(float[] vectorAction, string textAction)
    //{
    //    Counter++;
    //    if (!Physics.Raycast(AgentRigidbody.position, Vector3.down, 20) && !Colliding && !Falling)
    //    {
    //        Falling = true;
    //        SetReward(-1);
    //        DeathCount++;
    //        Done();
    //    }
    //    Vector3 controlSignal = Vector3.zero;
    //    controlSignal.x = Mathf.Clamp(vectorAction[0], -1f, 1f);
    //    controlSignal.z = Mathf.Clamp(vectorAction[1], -1f, 1f);
    //    AgentRigidbody.AddForce(controlSignal * MoveForce, ForceMode.VelocityChange);
    //    if (Mathf.Clamp(vectorAction[2], 0, 1f) == 1 && IsGrounded())
    //    {
    //        AgentRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    //    }
    //    UpdateRewards(controlSignal);
    //}

    public virtual void UpdateRewards(Vector3 movement) { }

    public bool HasFallen()
    {
        if (!Physics.Raycast(AgentRigidbody.position, Vector3.down, 20) && !Colliding && !Falling)
        {
            Falling = true;
            SetReward(-1);
            DeathCount++;
            Done();
            return true;
        }
        else
            return false;
    }



    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (!HasFallen())
        {
            int directionX = 0, directionZ = 0, directionY = 0;

            int move = Mathf.FloorToInt(vectorAction[0]);
            int jump = Mathf.FloorToInt(vectorAction[1]);

            switch (move)
            {
                case 1: directionX = -1; break;
                case 2: directionX = 1; break;
                case 3: directionZ = -1; break;
                case 4: directionZ = 1; break;
                default: break;
            }
            if (jump == 1 && IsGrounded()) { directionY = 1; }

            Vector3 controlSignal = new Vector3(directionX * 40f, directionY * 300f, directionZ * 40f);
            AgentRigidbody.AddForce(controlSignal);

            UpdateRewards(controlSignal);
            Counter++;
        }
    }

    


    public void OnTriggerEnter(Collider other)
    {
        if (!Colliding)
        {
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
                    AddReward(10.0f / (float)Counter);
                    GoalCount++;
                    Done();
                    break;
                default:
                    break;
            }
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
        if (MapGenerator != null)
            MapGenerator.GenerateNewLayout();

        Vector3 pos = new Vector3();
        pos.x = StartPosition.x + Random.Range(-5, 5);
        pos.z = StartPosition.z + Random.Range(-5, 5);
        pos.y = StartPosition.y;
        transform.localPosition = pos;
        AgentRigidbody.velocity = Vector3.zero;

        ResetObstacles(Academy.SpawnLimit, Academy.Obstacle);
        Academy.LogReset(GoalCount, DeathCount, GetReward());
        Counter = 0;

    }
    public void CreateObstacles(float maximum, GameObject blocks)
    {

        for (int i = 0; i < maximum; i++)
        {
            GameObject ob = Instantiate(blocks, this.transform.parent);
            Obstacles.Add(ob);
            if (MapGenerator != null)
                ob.transform.localPosition = new Vector3(Random.Range(MapGenerator.XRange.x, MapGenerator.XRange.y), 0.5f, Random.Range(MapGenerator.ZRange.x, MapGenerator.ZRange.y));
            else
                ob.transform.localPosition = new Vector3(Random.Range(Academy.ObstacleSpawnXRange.x, Academy.ObstacleSpawnXRange.y), 0.5f, Random.Range(Academy.ObstacleSpawnZRange.x, Academy.ObstacleSpawnZRange.y));
        }
    }

    public void ResetObstacles(float maximum, GameObject blocks)
    {
        foreach (GameObject obstacle in Obstacles)
        {
            Destroy(obstacle);
        }
        Obstacles.Clear();
        CreateObstacles(maximum, blocks);
        foreach (GameObject obstacle in Obstacles)
        {
            if (MapGenerator != null)
                obstacle.transform.localPosition = new Vector3(Random.Range(MapGenerator.XRange.x, MapGenerator.XRange.y), 0.5f, Random.Range(MapGenerator.ZRange.x, MapGenerator.ZRange.y));
            else
                obstacle.transform.localPosition = new Vector3(Random.Range(Academy.ObstacleSpawnXRange.x, Academy.ObstacleSpawnXRange.y), 0.5f, Random.Range(Academy.ObstacleSpawnZRange.x, Academy.ObstacleSpawnZRange.y));
        }
    }
}
