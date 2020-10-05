using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUp : MonoBehaviour
{
    
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;
    public GameObject stage5;
    public GameObject stage6;

    void Start()
    {
        stage1.SetActive(true);

        stage2.SetActive(false);
        stage3.SetActive(false);
        stage4.SetActive(false);
        stage5.SetActive(false);
        stage6.SetActive(false);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stage1.SetActive(true);

            stage2.SetActive(false);
            stage3.SetActive(false);
            stage4.SetActive(false);
            stage5.SetActive(false);
            stage6.SetActive(false);
          
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stage1.SetActive(false);

            stage2.SetActive(true);

            stage3.SetActive(false);
            stage4.SetActive(false);
            stage5.SetActive(false);
            stage6.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            stage1.SetActive(false);
            stage2.SetActive(false);

            stage3.SetActive(true);

            stage4.SetActive(false);
            stage5.SetActive(false);
            stage6.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            stage1.SetActive(false);
            stage2.SetActive(false);
            stage3.SetActive(false);

            stage4.SetActive(true);

            stage5.SetActive(false);
            stage6.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            stage1.SetActive(false);
            stage2.SetActive(false);
            stage3.SetActive(false);
            stage4.SetActive(false);

            stage5.SetActive(true);

            stage6.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            stage1.SetActive(false);
            stage2.SetActive(false);
            stage3.SetActive(false);
            stage4.SetActive(false);
            stage5.SetActive(false);

            stage6.SetActive(true);

        }


    }

}
