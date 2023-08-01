using ParticleProperty;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace ScalarField
{

    [System.Serializable]
    public class FieldNode
    {
        [SerializeField]
        private Vector3 nodePosition;
        [SerializeField]
        private double nodeDensity;
        [SerializeField]
        private double enclosedParticleDis;
        [SerializeField]
        private Vector3 nodeGradient;
        [SerializeField]
        private Vector3 nodeGridPos;
        public FieldNode(Vector3 pos,Vector3 gridPos)
        {
            nodePosition = pos;
            nodeGridPos = gridPos;
        }
        public void SetEnclosedParticleDis(double dis)
        {
            enclosedParticleDis = dis;
        }
        public double GetEnclosedParticleDis()
        {
            return enclosedParticleDis;
        }
        public double GetNodeDensity()
        {
            return nodeDensity;
        }
        public Vector3 GetNodePosition()
        {
            return nodePosition;
        }
        public Vector3 GetNodeGradient()
        {
            return nodeGradient;
        }
        public Vector3 GetNodeGridPos()
        {
            return nodeGridPos;
        }
        public void SetNodeDensity(double density)
        {
            nodeDensity = density;
        }
        public void SetNodeGradient(Vector3 g)
        {
            nodeGradient = g;
        }

        public void NodeDensityPlusDis(double dis)
        {
            enclosedParticleDis = enclosedParticleDis + dis;
        }
    }
    [System.Serializable]
    public class DensityField
    {
        [SerializeField]
        public string name;
        [SerializeField]
        private List<FieldNode> fieldNode;
        [SerializeField]
        private int[] boxDensity;
        [SerializeField]
        private List<LUTUnit> LUT_;
        [SerializeField]
        private int xNum;  //total nodes number on x axis
        [SerializeField]
        private int yNum;
        [SerializeField]
        private int zNum;
        [SerializeField]
        private float xStep;  //distance between two nodes along x axis
        [SerializeField]
        private float yStep;
        [SerializeField]
        private float zStep;
        [SerializeField]
        private float AveNodeDensity;
        public float XSTEP { get { return xStep; } }
        public float YSTEP { get { return yStep; } }
        public float ZSTEP { get { return zStep; } }
        public int XNUM { get { return xNum; } }
        public int YNUM { get { return yNum; } }
        public int ZNUM { get { return zNum; } }

        public float AVE_NODE_DENSITY { get { return AveNodeDensity; } }


        public  int VectorToBoxIndex(Vector3 v, ParticleGroup pG)  //return the box index by inputting a random pos  //can judge the outside pointing,
        {
            int a = (int)((v.x - pG.XMIN) / XSTEP) + (int)((v.y - pG.YMIN) / YSTEP) * XNUM + (int)((v.z - pG.ZMIN) / ZSTEP) * XNUM * YNUM;
            if (a >= GetNodeNum() || a <= 0 || v.x > pG.XMAX || v.x < pG.XMIN || v.y > pG.YMAX || v.y < pG.YMIN || v.z > pG.ZMAX || v.z < pG.ZMIN)
            { return -1; }
            else
                return a;
        }
        public void InitializeDensityFieldByGapDis(string pgName,float xmin, float xmax, int xAxisNum, float ymin, float ymax, int yAxisNum, float zmin, float zmax, int zAxisNum)
        {name = pgName;
            fieldNode = new List<FieldNode>();
            int xindex = 0, yindex = 0, zindex = 0;
            xStep = GetProcessedFloat((xmax - xmin) / xAxisNum);
            yStep = GetProcessedFloat((ymax - ymin) / yAxisNum);
            zStep = GetProcessedFloat((zmax - zmin) / zAxisNum);

            for (float i = zmin; i <= zmax; i += zStep)
            {
                yindex = 0;
                for (float j = ymin; j <= ymax; j += yStep)
                {
                    xindex = 0;
                    for (float k = xmin; k <= xmax; k += xStep)
                    {
                        FieldNode fd = new FieldNode(new Vector3(k, j, i), new Vector3(xindex, yindex, zindex));
                      
                        fieldNode.Add(fd);
                        xindex++;
                    }
                    yindex++;
                }
                zindex++;
            }


            xNum = xindex;
            yNum = yindex;
            zNum = zindex;

            DiscreteClear();

        }
        public float GetProcessedFloat(float f)
        {
            int EffectiveCount = 2;
            string SNumber = f.ToString();
            char[] CNumberArr = SNumber.ToCharArray();
            int DotIndex = SNumber.IndexOf('.');
            double value = 0;
            int TempPrecision = 0;
            for (int i = DotIndex + 1; i < SNumber.Length; i++)
            {
                TempPrecision++;
                if (CNumberArr[i] != '0')
                {
                    value = Math.Round(f, TempPrecision + EffectiveCount - 1);
                    break;
                }
            }
            return (float)value;
        }

       
        public int NodePosToIndex(int z, int y, int x)
        {
            return (z) * xNum * yNum + (y) * xNum + x;

        }

        public void DiscreteClear()
        {
            boxDensity = new int[xNum * yNum * zNum];

            LUT_ = new List<LUTUnit>();
            for (int i = 0; i < xNum*yNum*zNum; i++)
                LUT_.Add(new LUTUnit());

        }

        public void AddToLUT(int index, int targetint)
        {
            LUT_[index].AddToLUT(targetint);
        }

       
        #region Get
        public Vector3 GetNodeGradient(int i)
        {
            return fieldNode[i].GetNodeGradient();
        }
        public Vector3 GetNodedPos(int i)
        {
            return fieldNode[i].GetNodePosition();
        }
        public Vector3 GetNodeGridPos(int i)
        {
            return fieldNode[i].GetNodeGridPos();
        }
        public double GetNodeDensity(int i)
        {
            return fieldNode[i].GetNodeDensity();
        }
 
        public int GetNodeNum()
        {
            return fieldNode.Count;
        }

        public List<int> GetLUTUnit(int index)
        {
            return LUT_[index].GetLTUnit();
        }


        #endregion

        #region Set
        public void SetNodeDensity(int i, double density)
        {
            fieldNode[i].SetNodeDensity(density);
        }
        public void SetNodeGradient(int i, Vector3 g)
        {
            fieldNode[i].SetNodeGradient(g);
        }
        public void SetAveNodeDensity(float f)
        {
            AveNodeDensity = f;
        }

        #endregion
    }

    [System.Serializable]
    public class LUTUnit
    {[SerializeField]
        List<int> LUTUnit_;
        public LUTUnit()
        {
            LUTUnit_ = new List<int>();
        }
        public void AddToLUT(int targetint)
        {
            LUTUnit_.Add(targetint);
        }

        public List<int> GetLTUnit()
        {
            return LUTUnit_;
        }
    }
}
