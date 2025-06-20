using System.Collections.Generic;
using UnityEngine;

public class MeTAPaint : MonoBehaviour
{
    public static List<Vector3> flowToMax;
    public static void Init()
    {
        flowToMax = new List<Vector3>();
    }

    
    public static void SelectMC(List<Vector3> input, DensityField dF, Particles pG,ref float thre, MarchingCubeGPUCSHelper mcHelper)
    {
        var processedUserDraw = InputProcessor(input, dF, pG);
        flowToMax = new List<Vector3>();
        for (var i = 0; i < processedUserDraw.Count; i++)
        {
            var v = Emit(processedUserDraw[i], Vector3.zero, dF, pG);
            flowToMax.Add(v[v.Count - 1]);
        }

        thre = (float)CalThreshold(processedUserDraw, pG) * 0.6f;

        mcHelper.SetDensityTexture(dF);
        

    }
    public static void AdjustThreStart(PointRenderer pR)
    {
        Selection.Return();
        pR.GenerateMesh();
    }

    public static void AdjustingThre(DensityField dF, Particles pG,MarchingCubeGPUCSHelper mcHelper,float deltaY,ref float thre,ref float thre_init)
    {
        thre -= deltaY;  
        if (thre > 20f)
            thre = 20f;
        if (thre < -20f)
            thre = -20f;
        
        mcHelper.SetMCFlagTexture(MeTAPaint.CalculateFocus(thre_init * Mathf.Pow(2, thre), dF, pG)); 
        mcHelper.SetMCGPUThreshold(thre_init * Mathf.Pow(2, thre));
    }
    
    public static void AdjustThreEnd(DensityField dF, Particles pG,PointRenderer pR,ref float thre,ref float thre_init)
    {
        Selection.AddParticles(SelectParticles(dF, thre_init * Mathf.Pow(2, thre), pG));
        pR.GenerateMesh(true);
    }
    
    public static List<int> SelectParticles(DensityField dF, float thre, Particles pG)
    {
        var targetnodeIndex = CalculateFocus(thre, dF, pG);

        var selectedparticle = new List<int>();
        for (var i = 0; i < targetnodeIndex.Count; i++)
            foreach (var j in dF.GetLUTUnit(targetnodeIndex[i]))
                if (pG.GetParticleDensity(j) > thre)
                    selectedparticle.Add(j);

        return selectedparticle;
    }

    
    
    
    private static bool Contains(List<List<int>> targetBoxes, int index, List<int> seed)
    {
        foreach (var box in targetBoxes)
            if (box.Contains(index))
            {
                seed.Add(index);
                return true;
            }

        return false;
    }



    private static double CalThreshold(List<Vector3> userDraw, Particles pG) 
    {
        double total = 0f;
        var num = 0;
        for (var j = 0; j < pG.GetParticlenum(); j++)
        for (var i = 0; i < userDraw.Count; i++)
            if ((pG.GetParticleObjectPos(j) - userDraw[i]).magnitude < pG.GetSmoothLength().x)
            {
                num++;
                total += pG.GetParticleDensity(j);
                break;
            }

        return total / num;
    }

    public static List<int> CalculateFocus(float thre, DensityField dF, Particles pG)
    {
        var targetBoxes = new List<List<int>>();
        var targetSeed = new List<List<int>>();
        var seed = new List<int>();
        for (var j = 0; j < flowToMax.Count; j++)
        {
            if (Contains(targetBoxes, dF.OBJPosClipToGridIndex(flowToMax[j], pG), seed))
                continue;
            targetBoxes.Add(dF.FloodFilling(dF.OBJPosClipToGridIndex(flowToMax[j], pG), thre, pG));
            targetSeed.Add(seed);
        }


        var countBuffere = 0;
        var componentIndexBuffer = 0;
        ;
        foreach (var s in targetSeed)
            if (countBuffere < s.Count)
            {
                countBuffere = s.Count;
                componentIndexBuffer = targetSeed.IndexOf(s);
            }

        return targetBoxes[componentIndexBuffer];
    }

    private static List<Vector3> InputProcessor(List<Vector3> userDraw, DensityField dF, Particles pG)
    {
        var processedUserDraw = new List<Vector3>();
        for (var j = 0; j < userDraw.Count; j++)
            if (dF.OBJPosClipToGridIndex(userDraw[j], pG) != -1) // point in map
                processedUserDraw.Add(userDraw[j]);

        return processedUserDraw;
    }



    private static List<Vector3> Emit(Vector3 saddle_pos, Vector3 direction, DensityField dF, Particles pG)
    {
        var firststepfactor = 20f;
        var step = (dF.XSTEP + dF.YSTEP + dF.ZSTEP) / 3f / 10f;
        var nodes = new List<Vector3>();
        var index = 0;
        var oldPos = saddle_pos;
        var newPos = saddle_pos + step * firststepfactor * direction.normalized;
        nodes.Add(oldPos);
        nodes.Add(newPos);

        while (dF.InterpolateDensity(oldPos) <= dF.InterpolateDensity(newPos) && index <= 1000)
        {
            var g = dF.InterpolateGradient(oldPos).normalized * step;
            oldPos = newPos;
            newPos += g;
            nodes.Add(newPos);
            index++;
        }

        return nodes;
    }
}