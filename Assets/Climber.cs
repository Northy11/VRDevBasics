using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class Climber : MonoBehaviour
{
    public CharacterController character;
    public static XRController climbingHand;
    private ContinousMovement continousMovement;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        continousMovement = GetComponent<ContinousMovement>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (climbingHand)
        {
            continousMovement.enabled = false;
            Climb();
        }
        else
        {
            continousMovement.enabled = true;

        }
        
    }
    void Climb()
    {
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        character.Move(transform.rotation*(-velocity) * Time.fixedDeltaTime); 

    }
}

