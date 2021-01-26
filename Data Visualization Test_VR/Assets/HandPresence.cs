using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using TMPro;
//assign controllers


//getting the controllers in so they can be used
public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public static InputDevice targetDevice;
    public List<GameObject> controllerPrefabs;
    private GameObject spawnedController;
 

   
    
    void Awake()
    {
        //fix controllers
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            //Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);//name must match in prefab with VR device
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
                spawnedController.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);//one has to be 20 the other 50
                //0.3 for size 10


                //Debug.Log("Found " + targetDevice.name + "  " + spawnedController.transform.localScale.magnitude);
            }
            else
            {
                Debug.LogError("Did not find corresponding controller model: " + targetDevice.name);
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////check
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);//press primary button B on right controller and get value
        

        if (primaryValue )
        {

            //Debug.Log("Press primary button!");
        }

     
    }

   
}
