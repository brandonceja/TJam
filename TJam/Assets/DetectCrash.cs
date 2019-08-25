using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCrash : MonoBehaviour
{
    public CarEngine car;

    private void OnTriggerEntry(Collider other)
    {
        Debug.Log("Stop");
        if (other.gameObject.tag == "Car")
        {
            Debug.Log("Stop");
            car.canDrive = false;
        }
    }
}
