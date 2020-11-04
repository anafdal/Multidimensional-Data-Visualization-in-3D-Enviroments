using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class LabelPositionZ: MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile1;


    // List for holding data from CSV reader
    private List<Dictionary<string, object>> dataList1;
    

    //list
    private List<float> PM10 = new List<float>();


    //column names
    private string geoArea;
    private string pm10Rate;


    //scales
    public float plotScale;
    public float sizeScale;
    public float yScale;
    public float zScale;

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;

    //other
    private List<string> columnList1;
    

    // Object which will contain instantiated prefabs in hiearchy
    public GameObject LabelHolder;




    // Use this for initialization
    void Start()
    {
        dataList1 = CSVReader.Read(inputfile1);
       
        // Declare list of strings, fill with keys (column names)
        columnList1 = new List<string>(dataList1[1].Keys);
        

        geoArea = columnList1[0];//column for state
        pm10Rate = columnList1[1];//column for PM10 consumption

        //tempValue = new float[dataList1.Count];//temporary array


        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {

            float z = j;//per date
            float zdef = zScale * z;

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(LabelHolder.transform.position.x, LabelHolder.transform.position.y, zdef*plotScale),
                    Quaternion.identity);


            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(LabelHolder.transform, true);

            // Assigns original values to dataPointName
            string dataPointName =
                " Month: " + columnList1[j];    //date
                

            // Debug.Log(x + " " + y + " " + z);

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;
        }

  


    }


}
