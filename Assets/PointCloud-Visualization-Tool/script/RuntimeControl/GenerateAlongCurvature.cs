
using UnityEngine;

public class GenerateAlongCurvature : MonoBehaviour{

    Particles pG;
    public GameObject cylinder;

    public void Generate()
    {        
        pG = this.transform.parent.GetComponentInChildren<DataLoader>().particles;
        for (int i = 0; i < pG.GetParticlenum(); i++)
        {
            Vector4 v=pG.GetParticleWorldPos(i,this.transform.parent);
            Instantiate(cylinder, new Vector3(v.x, v.y, v.z),Quaternion.identity).transform.up=pG.GetParticlePrimaryCurvature(i).normalized;
        }
        
    }
    

}
