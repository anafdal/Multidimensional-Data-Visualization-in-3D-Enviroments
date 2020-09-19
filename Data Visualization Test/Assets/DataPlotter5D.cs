using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataPlotter5D : MonoBehaviour
{

    // Name of the input file, no extension
    public string inputfile1;
    public string inputfile2;
    public string inputfile3;

    // List for holding data from CSV reader
    private List<Dictionary<string, object>> dataList1;
    private List<Dictionary<string, object>> dataList2;
    private List<Dictionary<string, object>> dataList3;

    //list
    private List<float> Methane = new List<float>();//list of all the total cases
    private List<float> Carbon = new List<float>();//list of all the total cases
    private List<float> Fossil = new List<float>();


    //column names
    private string geoArea;
    private string methaneRate;
    private string carbonRate;
    private string fossilRate;


    //scales
    public float plotScale;
    public float sizeScale;
    public float yScale;

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;

    //other
    private List<string> columnList1;
    private List<string> columnList2;
    private List<string> columnList3;

    private float min1;
    private float max2;

    private float min3;
    private float max4;

    private float min5;
    private float max6;


    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;

    // Use this for initialization
    void OnEnable()
    {
        dataList1 = CSVReader.Read(inputfile1);
        dataList2 = CSVReader.Read(inputfile2);
        dataList3 = CSVReader.Read(inputfile3);


        // Declare list of strings, fill with keys (column names)
       columnList1 = new List<string>(dataList1[1].Keys);
       columnList2 = new List<string>(dataList2[1].Keys);
       columnList3 = new List<string>(dataList3[1].Keys);

        geoArea = columnList1[0];//column for states

        methaneRate = columnList1[1];//column for methane
        carbonRate = columnList2[1];//column for carbon
        fossilRate = columnList3[1];//column for fossil consumption

        //tempValue = new float[dataList1.Count];//temporary array

        min1 = Statistics.FindMinValue3(methaneRate, dataList1, columnList1);
        max2 = Statistics.FindMaxValue3(methaneRate, dataList1, columnList1);

        min3 = Statistics.FindMinValue3(carbonRate, dataList2, columnList2);
        max4 = Statistics.FindMaxValue3(carbonRate, dataList2, columnList2);

        min5 = Statistics.FindMinValue3(fossilRate, dataList3, columnList3);
        max6 = Statistics.FindMaxValue3(fossilRate, dataList3, columnList3);
    
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {
            float z = j;//per date
            methaneRate = columnList1[j];//column for date
            carbonRate = columnList2[j];
            fossilRate = columnList3[j];


            Methane = ChangeDate(Methane, methaneRate, dataList1);
            Carbon = ChangeDate(Carbon, carbonRate, dataList2);
            Fossil = ChangeDate(Fossil, fossilRate, dataList3);

            //Loop through Pointlist
            for (var i = 0; i < dataList1.Count; i++)//go through row for states
            {
                float x = i;//per state

                float normalMethane = Statistics.normalizeValue(min1, max2, Methane[i]);//make a list so you can normalize the whole thing
                float normalCarbon = Statistics.normalizeValue(min3, max4, Carbon[i]);//make a list so you can normalize the whole thing
                float normalFossil = Statistics.normalizeValue(min5, max6, Fossil[i]);//make a list so you can normalize the whole thing


                float y = normalFossil;
                float ydef = yScale * y;//use third axis as well

                //float ydef = (float)0.01 * y;



                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(x, ydef, z) * plotScale,
                        Quaternion.identity);


                dataPoint.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.red, Mathf.PingPong(normalCarbon, 1));//color interpolation represented by carbon


                dataPoint.transform.localScale = new Vector3(normalMethane * sizeScale, normalMethane * sizeScale, normalMethane * sizeScale);//size interpolation by methane

                //new Vector3(normalVal*100, y, z) * plotScale

                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    dataList1[i][geoArea] + " "          //state
                    + columnList1[j] + " "               //date
                    + dataList1[i][methaneRate];         //cases

                // Debug.Log(x + " " + y + " " + z);

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName;

                // Gets material color and sets it to a new RGB color we define

            }
        }     
    }

   
    static List<float> ChangeDate(List<float> Case, string valueRate, List<Dictionary<string, object>> dataList)
    {
        float [] tempValue = new float[dataList.Count];//temporary array, a placeholder for the values
        Case.Clear();

        for (var n = 0; n < dataList.Count; n++)
        {

            tempValue[n] = System.Convert.ToSingle(dataList[n][valueRate]);//add previous values
            Case.Add(tempValue[n]);

        }

        //Debug.Log(temporary.Count);
        return Case;
    }

}
