using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Cart : Agent
{
    public GameObject pole;
    Rigidbody m_PoleRb;
    Rigidbody m_CartRb;
    void Start()
    {
        // Debug.Log("Pleeeeeeaaaaaaaase tell me this works!");
        m_CartRb = GetComponent<Rigidbody>();
        m_PoleRb = pole.GetComponent<Rigidbody>();
    }
    
    public override void OnEpisodeBegin()
    {
        // Debug.Log("It - works - beep - bop");
        this.m_CartRb.angularVelocity = Vector3.zero;
        this.m_CartRb.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 0, 0);
        // why local position?

        m_PoleRb.velocity = Vector3.zero;
        m_PoleRb.angularVelocity = Vector3.zero;
        pole.transform.localPosition = new Vector3(0, 1.5f, 0);
        pole.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        pole.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-20f, 20f));
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.position.x);
        sensor.AddObservation(pole.transform.rotation.z);
        sensor.AddObservation(m_CartRb.velocity.x);
        sensor.AddObservation(m_PoleRb.angularVelocity.z);
        // Debug.Log("Rottota: " + pole.transform.rotation.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.y = actionBuffers.ContinuousActions[0];
        // m_CartRb.AddForce(actionBuffers.ContinuousActions[0], 0, 0, ForceMode.Force); //controlSignal * forceMultiplier
        
        // Debug.Log("Hello: " + controlSignal);
        this.transform.Translate(controlSignal);
        // if (Mathf.Abs(this.transform.position.x) < 10)
        // {
        //     this.transform.Translate(controlSignal);
        // }

        if (Mathf.Abs(pole.transform.rotation.z) > 0.2) //0.7
        {
            EndEpisode();
        }
        else
        {
            SetReward(1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        // Debug.Log("Action: " + continuousActionsOut);
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
    
}
