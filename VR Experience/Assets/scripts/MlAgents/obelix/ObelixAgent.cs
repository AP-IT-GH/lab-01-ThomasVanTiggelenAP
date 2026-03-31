using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class ObelixAgent : Agent
{
    public Transform menhirPrefab;
    public Transform destinationPrefab;
    public Transform playfield;
    public Vector2 playfieldSize = new Vector2(10f, 10f);

    public int numberOfMenhirs = 4;
    public float spawnRadius = 4f;

    public float speedMultiplier = 10f;      // Verhoogd van 5 naar 10 zodat hij sneller beweegt
    public float rotationMultiplier = 200f;

    public float pickupDistance = 1.5f;      // Afstand om menhir op te pakken
    public float deliverDistance = 1.5f;     // Afstand om menhir af te leveren

    private Rigidbody rb;
    private List<Transform> menhirs = new List<Transform>();
    private List<Transform> destinations = new List<Transform>();
    private bool isCarrying = false;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = playfield.position + new Vector3(0f, 1f, 0f);
        transform.rotation = Quaternion.identity;
        isCarrying = false;

        ClearList(menhirs);
        ClearList(destinations);

        // Verdeel cirkel in gelijke segmenten zodat objecten nooit overlappen
        int totalObjects = numberOfMenhirs * 2;
        float segmentSize = 360f / totalObjects;

        List<int> indices = new List<int>();
        for (int i = 0; i < totalObjects; i++) indices.Add(i);
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = indices[i]; indices[i] = indices[j]; indices[j] = tmp;
        }

        for (int i = 0; i < numberOfMenhirs; i++)
        {
            float menhirAngle = (indices[i] * segmentSize + Random.Range(0f, segmentSize * 0.8f)) * Mathf.Deg2Rad;
            Vector3 menhirPos = new Vector3(
                Mathf.Cos(menhirAngle) * spawnRadius,
                2f,
                Mathf.Sin(menhirAngle) * spawnRadius
            ) + playfield.position;

            Transform m = Instantiate(menhirPrefab, menhirPos, Quaternion.identity, playfield);
            menhirs.Add(m);

            float destAngle = (indices[i + numberOfMenhirs] * segmentSize + Random.Range(0f, segmentSize * 0.8f)) * Mathf.Deg2Rad;
            Vector3 destPos = new Vector3(
                Mathf.Cos(destAngle) * spawnRadius,
                0.5f,
                Mathf.Sin(destAngle) * spawnRadius
            ) + playfield.position;

            Transform d = Instantiate(destinationPrefab, destPos, Quaternion.identity, playfield);
            destinations.Add(d);
        }
    }

    private void ClearList(List<Transform> list)
    {
        foreach (Transform t in list)
        {
            if (t != null) Destroy(t.gameObject);
        }
        list.Clear();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Positie van de agent genormaliseerd
        sensor.AddObservation(transform.localPosition.x / 5f);
        sensor.AddObservation(transform.localPosition.z / 5f);

        // Draagt hij een menhir?
        sensor.AddObservation(isCarrying ? 1f : 0f);

        // Richting naar dichtstbijzijnde menhir
        Transform closestMenhir = GetClosest(menhirs);
        if (closestMenhir != null)
        {
            Vector3 dir = (closestMenhir.position - transform.position).normalized;
            sensor.AddObservation(dir.x);
            sensor.AddObservation(dir.z);
            // Ook afstand meegeven als observatie
            sensor.AddObservation(Vector3.Distance(transform.position, closestMenhir.position) / 10f);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }

        // Richting naar dichtstbijzijnde destination
        Transform closestDestination = GetClosest(destinations);
        if (closestDestination != null)
        {
            Vector3 dir = (closestDestination.position - transform.position).normalized;
            sensor.AddObservation(dir.x);
            sensor.AddObservation(dir.z);
            // Ook afstand meegeven als observatie
            sensor.AddObservation(Vector3.Distance(transform.position, closestDestination.position) / 10f);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Van het platform gevallen
        if (transform.position.y < -1f)
        {
            AddReward(-1f);
            EndEpisode();
            return;
        }

        // Beweging
        float moveZ = actions.ContinuousActions[0];
        float rotY = actions.ContinuousActions[1];

        Vector3 move = transform.forward * moveZ * speedMultiplier * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        Quaternion rot = Quaternion.Euler(0f, rotY * rotationMultiplier * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * rot);

        // Kleine tijdstraf zodat hij efficient leert bewegen
        AddReward(-0.0001f);

        if (!isCarrying)
        {
            // FASE 1: zoek en pak de dichtstbijzijnde menhir op
            Transform closestMenhir = GetClosest(menhirs);
            if (closestMenhir != null)
            {
                float dist = Vector3.Distance(transform.position, closestMenhir.position);

                // Shaping: hoe dichter bij de menhir, hoe meer kleine reward
                AddReward(0.002f / (dist + 0.1f));

                // Oppakken via afstandscheck
                if (dist < pickupDistance)
                {
                    isCarrying = true;
                    menhirs.Remove(closestMenhir);
                    Destroy(closestMenhir.gameObject);
                    AddReward(0.5f);
                }
            }
        }
        else
        {
            // FASE 2: breng de menhir naar de dichtstbijzijnde destination
            Transform closestDest = GetClosest(destinations);
            if (closestDest != null)
            {
                float dist = Vector3.Distance(transform.position, closestDest.position);

                // Shaping: hoe dichter bij de destination, hoe meer kleine reward
                AddReward(0.002f / (dist + 0.1f));

                // Afleveren via afstandscheck
                if (dist < deliverDistance)
                {
                    isCarrying = false;
                    destinations.Remove(closestDest);
                    Destroy(closestDest.gameObject);
                    AddReward(1.0f);

                    // Alle menhirs afgeleverd = episode klaar
                    if (destinations.Count == 0)
                    {
                        AddReward(2.0f);
                        EndEpisode();
                    }
                }
            }
        }
    }

    private Transform GetClosest(List<Transform> list)
    {
        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (Transform t in list)
        {
            if (t == null) continue;
            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetAxis("Vertical");
        actions[1] = Input.GetAxis("Horizontal");
    }
}