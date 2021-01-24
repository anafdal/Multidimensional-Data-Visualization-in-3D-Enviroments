using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
//getting title/data of points

public class GetLabelName : MonoBehaviour
{
    public TMP_Text data;
    void Start()
    {
        data.text = transform.name;
    }

    
}
