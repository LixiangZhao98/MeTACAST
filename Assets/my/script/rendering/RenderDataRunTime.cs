using UnityEngine;

public class RenderDataRunTime : MonoBehaviour
{
    public Material unselected_mat;
    public Material selected_mat;
    public Material target_mat;
    public GameObject map;
    public float mapRealSize=0.5f; //real size in VR of wim
    public float ratio;
    [SerializeField]
    public Mesh unselected_mesh0;
    [SerializeField]
    public Mesh selected_mesh0;
    [SerializeField]
    public Mesh target_mesh0;
      [SerializeField]
    public Mesh unselected_mesh1;
    [SerializeField]
    public Mesh selected_mesh1;
    [SerializeField]
    public Mesh target_mesh1;
    public bool frameBufferSwitch;
     Matrix4x4 m;

     private void Start()
     {
        frameBufferSwitch=false;
     }
    private void Update()
    {

        Draw(map, ratio);
        // Draw(origin, 1f);
    }

    public void GenerateMesh(bool fromStarck=true)
    {
        if(!frameBufferSwitch)
    {
        DestroyImmediate(unselected_mesh0, true);
        DestroyImmediate(selected_mesh0, true);
        DestroyImmediate(target_mesh0, true);
        unselected_mesh0 = new Mesh();
        selected_mesh0 = new Mesh();
        target_mesh0 = new Mesh();
        DisplayParticles.GenerateMeshFromPg( unselected_mesh0, selected_mesh0, target_mesh0, DataMemory.allParticle, fromStarck);
        ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * mapRealSize;
        frameBufferSwitch=true;
    }
    else
    {
        DestroyImmediate(unselected_mesh1, true);
        DestroyImmediate(selected_mesh1, true);
        DestroyImmediate(target_mesh1, true);
        unselected_mesh1 = new Mesh();
        selected_mesh1 = new Mesh();
        target_mesh1 = new Mesh();
        DisplayParticles.GenerateMeshFromPg( unselected_mesh1, selected_mesh1, target_mesh1, DataMemory.allParticle, fromStarck);

        ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * mapRealSize;
         frameBufferSwitch=false;
    }

    }

    public void Draw(GameObject g, float s)
    {
                if(!frameBufferSwitch)
        {
        
            
           m = Matrix4x4.TRS(g.transform.position, g.transform.rotation, new Vector3(s, s, s));
            Graphics.DrawMesh(unselected_mesh1, m, unselected_mat, 1);
            Graphics.DrawMesh(selected_mesh1, m, selected_mat, 1);
             Graphics.DrawMesh(target_mesh1, m, target_mat, 1);
            g.transform.localScale = new Vector3(s, s, s);
        }

        else
        {
                        
           m = Matrix4x4.TRS(g.transform.position, g.transform.rotation, new Vector3(s, s, s));
            Graphics.DrawMesh(unselected_mesh0, m, unselected_mat, 1);
            Graphics.DrawMesh(selected_mesh0, m, selected_mat, 1);
             Graphics.DrawMesh(target_mesh0, m, target_mat, 1);
            g.transform.localScale = new Vector3(s, s, s);
        }
    }
}
