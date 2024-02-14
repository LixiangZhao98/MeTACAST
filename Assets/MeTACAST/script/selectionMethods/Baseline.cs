//
//  Baseline.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//
using System.Collections.Generic;
using UnityEngine;

namespace LixaingZhao.MeTACAST{
public class Baseline : MonoBehaviour
{
    public static List<int> SelectParticles(Vector3 input, float R, ParticleGroup pG)
    {
        List<int> selectedparticle = new List<int>();
        for (int i=0;i<pG.GetParticlenum();i++)
        {
            if ((input - pG.GetParticlePosition(i)).magnitude < R / 2&& !pG.GetFlag(i)&&!selectedparticle.Contains(i))
            { selectedparticle.Add(i);
                pG.SetFlag(i,true);
            }
        }
        return selectedparticle;
    }

    public static bool Erase(Vector3 input, float R, ParticleGroup pG)
    {
        
        List<int> selectedparticle = new List<int>();
        List<int> last=DataMemory.GetpStack();
       
       
            for (int i = 0; i < last.Count; i++)
            {
                
                    if ((input - pG.GetParticlePosition(last[i])).magnitude > R / 2)
                    { selectedparticle.Add(last[i]); }
                
              
            }
        if (selectedparticle.Count <last.Count)
        { DataMemory.AddParticlesDirectly(selectedparticle);  return true; }
        else
        {
            selectedparticle = null;
            return false;
        }
          
        
    }
}
}
