using HTC.UnityPlugin.Vive;
using PavelKouril.MarchingCubesGPU;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class Selection : MonoBehaviour
{
    SelectionTech selectionTech;
    public Transform marker;
    public Transform map;
    public ControllerDraw controllerDraw;
    public UserstudyController UC;
  
    MarchingCubeGPU McGPU;
    public RenderDataRunTime RD;
   
    //-----linebased
    float densityThreInitial = 0f;
    float threshold_linear;
    public float thremovespeed = 2f;
    //-----baseline
    float R = 0f;
    public float radiusmovespeed = 1f;



    bool doingNothing = true;
    float eraseNum=0f;
    public void Init(SelectionTech s)
    {
     
        McGPU = this.transform.parent.GetComponentInChildren<MarchingCubeGPU>();
        RD = this.transform.parent.GetComponentInChildren<RenderDataRunTime>();

        threshold_linear = 0f;
        selectionTech = s;
        float ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * RD. mapRealSize;
        switch (selectionTech)
        {
            case SelectionTech.Point:
              
                controllerDraw.enabled = false;
                marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); R = 0.012f / ratio;
                break;

            case SelectionTech.Brush:
                Linebased.Init();
                controllerDraw.enabled = true;
               
                marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f);  R = 0.012f / ratio; 
                break;
            case SelectionTech.Paint:
                Structurebased.Init();
         
                controllerDraw.enabled = true;
                marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); R = 0.012f / ratio;
                break;
            case SelectionTech.BaseLine:
       UnityEngine. Debug.Log("dwa");
                marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); R = 0.012f / ratio; 
                controllerDraw.enabled = false;
                break;
            default:
                break;
        }
    }
    public void initializeRadius()
    {
        float ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * RD.mapRealSize;
        marker.gameObject.transform.localScale = new Vector3(0.012f, 0.012f, 0.012f); R = 0.012f / ratio; 
    
    }
    private void Update()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.AKey)) //release the second stack
        {
            Undo();

        }
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.BKey)) //release the second stack
        {
            Redo();

        }
        doingNothing = true;
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)) //release the second stack
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
           
            doingNothing = false;
            marker.gameObject.transform.localScale = map.transform.localScale * R;
            eraseNum = 0;
            UC.AddEraseStart();
            marker.transform.GetChild(0).gameObject.SetActive(false);
            marker.transform.GetChild(1).gameObject.SetActive(true);

        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Grip))
        {
            doingNothing = false;
            UC.AddErasing();
            if (Baseline.Erase(map.InverseTransformPoint(marker.position), R, DataMemory.allParticle))
            { eraseNum++;
                RD.GenerateMesh();
            } ;
           
           
          
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Grip))
        {
            doingNothing = false;
           // marker.gameObject.transform.localScale = map.transform.localScale * R;
            UC.AddEraseEnd();
            List<int> last = DataMemory.GetpStack();
            for (int i = 0; i < eraseNum; i++)
                DataMemory.Return();
            DataMemory.ReleaseOperatorStack();
            DataMemory.AddParticlesDirectly(last);
            marker.transform.GetChild(0).gameObject.SetActive(true);
            marker.transform.GetChild(1).gameObject.SetActive(false);
        }


        float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed; //adjust the radius
        if (moveyLeft != 0)
        {
            doingNothing = false;
            R += moveyLeft;
            if (R > 20f * DataMemory.densityField.XSTEP)
                R = 20f * DataMemory.densityField.XSTEP;
            if (R < 1f * DataMemory.densityField.XSTEP)
                R = 1f * DataMemory.densityField.XSTEP;
            marker.gameObject.transform.localScale = map.transform.localScale * R;
        }

        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))   //adjust thre
        {
            doingNothing = false;
            UC.AddAdjustThreStart();
            CancelInvoke();
        }
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;
            UC.AddAdjustThreEnd();
            Invoke("MCDisappear", 1f);
        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;
            UC.AddAdjustingThre();
        }

        if (doingNothing)    //nothing
           UC.AddDoingNothing();
    }

    public void Undo()
    {
        DataMemory.Return();
        RD.GenerateMesh();
        UC.Undo();
        McGPU.SetMCGPUThreshold(0f);
    }
    public void Redo()
    {
        DataMemory.Forward();
        RD.GenerateMesh();
        UC.Redo();
    }
    public void Reset()
    {
        DataMemory.StacksInitialize();
        Structurebased.Init();
        Linebased.Init();
        RD.GenerateMesh();
        UC.Reset();
    }

    #region pointbased
    void pointbasedselection()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            UC.AddSelectionStart();
            doingNothing = false;
        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
            UC.AddSelecting();
            Pointbased.SelectMC(map.InverseTransformPoint(marker.position), DataMemory.densityField, DataMemory.allParticle, McGPU);
        }
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
            UC.AddSelectionEnd();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Pointbased.SelectParticles(map.InverseTransformPoint(marker.position), DataMemory.densityField, DataMemory.allParticle, McGPU, UC);
            sw.Stop();
            UnityEngine.Debug.Log("MeTAPoint finish in " + sw.ElapsedMilliseconds);
            UC.AddGotSelection();
            RD.GenerateMesh();
     

 
            McGPU.SetMCGPUThreshold(0f);
        }
    }
    #endregion


    #region linebase
    void linebasedselection()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            UC.AddSelectionStart();
            doingNothing = false;
            Linebased.Init();
            threshold_linear = 0f;
        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
            UC.AddSelecting();
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {

            doingNothing = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Linebased.SelectMC(/*GameObject.Find("maxO"), */controllerDraw.GetPointList(), R, DataMemory.densityField, DataMemory.densityField2, ref densityThreInitial, DataMemory.allParticle, McGPU);
            McGPU.SetMCFlagTexture(Linebased.GetboxIndexesOfComponentsEnclosingMaxLine(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField2, DataMemory.allParticle)); //只保留包裹maxLine的体  可以把一些噪声除掉，只保留在maxline上的连续体
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));


            UC.AddSelectionEnd();
           
            Linebased.SelectParticles(DataMemory.densityField, DataMemory.densityField2, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            sw.Stop();
            UnityEngine.Debug.Log("MeTABrush finish in " + sw.ElapsedMilliseconds);

            UC.AddGotSelection();
            RD.GenerateMesh();

            controllerDraw.Initiate();
            Invoke("MCDisappear", 1f);
        }





        float moveyRight = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY) * Time.deltaTime * thremovespeed;  //adjust thre
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;


            DataMemory.Return();
            RD.GenerateMesh();


        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;
            McGPU.SetMCFlagTexture(Linebased.GetboxIndexesOfComponentsEnclosingMaxLine(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField2, DataMemory.allParticle)); //只保留包裹maxLine的体  可以把一些噪声除掉，只保留在maxline上的连续体
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;
            Linebased.SelectParticles(DataMemory.densityField, DataMemory.densityField2, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            RD.GenerateMesh();
        }

        if (moveyRight != 0)
        {
            doingNothing = false;
            threshold_linear -= moveyRight;    //向前推增大面
            if (threshold_linear > 20f)
                threshold_linear = 20f;
            if (threshold_linear < -20f)
                threshold_linear = -20f;


        }


        float moveyLeft = ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed;  //adjust the radius
        if (moveyLeft != 0)
        {
            doingNothing = false;
            R += moveyLeft;
            if (R > 20f * DataMemory.densityField.XSTEP)
                R = 20f * DataMemory.densityField.XSTEP;
            if (R < 1f * DataMemory.densityField.XSTEP)
                R = 1f * DataMemory.densityField.XSTEP;
            marker.gameObject.transform.localScale = map.transform.localScale * R;
        }

    }
    #endregion

    #region structurebased
    void structurebasedselection()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            UC.AddSelectionStart();
            doingNothing = false;
            Structurebased.Init();
            threshold_linear = 0f;
        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
            UC.AddSelecting();
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
              
            Structurebased.SelectMC(/*GameObject.Find("maxO"),*/ controllerDraw.GetPointList(), DataMemory.densityField, ref densityThreInitial, DataMemory.allParticle, McGPU);
            McGPU.SetMCFlagTexture(Structurebased.GetboxIndexesOfComponentsByMaxNumSeed(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField, DataMemory.allParticle)); //只保留包裹maxLine的体  可以把一些噪声除掉，只保留在maxline上的连续体
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));

            UC.AddSelectionEnd();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Structurebased.SelectParticles(DataMemory.densityField, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            sw.Stop();
            UnityEngine.Debug.Log("MeTAPaint finish in "+ sw.ElapsedMilliseconds);

            UC.AddGotSelection();
            RD.GenerateMesh();

            controllerDraw.Initiate();
            Invoke("MCDisappear", 1f);
        }




        float moveyRight = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.JoystickY) * Time.deltaTime * thremovespeed;  //adjust thre
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;


            DataMemory.Return();
            RD.GenerateMesh();


        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;
            McGPU.SetMCFlagTexture(Structurebased.GetboxIndexesOfComponentsByMaxNumSeed(densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.densityField, DataMemory.allParticle)); //只保留包裹maxLine的体  可以把一些噪声除掉，只保留在maxline上的连续体
            McGPU.SetMCGPUThreshold(densityThreInitial * Mathf.Pow(2, threshold_linear));
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.JoystickTouch))
        {
            doingNothing = false;
            Structurebased.SelectParticles(DataMemory.densityField, densityThreInitial * Mathf.Pow(2, threshold_linear), DataMemory.allParticle);
            RD.GenerateMesh();
        }
        if (moveyRight!= 0)
        {
            doingNothing = false;
            threshold_linear -= moveyRight;    //向前推增大面
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
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {

            doingNothing = false;
            brushNum = 0;
            brushInOnePress = new List<int>();
          
        }
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
            UC.AddSelectionStart();
            UC.AddSelectionEnd();
            brushInOnePress.AddRange( Baseline.SelectParticles(map.InverseTransformPoint(marker.position), R, DataMemory.allParticle));
            UC.AddGotSelection();
            RD.GenerateMesh(false);
            brushNum++;
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            doingNothing = false;
            //for (int i = 0; i < brushNum; i++)
            //    DataMemory.Return();
            //DataMemory.ReleaseOperatorStack();
            DataMemory.AddParticles(brushInOnePress);
        }
        float moveyLeft= ViveInput.GetAxis(HandRole.LeftHand, ControllerAxis.JoystickY) * Time.deltaTime * radiusmovespeed; //adjust the radius
        if (moveyLeft != 0)
        {
            doingNothing = false;
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

 

