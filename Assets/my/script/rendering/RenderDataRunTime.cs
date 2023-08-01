using UnityEngine;

public class RenderDataRunTime : MonoBehaviour
{
    public Material unselected_mat;
    public Material selected_mat;
    public Material target_mat;
    public GameObject map;
    public float mapRealSize=0.5f; //real size in VR of the map in the hand (0.1m is the 0.1m in VR world)
    public float ratio;
    [SerializeField]
    public Mesh unselected_mesh;
    [SerializeField]
    public Mesh selected_mesh;
    [SerializeField]
    public Mesh target_mesh;
    private void Update()
    {

        Draw(map, ratio);
        // Draw(origin, 1f);
    }

    public void GenerateMesh(bool fromStarck=true)
    {
        DestroyImmediate(unselected_mesh, true);
        DestroyImmediate(selected_mesh, true);
        DestroyImmediate(target_mesh, true);
        unselected_mesh = new Mesh();
        selected_mesh = new Mesh();
        target_mesh = new Mesh();
        DisplayParticles.GenerateMeshFromPg( unselected_mesh, selected_mesh, target_mesh, DataMemory.allParticle, fromStarck);

        ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * mapRealSize;
    }

    public void Draw(GameObject g, float s)
    {
        
        {
            
            Matrix4x4 m = Matrix4x4.TRS(g.transform.position, g.transform.rotation, new Vector3(s, s, s));
            Graphics.DrawMesh(unselected_mesh, m, unselected_mat, 1);
            Graphics.DrawMesh(selected_mesh, m, selected_mat, 1);
             Graphics.DrawMesh(target_mesh, m, target_mat, 1);
            g.transform.localScale = new Vector3(s, s, s);
        }
    }
}
