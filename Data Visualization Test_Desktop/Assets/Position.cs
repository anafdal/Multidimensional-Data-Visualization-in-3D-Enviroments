using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
//rearrange grid for the Z and X axes


public class Position : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile1;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> dataList1;

    //scales
    private float plotScale;
    private float yScale;
    private float zScale;
    private float xScale;
    public float sizeScale = 7.0f;//7 for GetZ and 2 for GetX

    //other
    private List<string> columnList1;
    //column names
    private string geoArea;
    // List for holding data from CSV reader
    private List<float> PM10 = new List<float>();
    //column names
    private string pm10Rate;
    

    // Object which will contain instantiated prefabs in hiearchy
    public GameObject grid;

    //parent of all the grid lines
    public GameObject GridHolder;

    // Use this for initialization
    void Start()
    {
        //get the correct sizes from th DataPlotter5D script
        DataPlotter5D scale = FindObjectOfType<DataPlotter5D>();
        plotScale = scale.plotScale;
        zScale = scale.zScale;
        xScale = scale.xScale;
        yScale = scale.yScale;

        //read file
        dataList1 = CSVReader.Read(inputfile1);

        // Declare list of strings, fill with keys (column names)
        columnList1 = new List<string>(dataList1[1].Keys);
        pm10Rate = columnList1[1];//column for PM10
        geoArea = columnList1[0];//column for state


        if (grid.transform.CompareTag("X"))//check orientation of label
        {
            GetX();
        }
        else if (grid.transform.CompareTag("Z"))
        {
            GetZ();
        }
        else
        {
            Debug.Log("Error!");
        }

    }

    public void GetX()//grid position for state/X axis
    {
        
        for (var i = 0; i < dataList1.Count; i++)//go through row for states
        {
            float x = i;//per region
            float xdef = x * xScale * plotScale;

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    grid,
                    new Vector3(xdef, grid.transform.position.y, grid.transform.position.z),
                    grid.transform.rotation);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(GridHolder.transform, true);

            //change size
            dataPoint.transform.localScale = new Vector3(dataPoint.transform.localScale.x, dataPoint.transform.localScale.y * sizeScale, dataPoint.transform.localScale.z);

            // Assigns original values to dataPointName
            string dataPointName =
                " " + dataList1[i][geoArea];//region

            dataPoint.transform.name = dataPointName;
        }

        
    }

    public void GetZ()//label position for date/Z axis
    {
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {

            float z = j;//per date
            float zdef = zScale * z;

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    grid,
                    new Vector3(grid.transform.position.x, grid.transform.position.y, zdef * plotScale),
                    grid.transform.rotation);

            dataPoint.transform.localScale = new Vector3(dataPoint.transform.localScale.x, dataPoint.transform.localScale.y*sizeScale, dataPoint.transform.localScale.z);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(GridHolder.transform, true);

            // Assigns original values to dataPointName
            string dataPointName =
                " Month: " + columnList1[j];    //date


            // Debug.Log(x + " " + y + " " + z);

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;
        }
    }

 
    
}
