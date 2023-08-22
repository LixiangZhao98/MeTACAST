using ParticleProperty;
using PavelKouril.MarchingCubesGPU;
using ScalarField;
using System.Collections.Generic;
using UnityEngine;

public class Pointbased 
{
 static List<int> targetIndex;
    public static void SelectMC(Vector3 input,DensityField dF,ParticleGroup pG,MarchingCubeGPU McGPU)
    {
       
        if (dF.VectorToBoxIndex(input, pG) != -1)
        { 
           
                targetIndex = Utility.FloodFilling(GetRevisedNodeIndex_Gradient(pG, dF, input), Utility.InterpolateVector(input, pG, dF), dF, pG);
                McGPU.SetMCFlagTexture(targetIndex);
                McGPU.SetMCGPUThreshold((float)Utility.InterpolateVector(input, pG, dF));
            
         
        }
        else
            McGPU.SetMCGPUThreshold(0);
    }
    public static void SelectParticles(Vector3 input, DensityField dF, ParticleGroup pG, MarchingCubeGPU McGPU)
    {
       
        if (dF.VectorToBoxIndex(input, pG) != -1)
        {
           
             Utility.FloodFilling(GetRevisedNodeIndex_Gradient(pG, dF, input), Utility.InterpolateVector(input, pG, dF), dF, pG, true);
            
        }
        else
            McGPU.SetMCGPUThreshold(0);
      
    }
    public static int GetRevisedNodeIndex_Gradient(ParticleGroup pG, DensityField dF, Vector3 userInputVec)
    {
        int index = 0;
        Vector3 oldPos = userInputVec;
        Vector3 newPos = userInputVec;
        float step = (dF.XSTEP + dF.YSTEP + dF.ZSTEP) / 3 / 10;
        while (Utility.InterpolateVector(oldPos, pG, dF) <= Utility.InterpolateVector(newPos, pG, dF) && index <= 200)
        {
            Vector3 g = Utility.InterpolateGradient(oldPos, pG, dF).normalized * step;
            oldPos = newPos;
            newPos += g;
            index++;
        }

        return dF.VectorToBoxIndex(newPos, pG);
    }
}
