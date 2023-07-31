using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject operationImag;
    public GameObject script;
    public GameObject WelcomeText;
    public GameObject InTestText;
    public GameObject EndText;
    public GameObject technique_display;
    public GameObject   Questionnaire;
    public GameObject Bigpointcloud;
    public STATE state;
    public UserstudyController UC;
    void Start()
    {
        state = STATE.welcome;
        InTestText.SetActive(false);
        EndText.SetActive(false);
    }

    void Update()
    {
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.AKey) || ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.BKey) || Input.GetKeyDown(KeyCode.Space))  //next
        {
            if (state == STATE.welcome)
            {
                operationImag.SetActive(false);
                WelcomeText.SetActive(false);
                script.SetActive(true);                                      
                InTestText.SetActive(true);
                Bigpointcloud.SetActive(false);
                state = STATE.intest;
               
            }

            if(state == STATE.intest)
            {
                Invoke("PressNext", 2f);
           
            }

        }

        }
    public void PrintVoidSelection()
    {
        InTestText.GetComponent<Text>().text = "You need to select before Press <color=Blue><b>Next</b></color>\n";
    }

    void PressNext()
    {
        if (UC.isPre)
        {
            operationImag.SetActive(true);
            InTestText.GetComponent<Text>().text = UC.task.s.ToString() + " selection practice\n" + " Press <color=cyan><b>Next</b></color> to start." + "When we start, time and accuracy will be recorded. Please select the <color=yellow><b>Yellow</b></color> area as soon as possible.";
            technique_display.GetComponent<Text>().text = UC.task.s.ToString() + " Practice";
        }
        else
        {
            
                operationImag.SetActive(false);
                InTestText.GetComponent<Text>().text =/* "Selection Technique: " + UC.task.s.ToString() + "\n" + "Dataset: " + UC.task.d + "\n" +*/ "Please select the yellow area. Press <color=cyan><b>Next</b></color> if you finish.\n";
            technique_display.GetComponent<Text>().text = UC.task.s.ToString();

        }
    }
}



public enum STATE {welcome,intest}
