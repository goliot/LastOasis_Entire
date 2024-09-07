using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using Unity.MLAgents.Policies;
using System;
using Random = UnityEngine.Random;


public class BiAgent : Agent
{
    public bool Terminal = false;
    public static BiAgent instance;

    private bool initialized = false;

    public bool image_state;
    public int myTeam;

    public Building MySilo;
    public Building MyNexus;

    public Building EnemySilo;
    public Building EnemyNexus;

    [SerializeField]
    private Transform targetTransform;

    [HideInInspector]
    public Camera observationCamera; // ī�޶� ������Ʈ�� �����ϱ� ���� public ����

    [HideInInspector]
    private RenderTexture observationTexture; // ���� �ؽ��ĸ� ������ ����

    private int type;
    private string[] Types = { "Type1", "Type2", "Type3" };

    private float isUltimateUsed = 0;
    //private float reward = 0;

    private float mySiloMaxHealth;
    private float myNexusMaxHealth;

    private float enemySiloMaxHealth;
    private float enemyNexusMaxHealth;
    private int previousEnemyMobsCount;
    private int previousMyMobsCount;

    private AgentManager agentManager;

    private void Start()
    {
        BiAgent.instance = this;
    }

    public override void Initialize()
    {
        if (this.initialized)
        {
            return;
        }
        this.initialized = true;
        //if (Regame.instance.resetCount > 1)
        //{
        //    return;
        //}
        this.mySiloMaxHealth = this.MySilo.maxHealth;
        this.myNexusMaxHealth = this.MyNexus.maxHealth;

        this.enemySiloMaxHealth = this.EnemySilo.maxHealth;
        this.enemyNexusMaxHealth = this.EnemyNexus.maxHealth;

        if (this.image_state)
        {
            this.EnableCamera();
        }
        this.agentManager = GetComponent<AgentManager>();
    }

    /*public override void OnActionReceived(ActionBuffers actions)
    {
        int terminal = this.agentManager.DetermineTerminal();

        var agentAction = actions.DiscreteActions[0];

        float reward = this.ActionMaskingReward(agentAction);
        reward = reward + this.ActionReward(agentAction);

        float onGameReward = this.AgainstReward(agentAction);
        reward = reward + onGameReward;

        string team;
        if (this.myTeam == 0)
        {
            team = "Blue";

            this.previousMyMobsCount = GameManager.instance.blueCount;
            this.previousEnemyMobsCount = GameManager.instance.redCount;
        }
        else
        {
            team = "Red";

            this.previousMyMobsCount = GameManager.instance.redCount;
            this.previousEnemyMobsCount = GameManager.instance.blueCount;
        }

        if (terminal > 0)
        {
            //float onGameReward = this.AgainstReward(agentAction);
            //reward = reward + onGameReward;

            //this.reward = this.reward + reward;
            //this.reward = this.reward / this.StepCount;
            this.Terminal = true;
            SetReward(reward);
        }
        else
        {
            SetReward(reward);
        }
        Debug.Log($"{team} Team, {this.Types[this.type]} | Reward: {reward} | Cumulative Reward: {this.GetCumulativeReward()} | StepCount: {this.StepCount} | FPS: {FPS.fps}");
    }*/

    private float ActionMaskingReward(int agentAction)
    {
        float reward = 0;
        if (agentAction == 12 && this.isUltimateUsed == 0)
        {
            this.isUltimateUsed = 1;
        }
        else if (agentAction == 12 && this.isUltimateUsed == 1)
        {
            reward = reward - 1;
        }



        return reward;
    }

    private float ActionReward(int agentAction)
    {
        float reward = -1;

        int oneStepActionReward = -1 * this.agentManager.SubmitAction(0, this.Types[this.type], agentAction);
        if (oneStepActionReward != 2)
        {
            reward = reward + oneStepActionReward;
        }

        return reward / 2;
    }

