using UnityEngine;

public enum FieldType
{
    Electric,
    Gravity,
    SimpleAttractor,
    Vortex,
    Airflow,
    CurvatureField
}

public class ParticleFlow : MonoBehaviour
{
    public FieldType FieldType;
    public Gradient ParticleColourGradient;
    public float ForceMultiplier = 1.0f;
    public int NumAttractors = 5;
    public GameObject AttractorObj;
    public GameObject cylinder;
    private GameObject[] attractors;
    private DensityField dF;
    private readonly float g = 1f;
    private readonly float mass = 2f;
    private Particles pG;
    private ParticleSystem ps;

    // Use this for initialization
    private void Start()
    {
        // initAttractors();
        ps = GetComponent<ParticleSystem>();
        pG = transform.parent.GetComponentInChildren<DataLoader>().particles;
        ;
        dF = transform.parent.GetComponentInChildren<GPUKDECsHelper>().densityField;
    }

    private void Update()
    {
        // add variation to particle colour
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = ParticleColourGradient.Evaluate(Random.Range(0f, 1f));

        main.loop = false; // 关闭循环播放，确保粒子不会自动生成
        ps.Stop();

        for (var i = 0; i < pG.GetParticlenum(); i++) EmitParticle(pG.GetParticleWorldPos(i, transform.parent));
    }

    private void LateUpdate()
    {
        //put particles of the system into array & update them to gravity algorithm
        var particles = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(particles);


        for (var i = 0; i < particles.Length; i++)
        {
            var p = particles[i];
            Vector3 particleWorldPosition;
            if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
                particleWorldPosition = transform.TransformPoint(p.position);
            else if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Custom)
                particleWorldPosition = ps.main.customSimulationSpace.TransformPoint(p.position);
            else
                particleWorldPosition = p.position;

            Vector3 totalForce;

            switch (FieldType)
            {
                case FieldType.SimpleAttractor:
                    totalForce = applySimple(particleWorldPosition);
                    break;
                case FieldType.Gravity:
                    totalForce = applyGravity(particleWorldPosition);
                    break;
                case FieldType.Electric:
                    totalForce = applyElectric(p);
                    break;
                case FieldType.Vortex:
                    totalForce = applyVortex(p);
                    break;
                case FieldType.Airflow:
                    totalForce = applyAirFlow(particleWorldPosition);
                    break;
                case FieldType.CurvatureField:
                    totalForce = applyCurvature(particleWorldPosition);
                    break;
                default:
                    totalForce = applySimple(particleWorldPosition);
                    break;
            }

            if (FieldType == FieldType.Gravity)
                p.velocity += totalForce; //visualise  acceleration
            else
                p.position += totalForce; //visualise velocity

            particles[i] = p;
        }

