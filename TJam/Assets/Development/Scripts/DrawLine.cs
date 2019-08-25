using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    public Toggle toggle;

    public Transform origin;
    public Transform destination;
    public bool draw = false;


    private Vector3 deface;
    private Vector3 hide;


    private void Start()
    {
        deface = new Vector3(0.0f, 10f, 0.0f);
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, origin.position + deface);
        lineRenderer.SetPosition(1, destination.position + deface);
    }

    void Update()
    {
        if (toggle.isOn)
        {
            lineRenderer.SetWidth(8.0f, 8.0f);       
        }
        else
        {
            lineRenderer.SetWidth(0f, 0f);
        }
    }

    public void ToogleActive()
    {
        draw = true;
    }
}
