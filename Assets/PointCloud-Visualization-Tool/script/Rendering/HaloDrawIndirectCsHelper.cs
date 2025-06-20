using UnityEngine;

public class HaloDrawIndirectCsHelper : MonoBehaviour
{
    public Mesh instanceMesh;
    public Material instanceMaterial;
    private readonly uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    private ComputeBuffer argsBuffer;
    private Camera cam;
    private int instanceCount = 100000;

    private Particles pG;
    private ComputeBuffer positionBuffer;
    private int subMeshIndex;

    private void Update()
    {
        Graphics.DrawMeshInstancedIndirect(instanceMesh, subMeshIndex, instanceMaterial,
            new Bounds(Vector3.zero, new Vector3(1000.0f, 1000.0f, 1000.0f)), argsBuffer);
        instanceMaterial.SetMatrix("_LocalToWorld",
            Matrix4x4.TRS(transform.parent.position, transform.parent.rotation, new Vector3(1f, 1f, 1f)));
    }


    private void LateUpdate()
    {
        instanceMaterial.SetVector("_CamPos",
            new Vector4(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z, 1f));
    }

    private void OnDisable()
    {
        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = null;

        if (argsBuffer != null)
            argsBuffer.Release();
        argsBuffer = null;
    }

    public void HaloDrawIndirectCsHelperInit()
    {
        cam = Camera.main;
        pG = transform.parent.GetComponentInChildren<DataLoader>().particles;
        instanceMaterial = new Material(Shader.Find("Instanced/Halo"));
        Init(pG.GetParticlenum());
    }

    public void Init(int pointcount)
    {
        instanceCount = pointcount;
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        if (instanceMesh != null)
            subMeshIndex = Mathf.Clamp(subMeshIndex, 0, instanceMesh.subMeshCount - 1);
        if (positionBuffer != null)
            positionBuffer.Release();
        positionBuffer = new ComputeBuffer(instanceCount, 16);
        var positions = new Vector4[instanceCount];
        for (var i = 0; i < instanceCount; i++)
        {
            var lp = (float)(pG.GetParticleDensity(i) - pG.MINPARDEN) / (pG.MAXPARDEN - pG.MINPARDEN);
            var worldPos = pG.GetParticleWorldPos(i, transform.parent);
            positions[i] = new Vector4(worldPos.x, worldPos.y, worldPos.z, lp);
        }

        positionBuffer.SetData(positions);
        instanceMaterial.SetBuffer("positionBuffer", positionBuffer);


        if (instanceMesh != null)
        {
            args[0] = instanceMesh.GetIndexCount(subMeshIndex);
            args[1] = (uint)instanceCount;
            args[2] = instanceMesh.GetIndexStart(subMeshIndex);
            args[3] = instanceMesh.GetBaseVertex(subMeshIndex);
        }
        else
        {
            args[0] = args[1] = args[2] = args[3] = 0;
        }

        argsBuffer.SetData(args);
    }
}