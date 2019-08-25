using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color lineColor;
    private List<Transform> nodes = new List<Transform>();
    public string pathName;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        Vector3 previousNode = Vector3.zero;

        for (int i=1; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            if (i > 0)
            {
               previousNode = nodes[i - 1].position;
            }

            /*
             *  Loop to last node
              else if (i == 0 && nodes.Count > 1)
              {
                  previousNode = nodes[nodes.Count - 1].position;
              }
              */

            // Gizmos.DrawLine(previousNode, currentNode);
            DrawLine(previousNode, currentNode, 20f);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }

    public static void DrawLine(Vector3 p1, Vector3 p2, float width)
    {
        int count = 1 + Mathf.CeilToInt(width); // how many lines are needed.
        if (count == 1)
        {
            Gizmos.DrawLine(p1, p2);
        }
        else
        {
            Camera c = Camera.current;
            if (c == null)
            {
                Debug.LogError("Camera.current is null");
                return;
            }
            var scp1 = c.WorldToScreenPoint(p1);
            var scp2 = c.WorldToScreenPoint(p2);

            Vector3 v1 = (scp2 - scp1).normalized; // line direction
            Vector3 n = Vector3.Cross(v1, Vector3.forward); // normal vector

            for (int i = 0; i < count; i++)
            {
                Vector3 o = 0.99f * n * width * ((float)i / (count - 1) - 0.5f);
                Vector3 origin = c.ScreenToWorldPoint(scp1 + o);
                Vector3 destiny = c.ScreenToWorldPoint(scp2 + o);
                Gizmos.DrawLine(origin, destiny);
            }
        }
    }
}
