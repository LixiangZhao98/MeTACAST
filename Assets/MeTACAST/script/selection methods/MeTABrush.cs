using System.Collections.Generic;
using UnityEngine;

public class MeTABrush 
{
  static public List<Vector3> maxLine;
    static public void Init()
    {
        maxLine = new List<Vector3>();
    }
    public static void SelectMC( List<Vector3> inputs, float windowSize, DensityField dF, DensityField dF2, ref float thre, Particles pG, MarchingCubeGPUCSHelper McGpu)
    {
        List<Vector3> processedInputs = InputProcessor(inputs, dF, pG);
        List<Vector3> flowToMax = new List<Vector3>();
        for (int i = 0; i < processedInputs.Count; i++)
        {
            List<Vector3> v = dF.Emit(processedInputs[i], Vector3.zero);
            flowToMax.Add(v[v.Count - 1]);
        }

        float flowstep = 0.2f * dF.XSTEP;
        float targetstep = 0.4f * dF.XSTEP;
       
        for (int i = 0; i < flowToMax.Count - 1; i++)
        {
            maxLine.Add(flowToMax[i]);
            if ((flowToMax[i] - flowToMax[i + 1]).magnitude > dF.XSTEP * 0.5f)
            {
                Vector3 newPos = flowToMax[i];
                while ((newPos - flowToMax[i + 1]).magnitude > dF.XSTEP * 0.5f)
                {
                    Vector3 gradientVec = dF.InterpolateGradient(flowToMax[i]).normalized * flowstep;
                    Vector3 targetVec = (flowToMax[i + 1] - newPos).normalized * targetstep;
                    Vector3 direction = gradientVec + targetVec;
                    newPos += direction;
                    maxLine.Add(newPos);
                }
            }
        }

        for (int i = 0; i < dF2.GetNodeNum(); i++)
        {
            if (dF2.GetNodeDensity(i) != 0)
            {
                dF2.SetNodeDensity(i, 0f);
            }
        }

        int num = 0; double averageDen = 0f;
        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            for (int j = 0; j < maxLine.Count; j++)
            {

                if ((pG.GetFlowEnd(i) - maxLine[j]).magnitude < windowSize)
                {
                    num++;

                    averageDen += pG.GetParticleDensity(i);

                    List<int> includedNodes =GetNodesInElipsoid(pG, dF, 0.5f, i); 
                    foreach (var inc in includedNodes)
                    {
                        if (dF2.GetNodeDensity(inc) == 0)
                        { dF2.SetNodeDensity(inc, dF.GetNodeDensity(inc)); }
                    }

                    break;
                }
            }
        }

        averageDen /= num;
        averageDen *= 0.2f;
        McGpu.SetDensityTexture(dF2);
        thre = (float)averageDen;
    }

    static List<Vector3> InputProcessor(List<Vector3> userDraw, DensityField dF, Particles pG)
    {

        List<Vector3> processedUserDraw = new List<Vector3>();
        for (int j = 0; j < userDraw.Count; j++)
        {
            if (dF.OBJPosClipToGridIndex(userDraw[j], pG) != -1)   
            {
                processedUserDraw.Add(userDraw[j]);
            }
        }
        return processedUserDraw;
    }

    static public List<int> GetGridIndexesEnclosingMaxLine( float thre,DensityField dF, Particles pG)
    {
        List <int> targetBoxes=new List<int> ();
        for(int j = 0; j < maxLine.Count; j++)
        {
            if (targetBoxes.Contains(dF.OBJPosClipToGridIndex(maxLine[j], pG)))
                continue;
            targetBoxes.AddRange(dF.FloodFilling(dF.OBJPosClipToGridIndex(maxLine[j], pG), thre, pG));
        }
        //return Utility. GetExtendedNodes(targetBoxes,dF,pG);
        return targetBoxes;
    }
    public static List<int> SelectParticles(DensityField dF1, DensityField dF2,  float thre, Particles pG)
    {
        List<int> targetnodeIndex =GetGridIndexesEnclosingMaxLine( thre, dF2,pG);   

        List<int> selectedparticle = new List<int>();
        for (int i=0;i<targetnodeIndex.Count;i++)
        {
            foreach (var j in dF1.GetLUTUnit(targetnodeIndex[i]))
            {
                if (pG.GetParticleDensity(j) > thre)
                {
                    selectedparticle.Add(j);

                }
            }
        }
return selectedparticle;

    }
    
    
     public static List<int> GetNodesInElipsoid(Particles pG, DensityField dF, float dRatio, int parIndex)  //Input by particle position, return the nodes in elipsoid shape
    {
        Vector3 nodePos = pG.GetParticleObjectPos(parIndex);
        Vector3 sL = pG.GetMySmoothLength(parIndex);
        List<int> lint = new List<int>();
        float sLx = sL[0] * dRatio;
        float sLy = sL[1] * dRatio;
        float sLz = sL[2] * dRatio;
        float xmax, xmin, ymax, ymin, zmax, zmin; 
        xmax = nodePos.x + sLx;
        xmin = nodePos.x - sLx;
        ymax = nodePos.y + sLy;
        ymin = nodePos.y - sLy;
        zmax = nodePos.z + sLz;
        zmin = nodePos.z - sLz;

        xmax = xmax - pG.XMIN;  
        xmin = xmin - pG.XMIN;
        ymax = ymax - pG.YMIN;
        ymin = ymin - pG.YMIN;
        zmax = zmax - pG.ZMIN;
        zmin = zmin - pG.ZMIN;
        if (xmax > pG.XMAX - pG.XMIN)
            xmax = pG.XMAX - pG.XMIN - 1;
        if (ymax > pG.YMAX - pG.YMIN)
            ymax = pG.YMAX - pG.XMIN - 1;
        if (zmax > pG.ZMAX - pG.ZMIN)
            zmax = pG.ZMAX - pG.XMIN - 1;
        if (xmin < 0)
            xmin = 1;
        if (ymin < 0)
            ymin = 1;
        if (zmin < 0)
            zmin = 1;

        List<int> Lx = InternalNodes(xmin, xmax, dF.XSTEP);
        List<int> Ly = InternalNodes(ymin, ymax, dF.YSTEP);
        List<int> Lz = InternalNodes(zmin, zmax, dF.ZSTEP);
        if (Lx.Count == 0 || Ly.Count == 0 || Lz.Count == 0)
            return lint;
        else
        {

            foreach (var x in Lx)
            {
                foreach (var y in Ly)
                {
                    foreach (var z in Lz)
                    {
                        int index = dF.NodeGridPosToIndex(z, y, x);

                        Vector3 v = nodePos - dF.GetNodedPos(index);
                        v.x = Mathf.Abs(v.x / sLx);
                        v.y = Mathf.Abs(v.y / sLy);
                        v.z = Mathf.Abs(v.z / sLz);
                        double dis = 1 - Vector3.Dot(v, v);
                        if (dis > 0)
                            lint.Add(index);

                    }
                }
            }

            return lint;

        }

    }

  
    static List<int> InternalNodes(float min, float max, float step)
    {
        List<int> L = new List<int>();
        int low_boundary = Mathf.CeilToInt(min / step);
        int high_boundary = Mathf.FloorToInt(max / step);
        if (high_boundary >= low_boundary)
        {
            for (int i = low_boundary; i <= high_boundary; i++)
            {
                L.Add(i);
            }
        }
        return L;
    }
}
