//
//  Baseline.cs
//  MeTACAST
//
//  Copyright (c) 2022, 2023 Lixiang Zhao. All rights reserved.
//
using System.Collections.Generic;
using UnityEngine;


public class Baseline : MonoBehaviour
{
    public static List<int> SelectParticles(Vector3 input, float R, Particles pG)
    {
        List<int> selectedparticle = new List<int>();
        for (int i=0;i<pG.GetParticlenum();i++)
        {
            if ((input - pG.GetParticleObjectPos(i)).magnitude < R / 2&& !pG.GetIsSelected(i)&&!selectedparticle.Contains(i))
            { selectedparticle.Add(i);
                pG.SetIsSelected(i,true);
            }
        }
        return selectedparticle;
    }

    public static bool Erase(Vector3 input, float R, Particles pG)
    {
        
        List<int> selectedparticle = new List<int>();
        List<int> last=Selection.GetpStack();
       
       
            for (int i = 0; i < last.Count; i++)
            {
                
                    if ((input - pG.GetParticleObjectPos(last[i])).magnitude > R / 2)
                    { selectedparticle.Add(last[i]); }
                
              
            }
        if (selectedparticle.Count <last.Count)
        { Selection.AddParticlesDirectly(selectedparticle);  return true; }
        else
        {
            selectedparticle = null;
            return false;
        }
          
        
    }

}
