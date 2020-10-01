using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GetName : MonoBehaviour
{
    public TMP_Text data;
    void Start()
    {
        data.text = transform.parent.name;
    }

    
}
