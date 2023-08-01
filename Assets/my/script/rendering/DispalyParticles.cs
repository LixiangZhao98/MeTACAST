using ParticleProperty;
using System.Collections.Generic;
using UnityEngine;

 public class DisplayParticles
{


    // mesh for GPU
    static public void GenerateMeshFromPg( Mesh m_unsel, Mesh m_sel,Mesh m_target, ParticleGroup pG, bool loadflagFormStack=true)
    {
        if(loadflagFormStack)
        LoadFlagFromStack(pG);
        List<Vector3> unselected = new List<Vector3>();
        List<Vector3> selected = new List<Vector3>();
        List<Vector3> target = new List<Vector3>();
        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            if (pG.GetFlag(i))
                selected.Add(pG.GetParticlePosition(i));
            if (!pG.GetFlag(i))
            {
                if(pG.GetTarget(i))
                    target.Add(pG.GetParticlePosition(i));
                else
                    unselected.Add(pG.GetParticlePosition(i));
            }
               
        }

        m_unsel.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        int[] indecies = new int[unselected.Count];

        for (int i = 0; i < unselected.Count; ++i)
        {

            indecies[i] = i;

        }
        m_unsel.vertices = unselected.ToArray();

        m_unsel.SetIndices(indecies, MeshTopology.Points, 0);

        m_sel.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
         indecies = new int[selected.Count];

        for (int i = 0; i < selected.Count; ++i)
        {

            indecies[i] = i;

        }
        m_sel.vertices = selected.ToArray();

        m_sel.SetIndices(indecies, MeshTopology.Points, 0);


        m_target.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        indecies = new int[target.Count];

        for (int i = 0; i < target.Count; ++i)
        {

            indecies[i] = i;

        }
        m_target.vertices = target.ToArray();

        m_target.SetIndices(indecies, MeshTopology.Points, 0);

    }
   
    
    
    static public void LoadFlagFromStack(ParticleGroup pG)
    {
        for (int i = 0; i < pG.GetParticlenum(); i++)   //clear flags  //  initialize the flags
        {
            pG.SetFlag(i, false);
        }

       
            List<int> pStack = DataMemory.GetpStack();
            for (int i = 0; i < pStack.Count; i++)                 // load flag
            {
                pG.SetFlag(pStack[i], true);
            }
        
      
      
     

    }



    static public void DisplayMesh(GameObject m_mesh, ParticleGroup pG, bool deleteOld = true)
    {
        LoadFlagFromStack(pG);
        List<Vector3> unselected = new List<Vector3>();
        List<Vector3> selected = new List<Vector3>();
        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            if (pG.GetFlag(i))
                selected.Add(pG.GetParticlePosition(i));
            if (!pG.GetFlag(i))
                unselected.Add(pG.GetParticlePosition(i));
        }
        DisplayMeshByKind(m_mesh.transform.GetChild(0).gameObject, unselected, 0);
        DisplayMeshByKind(m_mesh.transform.GetChild(1).gameObject, selected, 1);

    }
    
   
   

    static public void DisplayMeshByKind(GameObject m_mesh, List<Vector3> vs, int matindex)  //selected or unselected
    {




        m_mesh.transform.localPosition = Vector3.zero;
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        int[] indecies = new int[vs.Count];

        for (int i = 0; i < vs.Count; ++i)
        {

            indecies[i] = i;

        }
        mesh.vertices = vs.ToArray();

        mesh.SetIndices(indecies, MeshTopology.Points, 0);

        m_mesh.GetComponent<MeshFilter>().mesh = mesh;

    }

    
 
}
