using HTC.UnityPlugin.Vive;
using System.Collections.Generic;
using UnityEngine;


public class Selection : MonoBehaviour
{
    public SelectionTech selectionTech = SelectionTech.Point;
    public Transform marker;
    private Transform pointcloudData;

    public ControllerDraw cD;
    private Particles pG;
    private DensityField dF;
    private DensityField dF2;
    private PointRenderer pR;
    private MarchingCubeGPUCSHelper mcHelper;
    private static Stack<List<int>> pStack;
    private static Stack<List<int>> pOperateStack;

    //-----linebased
    float densityThreInitial = 0f;
    float threshold_linear;
    public float thremovespeed = 2f;
    //-----baseline
    float R = 0f;
    public float radiusmovespeed = 1f;

    float eraseNum = 0f;
    

    public static void AddParticles(List<int> l)  // previous+new
    {
        if (pStack.Count == 0)
            pStack.Push(l);
        else
        {
            List<int> newl = new List<int>();
            newl.AddRange( pStack.Peek());
            newl.AddRange(l);
            pStack.Push(newl);
        }
    }

    public static void AddParticlesDirectly(List<int> l)  //only add new, previous is not considered
    {
        pStack.Push(l);
    }


    public static List<int> GetpStack()
    {
        if(pStack.Count>0)
            return pStack.Peek();
        else
            return new List<int>();
    }

    public static void Return() 
    {
        if (pStack.Count == 0)
            return;
        pOperateStack.Push(  pStack.Pop());
    }
    public static void Forward()
    {
        if (pOperateStack.Count == 0)
            return;
        pStack.Push(pOperateStack.Pop());
    }
     static void ReleaseOperatorStack() 
    {
        pOperateStack = new Stack<List<int>>();
    }
     static void StacksInitialize()
    {
        pStack = new Stack<List<int>>();
        pOperateStack=new Stack<List<int>>();

    }

    
    public void Init()
    {
        pointcloudData = this.transform.parent;
        mcHelper = this.transform.parent.GetComponentInChildren<MarchingCubeGPUCSHelper>();
        pG=this.transform.parent.GetComponentInChildren<DataLoader>().particles;
        dF=this.transform.parent.GetComponentInChildren<GPUKDECsHelper>().densityField;
        pR=this.transform.parent.GetComponentInChildren<PointRenderer>();
        StacksInitialize();
        threshold_linear = 0f;
        Init(selectionTech);
    }
    
