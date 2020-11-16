using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DataPlotter4D : MonoBehaviour
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
    private List<float> NO2 = new List<float>();//list of all the total cases
    private List<float> SO2 = new List<float>();//list of all the total cases
    private List<float> PM10 = new List<float>();


    //column names
    private string geoArea;
    private string no2Rate;
    private string so2Rate;
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
    private List<string> columnList2;
    private List<string> columnList3;

    private float min1;
    private float max2;

    private float min3;
    private float max4;

    private float min5;
    private float max6;

    //y-labels
    public TMP_Text y_min;
    public TMP_Text y_mid;
    public TMP_Text y_max;


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

        no2Rate = columnList1[1];//column for NO2
        so2Rate = columnList2[1];//column for SO2
        pm10Rate = columnList3[1];//column for PM10 consumption

        //tempValue = new float[dataList1.Count];//temporary array

        min1 = Statistics.FindMinValue3(no2Rate, dataList1, columnList1);
        max2 = Statistics.FindMaxValue3(no2Rate, dataList1, columnList1);

        min3 = Statistics.FindMinValue3(so2Rate, dataList2, columnList2);
        max4 = Statistics.FindMaxValue3(so2Rate, dataList2, columnList2);

        min5 = Statistics.FindMinValue3(pm10Rate, dataList3, columnList3);
        max6 = Statistics.FindMaxValue3(pm10Rate, dataList3, columnList3);
    
        for (var j = 1; j < columnList1.Count; j++)//through columns for dates
        {
            float z = j;//per date
            no2Rate = columnList1[j];//column for date
            so2Rate = columnList2[j];
            pm10Rate = columnList3[j];


            NO2 = ChangeDate(NO2, no2Rate, dataList1);
            SO2 = ChangeDate(SO2, so2Rate, dataList2);
            PM10 = ChangeDate(PM10, pm10Rate, dataList3);

            float zdef = zScale * z;

            GetYLabel();//assign y labels

            //Loop through Pointlist
            for (var i = 0; i < dataList1.Count; i++)//go through row for states
            {
                float x = i;//per state

                float normalNO2 = Statistics.normalizeValue(min1, max2, NO2[i]);//make a list so you can normalize the whole thing
                float normalSO2 = Statistics.normalizeValue(min3, max4, SO2[i]);//make a list so you can normalize the whole thing
                float normalPM10 = Statistics.normalizeValue(min5, max6, PM10[i]);//make a list so you can normalize the whole thing


                float y = normalPM10;
                float ydef = yScale * y;//use third axis as well
                float xdef = x * xScale;

                //float ydef = (float)0.01 * y;



                // Instantiate as gameobject variable so that it can be manipulated within loop
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(xdef, ydef, zdef) * plotScale,
                        Quaternion.identity);

                ///Color

                Color blueColor = new Color();
                ColorUtility.TryParseHtmlString("#2166AC", out blueColor);
                Color redColor = new Color();
                ColorUtility.TryParseHtmlString("#B2182B", out redColor);
                Color whiteColor = new Color();
                ColorUtility.TryParseHtmlString("#F7F7F7", out whiteColor);

                dataPoint.GetComponent<Renderer>().material.color = Slerp3(blueColor, whiteColor, redColor,normalSO2);//HSB:(https://colorbrewer2.org/#type=diverging&scheme=RdBu&n=3)
                dataPoint.transform.localScale = new Vector3(sizeScale, sizeScale, sizeScale);



                //dataPoint.GetComponent<Renderer>().material.color = Lerp3(Color.blue, Color.white, Color.red, Mathf.PingPong(normalNO2, 1));

                //dataPoint.transform.localScale = new Vector3(normalNO2 * sizeScale, normalNO2 * sizeScale, normalNO2 * sizeScale);//size interpolation by SO2

                //new Vector3(normalVal*100, y, z) * plotScale

                // Make child of PointHolder object, to keep points within container in hiearchy
                dataPoint.transform.parent = PointHolder.transform;

                // Assigns original values to dataPointName
                string dataPointName =
                    "City: " + dataList1[i][geoArea] + "\n"+ //state
                    " Month: " + columnList1[j];   //date

                string dataNeeded = " NO2 Emission: " + dataList1[i][no2Rate]+ "\n " +//NO2 cases
                    " SO2 Emission: " + SO2[i] + "\n" +        //SO2 rate
                    " PM10 Fuel Consumption: " + PM10[i];  //PM10 rate
                    //+ " Nomral NO2" + normalNO2;*/

                // Assigns name to the prefab
                dataPoint.transform.name = dataPointName+ "\n"+dataNeeded;



                // Gets material color and sets it to a new RGB color we define

            }
        }     
    }

    private void GetYLabel()
    {
        // Set y Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        y_min.text = min5.ToString("0.0");
        y_mid.text= (min5 + (max6 - min5) / 2f).ToString("0.0");
        y_max.text = max6.ToString("0.0");

        //set position
        y_min.transform.position= new Vector3(y_min.transform.position.x, Statistics.normalizeValue(min5, max6, min5)*yScale*plotScale, y_min.transform.position.z);
        y_max.transform.position = new Vector3(y_max.transform.position.x, Statistics.normalizeValue(min5, max6, max6) * yScale * plotScale, y_max.transform.position.z);

        y_mid.transform.position = new Vector3(y_mid.transform.position.x,(y_min.transform.position.y + (y_max.transform.position.y - y_min.transform.position.y) / 2f), y_mid.transform.position.z);
        
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

    /*
    Color Slerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return (HSBColor.Lerp(HSBColor.FromColor(a), HSBColor.FromColor(b), t/0.5f)).ToColor();
        else // 0.5 to 1.0 goes to b -> c
            return (HSBColor.Lerp(HSBColor.FromColor(b), HSBColor.FromColor(c), (t - 0.5f) / 0.5f)).ToColor();
    }*/

   Color Slerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return (LABColor.Lerp(LABColor.FromColor(a), LABColor.FromColor(b), t / 0.5f)).ToColor();
        else // 0.5 to 1.0 goes to b -> c
            return (LABColor.Lerp(LABColor.FromColor(b), LABColor.FromColor(c), (t - 0.5f) / 0.5f)).ToColor();
    }

   
  


}
