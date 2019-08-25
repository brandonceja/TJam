using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class DatasetManager : MonoBehaviour
{
    public int maxCars = 100;
    public string webServiceURL = "http://10.22.165.87:5000/cars";
    public string webServiceURLAlternate = "http://10.22.165.87:5000/cars";
    public GameObject carPrefab;
    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject spawn3;
    public GameObject spawn4;
    public GameObject spawn5;
    public GameObject spawn11;
    public GameObject spawn12;
    private MainDataset dataset1;
    private Dictionary<string, GameObject> nodeQueueTransform;
    public Dictionary<string, int> carDensity;
    private Dictionary<int, int> spawnsCount;
    
    public void restartDataset()
    {
        try
        {
            foreach (var car in GameObject.FindGameObjectsWithTag("Car"))
            {
                Destroy(car);
            }
        }
        catch (Exception ex)
        {

        }

        nodeQueueTransform = new Dictionary<string, GameObject>();
        carDensity = new Dictionary<string, int>();
        spawnsCount = new Dictionary<int, int>();

        // A correct website page.
        StartCoroutine(GetRequest(webServiceURL));
    }

    

    public void restartDatasetCustom()
    {
        try
        {
            foreach (var car in GameObject.FindGameObjectsWithTag("Car"))
            {
                Destroy(car);
            }
        }
        catch(Exception ex)
        {

        }
        

        nodeQueueTransform = new Dictionary<string, GameObject>();
        carDensity = new Dictionary<string, int>();
        spawnsCount = new Dictionary<int, int>();

        // A correct website page.
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        string topRoad = "";
        int maxTraffic = 0;
        foreach (var roads in carDensity.Keys)
        {
            if (carDensity[roads] > maxTraffic)
            {
                maxTraffic = carDensity[roads];
                topRoad = roads;
            }
        }
        string myData = "{\"speed\":[" + topRoad + "],\"slow\":[]}";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(webServiceURLAlternate, myData))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = webServiceURLAlternate.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                //Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string receivedData = webRequest.downloadHandler.text;
                Debug.Log(pages[page] + ":\nReceived: " + receivedData);

                dataset1 = JsonUtility.FromJson<MainDataset>(receivedData);

                Debug.Log("Plates read: " + dataset1.data.Count);

                createObjectsFromDataset();
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        restartDataset();
    }

    void displayDataset(MainDataset datasetIn)
    {
        for (int i = 0; i < datasetIn.data.Count; i++)
        {
            PlateInfo plateInfo = datasetIn.data[i];
            Debug.Log("------------------------------------------------\n");
            Debug.Log("Ruta: ");
            foreach (int Ruta in plateInfo.Ruta)
            {
                Debug.Log(Ruta);
            }
            Debug.Log("\nTrips:");
            foreach (int trip in plateInfo.Viaje)
            {
                Debug.Log(trip);
            }
            Debug.Log("Start: " + plateInfo.inicio);
        }
    }

    void createObjectsFromDataset()
    {

        for (int i = 0; i < maxCars; i++)
        {
            PlateInfo plateInfo = dataset1.data[i];

            if(plateInfo.Ruta.Count >= 2)
            {
                GameObject tmpSpawnPoint;

                tmpSpawnPoint = new GameObject();
                Transform tmpTransform = tmpSpawnPoint.GetComponent<Transform>();

                Transform[] tmpTransforms = { spawn1.transform, spawn2.transform, spawn3.transform, spawn4.transform, spawn5.transform, spawn11.transform, spawn12.transform };
                foreach(var tmpVar1 in tmpTransforms)
                {
                    //Debug.Log("Getting transforms: " + tmpVar1.transform.position.x + " | " + tmpVar1.position.y + " | " + tmpVar1.position.z);
                }
                bool shouldIContinue = true;
                try
                {
                    switch (plateInfo.Ruta[0])
                    {
                        case 1:
                            tmpTransform.position = tmpTransforms[plateInfo.Ruta[0] - 1].position;
                            tmpTransform.rotation = tmpTransforms[plateInfo.Ruta[0] - 1].rotation;
                            break;
                        case 2:
                            tmpTransform.position = tmpTransforms[plateInfo.Ruta[0] - 1].position;
                            tmpTransform.rotation = tmpTransforms[plateInfo.Ruta[0] - 1].rotation;
                            break;
                        case 3:
                            tmpTransform.position = tmpTransforms[plateInfo.Ruta[0] - 1].position;
                            tmpTransform.rotation = tmpTransforms[plateInfo.Ruta[0] - 1].rotation;
                            break;
                        case 4:
                            tmpTransform.position = tmpTransforms[plateInfo.Ruta[0] - 1].position;
                            tmpTransform.rotation = tmpTransforms[plateInfo.Ruta[0] - 1].rotation;
                            break;
                        case 5:
                            tmpTransform.position = tmpTransforms[plateInfo.Ruta[0] - 1].position;
                            tmpTransform.rotation = tmpTransforms[plateInfo.Ruta[0] - 1].rotation;
                            break;
                        case 11:
                            tmpTransform.position = tmpTransforms[5].position;
                            tmpTransform.rotation = tmpTransforms[5].rotation;
                            break;
                        case 12:
                            tmpTransform.position = tmpTransforms[6].position;
                            tmpTransform.rotation = tmpTransforms[6].rotation;
                            break;
                        default:
                            shouldIContinue = false;
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Debug.Log("Error para ruta: " + plateInfo.Ruta[0]);
                }
                
                if(shouldIContinue)
                {
                    //Debug.Log("Detected<" + plateInfo.Ruta[0] + ">: " + tmpTransform.position.x + " | " + tmpTransform.position.y + " | " + tmpTransform.position.z);

                    if (!nodeQueueTransform.ContainsKey(plateInfo.Ruta[0].ToString()))
                    {
                        Debug.Log("New Plate <" + plateInfo.Ruta[0] + "> " + tmpSpawnPoint.transform.position.x + " | " + tmpSpawnPoint.transform.position.y + " | " + tmpSpawnPoint.transform.position.z);
                        nodeQueueTransform.Add(plateInfo.Ruta[0].ToString(), tmpSpawnPoint);
                        spawnsCount.Add(plateInfo.Ruta[0], 1);
                    }
                    else
                    {
                        float displacementForSpawn = 15.0f;
                        tmpTransform.position += new Vector3(0, 0, displacementForSpawn*spawnsCount[plateInfo.Ruta[0]]);
                        nodeQueueTransform[plateInfo.Ruta[0].ToString()] = tmpSpawnPoint;
                        spawnsCount[plateInfo.Ruta[0]]++;
                    }
                    //Debug.Log("Creating car <" + plateInfo.Ruta[0] + "> at: " + nodeQueueTransform[plateInfo.Ruta[0].ToString()].GetComponent<Transform>().position.x + " , " + nodeQueueTransform[plateInfo.Ruta[0].ToString()].GetComponent<Transform>().position.y + " , " + nodeQueueTransform[plateInfo.Ruta[0].ToString()].GetComponent<Transform>().position.z);

                    GameObject tmpObject = Instantiate(carPrefab, nodeQueueTransform[plateInfo.Ruta[0].ToString()].GetComponent<Transform>());

                    List<Path> tmpPathList = new List<Path>();
                    for (int j = 2; j < plateInfo.Ruta.Count; j++)
                    {
                        Debug.Log("Ruta: " + plateInfo.Ruta[j - 1] + " | " + plateInfo.Ruta[j] + " | Duration: " + plateInfo.Viaje[j-1]);
                        //Debug.Log("Finding: " + "Path " + plateInfo.Ruta[j - 1] + "-" + plateInfo.Ruta[j]);

                        string tmpDictionaryKey = plateInfo.Ruta[j - 1] + "-" + plateInfo.Ruta[j];
                        tmpPathList.Add(GameObject.Find("Path " + plateInfo.Ruta[j - 1] + "-" + plateInfo.Ruta[j]).GetComponent<Path>());
                        tmpPathList[tmpPathList.Count - 1].pathName = tmpDictionaryKey;

                        
                        if (carDensity.ContainsKey(tmpDictionaryKey))
                        {
                            carDensity[tmpDictionaryKey]++;
                        }
                        else
                        {
                            carDensity.Add(tmpDictionaryKey, 1);
                        }
                        
                    }
                    tmpObject.GetComponent<CarEngine>().paths = tmpPathList;
                    //Debug.Log("Start: " + plateInfo.start);
                }



            }
            else
            {
                //Debug.Log("No hay suficientes puntos de ruta <" + plateInfo.plate + ">");
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                //Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                string receivedData = webRequest.downloadHandler.text;
                Debug.Log(pages[page] + ":\nReceived: " + receivedData);

                dataset1 = JsonUtility.FromJson<MainDataset>(receivedData);

                Debug.Log("Plates read: " + dataset1.data.Count);

                createObjectsFromDataset();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class PlateInfo
{
    public List<int> Ruta;
    public List<int> Viaje;
    public string inicio;
}

[System.Serializable]
public class MainDataset
{
    public List<PlateInfo> data;
}