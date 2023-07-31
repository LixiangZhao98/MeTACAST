using ParticleProperty;
using ScalarField;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class DisplayParticles
{
    static int limitPoints = 65000;
    [System.Obsolete]
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
    static public void GenerateMeshByPos( Mesh m_unsel, Mesh m_sel, Mesh m_target, Vector3[] pos, ParticleGroup pG)
    {
        LoadFlagFromStack(pG);
        List<Vector3> unselected = new List<Vector3>();
        List<Vector3> selected = new List<Vector3>();
        List<Vector3> target = new List<Vector3>();
        try
        {
            for (int i = 0; i < pG.GetParticlenum(); i++)
            {
                if (pG.GetFlag(i))
                    selected.Add(pos[i]);
                if (!pG.GetFlag(i))
                {
                    if(pG.GetTarget(i))
                        target.Add(pos[i]);
                    else
                    unselected.Add(pos[i]);
                }
                   
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
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
    //Particle
    static public void DisplayAllParticle(ParticleSystem ps, ParticleGroup allParticle)
    {
        if (allParticle.GetParticlenum() != 0)
        {
            ParticleSystem particleSystem;
            ParticleSystem.Particle[] particleSystemInformation;
            int pointCount = allParticle.GetParticlenum();
            particleSystem = ps;
            particleSystem.startSpeed = 0.0f;
            particleSystem.startLifetime = 10000.0f;
            particleSystemInformation = new ParticleSystem.Particle[pointCount];
            particleSystem.maxParticles = pointCount;
            particleSystem.Emit(pointCount);
            particleSystem.GetParticles(particleSystemInformation);

            for (int i = 0; i < pointCount; i++)
            {
                particleSystemInformation[i].position = allParticle.GetParticlePosition(i);    // 设置每个点的位置
                particleSystemInformation[i].startSize = allParticle.GetParticleSize(i);  /* allParticles[i].startColor = new Color(1f, 0f, 0f);*/
                //  particleSystemInformation[i].startColor = allParticle.GetParticleColor(i);   // 设置每个点的rgb
            }
            particleSystem.SetParticles(particleSystemInformation, pointCount);
        }
        else
            Debug.Log("no data.");
    }
    static public void DisplayNodebyColor(ParticleSystem ps, DensityField dF, Color c, float radius)
    {
        if (dF.GetNodeNum() != 0)
        {
            ParticleSystem particleSystem;
            ParticleSystem.Particle[] particleSystemInformation;
            int pointCount = dF.GetNodeNum();
            particleSystem = ps;
            particleSystem.startSpeed = 0.0f;
            particleSystem.startLifetime = 10000.0f;
            particleSystemInformation = new ParticleSystem.Particle[pointCount];
            particleSystem.maxParticles = pointCount;
            particleSystem.Emit(pointCount);
            particleSystem.GetParticles(particleSystemInformation);

            for (int i = 0; i < pointCount; i++)
            {
                particleSystemInformation[i].position = dF.GetNodedPos(i);    // 设置每个点的位置
                particleSystemInformation[i].startSize = 0.01f + radius * Mathf.Abs(((float)dF.GetNodeDensity(i))); /* allParticles[i].startColor = new Color(1f, 0f, 0f);*/
                particleSystemInformation[i].startColor = c;   // 设置每个点的rgb
            }
            particleSystem.SetParticles(particleSystemInformation, pointCount);
        }
        else
            Debug.Log("no data.");
    }
    // particle by color
    static public void DisplayAllParticlebyColor(ParticleSystem ps, ParticleGroup allParticle, Color c)
    {
        if (allParticle.GetParticlenum() != 0)
        {
            ParticleSystem particleSystem;
            ParticleSystem.Particle[] particleSystemInformation;
            int pointCount = allParticle.GetParticlenum();
            particleSystem = ps;
            particleSystem.startSpeed = 0.0f;
            particleSystem.startLifetime = 10000.0f;
            particleSystemInformation = new ParticleSystem.Particle[pointCount];
            particleSystem.maxParticles = pointCount;
            particleSystem.Emit(pointCount);
            particleSystem.GetParticles(particleSystemInformation);

            for (int i = 0; i < pointCount; i++)
            {
                particleSystemInformation[i].position = allParticle.GetParticlePosition(i);    // 设置每个点的位置
                particleSystemInformation[i].startSize = allParticle.GetParticleSize(i);  /* allParticles[i].startColor = new Color(1f, 0f, 0f);*/
                particleSystemInformation[i].startColor = c;   // 设置每个点的rgb
            }
            particleSystem.SetParticles(particleSystemInformation, pointCount);
        }
        else
            Debug.Log("no data.");
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

    //Mesh

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
    static public void DisplayMesh(GameObject m_mesh, ParticleGroup pG, Vector3 center, float length)  //display partly in cube
    {
        LoadFlagFromStack(pG);
        List<Vector3> unselected = new List<Vector3>();
        List<Vector3> selected = new List<Vector3>();

        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            if (!InViewCube(pG.GetParticlePosition(i), center, length))
                continue;

            if (pG.GetFlag(i))
                selected.Add(pG.GetParticlePosition(i) - center);
            else
                unselected.Add(pG.GetParticlePosition(i) - center);
        }
        DisplayMeshByKind(m_mesh.transform.GetChild(0).gameObject, unselected, 0);
        DisplayMeshByKind(m_mesh.transform.GetChild(1).gameObject, selected, 1);


    }
    static public void DisplayMesh(GameObject m_mesh, ParticleGroup pG, float radius, Vector3 center)  //display a user centered local sphere view
    {
        LoadFlagFromStack(pG);
        List<Vector3> unselected = new List<Vector3>();
        List<Vector3> selected = new List<Vector3>();

        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            if (Vector3.Magnitude(pG.GetParticlePosition(i) - center) > radius)
                continue;

            if (pG.GetFlag(i))
                selected.Add(pG.GetParticlePosition(i) - center);
            else
                unselected.Add(pG.GetParticlePosition(i) - center);
        }
        DisplayMeshByKind(m_mesh.transform.GetChild(0).gameObject, unselected, 0);
        DisplayMeshByKind(m_mesh.transform.GetChild(1).gameObject, selected, 1);


    }
    static public bool InViewCube(Vector3 pos, Vector3 center, float length)
    {
        if (pos.x > (center.x + length / 2))
            return false;
        if (pos.x < (center.x - length / 2))
            return false;
        if (pos.y > (center.y + length / 2))
            return false;
        if (pos.y < (center.y - length / 2))
            return false;
        if (pos.z > (center.z + length / 2))
            return false;
        if (pos.z < (center.z - length / 2))
            return false;
        return true;
    }
    static int[] GetRandomSequence(int total, int count)
    {
        int[] sequence = new int[total];
        int[] output = new int[count];

        for (int i = 0; i < total; i++)
        {
            sequence[i] = i;
        }
        int end = total - 1;
        for (int i = 0; i < count; i++)
        {
            //随机一个数，每随机一次，随机区间-1
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            //将区间最后一个数赋值到取到的数上
            sequence[num] = sequence[end];
            end--;
        }
        return output;
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

    static void InstantiateMesh(int meshInd, int nPoints, GameObject m_mesh, List<Vector3> vs, int matindex)
    {
        GameObject pointGroup = new GameObject("Mesh" + meshInd);
        pointGroup.tag = "mesh";
        pointGroup.AddComponent<MeshFilter>();
        pointGroup.AddComponent<MeshRenderer>();
        pointGroup.transform.parent = m_mesh.transform;
        pointGroup.transform.localPosition = Vector3.zero;
        pointGroup.GetComponent<Renderer>().material = GameObject.Find("material").GetComponent<Renderer>().materials[matindex];
        pointGroup.GetComponent<MeshFilter>().mesh = CreateMesh(meshInd, nPoints, limitPoints, vs);

    }
    static Mesh CreateMesh(int id, int nPoints, int limitPoints, List<Vector3> vs)
    {
        Mesh mesh = new Mesh();
        Vector3[] myPoints = new Vector3[nPoints];
        int[] indecies = new int[nPoints];
        // Color[] myColors = new Color[nPoints];

        for (int i = 0; i < nPoints; ++i)
        {
            myPoints[i] = vs[id * limitPoints + i];
            indecies[i] = i;
            //  myColors[i] =  c;
        }


        mesh.vertices = myPoints;
        //  mesh.colors = myColors;
        mesh.SetIndices(indecies, MeshTopology.Points, 0);
        mesh.uv = new Vector2[nPoints];
        mesh.normals = new Vector3[nPoints];


        return mesh;
    }



    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }
    static public void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
    static void DeleteOldMesh(GameObject mesh)
    {
        for (int i = 0; i < mesh.transform.childCount; i++)
        {
            if (mesh.transform.GetChild(i).CompareTag("mesh"))
                GameObject.Destroy(mesh.transform.GetChild(i).gameObject);

        }
    }
}
