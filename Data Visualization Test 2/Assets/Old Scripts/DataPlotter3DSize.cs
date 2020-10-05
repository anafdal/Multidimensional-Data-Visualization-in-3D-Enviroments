using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlotter3DSize : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile1;


    // List for holding data from CSV reader
    private List<Dictionary<string, object>> dataList1;


    //list
    private List<int> Case = new List<int>();//list of all the total cases



    //column names
    private string geoArea;
    private string caseRate;


    private int[] tempValue;

    public float plotScale = 20;

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;




    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;

    // Use this for initialization
    void Start()
    {


        dataList1 = CSVReader.Read(inputfile1);


        // Declare list of strings, fill with keys (column names)
        List<string> columnList1 = new List<string>(dataList1[1].Keys);


        geoArea = columnList1[0];//column for states
        caseRate = columnList1[1];//column for date


        tempValue = new int[dataList1.Count];//temporary array

        int test1 = Statistics.FindMinValue2(caseRate, dataList1, columnList1);
        int test2 = Statistics.FindMaxValue2(caseRate, dataList1, columnList1);


        //Debug.Log(test3);
        //Debug.Log(test4);


        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {
            float z = 0;//per date
            caseRate = columnList1[j];//column for date



            Case = ChangeDate(caseRate);


            //Loop through Pointlist
            for (var i = 0; i < dataList1.Count; i++)//go through row for states
            {
                float x = i;//per state

                float normalVal = Statistics.normalizeValue(test1, test2, Case[i]);//make a list so you can normalize the whole thing


                float y = j;

                //float ydef = (float)0.01 * y;
                //Debug.Log("Case"+Case[i]);




                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(x, y, z) * plotScale,
                        Quaternion.identity);


                //dataPoint.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(normalVal, 1));
          

                dataPoint.transform.localScale = new Vector3(normalVal * 100, normalVal * 100, normalVal * 100);//size

                //new Vector3(normalVal*100, y, z) * plotScale

                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    dataList1[i][geoArea] + " "          //state
                    + columnList1[j] + " "               //date
                    + dataList1[i][caseRate];            //cases

                // Debug.Log(x + " " + y + " " + z);

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName;

                // Gets material color and sets it to a new RGB color we define

            }

        }

    }

    public List<int> ChangeDate(string caseRate)
    {
        Case.Clear();

        for (var n = 0; n < dataList1.Count; n++)
        {

            tempValue[n] = System.Convert.ToInt32(dataList1[n][caseRate]);//add previous values
            Case.Add(tempValue[n]);

        }

        //Debug.Log(temporary.Count);
        return Case;
    }




}
