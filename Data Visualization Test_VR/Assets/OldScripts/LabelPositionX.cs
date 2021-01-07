using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class LabelPositionX : MonoBehaviour
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
    public float xScale;

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


        for (var i = 0; i < dataList1.Count; i++)//go through row for states
        {
            float x = i;//per region
            float xdef = xScale * x;
     
            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(xdef*plotScale, LabelHolder.transform.position.y, LabelHolder.transform.position.z),
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hiearchy
             dataPoint.transform.SetParent(LabelHolder.transform, true);

            // Assigns original values to dataPointName
            string dataPointName =
                " "+dataList1[i][geoArea];//region


            // Debug.Log(x + " " + y + " " + z);

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;
        }




    }


}
