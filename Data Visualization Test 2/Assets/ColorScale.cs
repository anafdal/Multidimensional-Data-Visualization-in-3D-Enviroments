using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScale : MonoBehaviour
{
    // Start is called before the first frame update
    public Color lerpedColor = Color.white;
    //[SerializeField] [Range(0.0f, 1.0f)] float lerpTime;

    //[SerializeField] Color myColor;

    float t = 0f;

    private void Start()
    {
        transform.GetComponent<Renderer>().material.color =Color.blue;
    }

    void Update()
    {
        /* if (transform.GetComponent<Renderer>().material.color == Color.red)
         {
             transform.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.white, Mathf.PingPong(Time.time, 1));//color interpolation represented by NO2
         }
         else if (transform.GetComponent<Renderer>().material.color == Color.white)
         {
             transform.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));//color interpolation represented by NO2
         }*/

        t = Mathf.Lerp(t, 1, Mathf.PingPong(Time.time, 1));

        if (t<=0.5f)
         {
             t = 0f;
             transform.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.white, Mathf.PingPong(Time.time, 1));//color interpolation represented by NO2
         }
        else 
        {
             t = 0.0f;
             transform.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));//color interpolation represented by NO2
        }
        print(t);
    }
}

