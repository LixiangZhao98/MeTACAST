using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using LixaingZhao.MeTACAST;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    RunTimeController RC;
    public GameObject PointUI;
    public GameObject BrushUI;
    public GameObject PaintUI;
       public enum Mode
        {
            Default,
            Menu
        }

       [HideInInspector]  public Mode initMode = Mode.Default;



  
        public GameObject selectDataUI;
        static public Mode currentMode;

        private void Awake()
        {
            RC = this.gameObject.transform.parent.GetComponentInChildren<RunTimeController>();
            EnterMode(initMode);
        }

        void Update()
        {if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.BKey)||ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.AKey)||ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Menu)) 
        {
            if(currentMode==Mode.Default)
            EnterMode(Mode.Menu);
            else
            EnterMode(Mode.Default);
        }
        }

      


        private void EnterMode(Mode mode)
        {currentMode=mode;
            switch (mode)
            {
                case Mode.Default:selectDataUI.SetActive(false); break;
                case Mode.Menu: selectDataUI.SetActive(true); break;
            }
        }





        public void SwitchTo_Nbody2()
        {RC.SetGRIDNum(64);
           RC.DATASET=Dataset.nbody2;
           
        }

         public void SwitchTo_Filament1()
        {  RC.SetGRIDNum(100);
           RC.DATASET=Dataset.Flocculentcube1;
           
        }

         public void SwitchTo_Filament2()
        {RC.SetGRIDNum(100);
           RC.DATASET=Dataset.Flocculentcube2;
          
        }

         public void SwitchTo_Nbody1()
        {RC.SetGRIDNum(64);
           RC.DATASET=Dataset.nbody1;
        }
                 public void SwitchTo_Shell()
        {RC.SetGRIDNum(100);
           RC.DATASET=Dataset.ball_hemisphere;
        }
                 public void SwitchTo_fiveElipsoids()
        {RC.SetGRIDNum(100);
           RC.DATASET=Dataset.fiveellipsolds; 
        }

        public void SwitchTo_Point()
        {
           PointUI.SetActive(true); BrushUI.SetActive(false); PaintUI.SetActive(false);
           RC.SwitchSelectionTech(SelectionTech.Point);
        }
        public void SwitchTo_Brush()
        {
            PointUI.SetActive(false); BrushUI.SetActive(true); PaintUI.SetActive(false);
           RC.SwitchSelectionTech(SelectionTech.Brush);
        }
        public void SwitchTo_Paint()
        {
            PointUI.SetActive(false); BrushUI.SetActive(false); PaintUI.SetActive(true);
           RC.SwitchSelectionTech(SelectionTech.Paint);
        }





}
