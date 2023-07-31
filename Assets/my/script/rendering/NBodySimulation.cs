using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Kodai
{
    
    public class NBodySimulation : MonoBehaviour, IParticleRenderable
    {
        const int SIMULATION_BLOCK_SIZE = 256;
        const int READ = 0;
        const int WRITE = 1;
        const int DEFAULT_PARTICLE_NUM = 65536;

        [SerializeField] ComputeShader NBodyCS;
        public Material m;

        [SerializeField] int divideLevel = 4;   //


        [SerializeField] float damping = 0.96f;
        [SerializeField] float softeningSquared = 0.1f;

        int kernelnum;
        int numBodies;
        ComputeBuffer[] bodyBuffers;
        Body[] bodyBodies;
        Vector3[] pointCloudBuffer;

        public RenderDataRunTime RD; 
        public void Init()
        {
            kernelnum = NBodyCS.FindKernel("CSMain");
            numBodies = DataMemory.allParticle.GetParticlenum();

            InitBuffer();

            DistributeBodies();
        }
        void Start()
        {
           
            calculate();
        }

        void Update()
        {
       
            calculate();

        }
        void calculate()
        {
            NBodyCS.SetFloat("_DeltaTime", Time.deltaTime);
            NBodyCS.SetFloat("_Damping", damping);
            NBodyCS.SetFloat("_SofteningSquared", softeningSquared);
            NBodyCS.SetInt("_NumBodies", numBodies);

            NBodyCS.SetVector("_ThreadDim", new Vector4(SIMULATION_BLOCK_SIZE, 1, 1, 0));
            NBodyCS.SetInt("_Division", divideLevel);
            NBodyCS.SetVector("_GroupDim", new Vector4(Mathf.CeilToInt(numBodies / SIMULATION_BLOCK_SIZE), 1, 1, 0));

            NBodyCS.SetBuffer(kernelnum, "_BodiesBufferRead", bodyBuffers[READ]);
            NBodyCS.SetBuffer(kernelnum, "_BodiesBufferWrite", bodyBuffers[WRITE]);

            NBodyCS.Dispatch(kernelnum, Mathf.CeilToInt(numBodies / SIMULATION_BLOCK_SIZE), 1, 1);

            Swap(bodyBuffers);

            GetPosAndGenerateMesh();
        }
        void OnDestroy()
        {
            bodyBuffers[READ].Release();
            bodyBuffers[WRITE].Release();
        }

        void InitBuffer()
        {
            bodyBuffers = new ComputeBuffer[2];
            bodyBuffers[READ] = new ComputeBuffer(numBodies, Marshal.SizeOf(typeof(Body)));
            bodyBuffers[WRITE] = new ComputeBuffer(numBodies, Marshal.SizeOf(typeof(Body)));
        }

        void DistributeBodies()
        {
            Body[] bodies = new Body[numBodies];

            int i = 0;
            while (i < numBodies)
            {
                bodies[i].position = DataMemory.allParticle.GetParticlePosition(i);
                bodies[i].velocity = Vector3.zero;
                bodies[i].mass = Random.Range(0.1f, 1.0f);
                i++;
            }

            bodyBuffers[READ].SetData(bodies);
            bodyBuffers[WRITE].SetData(bodies);
          
        }



        void Swap(ComputeBuffer[] buffer)
        {
            ComputeBuffer tmp = buffer[READ];
            buffer[READ] = buffer[WRITE];
            buffer[WRITE] = tmp;
        }



        #region IParticleRenderable

        public int GetParticleNum()
        {
            return numBodies;
        }

        public ComputeBuffer GetParticleBuffer()
        {
            return bodyBuffers[READ];
        }
        public void GetPosAndGenerateMesh()
        {
            bodyBodies=new Body[numBodies];
            bodyBuffers[READ].GetData(bodyBodies);
            pointCloudBuffer = new Vector3[numBodies];
            for (int i=0; i < numBodies; i++)
            { pointCloudBuffer[i] = bodyBodies[i].position; }
 
            DisplayParticles.GenerateMeshByPos(RD. unselected_mesh, RD.selected_mesh,RD.target_mesh ,pointCloudBuffer, DataMemory.allParticle);
            
        }

        public Vector3[] GetPointCloudBuffer()
        {
            return pointCloudBuffer;
        }
        //private void OnRenderObject()
        //{

        //    m.SetPass(0);
        //    m.SetBuffer("_Particles", bodyBuffers[READ]);

        //    Graphics.DrawProceduralNow(MeshTopology.LineStrip, numBodies);



        //}
        #endregion

    }


}