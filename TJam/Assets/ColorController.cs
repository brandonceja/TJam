using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    // Start is called before the first frame update
    public int highDensity = 10;
    public int mediumDensity = 5;
    public int lowDensity = 2;

    public Material[] material;

    private Path[] children;

    void Start()
    {
        children = GetComponentsInChildren<Path>();  
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < children.Length; i++)
        {

            Renderer rend = children[i].GetComponent<Renderer>();
            rend.enabled = true;
            float density = 0; ;
            if (GameObject.Find("GameManager").GetComponent<DatasetManager>().carDensity.ContainsKey(children[i].pathName))
                density = GameObject.Find("GameManager").GetComponent<DatasetManager>().carDensity[children[i].pathName];

            Debug.Log("DENSITY: " + density);

            if (density > highDensity)
            {
                Debug.Log("high");
                rend.sharedMaterial = material[2];
            }
            else if (density > mediumDensity)
            {
                Debug.Log("medium");
                rend.sharedMaterial = material[1];
            }
            else
            {
                Debug.Log("low");
                rend.sharedMaterial = material[0];
            }
        }
    }
}
