using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionCont : MonoBehaviour
{
    public XRController teleRay;//teleportation Ray
    public InputHelpers.Button teleActivationButton;//button you need to press
    public float activeValue=0.1f;//value threshhold


    // Update is called once per frame
    void Update()
    {
        if (teleRay)
        {
            teleRay.gameObject.SetActive(CheckIfActivated(teleRay));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice,
            teleActivationButton, out bool isActive, activeValue);

        return isActive;
        
    }
}
