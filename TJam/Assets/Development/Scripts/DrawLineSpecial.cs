using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineSpecial : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform node1;
    public Transform node2;
    public Transform node3;
    public Transform node4;
    public Transform node5;
    public Transform node6;
    public Transform node7;



    private Vector3 deface;

    void Start()
    {
        deface = new Vector3(0.0f, 10f, 0.0f);
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, node1.position + deface);
        lineRenderer.SetWidth(8.0f, 8.0f);
        lineRenderer.SetPosition(1, node2.position + deface);
    }
}
