using UnityEngine;

public class RenderDataRunTime : MonoBehaviour
{
    public GameObject visCenter;
    public static float visRealSize = 0.3f; //real size in VR of visualization
    public static float ratio;
    public Material unselected_mat;
    public Material selected_mat;


    [SerializeField]
    [HideInInspector]public static Mesh unselected_mesh;
    [SerializeField]
    [HideInInspector]public static Mesh selected_mesh;
    [SerializeField]
    [HideInInspector] public static Mesh[] target_mesh;

    public static bool drawEnable;
    private RunTimeController RC;
    static int tarMeshNum;
    Matrix4x4 m;

     private void Start()
     {
        drawEnable = false;
     }
    void Awake()
    {
        RC = this.transform.parent.GetComponentInChildren<RunTimeController>();
        tarMeshNum = RC.LoadFlagNames.Count;
        target_mesh = new Mesh[tarMeshNum];
        for (int i = 0; i < tarMeshNum; i++)
            target_mesh[i] = new Mesh();
    }
    private void Update()
    {

        if (drawEnable)
        {
            m = Matrix4x4.TRS(visCenter.transform.position, visCenter.transform.rotation, new Vector3(ratio, ratio, ratio));
            visCenter.transform.localScale = new Vector3(ratio, ratio, ratio);
        }
    }

    public static void GenerateMesh(bool fromStarck = true)
    {

        DestroyImmediate(unselected_mesh, true);
        DestroyImmediate(selected_mesh, true);
        foreach (var m in target_mesh)
            DestroyImmediate(m, true);

        unselected_mesh = new Mesh();
        selected_mesh = new Mesh();
        target_mesh = new Mesh[tarMeshNum];
        for (int i = 0; i < tarMeshNum; i++)
            target_mesh[i] = new Mesh();
        DisplayParticles.GenerateMeshFromPg(unselected_mesh, selected_mesh, target_mesh, DataMemory.allParticle, fromStarck);
        ratio = 1f / (DataMemory.allParticle.XMAX - DataMemory.allParticle.XMIN) * visRealSize;
        drawEnable = true;
    }

    void LateUpdate()
    {

        if (drawEnable)
        {
            Graphics.DrawMesh(unselected_mesh, m, unselected_mat, 1);
            Graphics.DrawMesh(selected_mesh, m, selected_mat, 1);
            for (int i = 0; i < target_mesh.Length; i++)
                Graphics.DrawMesh(target_mesh[i], m, RC.LoadFlagNames[i].target_mat, 1);
        }
    }
    public void SetUnselectedUV1(Vector3[] lp)
    {
        unselected_mesh.SetUVs(1, lp);
    }
}
