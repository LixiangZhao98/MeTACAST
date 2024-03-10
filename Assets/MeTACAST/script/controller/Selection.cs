//
//  Selection.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//

using HTC.UnityPlugin.Vive;
using System.Collections.Generic;
using UnityEngine;
using static EnumVariables;



public class Selection : MonoBehaviour
{
    [SerializeField, SetProperty("SELECTIONTECH")]
    private SelectionTech selectionTech = SelectionTech.Point;
    public SelectionTech SELECTIONTECH
    {
        get { return selectionTech; }
        set
        {
            selectionTech = value;
            if (Application.isPlaying)
                Init(selectionTech);

        }
    }

   
    public Transform marker;
    public Transform map;
    public ControllerDraw controllerDraw;


    MarchingCubeGPU McGPU;

    //-----linebased
    float densityThreInitial = 0f;
    float threshold_linear;
    public float thremovespeed = 2f;
    //-----baseline
    float R = 0f;
    public float radiusmovespeed = 1f;

    float eraseNum = 0f;


    public void SwitchSelectionTech()
    {

        Init(selectionTech);

    }
    public void Init(SelectionTech s)
    {

        McGPU = this.transform.parent.GetComponentInChildren<MarchingCubeGPU>();
        
        threshold_linear = 0f;
        selectionTech = s;
        float ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * RenderDataRunTime.visRealSize;
        marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); R = 0.012f / ratio;
        switch (selectionTech)
        {
            case SelectionTech.Point:

                controllerDraw.enabled = false;
                
                break;

            case SelectionTech.Brush:
                Linebased.Init();
                controllerDraw.enabled = true;

              
                break;
            case SelectionTech.Paint:
                Structurebased.Init();

                controllerDraw.enabled = true;
    
                break;
            case SelectionTech.BaseLine:
   
                controllerDraw.enabled = false;
                break;
            default:
                break;
        }
    }

    private void Update()
    {

        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default) //release the second stack
        {
            DataMemory.ReleaseOperatorStack();

        }
        switch (selectionTech)
        {
            case SelectionTech.Point:
                pointbasedselection();
                break;
            case SelectionTech.Brush:
                linebasedselection();
                break;
            case SelectionTech.BaseLine:
                baseline();
                break;
            case SelectionTech.Paint:
                structurebasedselection();
                break;
            default:
                break;
        }


        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Grip))//erase
        {

            marker.gameObject.transform.localScale = map.transform.localScale * R;
            eraseNum = 0;

            marker.transform.GetChild(0).gameObject.SetActive(false);
            marker.transform.GetChild(1).gameObject.SetActive(true);

        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Grip))
        {

            if (Baseline.Erase(map.InverseTransformPoint(marker.position), R, DataMemory.allParticle))
            {
                eraseNum++;
                RenderDataRunTime.GenerateMesh();
            };



        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Grip))
        {
   
            List<int> last = DataMemory.GetpStack();
            for (int i = 0; i < eraseNum; i++)
                DataMemory.Return();
            DataMemory.ReleaseOperatorStack();
            DataMemory.AddParticlesDirectly(last);
            marker.transform.GetChild(0).gameObject.SetActive(true);
            marker.transform.GetChild(1).gameObject.SetActive(false);
        }


        // float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed; //adjust the radius
        // if (moveyLeft != 0)
        // {
        //     R += moveyLeft;
        //     if (R > 20f * DataMemory.densityField.XSTEP)
        //         R = 20f * DataMemory.densityField.XSTEP;
        //     if (R < 1f * DataMemory.densityField.XSTEP)
        //         R = 1f * DataMemory.densityField.XSTEP;
        //     marker.gameObject.transform.localScale = map.transform.localScale * R;
        // }

        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))   //adjust thre
        {

            CancelInvoke();
        }
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {

            Invoke("MCDisappear", 1f);
        }
    }

    public void Undo()
    {
        DataMemory.Return();
        RenderDataRunTime.GenerateMesh();
        McGPU.SetMCGPUThreshold(0f);
    }
    public void Redo()
    {
        DataMemory.Forward();
        RenderDataRunTime.GenerateMesh();
    }
    public void Reset()
    {
        DataMemory.StacksInitialize();
        Structurebased.Init();
        Linebased.Init();
        RenderDataRunTime.GenerateMesh();

    }

    #region MeTAPoint
    void pointbasedselection()
    {
       
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            Pointbased.SelectMC(map.InverseTransformPoint(marker.position), DataMemory.densityField, DataMemory.allParticle, McGPU);
        }
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            Pointbased.SelectParticles(map.InverseTransformPoint(marker.position), DataMemory.densityField, DataMemory.allParticle, McGPU);
            RenderDataRunTime.GenerateMesh();
            McGPU.SetMCGPUThreshold(0f);
        }
    }
    #endregion


    #region MeTABrush
    void linebasedselection()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {

            Linebased.Init();
            threshold_linear = 0f;
        }


        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {

            Linebased.SelectMC(controllerDraw.GetPointList(), R, DataMemory.densityField, DataMemory.densityField2, ref densityThreInitial, DataMemory.allParticle, McGPU);
            McGPU.SetMCFlagTexture(Linebased.GetboxIndexesOfComponentsEnclosingMaxLine(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField2, DataMemory.allParticle)); 
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));




            Linebased.SelectParticles(DataMemory.densityField, DataMemory.densityField2, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            RenderDataRunTime.GenerateMesh();
            controllerDraw.Initiate();
            Invoke("MCDisappear", 1f);
        }





        float moveyRight = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY) * Time.deltaTime * thremovespeed;  //adjust thre
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            DataMemory.Return();
            RenderDataRunTime.GenerateMesh();


        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            McGPU.SetMCFlagTexture(Linebased.GetboxIndexesOfComponentsEnclosingMaxLine(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField2, DataMemory.allParticle)); //ֻ��������maxLine����  ���԰�һЩ����������ֻ������maxline�ϵ�������
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            Linebased.SelectParticles(DataMemory.densityField, DataMemory.densityField2, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            RenderDataRunTime.GenerateMesh();
        }

        if (moveyRight != 0)
        {
            threshold_linear -= moveyRight;    //��ǰ��������
            if (threshold_linear > 20f)
                threshold_linear = 20f;
            if (threshold_linear < -20f)
                threshold_linear = -20f;


        }


        float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed;  //adjust the radius
        if (moveyLeft != 0)
        {
            R += moveyLeft;
            if (R > 20f * DataMemory.densityField.XSTEP)
                R = 20f * DataMemory.densityField.XSTEP;
            if (R < 1f * DataMemory.densityField.XSTEP)
                R = 1f * DataMemory.densityField.XSTEP;
            marker.gameObject.transform.localScale = map.transform.localScale * R;
        }

    }
    #endregion

    #region MeTAPaint
    void structurebasedselection()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
    
            Structurebased.Init();
            threshold_linear = 0f;
        }


        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            Structurebased.SelectMC(/*GameObject.Find("maxO"),*/ controllerDraw.GetPointList(), DataMemory.densityField, ref densityThreInitial, DataMemory.allParticle, McGPU);
            McGPU.SetMCFlagTexture(Structurebased.GetboxIndexesOfComponentsByMaxNumSeed(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField, DataMemory.allParticle)); //ֻ��������maxLine����  ���԰�һЩ����������ֻ������maxline�ϵ�������
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));


            Structurebased.SelectParticles(DataMemory.densityField, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);

        
            RenderDataRunTime.GenerateMesh();

            controllerDraw.Initiate();
            Invoke("MCDisappear", 1f);
        }




        float moveyRight = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY) * Time.deltaTime * thremovespeed;  //adjust thre
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            DataMemory.Return();
            RenderDataRunTime.GenerateMesh();


        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            McGPU.SetMCFlagTexture(Structurebased.GetboxIndexesOfComponentsByMaxNumSeed(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField, DataMemory.allParticle)); 
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            Structurebased.SelectParticles(DataMemory.densityField, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            RenderDataRunTime.GenerateMesh();
        }
        if (moveyRight != 0)
        {
            threshold_linear -= moveyRight;  
            if (threshold_linear > 20f)
                threshold_linear = 20f;
            if (threshold_linear < -20f)
                threshold_linear = -20f;


        }



    }
    #endregion


    #region baseline
    int brushNum = 0;
    List<int> brushInOnePress;

    void baseline()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {

            brushNum = 0;
            brushInOnePress = new List<int>();

        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            brushInOnePress.AddRange(Baseline.SelectParticles(map.InverseTransformPoint(marker.position), R, DataMemory.allParticle));
            RenderDataRunTime.GenerateMesh(false);
            brushNum++;
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            DataMemory.AddParticles(brushInOnePress);
        }
        float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed; //adjust the radius
        if (moveyLeft != 0)
        {
            R += moveyLeft;
            if (R > 20f * DataMemory.densityField.XSTEP)
                R = 20f * DataMemory.densityField.XSTEP;
            if (R < 1f * DataMemory.densityField.XSTEP)
                R = 1f * DataMemory.densityField.XSTEP;
            marker.gameObject.transform.localScale = map.transform.localScale * R;
        }

    }
    #endregion



    void MCDisappear()
    {
        McGPU.SetMCGPUThreshold(0f);
    }
}



