//
//  Structurebased.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//


using System.Collections.Generic;
using UnityEngine;


public class Structurebased : MonoBehaviour
{
   static public List<Vector3> flowToMax = new List<Vector3>();
    static public void Init()
    { 
        flowToMax = new List<Vector3>();
    }
    static public bool Contains(List<List<int>> targetBoxes,int index,List<int> seed)
    {
        foreach (var box in targetBoxes)
        {

            if (box.Contains(index))
            {
                seed.Add(index);
                return true; }
        }
        seed=new List<int>();
        return false;
    }
    public static void SelectMC(List<Vector3> userDraw, DensityField dF, ref float den_thre, ParticleGroup pG, MarchingCubeGPU McGpu)
    {
        List<Vector3> processedUserDraw = UserDrawPreProcessing(userDraw, dF, pG);
        flowToMax = new List<Vector3>();
        for (int i = 0; i < processedUserDraw.Count; i++)
        {
            List<Vector3> v = Utility.Emit(processedUserDraw[i], Vector3.zero, dF, pG);
            flowToMax.Add(v[v.Count - 1]);
        }
        den_thre = (float)GetThreshodbyParticles(processedUserDraw, pG)*0.6f ;
     
        

        McGpu.SetDensityTexture(dF);
     
      
    }
    static double GetThreshodbyParticles(List<Vector3> userDraw, ParticleGroup pG)  //by the ave density of input
    {

        double total = 0f;
        int num = 0;
        for (int j = 0; j < pG.GetParticlenum(); j++)
        {
            for (int i = 0; i < userDraw.Count; i++)
            {
                if ((pG.GetParticlePosition(j) - userDraw[i]).magnitude < pG.GetSmoothLength().x)
                {
                    num++;
                    total += pG.GetParticleDensity(j);
                    break;
                }
            }
        }
        return total/num;
    }

        static public List<int> GetboxIndexesOfComponentsByMaxNumSeed(float thre, DensityField dF, ParticleGroup pG)
    {
        List<List<int>> targetBoxes = new List<List<int>>();
        List<List<int>> targetSeed = new List<List<int>>();
        List<int> seed = new List<int>();
        for (int j = 0; j < flowToMax.Count; j++)
        {

            if (Contains(targetBoxes, dF.VectorToBoxIndex(flowToMax[j], pG), seed))
                continue;
            targetBoxes.Add(Utility.FloodFilling(dF.VectorToBoxIndex(flowToMax[j], pG), thre, dF, pG));
            targetSeed.Add(seed);
        }

  

        int countBuffere = 0;
        int componentIndexBuffer = 0; ;
        foreach (var s in targetSeed)
        {
            if (countBuffere < s.Count)
            {
                countBuffere = s.Count;
                componentIndexBuffer = targetSeed.IndexOf(s);
            }
        }
        return targetBoxes[componentIndexBuffer];
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

    public static void SelectParticles(DensityField dF,  float thre, ParticleGroup pG)
    {
        List<int> targetnodeIndex = GetboxIndexesOfComponentsByMaxNumSeed(thre, dF, pG);   

        List<int> selectedparticle = new List<int>();
        for (int i = 0; i < targetnodeIndex.Count; i++)
        {
            foreach (var j in dF.GetLUTUnit(targetnodeIndex[i]))
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
