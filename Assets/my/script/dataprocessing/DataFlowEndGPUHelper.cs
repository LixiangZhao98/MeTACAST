using ParticleProperty;
using ScalarField;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DataFlowEndGPUHelper 
{
    static ComputeShader FlowEnd_Cs;
    static ComputeBuffer parPos;
    static ComputeBuffer flowEnd;
    static ComputeBuffer gradient;
    static public void StartFlowEndCalculation(ParticleGroup pG, DensityField dF, ComputeShader cs)
    {
        FlowEnd_Cs = cs;
        parPos = new ComputeBuffer(pG.GetParticlenum(), 3 * sizeof(float), ComputeBufferType.Default);
        flowEnd = new ComputeBuffer(pG.GetParticlenum(), 3 * sizeof(float), ComputeBufferType.Default);
        gradient = new ComputeBuffer(dF.GetNodeNum(), 3 * sizeof(float), ComputeBufferType.Default);

        int kernel_ParticleFlowEnd = FlowEnd_Cs.FindKernel("ParticleFlowEnd");

        FlowEnd_Cs.SetVector("gridMinPos", new Vector4(pG.XMIN,pG.YMIN,pG.ZMIN, 0f));
        FlowEnd_Cs.SetVector("gridStep", new Vector4(dF.XSTEP, dF.YSTEP, dF.ZSTEP, 0f));
        FlowEnd_Cs.SetVector("gridNum", new Vector4(dF.XNUM, dF.YNUM, dF.ZNUM, 0f));
        FlowEnd_Cs.SetVector("parNum", new Vector4(pG.GetParticlenum(),0f,0f, 0f));
        Vector3[] parPos_ = new Vector3[pG.GetParticlenum()];
        Vector3[] flowEnd_ = new Vector3[pG.GetParticlenum()];
        Vector3[] gradient_ = new Vector3[dF.GetNodeNum()];
        FlowEnd_Cs.SetBuffer(kernel_ParticleFlowEnd, "ParPos", parPos);
        FlowEnd_Cs.SetBuffer(kernel_ParticleFlowEnd, "flowEnd", flowEnd);
        FlowEnd_Cs.SetBuffer(kernel_ParticleFlowEnd, "Gradient", gradient);
        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            parPos_[i] = pG.GetParticlePosition(i);
        }
        for (int i = 0; i < dF.GetNodeNum(); i++)
        {
            gradient_[i] = dF.GetNodeGradient(i);
        }
        
        parPos.SetData(parPos_);
        gradient.SetData(gradient_);
        flowEnd.SetData(flowEnd_);

        

        FlowEnd_Cs.Dispatch(kernel_ParticleFlowEnd,32 / 8, 32/8, pG.GetParticlenum() / 1024/8);  //pilot density

        Vector3[] flowEnd_out = new Vector3[pG.GetParticlenum()];
        flowEnd.GetData(flowEnd_out);
        Debug.Log(flowEnd_out[6]);
        Parallel.For(0, pG.GetParticlenum(), i =>
        {
            pG.SetFlowEnd(i, flowEnd_out[i]);
        });
        }
}
