using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
//Main script that controlers main features of the plot as well as plots the points

public class DataPlotter5D : MonoBehaviour
{

    // Name of the input file, no extension
    public string[] inputFiles;
    private ArrayList database = new ArrayList();//store the csv reads here
  

    // List for holding data from CSV reader
    private List<float> NO2 = new List<float>();//list of all the total cases fo each
    private List<float> SO2 = new List<float>();
    private List<float> PM10 = new List<float>();


    //column names
    private string geoArea;
    private string pm10Rate;
    private string no2Rate;
    private string so2Rate;


    //scales
    public float plotScale;//scale of graph
    public float sizeScale;//scale of points
    public float yScale;//first dimension scale
    public float zScale;//second dimension scale
    public float xScale;//third dimension scale

    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;

    //other
    private ArrayList columnList = new ArrayList();

    private List<string> columnList1;
    private List<string> columnList2;
    private List<string> columnList3;


    //y-labels
    public TMP_Text y_min;
    public TMP_Text y_mid;
    public TMP_Text y_max;


    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;

    // Use this for initialization
    void Start()
    {
        if (inputFiles.Length !=0)//read csv files and plot
        {
            //read files
            foreach(var element in inputFiles)//read all available datasets
            {
                database.Add(CSVReader.Read(element));
            }

            //plot
            PlotPoints(database);
       
        }
        else//error
        {
            Debug.Log("Error! The datasets have not been specified properly!");
        }
    }

    public void PlotPoints(ArrayList database)//organize the datasets into columns and the plot them based on appropriate dimension level
    {
       
        if (database.Count > 0)//collect first dimension
        {
            // Declare list of strings, fill with keys (column names)
            List<Dictionary<string, object>> dataList1 = (List<Dictionary<string, object>>)database[0];//pm10
            columnList1 = new List<string>(dataList1[1].Keys);
            pm10Rate = columnList1[1];//column for PM10
            //Debug.Log(database.Count);

            geoArea = columnList1[0];//column for states

            if (database.Count == 1)//plot if it only is 3D
            {
                Plot3D(dataList1);
            }
            else//if one more dimension, collect the second dataset
            {
                List<Dictionary<string, object>> dataList2 = (List<Dictionary<string, object>>)database[1];//so2
                columnList2 = new List<string>(dataList2[1].Keys);
                so2Rate = columnList2[1];//column for SO2

                if (database.Count == 2)//plot if it only is 4D
                {
                    Plot4D(dataList1, dataList2);
                }
                if (database.Count > 2) { 
                //if one more dimension, collect final dataset and then plot
                
                    List<Dictionary<string, object>> dataList3 = (List<Dictionary<string, object>>)database[2];//no2
                    columnList3 = new List<string>(dataList3[1].Keys);
                    no2Rate = columnList3[1];//column for NO2 

                    //plot
                    Plot5D(dataList1, dataList2, dataList3);
                }
                else
                {
                    Debug.Log("Error!");
                }
            }
        }
        else
        {
            Debug.Log("Error! No datasets detected!");
        }

    }

