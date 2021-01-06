using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
//script used to rearrange and position the lables for the Z and X axes

public class LabelPosition : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile1;
 
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> dataList1;


    //column names
    private string geoArea;
  

    //scales
    private float plotScale;
    //public float yScale;
    private float zScale;
    private float xScale;

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;

    //other
    private List<string> columnList1;


    // Object which will contain instantiated prefabs in hiearchy
    public GameObject LabelHolder;



    // Use this for initialization
    void Start()
    {

        //get the correct sizes from th DataPlotter5D script
        DataPlotter5D scale = FindObjectOfType<DataPlotter5D>();
        plotScale = scale.plotScale;
        zScale = scale.zScale;
        xScale = scale.xScale;

        //Debug.Log("test" + plotScale + " " + zScale + " " + xScale);

        //read a dataset
        dataList1 = CSVReader.Read(inputfile1);

        // Declare list of strings, fill with keys (column names)
        columnList1 = new List<string>(dataList1[1].Keys);
        geoArea = columnList1[0];//column for state

        if (LabelHolder.transform.CompareTag("X"))//check orientation of label
        {
            LabelX();
        }
        else if (LabelHolder.transform.CompareTag("Z"))
        {
            LabelZ();
        }
        else
        {
            Debug.Log("Error!");
        }

    }

    public void LabelX()//label position for state/X axis
    {
        
        for (var i = 0; i < dataList1.Count; i++)//go through row for states
        {
            float x = i;//per region
            float xdef = x * xScale;

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(xdef * plotScale, LabelHolder.transform.position.y, LabelHolder.transform.position.z),
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(LabelHolder.transform, true);


            // Assigns original values to dataPointName
            string dataPointName =
                " " + dataList1[i][geoArea];//region

            dataPoint.transform.name = dataPointName;
        }
    }

    public void LabelZ()//label position for date/Z axis
    {
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {

            float z = j;//per date
            float zdef = zScale * z;

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(LabelHolder.transform.position.x, LabelHolder.transform.position.y, zdef * plotScale),
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

