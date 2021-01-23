using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
//Provides a colorscale and size scale for reference

public class RefScale : MonoBehaviour
{
    
    //array of normalized values 
    private int[] test = { 0,1,2,3,4,5,6,7,8,9,10};

    
    // The reference for the data points that will be instantiated
    public GameObject Ref1;//color
    public GameObject Ref2;//size

    //scale
    private float sizeScale;
    //private float plotScale;

    //parent
    public GameObject RefHolder;

    void Start()
    {
        //get the correct sizes from th DataPlotter5D script
        DataPlotter5D scale = FindObjectOfType<DataPlotter5D>();
        sizeScale = scale.sizeScale;
        
        

            ColorScale();
            SizeScale();
        
    }

    public void ColorScale()
    {

        for (var i = 0; i < test.Length; i++)//go through row for states
        {
            float color = (float)i/10;//per color
            float ydef = i + 0.01f;//per y position

            //Debug.Log(color);

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    Ref1,
                    new Vector3(Ref1.transform.position.x, Ref1.transform.position.y*ydef, Ref1.transform.position.z),
                    Quaternion.identity);

            ///Color
            Color blueColor = new Color();
            ColorUtility.TryParseHtmlString("#2166AC", out blueColor);
            Color redColor = new Color();
            ColorUtility.TryParseHtmlString("#B2182B", out redColor);
            Color whiteColor = new Color();
            ColorUtility.TryParseHtmlString("#F7F7F7", out whiteColor);

            dataPoint.GetComponent<Renderer>().material.color = Slerp3(blueColor, whiteColor, redColor, color);//HSB:(https://colorbrewer2.org/#type=diverging&scheme=RdBu&n=3)
                                                                                                                   

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(RefHolder.transform, true);


            // Assigns original values to dataPointName
            string dataPointName =
                " " + i;//name

            dataPoint.transform.name = dataPointName;
        }
    }
    public void SizeScale()
    {

        for (var i = 0; i < test.Length; i++)//go through row for states
        {
            float size = (float)i / 10;//per color
            float ydef = i + 0.01f;//per y position

            

            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    Ref2,
                    new Vector3(Ref2.transform.position.x, Ref2.transform.position.y * ydef, Ref2.transform.position.z),
                    Quaternion.identity);

            //Size 
            dataPoint.transform.localScale = new Vector3(size*sizeScale, size*sizeScale, size*sizeScale);//size interpolation by size

            // Make child of PointHolder object, to keep points within container in hiearchy
            dataPoint.transform.SetParent(RefHolder.transform, true);


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