    public void Plot5D(List<Dictionary<string, object>> dataList1, List<Dictionary<string, object>> dataList2, List<Dictionary<string, object>> dataList3)//plot 5D data
    {
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {
            float z = j;//per date
            pm10Rate = columnList1[j];//column for date
            so2Rate = columnList2[j];
            no2Rate = columnList3[j];

            PM10 = ChangeDate(PM10, pm10Rate, dataList1);//input 1
            SO2 = ChangeDate(SO2, so2Rate, dataList2);//input 2
            NO2 = ChangeDate(NO2, no2Rate, dataList3);//input 3


            float zdef = zScale * z;//z scale
            GetYLabel(dataList1);//assign y labels

            //Loop through Pointlist
            for (var i = 0; i < dataList1.Count; i++)//go through row for states
            {
                float x = i;//per state
                float normalPM10 = Statistics.normalizeValue(getMin(pm10Rate, dataList1, columnList1), getMax(pm10Rate, dataList1, columnList1), PM10[i]);//make a list so you can normalize the whole thing
                float y = normalPM10;
                float ydef = yScale * y;//use third axis as well
                float xdef = x * xScale;//x scale

              
                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(xdef, ydef, zdef) * plotScale,
                        Quaternion.identity);

                ///Color
                 float normalSO2 = Statistics.normalizeValue(getMin(so2Rate, dataList2, columnList2), getMax(so2Rate, dataList2, columnList2), SO2[i]);//make a list so you can normalize the whole thing
                 Color blueColor = new Color();
                 ColorUtility.TryParseHtmlString("#2166AC", out blueColor);
                 Color redColor = new Color();
                 ColorUtility.TryParseHtmlString("#B2182B", out redColor);
                 Color whiteColor = new Color();
                 ColorUtility.TryParseHtmlString("#F7F7F7", out whiteColor);

                 dataPoint.GetComponent<Renderer>().material.color = Slerp3(blueColor, whiteColor, redColor, normalSO2);//HSB:(https://colorbrewer2.org/#type=diverging&scheme=RdBu&n=3)
                                                                                                                        //dataPoint.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);

                //Debug.Log(normalSO2);

                 //Size 
                 float normalNO2 = Statistics.normalizeValue(getMin(no2Rate, dataList3, columnList3), getMax(no2Rate, dataList3, columnList3), NO2[i]);//make a list so you can normalize the whole thing
                 //dataPoint.GetComponent<Renderer>().material.color = Lerp3(Color.blue, Color.white, Color.red, Mathf.PingPong(normalNO2, 1));

                 dataPoint.transform.localScale = new Vector3(normalNO2 * sizeScale, normalNO2 * sizeScale, normalNO2 * sizeScale);//size interpolation by SO2
                

                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    "City: " + dataList1[i][geoArea] + "\n" + //state
                    " Month: " + columnList1[j];   //date

                string dataNeeded = " NO2 Emission: " + dataList1[i][no2Rate] + "\n " +//NO2 cases
                    " SO2 Emission: " + SO2[i] + "\n" +        //SO2 rate
                    " PM10 Fuel Consumption: " + PM10[i];  //PM10 rate
                                                           //+ " Nomral NO2" + normalNO2;*/

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName + "\n" + dataNeeded;


            }
        }
    }

    public void Plot4D(List<Dictionary<string, object>> dataList1, List<Dictionary<string, object>> dataList2)//plot 4D data
    {
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {
            float z = j;//per date
            pm10Rate = columnList1[j];//column for date
            so2Rate = columnList2[j];
            

            PM10 = ChangeDate(PM10, pm10Rate, dataList1);//input 1
            SO2 = ChangeDate(SO2, so2Rate, dataList2);//input 2
            
            float zdef = zScale * z;//z scale
            GetYLabel(dataList1);//assign y labels

            //Loop through Pointlist
            for (var i = 0; i < dataList1.Count; i++)//go through row for states
            {
                float x = i;//per state
                float normalPM10 = Statistics.normalizeValue(getMin(pm10Rate, dataList1, columnList1), getMax(pm10Rate, dataList1, columnList1), PM10[i]);//make a list so you can normalize the whole thing
                float y = normalPM10;
                float ydef = yScale * y;//use third axis as well
                float xdef = x * xScale;//x scale


                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(xdef, ydef, zdef) * plotScale,
                        Quaternion.identity);

                ///Color
                float normalSO2 = Statistics.normalizeValue(getMin(so2Rate, dataList2, columnList2), getMax(so2Rate, dataList2, columnList2), SO2[i]);//make a list so you can normalize the whole thing
                Color blueColor = new Color();
                ColorUtility.TryParseHtmlString("#2166AC", out blueColor);
                Color redColor = new Color();
                ColorUtility.TryParseHtmlString("#B2182B", out redColor);
                Color whiteColor = new Color();
                ColorUtility.TryParseHtmlString("#F7F7F7", out whiteColor);

                dataPoint.GetComponent<Renderer>().material.color = Slerp3(blueColor, whiteColor, redColor, normalSO2);//HSB:(https://colorbrewer2.org/#type=diverging&scheme=RdBu&n=3)
                                                                                                                       //dataPoint.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);

                //Size 
                //dataPoint.transform.localScale = new Vector3(normalNO2 * sizeScale, normalNO2 * sizeScale, normalNO2 * sizeScale);//size interpolation by SO2


                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    "City: " + dataList1[i][geoArea] + "\n" + //state
                    " Month: " + columnList1[j];   //date

                string dataNeeded = 
                    " SO2 Emission: " + SO2[i] + "\n" +        //SO2 rate
                    " PM10 Fuel Consumption: " + PM10[i];  //PM10 rate
                                                           //+ " Nomral NO2" + normalNO2;*/

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName + "\n" + dataNeeded;


            }
        }
    }
    public void Plot3D(List<Dictionary<string, object>> dataList1)//plot 3D data
    {
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {
            float z = j;//per date
            pm10Rate = columnList1[j];//column for date
            PM10 = ChangeDate(PM10, pm10Rate, dataList1);//input 1
            

            float zdef = zScale * z;//z scale
            GetYLabel(dataList1);//assign y labels

            //Loop through Pointlist
            for (var i = 0; i < dataList1.Count; i++)//go through row for states
            {
                float x = i;//per state
                float normalPM10 = Statistics.normalizeValue(getMin(pm10Rate, dataList1, columnList1), getMax(pm10Rate, dataList1, columnList1), PM10[i]);//make a list so you can normalize the whole thing
                float y = normalPM10;
                float ydef = yScale * y;//use third axis as well
                float xdef = x * xScale;//x scale


                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(xdef, ydef, zdef) * plotScale,
                        Quaternion.identity);

                ///Color
                //dataPoint.GetComponent<Renderer>().material.color = Slerp3(blueColor, whiteColor, redColor, normalSO2);//HSB:(https://colorbrewer2.org/#type=diverging&scheme=RdBu&n=3)                                                                                                       
                //Size 
                //dataPoint.transform.localScale = new Vector3(normalNO2 * sizeScale, normalNO2 * sizeScale, normalNO2 * sizeScale);//size interpolation by SO2


                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    "City: " + dataList1[i][geoArea] + "\n" + //state
                    " Month: " + columnList1[j];   //date

                string dataNeeded =
                    " PM10 Fuel Consumption: " + PM10[i];  //PM10 rate
                                                           //+ " Nomral NO2" + normalNO2;*/

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName + "\n" + dataNeeded;


            }
        }
    }
    public float getMin(string rate, List<Dictionary<string, object>> dataList, List<string> columnList)//calculate minimum value
    {
        //min3 = Statistics.FindMinValue3(so2Rate, dataList2, columnList2);
 
        float min = Statistics.FindMinValue3(rate, dataList, columnList);
        return min;
    }

    public float getMax(string rate, List<Dictionary<string, object>> dataList, List<string> columnList)//calculate maximum value
    {
       
        //max4 = Statistics.FindMaxValue3(so2Rate, dataList2, columnList2);
      
        float max = Statistics.FindMaxValue3(rate, dataList, columnList);
        return max;
    }
    private void GetYLabel(List<Dictionary<string, object>> dataList1)
    {
        // Set y Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        y_min.text = getMin(pm10Rate, dataList1, columnList1).ToString("0.0");
        y_mid.text = (getMin(pm10Rate, dataList1, columnList1) + (getMax(pm10Rate, dataList1, columnList1) - getMin(pm10Rate, dataList1, columnList1)) / 2f).ToString("0.0");
        y_max.text = getMax(pm10Rate, dataList1, columnList1).ToString("0.0");

        //set position
        y_min.transform.position = new Vector3(y_min.transform.position.x, Statistics.normalizeValue(getMin(pm10Rate, dataList1, columnList1), getMax(pm10Rate, dataList1, columnList1), getMin(pm10Rate, dataList1, columnList1)) * yScale * plotScale, y_min.transform.position.z);
        y_max.transform.position = new Vector3(y_max.transform.position.x, Statistics.normalizeValue(getMin(pm10Rate, dataList1, columnList1), getMax(pm10Rate, dataList1, columnList1), getMax(pm10Rate, dataList1, columnList1)) * yScale * plotScale, y_max.transform.position.z);

        y_mid.transform.position = new Vector3(y_mid.transform.position.x, (y_min.transform.position.y + (y_max.transform.position.y - y_min.transform.position.y) / 2f), y_mid.transform.position.z);

    }


    static List<float> ChangeDate(List<float> Case, string valueRate, List<Dictionary<string, object>> dataList)
    {
        float[] tempValue = new float[dataList.Count];//temporary array, a placeholder for the values
        Case.Clear();

        for (var n = 0; n < dataList.Count; n++)
        {

            tempValue[n] = System.Convert.ToSingle(dataList[n][valueRate]);//add previous values
            Case.Add(tempValue[n]);

        }

        //Debug.Log(temporary.Count);
        return Case;
    }

    Color Slerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return (LABColor.Lerp(LABColor.FromColor(a), LABColor.FromColor(b), t / 0.5f)).ToColor();
        else // 0.5 to 1.0 goes to b -> c
            return (LABColor.Lerp(LABColor.FromColor(b), LABColor.FromColor(c), (t - 0.5f) / 0.5f)).ToColor();
    }

}

