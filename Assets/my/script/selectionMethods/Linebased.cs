//
//  Linebased.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//


using System.Collections.Generic;
using UnityEngine;

namespace LixaingZhao.MeTACAST{
public class Linebased 
{
  static public List<Vector3> maxLine = new List<Vector3>();
    static public void Init()
    {
        maxLine = new List<Vector3>();
    }
    public static void SelectMC( List<Vector3> userDraw, float window_for_saddle_on_max, DensityField dF, DensityField dF2, ref float den_thre, ParticleGroup pG, MarchingCubeGPU McGpu)
    {

        List<Vector3> processedUserDraw = UserDrawPreProcessing(userDraw, dF, pG);
        List<Vector3> flowToMax = new List<Vector3>();
        for (int i = 0; i < processedUserDraw.Count; i++)
        {
            List<Vector3> v = Utility.Emit(processedUserDraw[i], Vector3.zero, dF, pG);
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
                    Vector3 gradientVec = Utility.InterpolateGradient(flowToMax[i], pG, dF).normalized * flowstep;
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
                if ((pG.GetFlowEnd(i) - maxLine[j]).magnitude < window_for_saddle_on_max)
                {
                    num++;
                    averageDen += pG.GetParticleDensity(i);

                    List<int> includedNodes =Utility. GetNodesInArea(pG, dF, 0.5f, i); //������Ϊ0��node
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
        den_thre = (float)averageDen;
    }

    static List<Vector3> UserDrawPreProcessing(List<Vector3> userDraw, DensityField dF, ParticleGroup pG)
    {

        List<Vector3> processedUserDraw = new List<Vector3>();
        for (int j = 0; j < userDraw.Count; j++)
        {
            if (dF.VectorToBoxIndex(userDraw[j], pG) != -1)   // point in map
            {
                processedUserDraw.Add(userDraw[j]);
            }
        }
        return processedUserDraw;
    }

    static public List<int> GetboxIndexesOfComponentsEnclosingMaxLine( float thre,DensityField dF, ParticleGroup pG)
    {
        List <int> targetBoxes=new List<int> ();
        for(int j = 0; j < maxLine.Count; j++)
        {
            if (targetBoxes.Contains(dF.VectorToBoxIndex(maxLine[j], pG)))
                continue;
            targetBoxes.AddRange(Utility.FloodFilling(dF.VectorToBoxIndex(maxLine[j], pG), thre, dF, pG));
        }
        //return Utility. GetExtendedNodes(targetBoxes,dF,pG);
        return targetBoxes;
    }
    public static void SelectParticles(DensityField dF1, DensityField dF2,  float thre, ParticleGroup pG)
    {
        List<int> targetnodeIndex =GetboxIndexesOfComponentsEnclosingMaxLine( thre, dF2,pG);   //ֻ��������maxLine����

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

        DataMemory.AddParticles(selectedparticle);

    }
}
}