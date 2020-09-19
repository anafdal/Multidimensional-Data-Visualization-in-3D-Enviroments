using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changeables : MonoBehaviour
{
    public float plot_Scale = 200;
    public float size_Scale = 100;
    public float y_Scale = 10;

    private DataPlotter5D otherScript;

    void Awake()
    {
        otherScript = GetComponent<DataPlotter5D>();
        //otherScript.
        otherScript.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        otherScript.sizeScale = size_Scale;
        otherScript.yScale = y_Scale;
        otherScript.plotScale = plot_Scale;

        if (Input.GetKeyDown(KeyCode.A)) // Just as an example
        {
            otherScript.enabled = true;
        }
    }
}
