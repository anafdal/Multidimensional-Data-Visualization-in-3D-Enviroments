using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;
    public List<GameObject> controllerPrefabs;
    private GameObject spawnedController;
 

    // Update is called once per frame
    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        //InputDevices.GetDevices(devices);// containes everything we use
        //InputDeviceCharacteristics rightController = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);//name must match in prefab with VR device
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
                spawnedController.transform.localScale = new Vector3(1f, 1f, 1f);//one has to be 20 the other 50
                //0.3 for size 10


                //Debug.Log("Found " + targetDevice.name + "  " + spawnedController.transform.localScale.magnitude);
            }
            else
            {
                Debug.LogError("Did not find corresponding controller model: " + targetDevice.name);
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);//press primary button B on right controller and get value

        if (primaryValue)
        {
            Debug.Log("Pressing Primary Button");
        }
    }
}
