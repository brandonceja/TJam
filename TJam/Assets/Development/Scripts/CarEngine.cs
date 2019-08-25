using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public float maxSteerAngle = 45f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public float MaxMotorTorque = 500f;
    public float currentSpeed;
    public float maximumSpeed = 120f;
    public float breakPower = 10000f;

    private int currentPath = 0;
    private int currentNode = 0;

    public bool canDrive = true;

    public List<Path> paths;


    private List<Transform>[] nodes;


    private void Start()
    {
        nodes = new List<Transform>[paths.Count];

        for (int i = 0; i < paths.Count; i++)
        {
            Transform[] pathTransforms = paths[i].GetComponentsInChildren<Transform>();
            nodes[i] = new List<Transform>();

            for (int j = 0; j < pathTransforms.Length; j++)
            {
                if (pathTransforms[j] != paths[i].transform)
                {
                    nodes[i].Add(pathTransforms[j]);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();

        float distance;
        float time = 0.5f;

        if(currentNode < nodes[currentPath].Count)
        {
            distance = Vector3.Distance(nodes[currentPath][currentNode].position, nodes[currentPath][currentNode + 1].position);
            maximumSpeed = (distance / time) * 1000f;
        }

        

    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentPath][currentNode].position);
        relativeVector /= relativeVector.magnitude;

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;

        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        if (canDrive)
        {
           
            if (currentSpeed < maximumSpeed)
            {
                wheelFR.brakeTorque = 0f;
                wheelFL.brakeTorque = 0f;
                wheelFL.motorTorque = MaxMotorTorque;
                wheelFR.motorTorque = MaxMotorTorque;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
            }
            
            if (Vector3.Distance(transform.position, nodes[currentPath][currentNode].position) < 12.0f 
                && currentNode > 0 && currentSpeed > 0)
            {
                   wheelFL.motorTorque = -6000f;
                   wheelFR.motorTorque = -6000f;
                    
            }
        }
        else
        {
            if(currentSpeed > 0)
            {
                wheelFR.brakeTorque  = Mathf.Infinity;
                wheelFL.brakeTorque = Mathf.Infinity;
            }
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentPath][currentNode].position) < 5.0f)
        {
            if (currentPath == nodes.Length - 1 && currentNode == nodes[currentPath].Count - 1)
            {
                Stop();
            }


            if (currentNode == nodes[currentPath].Count - 1)
            {
                //10.22.173.231:5000
                GameObject.Find("GameManager").GetComponent<DatasetManager>().carDensity[paths[currentPath].pathName]--;
               
                currentPath++;
                currentNode = 0;
            }
            else
                currentNode++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            canDrive = false;
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if(distance < 7)
                this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canDrive = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Stop()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

}
