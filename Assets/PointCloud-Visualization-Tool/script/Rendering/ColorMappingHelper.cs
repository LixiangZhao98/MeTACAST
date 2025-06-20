using UnityEngine;

public class ColorMappingHelper : MonoBehaviour
{
    private Particles pG;

    public void ColorMappingHelperInit()
    {
        pG = transform.parent.GetComponentInChildren<DataLoader>().particles;
        transform.parent.name += "_Color";
        var lp = new Vector3[pG.GetParticlenum()];
        for (var i = 0; i < pG.GetParticlenum(); i++)
            lp[i] = new Vector3((float)(pG.GetParticleDensity(i) - pG.MINPARDEN) / (pG.MAXPARDEN - pG.MINPARDEN), 0f,
                0f);
        transform.parent.GetComponentInChildren<PointRenderer>().SetUnselectedUV1(lp);
    }
}