using System.Collections.Generic;
using UnityEngine;


public static class MeTAPoint 
{
 static List<int> targetIndex;
    public static void SelectMC(Vector3 input,DensityField dF,Particles pG,MarchingCubeGPUCSHelper McGPU)
    {
       
        if (dF.OBJPosClipToGridIndex(input, pG) != -1)
        { 
                targetIndex = dF.FloodFilling(CalculateFocus(pG, dF, input), dF.InterpolateDensity(input), pG);
                McGPU.SetMCFlagTexture(targetIndex);
                McGPU.SetMCGPUThreshold((float)dF.InterpolateDensity(input));
        }
        else
            McGPU.SetMCGPUThreshold(0);
    }
    public static void SelectParticles(Vector3 input, DensityField dF, Particles pG,PointRenderer pR, MarchingCubeGPUCSHelper mcHelper)
    {
       
        if (dF.OBJPosClipToGridIndex(input, pG) != -1)
        {
            dF.FloodFilling(CalculateFocus(pG, dF, input), dF.InterpolateDensity(input), pG, true);
        }
      
        pR.GenerateMesh(true);
        mcHelper.SetMCGPUThreshold(0f);
    }
    
    
    
    private static int CalculateFocus(Particles pG, DensityField dF, Vector3 userInputVec)
    {
        int index = 0;
        Vector3 oldPos = userInputVec;
        Vector3 newPos = userInputVec;
        float step = (dF.XSTEP + dF.YSTEP + dF.ZSTEP) / 3 / 10;
        while (dF.InterpolateDensity(oldPos) <= dF.InterpolateDensity(newPos) && index <= 200)
        {
            Vector3 g = dF.InterpolateGradient(oldPos).normalized * step;
            oldPos = newPos;
            newPos += g;
            index++;
        }

        return dF.OBJPosClipToGridIndex(newPos, pG);
    }
}

