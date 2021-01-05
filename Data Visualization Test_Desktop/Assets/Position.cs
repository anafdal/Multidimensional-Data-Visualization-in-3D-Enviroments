using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class Position : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile1;



    // List for holding data from CSV reader
    private List<Dictionary<string, object>> dataList1;

    //scales
    public float plotScale;
    //public float yScale;
    public float zScale;
    public float xScale;

    // The prefab for the data points that will be instantiated
    //public GameObject PointPrefab;

    //other
    private List<string> columnList1;


    // Object which will contain instantiated prefabs in hiearchy
    //public GameObject LabelHolder;
    public GameObject test;


    // Use this for initialization
    void Start()
    {
        dataList1 = CSVReader.Read(inputfile1);

        // Declare list of strings, fill with keys (column names)
        columnList1 = new List<string>(dataList1[1].Keys);
        

        GetX();

    }

    public void GetX()//label position for state/X axis
    {
        ArrayList totalX = new ArrayList();

        for (var i = 0; i < dataList1.Count; i++)//go through row for states
        {
            float x = i;//per region
            float xdef = x * xScale * plotScale;

            totalX.Add(xdef);
            Debug.Log(xdef);
        }

        float distance = (float)(Convert.ToDouble(totalX[dataList1.Count-1]) - Convert.ToDouble(totalX[0]));
        newScale(test, distance);
    }

    public void GetZ()//label position for date/Z axis
    {
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {

            float z = j;//per date
            float zdef = zScale * z*plotScale;

           
        }
    }

    public void newScale(GameObject theGameObject, float newSize)
    {

        float size = theGameObject.GetComponent<Renderer>().bounds.size.x;

        Vector3 rescale = theGameObject.transform.localScale;

        rescale.x = newSize * rescale.x / size;

        theGameObject.transform.localScale = rescale;

    }
}
