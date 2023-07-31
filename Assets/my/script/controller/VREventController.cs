using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
public class VREventController : MonoBehaviour

{
    public GameObject Player;

    public float SpeedRatio;
    float movingSpeed;
    private RunTimeController r;
    public bool walk;
    private void Start()
    {
        r = transform.parent.GetComponentInChildren<RunTimeController>(); 
        movingSpeed = DataMemory.allParticle.GetXScale() / 2.5f;
    }
    void Update()
    {
        
        if (ViveInput.GetPressDown(HandRole.LeftHand,  ControllerButton.Grip))
        {
            if (!r.SimulationOn)
           r.SimulationOn=true;
            else
                r.SimulationOn=false;
        }
        if(walk)
        {
            float movey = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * movingSpeed * SpeedRatio;
            if (movey != 0f)
                Player.transform.Translate(Player.transform.Find("Camera").transform.forward * movey);

        }


    }
}
