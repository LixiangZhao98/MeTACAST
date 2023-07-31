using HTC.UnityPlugin.Vive;
using PavelKouril.MarchingCubesGPU;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserstudyController : MonoBehaviour
{
    public int PID;


    public int taskId;
    public int trail;
    public bool isPre;
    public Task task;

    public Transform map;
    public Transform marker;
    public Camera cam;
    List<Task> tasks;
    RunTimeController rt;
   public RenderDataRunTime RD;
    public MarchingCubeGPU MCgpu;
    public UI ui;
    public Selection se;
   
     string path;
   public  List<string> allLine;
    string oneLine;
    int tragetTrail = 3;

    float startTime = 0f;
    private void Awake()
    {
      
        rt = this.gameObject.GetComponent<RunTimeController>();
        allLine=new List<string>();

        trail = 0; 
        tasks = LatinSquare.GetTask(PID);

        StartTask(taskId);

    }


    private void Update()
    {
        if(isPre)
        {
            Pre();
        }
            else
        {
            Study();
           
        }

        if (task.d == Dataset.Flocculentcube1)
        {
            if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Joystick))
                {
                if (DataMemory.GetpStack().Count == 0)
                {
                    AddRedisplayTarget();
                    StartCoroutine(Disappear()); }
            }
     
        }
    }

    public void StartTask(int num)
    {
       
        
        DataMemory.StacksInitialize();
        RD.GenerateMesh();
        LoadTask(num);
        oneLine = "Time,ParticipantID,DatasetID,TechniqueID,RepetitionID,mapPosx,mapPosy,mapPosz,mapRotaionx,mapRotationy,mapRotationz,markerHandPosx,markerHandPosy,markerHandPosz,reletivePosx,reletivePosy,reletivePosz,Lable";
        allLine = new List<string>();
        allLine.Add(oneLine);
        AddStart();
       Structurebased.Init();
       Linebased.Init();
        Debug.Log("Start record "  + "ParticipantID_" + PID + "_" + "DatasetID_" + (int)task.d + "_" + "TechniqueID_" + (int)task.s + "_" + "RepetitionID_" + trail + ".csv");

    }
    public void WriteToCsv()
    {
        csvController.GetInstance().WriteCsv(allLine.ToArray(), path);
    }
    public void AddLine(Lable l)
    {
        oneLine = Time.realtimeSinceStartup - startTime+","+ PID + "," +(int) task.d +"," + (int)task.s + "," + trail + "," + map.position.x + "," + map.position.y + "," + map.position.z + "," + map.eulerAngles.x + "," + map.eulerAngles.y + "," + map.eulerAngles.z + "," + marker.position.x + "," + marker.position.y + "," + marker.position.z + ","+ map.InverseTransformPoint(marker.position).x+","+ map.InverseTransformPoint(marker.position).y+","+ map.InverseTransformPoint(marker.position).z+"," + l.ToString();
        allLine.Add(oneLine);
    }


    void Pre()
    {
        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.AKey)|| ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.BKey)||Input.GetKeyDown(KeyCode.Space))  //next
        {
            if (DataMemory.GetpStack().Count > 0)
            {
                taskId++; trail = 0;
                if (taskId >= tasks.Count)
                {
                    DataMemory.StacksInitialize();
                    RD.gameObject.SetActive(false);
                    ui.EndText.SetActive(true);
                    ui.InTestText.SetActive(false);
                }
                else
                    StartTask(taskId);
            }
        }
    }


    void Study()
    {
       

        if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.AKey) || ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.BKey))  //next
        {

            if(DataMemory.GetpStack().Count > 0)
            {
                MCgpu.SetMCGPUThreshold(0f);
                MCgpu.SetMCFlagTexture(new List<int>());
                AddLine(Lable.End);
                AddSelectionResult();
               
                if (!isPre)
                    WriteToCsv();

                allLine = new List<string>();
                trail++;
                if (trail == tragetTrail)
                {
                    trail = 0;
                    map.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                    taskId++;
                    if (taskId >= tasks.Count)
                    {
                        DataMemory.StacksInitialize();
                        RD.gameObject.SetActive(false);
                        ui.EndText.SetActive(true);
                        ui.InTestText.SetActive(false);
                    }
                    else
                    StartTask(taskId);
                }
                else
                {
                    AddLine(Lable.End);
                    if (trail == 1)
                        map.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    if (trail == 2)
                        map.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
                    StartTask(taskId);
                }
             
            }
            else
            {
                ui.PrintVoidSelection();
            }
            
        }

    }


 public void AddStart()
    {
        startTime = Time.realtimeSinceStartup;
        AddLine(Lable.Start);
    }
    public void AddDoingNothing()
    {
        AddLine(Lable.Nothing);
    }
    public void AddAdjustThreStart()
    {
        AddLine(Lable.AdjustingThresholdStart);
    }
    public void AddAdjustingThre()
    {
        AddLine(Lable.AdjustingThreshold);
    }
    public void AddAdjustThreEnd()
    {
        AddLine(Lable.AdjustingThresholdEnd);
    }
    public void AddEraseStart()
    {
        AddLine(Lable.MinusStart);
    }
    public void AddErasing()
    {
        AddLine(Lable.Minusing);
    }
    public void AddEraseEnd()
    {
        AddLine(Lable.MinusEnd);
    }
    public void AddSelectionStart()
    {
        AddLine(Lable.SelectionStart);
    }
    public void AddSelecting()
    {
        AddLine(Lable.Selecting);
    }
    public void AddGotSelection()
    {
        AddLine(Lable.GotSelection);
    }
    public void AddSelectionResult()
    { int suppose_sel_sel = 0;
      int suppose_sel_unsel = 0;
      int suppose_unsel_sel = 0;
      int suppose_unsel_unsel = 0;
        for (int i=0;i<DataMemory.allParticle.GetParticlenum();i++)
        {
            if(DataMemory.allParticle.GetFlag(i))
            {
                if (DataMemory.allParticle.GetTarget(i))
                    suppose_sel_sel++;
                else
                    suppose_unsel_sel++;

            }
            else
            {
                if (DataMemory.allParticle.GetTarget(i))
                    suppose_sel_unsel++;
                else
                    suppose_unsel_unsel++;
            }
        }
        oneLine = "Supposed to be selected_Selected"+","+ suppose_sel_sel+","+ "Not supposed to be selected_Not Selected"+","+suppose_unsel_unsel+","+ "Supposed to be selected_Not Selected"+","+suppose_sel_unsel+","+ "Not supposed to be selected_Selected"+","+suppose_unsel_sel;
        allLine.Add(oneLine);
    }
    public void AddSelectionEnd()
    { 
            AddLine(Lable.SelectionEnd);
    }
    public void AddRedisplayTarget()
    {
        AddLine(Lable.RedisplayTarget);
    }
    public void Reset()
    {

        AddLine(Lable.Reset);
    }
    public void Undo()
    {

        AddLine(Lable.Undo);
    }
    public void Redo()
    {

        AddLine(Lable.Redo);
    }
    void LoadTask(int num)

    {
        task = tasks[num];
        rt.selectionTech = task.s;
        rt.dataset = task.d;
        isPre = task.isPre;
        path = Application.dataPath + "/my/userstudy/" + PID + "/" + "ParticipantID_" + PID + "_" + "DatasetID_" + (int)task.d + "_" + "TechniqueID_" + (int)task.s + "_" + "RepetitionID_" + trail + "TaskID_"+taskId+".csv";
        try
        {
            if (task.d == Dataset.Flocculentcube1)
            {
                rt.LoadFlagName = trail.ToString();
                DataMemory.StacksInitialize();
                DataMemory.allParticle.ClearTarget();
                if (rt.LoadFlag)
                    DataMemory.LoadFlagsToStack(rt.LoadFlagName);
                RD.GenerateMesh();
            }
            else
                rt.LoadFlagName = "1";
        }
 
        catch(Exception e)
        {

        }

        if (isPre)
        
            cam.backgroundColor = new Color(0f, 9f / 255f, 27f / 255f, 0f);
        
        else
            cam.backgroundColor = Color.black;

        se.initializeRadius();

        if(task.d==Dataset.Flocculentcube1)
        {
            if(DataMemory.GetpStack().Count==0)
            StartCoroutine(Disappear());
        }
        else
        {
          RD.target_mat.SetColor("_EmissionColor", new Color(1f,247f/255f,0f));
        }
    }

    private IEnumerator Disappear()
    {
        RD.target_mat.SetColor("_EmissionColor", new Color(1f, 247f / 255f, 0f));
        yield return new WaitForSeconds(2f);
        RD.target_mat.SetColor("_EmissionColor", new Color(30f / 255f, 144f / 255f, 1f));
    }
    public enum Lable {
        Nothing,//
        Start,End,Reset,//
        Undo,Redo,//
        SelectionStart, Selecting, SelectionEnd,GotSelection,//
        MinusStart, Minusing, MinusEnd,//
        AdjustingThresholdStart, AdjustingThreshold, AdjustingThresholdEnd,//
        RedisplayTarget
    };
}
