//
//  ParticleProperty.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace LixaingZhao.MeTACAST
{

    [System.Serializable]
    public class ParticleGroup
    {
        #region variables
        [SerializeField]
        public string name;
        [SerializeField]
        private List<Particle> particleGroup;
        [SerializeField]
        private float xmin, ymin, zmin;
        [SerializeField]
        private float xmax, ymax, zmax;
        [SerializeField]
        Vector3 smoothLength;
        public float XMIN { get { return xmin; } set { xmin = value; } }
        public float XMAX { get { return xmax; } set { xmax = value; } }
        public float YMIN { get { return ymin; } set { ymin = value; } }
        public float YMAX { get { return ymax; } set { ymax = value; } }
        public float ZMIN { get { return zmin; } set { zmin = value; } }
        public float ZMAX { get { return zmax; } set { zmax = value; } }

        #endregion
        #region Get Property
        public Vector3 GetMinParPos()

            { return new Vector3(xmin, ymin, zmin); }
        public Vector3 getMaxParPos()
        {
            return new Vector3(xmax, ymax, zmax);
        }
        public float GetXScale()
        {
            return xmax - xmin;
        }
        public float GetYScale()
        {
            return ymax - ymin;
        }
        public float GetZScale()
        {
            return zmax - zmin;
        }
        public Vector3 GetCenter()
        {
            return GetMinParPos()+getMaxParPos()/2;
        }
        public float GetLongestAxisScale()
        {
            float max;
            if (GetXScale() >= GetYScale())
                max = GetXScale();
            else
                max = GetYScale();

            if (max >= GetZScale())
                return max;
            else
                return GetZScale();
        }
        public int GetParticlenum()
        {
            return particleGroup.Count;
        }
        public Vector3 GetParticlePosition(int i)
        {
            return this.particleGroup[i].GetPosition();
        }
        #endregion
        #region Set Property /Add

       public ParticleGroup()
        {
            particleGroup = new List<Particle>();
        }
        public void AddParticle(Particle p)
        {
            particleGroup.Add(p);
        }

       
        public Vector3 GetMySmoothLength(int i)
        {
            return particleGroup[i].GetMySmoothLength();
        }
        public Vector3 GetFlowEnd(int i)
        {
            return particleGroup[i].GetFlowEnd();
        }
        public double GetParticleDensity(int i)
        {
            return particleGroup[i].GetParticleDensity();
        }
        public bool GetFlag(int i)
        {
            return particleGroup[i].GetFlag();
        }
        public bool GetTarget(int i)
        {
            return particleGroup[i].GetTarget();
        }
        
        public Vector3 GetSmoothLength()
        {
            return smoothLength;
        }
        public void SetFlowEnd(int i, Vector3 v)
        {
            particleGroup[i].SetFlowEnd(v);
        }
        public void SetMySmoothLength(float Sx, float Sy, float Sz, int i)
        {
            particleGroup[i].SetMySmoothLength(Sx, Sy, Sz);
        }
        public void SetParticleDensity(int i, double density)
        {
            particleGroup[i].SetParticleDensity(density);
        }
       
        public void SetFlag(int i,bool flag)
        {
            particleGroup[i].SetFlag(flag);
        }
        public void SetTarget(int i, bool flag, int type)
        {
              particleGroup[i].SetTarget(flag,type);
        }
        public void SetSmoothLength(Vector3 v)
        {
            smoothLength = v;
        }

        #endregion
        #region load and save
       
        public void LoadDatasetByByte(string path, string dataname)
        {
           this. name=dataname;
            Vector3[] vs = DataPosPreProcessing(LoadDataBybyte.StartLoad(path));
            particleGroup = new List<Particle>();
            for (int i=0;i<vs.Length;i++)
            {
                Particle p = new Particle(vs[i]);
                this.AddParticle(p);
            }
         
        }
        public void LoadDatasetByVec3s(Vector3[] v, string dataname,bool forSimulation=false)
        {
            this.name = dataname;
            if(!forSimulation)
            v = DataPosPreProcessing(v);
            particleGroup = new List<Particle>();
            for (int i = 0; i < v.Length; i++)
            {
                Particle p = new Particle(v[i]);
                this.AddParticle(p);
            }
        }
        public void LoadDatasetByCsv(string path,string dataname)
        {
            this.name = dataname;
            Vector3[] vs = DataPosPreProcessing(csvController.GetInstance().StartLoad(path));
            particleGroup = new List<Particle>();
            for (int i = 0; i < vs.Length; i++)
            {
                Particle p = new Particle(vs[i]);
                this.AddParticle(p);
            }
        }
       public Vector3[]  DataPosPreProcessing( Vector3[] vs)   //put the data near the origin if the data is far
        {
            Vector3 vSum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < vs.Length; i++)    //find the average point(must located within the dataset.initialize the min and max to it
            {
                vSum = vSum + vs[i];
            }
            Vector3 vAve = vSum / vs.Length;
          
            xmin = vAve.x;
            xmax = vAve.x;
            ymin = vAve.y;
            ymax = vAve.y;
            zmin = vAve.z;
            zmax = vAve.z;
            for (int i = 0; i < vs.Length; i++)  //find the max and min
            {
                if (xmin > vs[i].x)
                    xmin = vs[i].x;
                if (xmax < vs[i].x)
                    xmax = vs[i].x;
                if (ymin > vs[i].y)
                    ymin = vs[i].y;
                if (ymax < vs[i].y)
                    ymax = vs[i].y;
                if (zmin > vs[i].z)
                    zmin = vs[i].z;
                if (zmax < vs[i].z)
                    zmax = vs[i].z;
            }

            Vector3 middle = new Vector3((xmax + xmin) / 2, (ymax + ymin) / 2, (zmax + zmin) / 2);  //revise the pos to near view
            Vector3[] vsRevised = new Vector3[vs.Length];
            for (int i = 0; i < vs.Length; i++)
            {
                vsRevised[i] = vs[i] - middle;
            }


            vSum = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < vsRevised.Length; i++)    //find the average point(must located within the dataset.initialize the min and max to it
            {
                vSum = vSum + vsRevised[i];
            }
            vAve = vSum / vsRevised.Length;

            xmin = vAve.x;
            xmax = vAve.x;
            ymin = vAve.y;
            ymax = vAve.y;
            zmin = vAve.z;
            zmax = vAve.z;
            float[] xPos = new float[vsRevised.Length];
            float[] yPos = new float[vsRevised.Length];
            float[] zPos = new float[vsRevised.Length];
            for (int i = 0; i < vsRevised.Length; i++)  //Find the max and min   Calculate the smoothlength
            {
                if (xmin > vsRevised[i].x)
                    xmin = vsRevised[i].x;
                if (xmax < vsRevised[i].x)
                    xmax = vsRevised[i].x;
                if (ymin > vsRevised[i].y)
                    ymin = vsRevised[i].y;
                if (ymax < vsRevised[i].y)
                    ymax = vsRevised[i].y;
                if (zmin > vsRevised[i].z)
                    zmin = vsRevised[i].z;
                if (zmax < vsRevised[i].z)
                    zmax = vsRevised[i].z;
                xPos[i] = vsRevised[i].x;
                yPos[i] = vsRevised[i].y;
                zPos[i] = vsRevised[i].z;
            }
            Utility .q_sort(xPos, 0, vsRevised.Length - 1);
            Utility.q_sort(yPos, 0, vsRevised.Length - 1);
            Utility.q_sort(zPos, 0, vsRevised.Length - 1);
            double smoothingLengthX = 2 * (xPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.8))] - xPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.2))]) / Mathf.Log10(vsRevised.Length);
            double smoothingLengthY = 2 * (yPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.8))] - yPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.2))]) / Mathf.Log10(vsRevised.Length);
            double smoothingLengthZ = 2 * (zPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.8))] - zPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.2))]) / Mathf.Log10(vsRevised.Length);
            this.SetSmoothLength(new Vector3((float)smoothingLengthX, (float)smoothingLengthY, (float)smoothingLengthZ));
            return vsRevised;
        }

      public void StoreFlags(string name)
        {
            List<int> flagtrue = DataMemory.GetpStack().ToList();

            if (flagtrue.Count == 0)
                Debug.Log("No marked particles");
            else
            {
                SaveData.FlagsToFile(name, flagtrue.ToArray());
            }
        }

                public void SaveSelectedAsNewData(string name)
        {
            List<int> flagtrue = DataMemory.GetpStack().ToList();


            if (flagtrue.Count == 0)
                Debug.Log("No marked particles");
            else
            {            List<Vector3> dataPos=new List<Vector3>();
            foreach(var d in flagtrue)
            {
                dataPos.Add(DataMemory.allParticle.GetParticlePosition(d));
            }
                SaveData.Vec3sToFile(name, dataPos.ToArray());
            }
        }

               public void SaveTargetAsNewData(string name)
        {
              List<Vector3> dataPos=new List<Vector3>();
             for(int i=0;i< DataMemory.allParticle.GetParticlenum();i++)
            {
                if(DataMemory.allParticle.GetTarget(i))
                dataPos.Add(DataMemory.allParticle.GetParticlePosition(i));
            }

            if (dataPos.Count == 0)
                Debug.Log("No Target particles");
            else
                     
                 SaveData.Vec3sToFile(name, dataPos.ToArray());
            
               
            }


                          public void SaveDataAsNewData(string name)
        {
              List<Vector3> dataPos=new List<Vector3>();
             for(int i=0;i< DataMemory.allParticle.GetParticlenum();i++)
            {
                dataPos.Add(DataMemory.allParticle.GetParticlePosition(i));
            }

            if (dataPos.Count == 0)
                Debug.Log("No Target particles");
            else
                     
                 SaveData.Vec3sToFile(name, dataPos.ToArray());
            
               
            }

