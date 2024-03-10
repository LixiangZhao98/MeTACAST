//The following code is modified from "unity-marching-cubes-gpu" by Pavel Kouřil. Original at: "https://github.com/pavelkouril/unity-marching-cubes-gpu"
using System.Collections.Generic;
using UnityEngine;


    public class MarchingCubeGPU : MonoBehaviour
    {
        public float MCGPUThreshold;
        public ComputeShader MarchingCubesCS;
        public Material meshMaterial;
        public GameObject origin;
        public Texture3D DensityTexture { get; set; }
        public Texture3D PosTexture { get; set; }
        public Texture3D McFlagTexture { get; set; }
        Color[] colors_den;
        Color[] colors_pos;
        Color[] colors_McFlag;
        int kernelMC;
        int ResolutionX;
        int ResolutionY;
        int ResolutionZ;
        ComputeBuffer appendVertexBuffer;
        ComputeBuffer argBuffer;
        int[] args;
        Bounds bounds;

        public void Init()
        {
            kernelMC = MarchingCubesCS.FindKernel("MarchingCubes");
            ResolutionX = DataMemory.densityField.XNUM;
            ResolutionY = DataMemory.densityField.YNUM;
            ResolutionZ = DataMemory.densityField.ZNUM;

            SetDensityTexture(DataMemory.densityField);
            SetPosTexture(DataMemory.densityField);
            List<int> a = new List<int>();
            for (int i = 0; i < DataMemory.densityField.GetNodeNum(); i++)
                a.Add(i);
            SetMCFlagTexture(a);

            appendVertexBuffer = new ComputeBuffer((ResolutionX) * (ResolutionY) * (ResolutionZ) * 5, sizeof(float) * 18, ComputeBufferType.Append);
          

            argBuffer = new ComputeBuffer(4, sizeof(int), ComputeBufferType.IndirectArguments);
            meshMaterial.SetPass(0);
            meshMaterial.SetBuffer("triangleRW", appendVertexBuffer);
            MarchingCubesCS.SetBuffer(kernelMC, "triangleRW", appendVertexBuffer);
            MarchingCubesCS.SetInt("_gridSize", ResolutionX);
            SetMCGPUThreshold(0f);
            bounds = new Bounds(Vector3.zero, Vector3.one * 100000);
        }

        private void Update()
        {

            MarchingCubesCS.SetFloat("_isoLevel", MCGPUThreshold);

            appendVertexBuffer.SetCounterValue(0);

            if (MCGPUThreshold!=0)
            {
                MarchingCubesCS.Dispatch(kernelMC, ResolutionX / 8, ResolutionY / 8, ResolutionZ / 8);
            }

            args = new int[] { 0, 1, 0, 0 };
            argBuffer.SetData(args);

            ComputeBuffer.CopyCount(appendVertexBuffer, argBuffer, 0);
 

            argBuffer.GetData(args);

            if (MCGPUThreshold != 0)
            {
                meshMaterial.SetMatrix("_LocalToWorld", origin.transform.localToWorldMatrix);
                meshMaterial.SetMatrix("_WorldToLocal", origin.transform.worldToLocalMatrix);
                Graphics.DrawProcedural(meshMaterial, bounds, MeshTopology.Triangles, args[0] * 3, 1);

            }
   


            args[0] *= 3;
            argBuffer.SetData(args);
          
        }



        struct Vertex_
        {
            public Vector3 point;
            public Vector3 Norm;
        }
        struct Triangle
        {
            public Vertex_ v1;
            public Vertex_ v2;
            public Vertex_ v3;
        };
        private void OnDestroy()
        {
        if (!this.enabled)
            return;
        appendVertexBuffer.Release();
        argBuffer.Release();

    }


        public void SetMCGPUThreshold(float f)
        {
            MCGPUThreshold = f;
        }

        public void SetDensityTexture(DensityField dF)
        {
            DensityTexture = new Texture3D(ResolutionX, ResolutionY, ResolutionZ, TextureFormat.RFloat, false);
            DensityTexture.wrapMode = TextureWrapMode.Clamp;
            colors_den = new Color[ResolutionX * ResolutionY * ResolutionZ];
            for (int i = 0; i < colors_den.Length; i++) colors_den[i] = Color.black;

            var idx = 0;
            for (var z = 0; z < ResolutionZ; z++)
            {
                for (var y = 0; y < ResolutionY; y++)
                {
                    for (var x = 0; x < ResolutionX; x++, idx++)
                    {
                       
                     
                            colors_den[idx].r = (float)dF.GetNodeDensity(idx);
                    }
                }
            }
            DensityTexture.SetPixels(colors_den);
            DensityTexture.Apply();
            MarchingCubesCS.SetTexture(kernelMC, "_densityTexture", DensityTexture);

        }

        public void SetPosTexture(DensityField dF)
        {
            PosTexture = new Texture3D(ResolutionX, ResolutionY, ResolutionZ, TextureFormat.RGBAFloat, false);
            PosTexture.wrapMode = TextureWrapMode.Clamp;
            colors_pos = new Color[ResolutionX * ResolutionY * ResolutionZ];
            for (int i = 0; i < colors_pos.Length; i++) colors_pos[i] = Color.clear;

            var idx = 0;
            for (var z = 0; z < ResolutionZ; z++)
            {
                for (var y = 0; y < ResolutionY; y++)
                {
                    for (var x = 0; x < ResolutionX; x++, idx++)
                    {
                        colors_pos[idx].r = dF.GetNodedPos(idx).x;
                        colors_pos[idx].g = dF.GetNodedPos(idx).y;
                        colors_pos[idx].b = dF.GetNodedPos(idx).z;

                    }
                }
            }
            PosTexture.SetPixels(colors_pos);
            PosTexture.Apply();
            MarchingCubesCS.SetTexture(kernelMC, "_posTexture", PosTexture);

        }
        public void SetMCFlagTexture(List<int> list)
        {
            McFlagTexture = new Texture3D(ResolutionX, ResolutionY, ResolutionZ, TextureFormat.RFloat, false);
            McFlagTexture.wrapMode = TextureWrapMode.Clamp;
            colors_McFlag = new Color[ResolutionX * ResolutionY * ResolutionZ];
            for (int i = 0; i < colors_McFlag.Length; i++) colors_McFlag[i] = Color.black;

            for (var i = 0; i < list.Count; i++)
            {
                colors_McFlag[list[i]].r = 1;
            }
            McFlagTexture.SetPixels(colors_McFlag);
            McFlagTexture.Apply();
            MarchingCubesCS.SetTexture(kernelMC, "_mcFlagTexture", McFlagTexture);
        }

    }
    