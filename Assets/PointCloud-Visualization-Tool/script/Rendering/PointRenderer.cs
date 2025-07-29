using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class PointRenderer : MonoBehaviour
{
    public Material unselected_mat;
    public Material selected_mat;

    public bool loadHighlight;
    [SerializeField] public List<HighLightCollection> highLightCollections = new();

    private Matrix4x4 objToWorldMat;

    private Particles pG;
    private Mesh selected_mesh;
    private Material[] target_mat;
    private Mesh[] target_mesh;
    private Mesh unselected_mesh;

    private void LateUpdate()
    {
        objToWorldMat = pG.GetObjToWorldMatrix(transform.parent);
        Graphics.DrawMesh(unselected_mesh, objToWorldMat, unselected_mat, 1);
        if (selected_mat != null)
            Graphics.DrawMesh(selected_mesh, objToWorldMat, selected_mat, 1);
        if (target_mesh.Length != 0)
            for (var i = 0; i < target_mesh.Length; i++)
                Graphics.DrawMesh(target_mesh[i], objToWorldMat, target_mat[i], 1);
    }


    public void Init()
    {
        pG = transform.parent.GetComponentInChildren<DataLoader>().particles;

        if (highLightCollections.Count != 0 && loadHighlight) //load target points with highlighted color
            LoadHighLights(highLightCollections);

        if (highLightCollections == null)
        {
            target_mat = new Material[0];
        }
        else
        {
            target_mat = new Material[highLightCollections.Count];
            for (var i = 0; i < highLightCollections.Count; i++)
                target_mat[i] = highLightCollections[i].mat;
        }

        GenerateMesh();
    }


    #region GenerateMesh

    public void GenerateMesh()
    {
        if (unselected_mesh != null)
            DestroyImmediate(unselected_mesh, true);
        if (selected_mesh != null)
            DestroyImmediate(selected_mesh, true);
        if (target_mesh != null)
            foreach (var m in target_mesh)
                DestroyImmediate(m, true);

        unselected_mesh = new Mesh();
        selected_mesh = new Mesh();
        if (target_mesh != null)
        {
            target_mesh = new Mesh[target_mat.Length];
            for (var i = 0; i < target_mesh.Length; i++)
                target_mesh[i] = new Mesh();
        }
        else
        {
            target_mesh = new Mesh[0];
        }

        GenerateMeshFromPg(unselected_mesh, selected_mesh, target_mesh, pG);
    }

    public void GenerateMeshFromPg(Mesh m_unsel, Mesh m_sel, Mesh[] m_targets, Particles pG)
    {

        var unselected = new List<Vector3>();
        var selected = new List<Vector3>();
        var targets = new List<List<Vector3>>();

        for (var i = 0; i < m_targets.Length; i++) targets.Add(new List<Vector3>());

        for (var i = 0; i < pG.GetParticlenum(); i++)
        {
            if (pG.GetIsSelected(i))
                selected.Add(pG.GetParticleObjectPos(i));
            if (!pG.GetIsSelected(i))
            {
                if (pG.GetTarget(i))
                    targets[pG.GetTargetType(i)].Add(pG.GetParticleObjectPos(i));
                else
                    unselected.Add(pG.GetParticleObjectPos(i));
            }
        }

        m_unsel.indexFormat = IndexFormat.UInt32;
        var indecies = new int[unselected.Count];

        for (var i = 0; i < unselected.Count; ++i) indecies[i] = i;

        m_unsel.vertices = unselected.ToArray();

        m_unsel.SetIndices(indecies, MeshTopology.Points, 0);

        if (m_sel != null)
        {
            m_sel.indexFormat = IndexFormat.UInt32;
            indecies = new int[selected.Count];

            for (var i = 0; i < selected.Count; ++i) indecies[i] = i;

            m_sel.vertices = selected.ToArray();

            m_sel.SetIndices(indecies, MeshTopology.Points, 0);
        }


        for (var n = 0; n < m_targets.Length; n++)
        {
            m_targets[n].indexFormat = IndexFormat.UInt32;
            indecies = new int[targets[n].Count];

            for (var i = 0; i < targets[n].Count; ++i) indecies[i] = i;

            m_targets[n].vertices = targets[n].ToArray();

            m_targets[n].SetIndices(indecies, MeshTopology.Points, 0);
        }
    }
    

    public void LoadHighLights(List<HighLightCollection> highLightCollections)
    {
        foreach (var highLightCollection in highLightCollections)
            for (var n = 0; n < highLightCollection.Names.Length; n++)
            {
                var flags = LoadPointCloud.LoadInt(Application.dataPath + "/PointCloud-Visualization-Tool/data/flags/" +
                                                   pG.name + "_" + highLightCollection.Names[n]);
                for (var i = 0; i < flags.Length; i++)
                    pG.SetTarget(flags[i], true, highLightCollections.IndexOf(highLightCollection));
            }
    }

    public void SetUnselectedUV1(Vector3[] lp)
    {
        unselected_mesh.SetUVs(1, lp);
    }

    # endregion
}