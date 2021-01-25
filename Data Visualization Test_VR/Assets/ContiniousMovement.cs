using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

//code is from Valem
//moving around
public class ContiniousMovement : MonoBehaviour
{
    public XRNode inputSource;//input
    private Vector2 inputAxis;//axis of direction
    private CharacterController character;//character controller
    private XRRig rig;//VR rig

    public float additionalHeight = 0.2f;
    public float speed = 90;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        
    }
    //character should follow movement
    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);//what makes direction follow head movement
        Vector3 direction = headYaw*new Vector3(inputAxis.x, 0, inputAxis.y);

        character.Move(direction * Time.deltaTime*speed);
    }


  
}
