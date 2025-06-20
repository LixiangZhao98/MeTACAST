
    using UnityEngine;
    using UnityEngine.Events;
    using System.ComponentModel;
    public class GPUKDECsHelper:MonoBehaviour
    {

        [HideInInspector] public DensityField densityField;
        private Particles pG;
        private ComputeShader kdeInstance;
        public ComputeShader kde;
        public GRIDRes gridNum;
        public bool gradient;
        public bool curvature;
        public bool loadFromLocal;
        private GRIDRes gridNum_LastFrame;
        public UnityEvent eventAfterDensityEst;
    
        private void Start()
        {
            gridNum_LastFrame = gridNum;
        }
    
        private void Update()
        {
            if (gridNum_LastFrame != gridNum)
            {
                KernelDensityEstimation();
                gridNum_LastFrame = gridNum;
            }
        }
    
        public void KernelDensityEstimation()
        {
            kdeInstance =  Instantiate(kde);
            densityField = new DensityField(); 
            pG = transform.parent.GetComponentInChildren<DataLoader>().particles;
            densityField.CreateField(pG,(int)gridNum,(int)gridNum,(int)gridNum);
            densityField.KDE(pG,kdeInstance,loadFromLocal);
            // densityField.KDE_LargeGrid(pG,kdeInstance,loadFromLocal,batchSize:128,gradient:gradient,curvature:curvature);
            if(gradient) densityField.CalGradient(pG);
            if(curvature) densityField.CalCurvature(pG);
            // densityField.CalNodeTangent(pG,new Vector3(1, 0, 0));
            eventAfterDensityEst?.Invoke(); //actions after data load
            

        }
     
        private void OnApplicationQuit()
        {
            Destroy(this);
        }
    }
    
