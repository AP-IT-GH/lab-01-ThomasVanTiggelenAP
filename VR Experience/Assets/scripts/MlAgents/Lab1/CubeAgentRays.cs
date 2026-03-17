using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class CubeAgentRays : Agent
{
    public Transform Target;
    public Transform GreenZone; // Referentie naar de groene plane
    public float speedMultiplier = 0.5f;
    public float rotationMultiplier = 5;

    private bool hasPickedUpTarget = false; // Bijhouden in welke fase we zitten

    public override void OnEpisodeBegin()
    {
        // reset de positie en orientatie als de agent gevallen is

        if (this.transform.localPosition.y < 0)
        {

            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }

        // Reset fase aan begin van elke episode
        hasPickedUpTarget = false;

        // Target terug zichtbaar maken
        Target.gameObject.SetActive(true);

        // verplaats de target naar een nieuwe willekeurige locatie

        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target en Agent posities
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(hasPickedUpTarget ? 1f : 0f); // welke fase?
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Acties, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);

        transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[1], 0, 0f);

        if (!hasPickedUpTarget)
        {
            // FASE 1: Zoek het blokje
            float distanceToTarget = Vector3.Distance(
                this.transform.localPosition, Target.localPosition
            );

            if (distanceToTarget < 1.42f)
            {
                hasPickedUpTarget = true;        // Ga naar fase 2
                Target.gameObject.SetActive(false); // Blokje verdwijnt
                AddReward(0.5f);                 // Tussenbeloning
            }
        }
        else
        {
            // FASE 2: Ga naar de groene zone
            float distanceToGreenZone = Vector3.Distance(
                this.transform.localPosition, GreenZone.localPosition
            );

            if (distanceToGreenZone < 2f)
            {
                SetReward(1.0f);  // Volle beloning
                EndEpisode();     // Episode klaar!
            }
        }

        // Van het platform gevallen?
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

}
