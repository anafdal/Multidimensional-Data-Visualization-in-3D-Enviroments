using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Provides a colorscale and size scale for reference

public class RefScale : MonoBehaviour
{
    
    //array of normalized values 
    private float[] myNum = { 0, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };

    //scales
    //public float yScale;
    private float zScale;
    private float xScale;

    // The reference for the data points that will be instantiated
    public GameObject Ref1;
    public GameObject Ref2;

    void Start()
    {
        
            ColorScale(Ref1);
        

    }
    public void ColorScale(GameObject Ref1)//label position for state/X axis
    {

        for (var i = 0; i < myNum.Length; i++)//go through row for states
        {
            float x = i;//per color
           

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    Ref1,
                    new Vector3(Ref1.transform.position.x, Ref1.transform.position.y+(i*100), Ref1.transform.position.z),
                    Quaternion.identity);

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(Ref1.transform, true);


            // Assigns original values to dataPointName
            string dataPointName =
                " " + i;//name

            dataPoint.transform.name = dataPointName;
        }
    }
    Color Slerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return (LABColor.Lerp(LABColor.FromColor(a), LABColor.FromColor(b), t / 0.5f)).ToColor();
        else // 0.5 to 1.0 goes to b -> c
            return (LABColor.Lerp(LABColor.FromColor(b), LABColor.FromColor(c), (t - 0.5f) / 0.5f)).ToColor();
    }
}