    public void Init(SelectionTech s)
    {

        selectionTech = s;
        marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);
        float ratio = transform.parent.localScale.x / (pG.XMAX - pG.XMIN);
        R = marker.gameObject.transform.localScale.x / ratio;
        switch (selectionTech)
        {
            case SelectionTech.Point:
                cD.enabled = false;
                break;
            case SelectionTech.Brush:
                MeTABrush.Init();
                dF2=new DensityField();
                dF2.CreateField(pG,dF.XNUM,dF.YNUM,dF.ZNUM);
                cD.enabled = true;
                break;
            case SelectionTech.Paint:
                MeTAPaint.Init();

                cD.enabled = true;
    
                break;
            case SelectionTech.BaseLine:
   
                cD.enabled = false;
                break;
            
            default:
                break;
        }
    }
  
    public enum SelectionTech { Point, Brush, Paint, BaseLine };
    private void Update()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default) //release the second stack
        {
            ReleaseOperatorStack();

        }
        switch (selectionTech)
        {
            case SelectionTech.Point:
                _MeTAPoint();
                break;
            case SelectionTech.Brush:
                _MeTABrush();
                break;
            case SelectionTech.BaseLine:
                baseline();
                break;
            case SelectionTech.Paint:
                _MeTAPaint();
                break;
            default:
                break;
        }


        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Grip))//erase
        {

            marker.gameObject.transform.localScale = pointcloudData.transform.localScale * R;
            eraseNum = 0;

            marker.transform.GetChild(0).gameObject.SetActive(false);
            marker.transform.GetChild(1).gameObject.SetActive(true);

        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Grip))
        {

            if (Baseline.Erase(pointcloudData.InverseTransformPoint(marker.position), R, pG))
            {
                eraseNum++;
                pR.GenerateMesh();
            };
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Grip))
        {
            List<int> last = GetpStack();
            for (int i = 0; i < eraseNum; i++)
                Return();
            ReleaseOperatorStack();
            AddParticlesDirectly(last);
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

    public void  Undo()
    {
        Return();
        pR.GenerateMesh(true);
        mcHelper.SetMCGPUThreshold(0f);
    }
    public void Redo()
    {
        Forward();
        pR.GenerateMesh(true);
    }
    public void Reset()
    {
        StacksInitialize();
        MeTAPaint.Init();
        MeTABrush.Init();
        pR.GenerateMesh(true);
    }

    
    
    
    
    #region MeTACAST
    void _MeTAPoint()
    {
        if(MenuController.currentMode!=MenuController.Mode.Default)
            return;
        
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
            MeTAPoint.SelectMC(pG.GetObjPosOfVec3(this.transform.parent,marker.position), dF, pG, mcHelper);
        }
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            MeTAPoint.SelectParticles(pG.GetObjPosOfVec3(this.transform.parent,marker.position),  dF, pG, pR, mcHelper);
        }
    }


    
    
    void _MeTAPaint()
    {
        if(MenuController.currentMode!=MenuController.Mode.Default)
            return;
        
        
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            MeTAPaint.Init();
            threshold_linear = 0f;
        }


        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            List<Vector3> inputs=new List<Vector3>();
            for (int i = 0; i < cD.GetPointList().Count; i++)
            {
                inputs.Add(pG.GetObjPosOfVec3(this.transform.parent,cD.GetPointList()[i]));
            }
            
            MeTAPaint.SelectMC(/*GameObject.Find("maxO"),*/ inputs, dF,  pG, ref densityThreInitial, mcHelper);
            mcHelper.SetMCFlagTexture(MeTAPaint.CalculateFocus(densityThreInitial * Mathf.Pow(2, threshold_linear), dF,pG)); 
            mcHelper.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));


            Selection.AddParticles(MeTAPaint.SelectParticles(dF, densityThreInitial * Mathf.Pow(2, threshold_linear), pG));

            pR.GenerateMesh(true);

            cD.ResetDrawing();
            
            Invoke("MCDisappear", 1f);
            
            
        }




        float moveyRight = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY) * Time.deltaTime * thremovespeed;  //adjust thre
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            Return();
            pR.GenerateMesh();
        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            mcHelper.SetMCFlagTexture(MeTAPaint.CalculateFocus(densityThreInitial * Mathf.Pow(2, threshold_linear), dF, pG)); 
            mcHelper.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            AddParticles(MeTAPaint.SelectParticles(dF, densityThreInitial * Mathf.Pow(2, threshold_linear), pG));
            pR.GenerateMesh(true);
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

    
    
    
    
    
    void _MeTABrush()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {

            MeTABrush.Init();
            threshold_linear = 0f;
        }


        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
            List<Vector3> inputs=new List<Vector3>();
            for (int i = 0; i < cD.GetPointList().Count; i++)
            {
                inputs.Add(pG.GetObjPosOfVec3(this.transform.parent,cD.GetPointList()[i]));
            }

            MeTABrush.SelectMC(inputs, R, dF, dF2, ref densityThreInitial,pG, mcHelper);
            

            mcHelper.SetMCFlagTexture(MeTABrush.GetGridIndexesEnclosingMaxLine(densityThreInitial * Mathf.Pow(2, threshold_linear), dF2, pG)); 
            mcHelper.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));


            AddParticles( MeTABrush.SelectParticles(dF, dF2, densityThreInitial * Mathf.Pow(2, threshold_linear), pG));
            pR.GenerateMesh(true);
            cD.ResetDrawing();
            Invoke("MCDisappear", 1f);
        }





        float moveyRight = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY) * Time.deltaTime * thremovespeed;  //adjust thre
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            Return();
            pR.GenerateMesh(true);


        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            mcHelper.SetMCFlagTexture(MeTABrush.GetGridIndexesEnclosingMaxLine(densityThreInitial * Mathf.Pow(2, threshold_linear), dF2, pG));
            mcHelper.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            AddParticles( MeTABrush.SelectParticles(dF, dF2, densityThreInitial * Mathf.Pow(2, threshold_linear), pG));
            
            pR.GenerateMesh(true);
        }

        if (moveyRight != 0)
        {
            threshold_linear -= moveyRight;   
            if (threshold_linear > 20f)
                threshold_linear = 20f;
            if (threshold_linear < -20f)
                threshold_linear = -20f;


        }


        float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed;  //adjust the radius
        if (moveyLeft != 0)
        {
            R += moveyLeft;
            if (R > 20f * dF.XSTEP)
                R = 20f * dF.XSTEP;
            if (R < 1f * dF.XSTEP)
                R = 1f * dF.XSTEP;
            marker.gameObject.transform.localScale = pointcloudData.transform.localScale * R;
        }

    }

    
    
    
    
    
    
    
    

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
            brushInOnePress.AddRange(Baseline.SelectParticles(pointcloudData.InverseTransformPoint(marker.position), R, pG));
            pR.GenerateMesh(false);
            brushNum++;
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
        {
           AddParticles(brushInOnePress);
        }
        float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed; //adjust the radius
        if (moveyLeft != 0)
        {
            R += moveyLeft;
            if (R > 20f * dF.XSTEP)
                R = 20f * dF.XSTEP;
            if (R < 1f * dF.XSTEP)
                R = 1f * dF.XSTEP;
            marker.gameObject.transform.localScale = pointcloudData.transform.localScale * R;
        }

    }
    #endregion



    void MCDisappear()
    {
        mcHelper.SetMCGPUThreshold(0f);
    }
}



