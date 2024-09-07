using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class agentt : Agent
{
    public override void Initialize()
    {
        Debug.Log("Initialize");       
    }

    /*public override void OnActionReceived(ActionBuffers actions)
    {
        bool terminal = Building.Terminal;
        if (terminal)
        {
            EndEpisode();
        }
        Debug.Log($"Step {this.StepCount}");
    }*/

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(0);
        sensor.AddObservation(1);
        sensor.AddObservation(2);
        sensor.AddObservation(3);
        sensor.AddObservation(4);
        sensor.AddObservation(5);
        sensor.AddObservation(6);
        sensor.AddObservation(7);
        sensor.AddObservation(8);
        sensor.AddObservation(8);
        sensor.AddObservation(10);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin");
    }
}
