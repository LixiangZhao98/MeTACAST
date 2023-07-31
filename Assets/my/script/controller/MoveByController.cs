
using UnityEngine;
using System.Collections;
using HTC.UnityPlugin.Vive;

public class MoveByController : MonoBehaviour {
    public GameObject Player;

    public float SpeedRatio;
    float movingSpeed;


    private void Start()
    {
        movingSpeed = DataMemory.allParticle.GetXScale()/2.5f;
    }
    private void Update()
    {
        float movey = ViveInput.GetPadTouchAxis(HandRole.RightHand).y * Time.deltaTime * movingSpeed*SpeedRatio;
        Player.transform.Translate(Player.transform.Find("Camera").transform.forward * movey);
        movey = ViveInput.GetPadTouchAxis(HandRole.LeftHand).y * Time.deltaTime * movingSpeed * SpeedRatio;
        Player.transform.Translate(Player.transform.Find("Camera").transform.forward * movey);
       
       }
    }