    private float AgainstReward(int agentAction)
    {

        float defensePenalty = -1 * (((this.myNexusMaxHealth - this.MyNexus.currentHealth) / this.myNexusMaxHealth) + ((this.mySiloMaxHealth - this.MySilo.currentHealth) / this.mySiloMaxHealth)) / 2;
        float attackReward = ((this.enemyNexusMaxHealth - this.EnemyNexus.currentHealth) / this.enemyNexusMaxHealth + (this.enemySiloMaxHealth - this.EnemySilo.currentHealth) / this.enemySiloMaxHealth) / 2;

        int killedPenalty;
        int killingReward;

        if (this.previousMyMobsCount > GameManager.instance.blueCount)
        {
            killedPenalty = -1;
        }
        else if (this.previousMyMobsCount < GameManager.instance.blueCount)
        {
            killedPenalty = 1;
        }
        else
        {
            killedPenalty = 0;
        }

        if (this.previousEnemyMobsCount > GameManager.instance.redCount)
        {
            killingReward = 1;
        }
        else if (this.previousEnemyMobsCount < GameManager.instance.redCount)
        {
            killingReward = -1;
        }
        else
        {
            killingReward = 0;
        }
        return (defensePenalty + attackReward) + (killedPenalty + killingReward / GameManager.instance.blueCount) / 2;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (this.image_state)
        {
            // ���� �ؽ��Ŀ��� ���� �����ͷ� ��ȯ
            Texture2D observation = ObservationToTexture(observationTexture);

            observation = this.ScaleTexture(observation, 224, 224);

            var instanceId = this.name;
            // �̹����� ������ ��� ����
            string imagePath = $"D:/source/RLs/results/observations/{instanceId}/";

            // �����Ӹ��� �̹��� ����
            if (Input.GetKey(KeyCode.X))
            {
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }
                CaptureObservations(observation, imagePath + $"observation_{this.StepCount}.png");
            }


            // ���� �����͸� VectorSensor�� �߰�
            sensor.AddObservation(observation);
        }
        else
        {
            float time = GameManager.instance.gameTime;

            float myTeam = 0;

            float myNexusHealth = this.MyNexus.currentHealth;
            float mySiloHealth = this.MySilo.currentHealth;

            float enemyNexusHealth = this.EnemyNexus.currentHealth;
            float enemySiloHealth = this.EnemySilo.currentHealth;

            float myResource;
            int myCount;
            int enemyCount;

            myResource = GameManager.instance.resource;

            myCount = GameManager.instance.blueCount;
            enemyCount = GameManager.instance.redCount;

            sensor.AddObservation(time);

            sensor.AddObservation(myTeam);
            sensor.AddObservation(this.type);
            sensor.AddObservation(this.isUltimateUsed);

            sensor.AddObservation(myNexusHealth);
            sensor.AddObservation(enemyNexusHealth);

            sensor.AddObservation(enemySiloHealth);
            sensor.AddObservation(enemySiloHealth);

            sensor.AddObservation(myResource);

            sensor.AddObservation(myCount);
            sensor.AddObservation(enemyCount);
        }
    }

    // RenderTexture�� Texture2D�� ��ȯ�ϴ� �Լ�
    private Texture2D ObservationToTexture(RenderTexture rt)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        RenderTexture.active = currentActiveRT;
        return tex;
    }

    private void CaptureObservations(Texture2D image, string path)
    {
        // �̹����� ���Ϸ� ����
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
        Debug.Log($"Saved image to {path}");
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //if (this.isLearning)
        //{
        //    if (this.b_Params.TeamId == (int) Team.Blue)
        //    {
        //        var heuristicAction = actionsOut.DiscreteActions;

        //        if (Input.GetKeyUp(KeyCode.Alpha1))
        //        {
        //            heuristicAction[0] = 1;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha2))
        //        {
        //            heuristicAction[0] = 2;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha3))
        //        {
        //            heuristicAction[0] = 3;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha4))
        //        {
        //            heuristicAction[0] = 4;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha5))
        //        {
        //            heuristicAction[0] = 5;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha6))
        //        {
        //            heuristicAction[0] = 6;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha7))
        //        {
        //            heuristicAction[0] = 7;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha8))
        //        {
        //            heuristicAction[0] = 8;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha9))
        //        {
        //            heuristicAction[0] = 9;
        //        }

        //        else if (Input.GetKey(KeyCode.Alpha0))
        //        {
        //            heuristicAction[0] = 10;
        //        }

        //        else if (Input.GetKey(KeyCode.Minus))
        //        {
        //            heuristicAction[0] = 11;
        //        }
        //        else if (Input.GetKey(KeyCode.Plus))
        //        {
        //            heuristicAction[0] = 12;
        //        }
        //        else if (Input.GetKey(KeyCode.Backspace))
        //        {
        //            heuristicAction[0] = 13;
        //        }
        //        else
        //        {
        //            heuristicAction[0] = 0;
        //        }
        //    }
        //    // Red ���� �� �߰� ���� (����)
        //}
        //if (this.MyTeam == 0)
        //{

        var heuristicAction = actionsOut.DiscreteActions;

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            heuristicAction[0] = 1;
        }

        else if (Input.GetKey(KeyCode.Alpha2))
        {
            heuristicAction[0] = 2;
        }

        else if (Input.GetKey(KeyCode.Alpha3))
        {
            heuristicAction[0] = 3;
        }

        else if (Input.GetKey(KeyCode.Alpha4))
        {
            heuristicAction[0] = 4;
        }

        else if (Input.GetKey(KeyCode.Alpha5))
        {
            heuristicAction[0] = 5;
        }

        else if (Input.GetKey(KeyCode.Alpha6))
        {
            heuristicAction[0] = 6;
        }

        else if (Input.GetKey(KeyCode.Alpha7))
        {
            heuristicAction[0] = 7;
        }

        else if (Input.GetKey(KeyCode.Alpha8))
        {
            heuristicAction[0] = 8;
        }

        else if (Input.GetKey(KeyCode.Alpha9))
        {
            heuristicAction[0] = 9;
        }

        else if (Input.GetKey(KeyCode.Alpha0))
        {
            heuristicAction[0] = 10;
        }

        else if (Input.GetKey(KeyCode.Minus))
        {
            heuristicAction[0] = 11;
        }
        else if (Input.GetKey(KeyCode.Plus))
        {
            heuristicAction[0] = 12;
        }
        else if (Input.GetKey(KeyCode.Backspace))
        {
            heuristicAction[0] = 13;
        }
        else
        {
            heuristicAction[0] = 0;
        }

        //}
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("BlueAgent Episode Begin");
        this.type = Random.Range(0, 3);
        this.previousEnemyMobsCount = 0;
        this.previousMyMobsCount = 0;
        this.Terminal = false;
    }

    public void EnableCamera()
    {
        this.observationCamera = GetComponent<Camera>();
        // ���� �ؽ��� �ʱ�ȭ
        this.observationTexture = new RenderTexture(this.observationCamera.pixelWidth, this.observationCamera.pixelHeight, 32); // ������ �ػ󵵿� ���̸� ����

        this.observationTexture.Create();

        this.observationCamera.targetTexture = observationTexture;
    }

    public void DisableCamera()
    {
        this.observationCamera = null;
        this.observationTexture = null;
    }
}
