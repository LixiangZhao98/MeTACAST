

using ScalarField;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



namespace ParticleProperty
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
        public Color GetParticleColor(int i)
        {
            return this.particleGroup[i].GetColor();
        }
        public float GetParticleSize(int i)
        {
            return this.particleGroup[i].GetSize();
        }
        public Vector3 GetParticleGradient(int i)
        {
            return this.particleGroup[i].GetGradient();
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

       
        public void ChangeParticleColor(int i, Color c)
        {

            particleGroup[i].ChangeColor(c);

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
        public List<Particle> GetGroup()
        {
            return particleGroup;
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
        public void SetParticleColor(int i, Color c)
        {
            particleGroup[i].SetParticleColor(c);
        }
        public void SetFlag(int i,bool flag)
        {
            particleGroup[i].SetFlag(flag);
        }
        public void SetTarget(int i, bool flag)
        {
            particleGroup[i].SetTarget(flag);
        }
        public void SetSmoothLength(Vector3 v)
        {
            smoothLength = v;
        }

        public void SetParticleGradient(int i, Vector3 v)
        {
            particleGroup[i].SetGradient(v);
        }
        public void ClearTarget()
        {
            for(int i=0;i<GetParticlenum();i++)
            {
                particleGroup[i].SetTarget(false);
            }
          
        }
        public void RemoveRange(int i, int r)
        {
            particleGroup.RemoveRange(i, r);
        }
        #endregion
        #region load and save
        public void LoadDatasetsByTxt(string[] dataname)
        {
            this.name = dataname[1]+"_"+ dataname[2];
            int i = 0;
            particleGroup = new List<Particle>();
            for (int j = 0; j < dataname.Length; j++)
            {
                string fileAddress = (Application.dataPath + "/data/" + dataname[j]);
                FileInfo fInfo0 = new FileInfo(fileAddress);
                string s = "";
                StreamReader r;

                if (fInfo0.Exists)
                {
                    r = new StreamReader(fileAddress);
                }
                else
                {
                    Debug.Log("NO THIS FILE!");
                    return;
                }

                while ((s = r.ReadLine()) != null)
                {
                    string[] words = s.Split(" "[0]);
                    Vector3 xyz = new Vector3(float.Parse(words[0]), float.Parse(words[1]), float.Parse(words[2]));
                    if (j == 0)
                        xyz = 2 * xyz;

                    if (xmin > xyz.x)
                        xmin = xyz.x;
                    if (xmax < xyz.x)
                        xmax = xyz.x;
                    if (ymin > xyz.y)
                        ymin = xyz.y;
                    if (ymax < xyz.y)
                        ymax = xyz.y;
                    if (zmin > xyz.z)
                        zmin = xyz.z;
                    if (zmax < xyz.z)
                        zmax = xyz.z;

                    Color colorRGB;
                    float size;
                    if (i < 306578)
                    {
                        size = 0.03f;
                        colorRGB = MyColor(30f, 144f, 255f, 255f);
                    }
                    else
                    {
                        size = 0.04f;
                        colorRGB = MyColor(30f, 144f, 255f, 255f);
                    }

                    Particle p = new Particle(xyz, colorRGB, size);
                    this.AddParticle(p);
                    i++;
                }
            }
            Debug.Log("Load success.");
        }
        public void LoadDatasetByByte(string path, string dataname)
        {
           this. name=dataname;
            Vector3[] vs = DataPosPreProcessing(LoadDataBybyte.StartLoad(path));
            particleGroup = new List<Particle>();
           Color colorRGB = MyColor(30f, 144f, 255f, 81f);
            float size = 0.04f;
            for (int i=0;i<vs.Length;i++)
            {
                Particle p = new Particle(vs[i], colorRGB, size);
                this.AddParticle(p);
            }
         
        }
        public void LoadDatasetByVec3s(Vector3[] v, string dataname,bool forSimulation=false)
        {
            this.name = dataname;
            if(!forSimulation)
            v = DataPosPreProcessing(v);
            particleGroup = new List<Particle>();
            Color colorRGB = MyColor(30f, 144f, 255f, 81f);
            float size = 0.04f;
            for (int i = 0; i < v.Length; i++)
            {
                Particle p = new Particle(v[i], colorRGB, size);
                this.AddParticle(p);
            }
        }
        public void LoadDatasetByCsv(string path,string dataname)
        {
            this.name = dataname;
            Vector3[] vs = DataPosPreProcessing(csvController.GetInstance().StartLoad(path));
            particleGroup = new List<Particle>();
            Color colorRGB = MyColor(30f, 144f, 255f, 81f);
            float size = 0.04f;
            for (int i = 0; i < vs.Length; i++)
            {
                Particle p = new Particle(vs[i], colorRGB, size);
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
        private Color MyColor(float r, float g, float b, float a)
        {
            Color c = new Color(r / 255f, g / 255f, b / 255f, a / 255);
            return c;
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
        private Color particleColor;
        [SerializeField]
        private float particleSize;
        [SerializeField]
        private double particleDensity;
        [SerializeField]
        private Vector3 my_SmoothLength;
        [SerializeField]
        private bool isSelected;
        [SerializeField]
        private bool isTarget;
        [SerializeField]
        private Vector3 gradiant;
        [SerializeField]
        private Vector3 flowEnd;

        public Particle(Vector3 v, Color c, float f)
        {
            this.particlePosition = v;
            this.particleColor = c;
            this.particleSize = f;
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
        public Color GetColor()
        {
            return particleColor;
        }
        public float GetSize()
        {
            return particleSize;
        }
        public double GetParticleDensity()
        {
            return particleDensity;
        }
        public Vector3 GetMySmoothLength()
        {
            return my_SmoothLength;
        }
        public Color GetParticleColor()
        {
            return particleColor;
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
        public void ChangeColor(Color c)
        {
            particleColor = c;
        }
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
        public void SetParticleColor(Color c)
        {
           particleColor=c;
        }
        public void SetFlag(bool flag)
        {
            isSelected = flag;
        }
        public void SetTarget(bool t)
        {
            isTarget = t;
        }
        public void SetGradient(Vector3 grad)
        {
            gradiant = grad;
        }
        #endregion
    }



   
}