        ps.SetParticles(particles, particles.Length); //set updated particles into the system
    }

    // potential flow  https://github.com/arkaragian/Fluid-Field/blob/master/field.js
    private Vector3 applyAirFlow(Vector3 particleWorldPosition)
    {
        var direction = Vector3.back * 10;
        var distance = float.MaxValue; // used to find closest attractor
        var maxDistance = 20f;
        var fieldStrength = 10f;

        foreach (var a in attractors)
        {
            distance = Vector3.Distance(particleWorldPosition, a.transform.position);
            if (distance < maxDistance)
            {
                var dx = particleWorldPosition.x - a.transform.position.x;
                var dz = particleWorldPosition.z - a.transform.position.z;

                var angle = Mathf.Atan2(dz, dx);
                var ux = fieldStrength / distance * Mathf.Cos(angle);
                var uz = fieldStrength / distance * Mathf.Sin(angle);

                var falloff = (maxDistance - distance) / distance;
                direction = direction + new Vector3(ux, 0, uz) * falloff;
            }
        }

        var totalForce = direction * ForceMultiplier * Time.deltaTime;
        return totalForce;
    }

    private Vector3 applySimple(Vector3 particleWorldPosition)
    {
        var direction = Vector3.zero;
        var distance = float.MaxValue; // used to find closest attractor

        foreach (var a in attractors)
            if (Vector3.Distance(particleWorldPosition, a.transform.position) < distance)
            {
                distance = Vector3.Distance(particleWorldPosition, a.transform.position);
                direction = (a.transform.position - particleWorldPosition).normalized;
            }

        var totalForce = direction * ForceMultiplier * Time.deltaTime;
        return totalForce;
    }

    /*
     * algo from: https://gamedevelopment.tutsplus.com/tutorials/adding-turbulence-to-a-particle-system--gamedev-13332
     */
    private Vector3 applyVortex(ParticleSystem.Particle p)
    {
        var distanceX = float.MaxValue;
        var distanceY = float.MaxValue;
        var distanceZ = float.MaxValue;
        var distance = float.MaxValue;

        var direction = Vector3.zero;
        foreach (var a in attractors)
        {
            if (Vector3.Distance(p.position, a.transform.localPosition) < distance)
            {
                distanceX = p.position.x - a.transform.localPosition.x;
                distanceY = p.position.y - a.transform.localPosition.y;
                distanceZ = p.position.z - a.transform.localPosition.z;
                distance = Vector3.Distance(p.position, a.transform.localPosition);
            }

            direction += (a.transform.localPosition - p.position).normalized;
        }

        var vortexScale = 10.0f;
        var vortexSpeed = 10.0f;
        var factor = 1 / (1 + (distanceX * distanceX + distanceZ * distanceZ) / vortexScale);

        var vx = distanceX * vortexSpeed * factor;
        var vy = distanceY * vortexSpeed * factor;
        var vz = distanceZ * vortexSpeed * factor;

        var totalForce = Quaternion.AngleAxis(90, Vector3.up) * new Vector3(vx, 0, vz) * ForceMultiplier + direction;
        return totalForce;
    }

    private Vector3 applyGravity(Vector3 particleWorldPosition)
    {
        var direction = Vector3.zero;
        var totalForce = Vector3.zero;
        foreach (var a in attractors)
        {
            direction = (a.transform.position - particleWorldPosition).normalized;
            var magnitude = direction.magnitude;
            Mathf.Clamp(magnitude, 5.0f, 10.0f); //eliminate extreme result for very close or very far objects

            var gforce = g * mass * mass / direction.magnitude * direction.magnitude;
            totalForce += direction * gforce * Time.deltaTime;
        }

        totalForce = totalForce * ForceMultiplier;
        return totalForce;
    }

    private Vector3 applyElectric(ParticleSystem.Particle p)
    {
        var totalForce = Vector3.zero;
        var force = Vector3.zero;
        var i = 0;
        foreach (var a in attractors)
        {
            var dist = Vector3.Distance(p.position, a.transform.position) * 100000;
            var fieldMag = 99999 / dist * dist;
            Mathf.Clamp(fieldMag, 0.0f, 5.0f);

            //alternate postive and negative charges
            if (i % 2 == 0)
            {
                force.x -= fieldMag * (p.position.x - a.transform.position.x) / dist;
                force.y -= fieldMag * (p.position.y - a.transform.position.y) / dist;
                force.z -= fieldMag * (p.position.z - a.transform.position.z) / dist;
            }
            else
            {
                force.x += fieldMag * (p.position.x - a.transform.position.x) / dist;
                force.y += fieldMag * (p.position.y - a.transform.position.y) / dist;
                force.z += fieldMag * (p.position.z - a.transform.position.z) / dist;
            }

            i++;
        }

        totalForce = force * ForceMultiplier;
        return totalForce;
    }

    private Vector3 applyCurvature(Vector3 particleWorldPosition)
    {
        var ratio = 0.1f;
        var totalForce =
            dF.InterpolatePrimaryCurvature(pG.GetObjPosOfVec3(transform.parent, particleWorldPosition))
                .normalized * dF.XSTEP * (1f / pG.GetXScale()) * ratio;
        return totalForce;
    }

    private void initAttractors()
    {
        attractors = new GameObject[NumAttractors];
        for (var i = 0; i < NumAttractors; i++)
        {
            GameObject newAttractor;
            if (AttractorObj == null)
            {
                newAttractor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newAttractor.GetComponent<Renderer>().material.color = Color.white;
            }
            else
            {
                newAttractor = Instantiate(AttractorObj);
            }

            newAttractor.transform.position = new Vector3(
                Random.Range(-4f, 4f),
                0,
                Random.Range(-4f, 4f));
            attractors[i] = newAttractor;
            // newAttractor.transform.parent = GameObject.Find("OBJ").transform;
        }
    }

    private void EmitParticle(Vector3 position)
    {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position; // 设置粒子起始位置
        emitParams.startSize = 0.002f; // 设置粒子大小（可选）
        emitParams.startColor = Color.red; // 设置粒子颜色（可选）
        ps.Emit(emitParams, 1); // 只发射一个粒子
    }
}