#endregion

    }
    [System.Serializable]
    public class Particle
    {
        #region variable/Constructor
        [SerializeField]
        private Vector3 particlePosition;
        [SerializeField]
        private double particleDensity;
        [SerializeField]
        private Vector3 my_SmoothLength;
        [SerializeField]
        private bool isSelected;
        [SerializeField]
        private bool isTarget;
        [SerializeField]
        private int targetType;
        [SerializeField]
        private Vector3 gradiant;
        [SerializeField]
        private Vector3 flowEnd;

        public Particle(Vector3 v)
        {
            this.particlePosition = v;
            particleDensity = 0;
            isSelected = false;
            gradiant = Vector3.zero;
        }
        #endregion
        #region Get
        public Vector3 GetFlowEnd()
        {
          return  flowEnd;
        }

        public Vector3 GetPosition()
        {
            return particlePosition;
        }

        public double GetParticleDensity()
        {
            return particleDensity;
        }
        public Vector3 GetMySmoothLength()
        {
            return my_SmoothLength;
        }
 
        public bool GetFlag()
        {
            return isSelected;
        }
        public bool GetTarget()
        {
            return isTarget;
        }
        public Vector3 GetGradient()
        {
            return gradiant;
        }
        #endregion
        #region Set

        public void SetFlowEnd(Vector3 v)
        {
            flowEnd = v;
        }
        public void SetMySmoothLength(float Sx, float Sy, float Sz)
        {
            my_SmoothLength = new Vector3(Sx, Sy, Sz);
        }
        public void SetParticleDensity(double density)
        {
            particleDensity = density;
        }

        public void SetFlag(bool flag)
        {
            isSelected = flag;
        }
        public void SetTarget(bool t, int type)
        {

            isTarget = t;
            targetType=type;
        }
        public void SetGradient(Vector3 grad)
        {
            gradiant = grad;
        }
        #endregion
    }



   
}





