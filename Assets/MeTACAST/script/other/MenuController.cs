using HTC.UnityPlugin.Vive;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    PointCloudRuntime PCR;
    private GPUKDECsHelper kde;
    public Selection  selection;
    public GameObject PointUI;
    public GameObject BrushUI;
    public GameObject PaintUI;
       public enum Mode
        {
            Default,
            Menu
        }

        public GameObject selectDataUI;
        Mode initMode = Mode.Default;

        static public Mode currentMode;

        private void Awake()
        {
        selection  = this.gameObject.transform.parent.GetComponentInChildren<Selection>();
        PCR = this.gameObject.transform.parent.GetComponentInChildren<PointCloudRuntime>();
        kde= this.gameObject.transform.parent.GetComponentInChildren<GPUKDECsHelper>();
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



        public void SwitchTo_Ball_Hemisphere()
        {
         kde.gridNum=GRIDRes.grid64;
         PCR.dataset=Dataset.ball_hemisphere__bin;
           
        }


        public void SwitchTo_Nbody2()
        {
           kde.gridNum=GRIDRes.grid64;
           PCR.dataset=Dataset.nbody2__bin;
           
        }

         public void SwitchTo_Filament1()
        { 
           kde.gridNum=GRIDRes.grid128;
           PCR.dataset=Dataset.Flocculentcube1__bin;
           
        }

         public void SwitchTo_Filament2()
        {
         kde.gridNum=GRIDRes.grid128;
         PCR.dataset=Dataset.Flocculentcube2__bin;
          
        }

         public void SwitchTo_Nbody1()
        {
         kde.gridNum=GRIDRes.grid64;
         PCR.dataset=Dataset.nbody1__bin;
        }
         public void SwitchTo_Shell()
        {
         kde.gridNum=GRIDRes.grid128;
         PCR.dataset=Dataset.ball_hemisphere__bin;
        }
        public void SwitchTo_fiveElipsoids()
        {
         kde.gridNum=GRIDRes.grid128;
         PCR.dataset=Dataset.fiveellipsold__bin; 
        }

        public void SwitchTo_Point()
        {
        PointUI.SetActive(true); BrushUI.SetActive(false); PaintUI.SetActive(false);
        selection.Init(Selection.SelectionTech.Point);
        }
        public void SwitchTo_Brush()
        {
        PointUI.SetActive(false); BrushUI.SetActive(true); PaintUI.SetActive(false);
        selection.Init(Selection.SelectionTech.Brush);
        }
        public void SwitchTo_Paint()
        {
        PointUI.SetActive(false); BrushUI.SetActive(false); PaintUI.SetActive(true);
        selection.Init(Selection.SelectionTech.Paint);
        }





}
