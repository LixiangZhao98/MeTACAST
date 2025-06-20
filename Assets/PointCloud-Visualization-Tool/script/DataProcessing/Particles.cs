using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Particles
{
    #region Variables
    [SerializeField]
    public string name;
    private List<Particle> particles;
    [SerializeField]
    private float xmin, ymin, zmin;
    [SerializeField]
    private float xmax, ymax, zmax;
    private float maxParDen;
    private float minParDen;
    private float aveParDen;
    [SerializeField]
    Vector3 smoothLength;
    
    public float XMIN { get { return xmin; } set { xmin = value; } }
    public float XMAX { get { return xmax; } set { xmax = value; } }
    public float YMIN { get { return ymin; } set { ymin = value; } }
    public float YMAX { get { return ymax; } set { ymax = value; } }
    public float ZMIN { get { return zmin; } set { zmin = value; } }
    public float ZMAX { get { return zmax; } set { zmax = value; } }

    public float MAXPARDEN { get { return maxParDen; } set { maxParDen = value; } }
    public float MINPARDEN { get { return minParDen; } set { minParDen = value; } }
    public float AVEPARDEN { get { return aveParDen; } set { aveParDen = value; } }
    #endregion
    
    #region Get Properties
    /// <summary>
    /// Gets the minimum particle position in the group
    /// </summary>
    /// <returns>Vector3 representing the minimum position</returns>
    public Vector3 GetMinObjPos()
    { 
        return new Vector3(xmin, ymin, zmin); 
    }

    /// <summary>
    /// Gets the maximum particle position in the group
    /// </summary>
    /// <returns>Vector3 representing the maximum position</returns>
    public Vector3 GetMaxObjPos()
    {
        return new Vector3(xmax, ymax, zmax);
    }

    /// <summary>
    /// Gets the scale along the X axis
    /// </summary>
    /// <returns>X scale value</returns>
    public float GetXScale()
    {
        return xmax - xmin;
    }

    /// <summary>
    /// Gets the scale along the Y axis
    /// </summary>
    /// <returns>Y scale value</returns>
    public float GetYScale()
    {
        return ymax - ymin;
    }

    /// <summary>
    /// Gets the scale along the Z axis
    /// </summary>
    /// <returns>Z scale value</returns>
    public float GetZScale()
    {
        return zmax - zmin;
    }

    /// <summary>
    /// Gets the center position of the particle group
    /// </summary>
    /// <returns>Vector3 representing the center position</returns>
    public Vector3 GetCenter()
    {
        return GetMinObjPos() + GetMaxObjPos() / 2;
    }

    /// <summary>
    /// Gets the longest axis scale of the particle group
    /// </summary>
    /// <returns>The length of the longest axis</returns>
    public float GetLongestAxisScale()
    {
        float max;
        if (GetXScale() >= GetYScale())
            max = GetXScale();
        else
            max = GetYScale();

        if (max >= GetZScale())
            return max;
        else
            return GetZScale();
    }

    /// <summary>
    /// Gets the number of particles in the group
    /// </summary>
    /// <returns>Count of particles</returns>
    public int GetParticlenum()
    {
        return particles.Count;
    }

    /// <summary>
    /// Gets the color of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Color of the particle</returns>
    public Color GetParticleColor(int i)
    {
        return particles[i].GetColor();
    }

    /// <summary>
    /// Gets the smooth length of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Vector3 representing smooth length</returns>
    public Vector3 GetMySmoothLength(int i)
    {
        return particles[i].GetMySmoothLength();
    }

    /// <summary>
    /// Gets the flow end position of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Vector3 representing flow end position</returns>
    public Vector3 GetFlowEnd(int i)
    {
        return particles[i].GetFlowEnd();
    }

    /// <summary>
    /// Gets the density of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Density value</returns>
    public double GetParticleDensity(int i)
    {
        return particles[i].GetParticleDensity();
    }

    /// <summary>
    /// Gets the gradient of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Vector3 representing gradient</returns>
    public Vector3 GetParticleGradient(int i)
    {
        return particles[i].GetGradient();
    }

    /// <summary>
    /// Gets the primary curvature of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Vector3 representing primary curvature</returns>
    public Vector3 GetParticlePrimaryCurvature(int i)
    {
        return particles[i].GetPrimaryCurvature();
    }

    /// <summary>
    /// Gets the secondary curvature of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Vector3 representing secondary curvature</returns>
    public Vector3 GetParticleSecondaryCurvature(int i)
    {
        return particles[i].GetSecondaryCurvature();
    }

    /// <summary>
    /// Gets the selection state of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Boolean flag value</returns>
    public bool GetIsSelected(int i)
    {
        return particles[i].GetIsSelected();
    }

    /// <summary>
    /// Gets the target flag of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Boolean target flag</returns>
    public bool GetTarget(int i)
    {
        return particles[i].GetTarget();
    }

    /// <summary>
    /// Gets the target type of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Target type integer</returns>
    public int GetTargetType(int i)
    {
        return particles[i].GetTargetType();
    }

    /// <summary>
    /// Gets the smooth length of the particle group
    /// </summary>
    /// <returns>Vector3 representing smooth length</returns>
    public Vector3 GetSmoothLength()
    {
        return smoothLength;
    }

    /// <summary>
    /// Gets the object position of a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <returns>Vector3 representing object position</returns>
    public Vector3 GetParticleObjectPos(int i)
    {
        return this.particles[i].GetPosition();
    }
    
    /// <summary>
    /// Gets the Object to World Matrix
    /// </summary>
    /// <param name="t">The transform of the root gameobject</param>
    /// <returns>The Object to World Matrix</returns>
    public Matrix4x4 GetObjToWorldMatrix(Transform t)
    {
        Matrix4x4 Translation = Matrix4x4.Translate(t.position);
        Matrix4x4 Rotation = Matrix4x4.Rotate(t.rotation);
        Matrix4x4 Scale = Matrix4x4.Scale(new Vector3(
            t.localScale.x,
            t.localScale.y,
            t.localScale.z
        ));
        Matrix4x4 RealSizeScaling = Matrix4x4.Scale(new Vector3(
            1f/GetXScale(),
            1f/GetYScale(),
            1f/GetZScale()
        ));
        return Translation * Rotation * Scale*RealSizeScaling;
    }
    /// <summary>
    /// Gets the particle's world position
    /// </summary>
    /// <param name="i">The index of particle</param>
    /// <param name="t">The transform of the root gameobject</param>
    /// <returns>The particle's world position</returns>
    public Vector3 GetParticleWorldPos(int i, Transform t)
    {
        return GetObjToWorldMatrix(t) .MultiplyPoint3x4(GetParticleObjectPos(i));
    }
    
    /// <summary>
    /// Gets the World to Object Matrix, which transforms a world vector3 to the object space
    /// </summary>
    /// <param name="t">The transform of the root gameobject</param>
    /// <returns>The World to Object Matrix</returns>
    public Matrix4x4 GetWorldToObjMatrix(Transform t)
    {
        Matrix4x4 InverseTranslation = Matrix4x4.Translate(-t.position);
        Matrix4x4 InverseRotation = Matrix4x4.Rotate(Quaternion.Inverse(t.rotation));
        Matrix4x4 InverseScale = Matrix4x4.Scale(new Vector3(
            1f/t.localScale.x,
            1f/t.localScale.y,
            1f/t.localScale.z
        ));
        Matrix4x4 InverseRealSizeScaling = Matrix4x4.Scale(new Vector3(
            GetXScale(),
            GetYScale(),
            GetZScale()
        ));
        return InverseRealSizeScaling*InverseScale*InverseRotation *InverseTranslation;
    }

    
    /// <summary>
    /// Gets the object position of a world position
    /// </summary>
    /// <param name="t">The transform of the root gameobject</param>
    /// <param name="v">The world position vector</param>
    /// <returns>The object position</returns>
    public Vector3 GetObjPosOfVec3(Transform t, Vector3 v)
    {
        return GetWorldToObjMatrix(t).MultiplyPoint3x4(v);
    }
    

    #endregion
    
    #region Set Properties
    /// <summary>
    /// Default constructor for ParticleGroup
    /// </summary>
    public Particles()
    {
        particles = new List<Particle>();
    }

    /// <summary>
    /// Adds a particle to the group
    /// </summary>
    /// <param name="p">Particle to add</param>
    public void AddParticle(Particle p)
    {
        particles.Add(p);
    }

    /// <summary>
    /// Sets the flow end position for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="v">Flow end position</param>
    public void SetFlowEnd(int i, Vector3 v)
    {
        particles[i].SetFlowEnd(v);
    }

    /// <summary>
    /// Sets the smooth length for a specific particle
    /// </summary>
    /// <param name="Sx">Smooth length in X</param>
    /// <param name="Sy">Smooth length in Y</param>
    /// <param name="Sz">Smooth length in Z</param>
    /// <param name="i">Index of the particle</param>
    public void SetMySmoothLength(float Sx, float Sy, float Sz, int i)
    {
        particles[i].SetMySmoothLength(Sx, Sy, Sz);
    }

    /// <summary>
    /// Sets the density for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="density">Density value</param>
    public void SetParticleDensity(int i, double density)
    {
        particles[i].SetParticleDensity(density);
    }

    /// <summary>
    /// Sets the gradient for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="c">Gradient vector</param>
    public void SetParticleGradient(int i, Vector3 c)
    {
        particles[i].SetGradient(c);
    }

    /// <summary>
    /// Sets the primary curvature for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="c">Primary curvature vector</param>
    public void SetParticlePrimaryCurvature(int i, Vector3 c)
    {
        particles[i].SetPrimaryCurvature(c);
    }

    /// <summary>
    /// Sets the secondary curvature for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="c">Secondary curvature vector</param>
    public void SetParticleSecondaryCurvature(int i, Vector3 c)
    {
        particles[i].SetSecondaryCurvature(c);
    }

    /// <summary>
    /// Sets the selection flag for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="flag">Flag value</param>
    public void SetIsSelected(int i, bool flag)
    {
        particles[i].SetIsSelected(flag);
    }

    /// <summary>
    /// Sets the target flag and type for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="flag">Target flag</param>
    /// <param name="type">Target type</param>
    public void SetTarget(int i, bool flag, int type)
    {
        particles[i].SetTarget(flag, type);
    }

    /// <summary>
    /// Sets the color for a specific particle
    /// </summary>
    /// <param name="i">Index of the particle</param>
    /// <param name="color">Color value</param>
    public void SetColor(int i, Color color)
    {
        particles[i].SetColor(color);
    }

    /// <summary>
    /// Sets the smooth length for the particle group
    /// </summary>
    /// <param name="v">Smooth length vector</param>
    public void SetSmoothLength(Vector3 v)
    {
        smoothLength = v;
    }
    #endregion
    
    #region Load and Save Methods
    /// <summary>
    /// Loads particle data from a PLY file
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <param name="dataname">Name of the dataset</param>
    public void LoadPly(string path, string dataname)
    {
        this.name = dataname;
        string filePath = path + dataname + ".ply";
        Vector3[] vs = DataPosPreProcessing(LoadPointCloud.LoadPly(filePath));
        particles = new List<Particle>();
        this.name = dataname;
        
        for (int i = 0; i < vs.Length; i++)
        {
            Particle p = new Particle(vs[i]);
            this.AddParticle(p);
        }
        
        LogInfo(dataname);
    }

    /// <summary>
    /// Loads particle data from a PCD file
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <param name="dataname">Name of the dataset</param>
    public void LoadPcd(string path, string dataname)
    {
        this.name = dataname;
        string filePath = path + dataname + ".pcd";
        Vector3[] vs = DataPosPreProcessing(LoadPointCloud.LoadPcd(filePath));
        particles = new List<Particle>();
        
        for (int i = 0; i < vs.Length; i++)
        {
            Particle p = new Particle(vs[i]);
            this.AddParticle(p);
        }
        
        LogInfo(dataname);
    }

    /// <summary>
    /// Loads particle data from a BIN file
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <param name="dataname">Name of the dataset</param>
    public void LoadBin(string path, string dataname)
    {
        this.name = dataname;
        string filePath = path + dataname + ".bin";
        Vector3[] vs = DataPosPreProcessing(LoadPointCloud.LoadBin(filePath));
        particles = new List<Particle>();
        
        for (int i = 0; i < vs.Length; i++)
        {
            Particle p = new Particle(vs[i]);
            this.AddParticle(p);
        }
        
        LogInfo(dataname);
    }

    /// <summary>
    /// Loads particle data from a TXT file
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <param name="dataname">Name of the dataset</param>
    public void LoadTxt(string path, string dataname)
    {
        this.name = dataname;
        string filePath = path + dataname + ".txt";
        Vector3[] vs = DataPosPreProcessing(LoadPointCloud.LoadTxt(filePath));
        particles = new List<Particle>();
        
        for (int i = 0; i < vs.Length; i++)
        {
            Particle p = new Particle(vs[i]);
            this.AddParticle(p);
        }
        
        LogInfo(dataname);
    }

    /// <summary>
    /// Loads particle data from a CSV file
    /// </summary>
    /// <param name="path">Path to the file</param>
    /// <param name="dataname">Name of the dataset</param>
    public void LoadCsv(string path, string dataname)
    {
        this.name = dataname;
        string filePath = path + dataname + ".csv";
        Vector3[] vs = DataPosPreProcessing(csvController.GetInstance().StartLoad(filePath));
        particles = new List<Particle>();
        
        for (int i = 0; i < vs.Length; i++)
        {
            Particle p = new Particle(vs[i]);
            this.AddParticle(p);
        }
        
        LogInfo(dataname);
    }

    /// <summary>
    /// Loads particle data from an array of Vector3 positions
    /// </summary>
    /// <param name="v">Array of positions</param>
    /// <param name="dataname">Name of the dataset</param>
    /// <param name="forSimulation">Flag indicating if for simulation</param>
    public void LoadVec3s(Vector3[] v, string dataname, bool forSimulation = false)
    {
        this.name = dataname;
        if (!forSimulation)
            v = DataPosPreProcessing(v);
            
        particles = new List<Particle>();
        for (int i = 0; i < v.Length; i++)
        {
            Particle p = new Particle(v[i]);
            this.AddParticle(p);
        }

        LogInfo(dataname);
    }

    /// <summary>
    /// Logs information about the loaded dataset
    /// </summary>
    /// <param name="dataname">Name of the dataset</param>
    void LogInfo(string dataname)
    {
        Debug.Log("Load success" + " " + dataname + " with " + GetParticlenum() + " particles." 
                  + " SmoothLength: " + GetSmoothLength().x + " " + GetSmoothLength().y + " " + GetSmoothLength().z
                  + ". Min ObjPos:" + GetMinObjPos() + ". Max ObjPos:" + GetMaxObjPos());
    }

    /// <summary>
    /// Preprocesses particle positions to center them near the origin
    /// </summary>
    /// <param name="vs">Array of particle positions</param>
    /// <returns>Processed array of positions</returns>
    public Vector3[] DataPosPreProcessing(Vector3[] vs)
    {
        Vector3 vSum = new Vector3(0f, 0f, 0f);
        for (int i = 0; i < vs.Length; i++)    //find the average point
        {
            vSum = vSum + vs[i];
        }
        Vector3 vAve = vSum / vs.Length;
      
        xmin = vAve.x;
        xmax = vAve.x;
        ymin = vAve.y;
        ymax = vAve.y;
        zmin = vAve.z;
        zmax = vAve.z;
        
        for (int i = 0; i < vs.Length; i++)  //find the max and min
        {
            if (xmin > vs[i].x)
                xmin = vs[i].x;
            if (xmax < vs[i].x)
                xmax = vs[i].x;
            if (ymin > vs[i].y)
                ymin = vs[i].y;
            if (ymax < vs[i].y)
                ymax = vs[i].y;
            if (zmin > vs[i].z)
                zmin = vs[i].z;
            if (zmax < vs[i].z)
                zmax = vs[i].z;
        }

        Vector3 middle = new Vector3((xmax + xmin) / 2, (ymax + ymin) / 2, (zmax + zmin) / 2);  //revise the pos to near view
        Vector3[] vsRevised = new Vector3[vs.Length];
        for (int i = 0; i < vs.Length; i++)
        {
            vsRevised[i] = vs[i] - middle;
        }

        vSum = new Vector3(0f, 0f, 0f);
        for (int i = 0; i < vsRevised.Length; i++)    //find the average point again
        {
            vSum = vSum + vsRevised[i];
        }
        vAve = vSum / vsRevised.Length;

        xmin = vAve.x;
        xmax = vAve.x;
        ymin = vAve.y;
        ymax = vAve.y;
        zmin = vAve.z;
        zmax = vAve.z;
        
        float[] xPos = new float[vsRevised.Length];
        float[] yPos = new float[vsRevised.Length];
        float[] zPos = new float[vsRevised.Length];
        
        for (int i = 0; i < vsRevised.Length; i++)  //Find the max and min and calculate the smoothlength
        {
            if (xmin > vsRevised[i].x)
                xmin = vsRevised[i].x;
            if (xmax < vsRevised[i].x)
                xmax = vsRevised[i].x;
            if (ymin > vsRevised[i].y)
                ymin = vsRevised[i].y;
            if (ymax < vsRevised[i].y)
                ymax = vsRevised[i].y;
            if (zmin > vsRevised[i].z)
                zmin = vsRevised[i].z;
            if (zmax < vsRevised[i].z)
                zmax = vsRevised[i].z;
            
            xPos[i] = vsRevised[i].x;
            yPos[i] = vsRevised[i].y;
            zPos[i] = vsRevised[i].z;
        }
        
        q_sort(xPos, 0, vsRevised.Length - 1);
        q_sort(yPos, 0, vsRevised.Length - 1);
        q_sort(zPos, 0, vsRevised.Length - 1);
        
        double smoothingLengthX = 2 * (xPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.8))] - xPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.2))]) / Mathf.Log10(vsRevised.Length);
        double smoothingLengthY = 2 * (yPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.8))] - yPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.2))]) / Mathf.Log10(vsRevised.Length);
        double smoothingLengthZ = 2 * (zPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.8))] - zPos[(int)Mathf.Ceil((float)(vsRevised.Length * 0.2))]) / Mathf.Log10(vsRevised.Length);
        
        this.SetSmoothLength(new Vector3((float)smoothingLengthX, (float)smoothingLengthY, (float)smoothingLengthZ));
        return vsRevised;
    }

    /// <summary>
    /// Quick sort implementation for float arrays
    /// </summary>
    /// <param name="f">Array to sort</param>
    /// <param name="begin">Start index</param>
    /// <param name="end">End index</param>
    public void q_sort(float[] f, int begin, int end)
    {
        if (begin < end)
        {
            float temp = f[begin];
            int i = begin;
            int j = end;
            
            while (i < j)
            {
                while (i < j && f[j] > temp)
                    j--;
                f[i] = f[j];
                
                while (i < j && f[i] <= temp)
                    i++;
                f[j] = f[i];
            }
            
            f[i] = temp;
            q_sort(f, i + 1, end);
            q_sort(f, begin, i - 1);
        }
        else
            return;
    }

    /// <summary>
    /// Stores particle flags to a file
    /// </summary>
    /// <param name="name">Name of the file to store</param>
    public void StoreFlags(string name)
    {
        List<int> flagtrue = Selection.GetpStack().ToList();

        if (flagtrue.Count == 0)
            Debug.Log("No marked particles");
        else
        {
            SavePointCloud.FlagsToBytes(name, flagtrue.ToArray());
        }
    }

    /// <summary>
    /// Saves particle data as a binary file
    /// </summary>
    /// <param name="filename">Name of the file to save</param>
    public void SaveAsBin(string filename)
    {
        List<Vector3> dataPos = new List<Vector3>();
        for (int i = 0; i < this.GetParticlenum(); i++)
        {
            dataPos.Add(GetParticleObjectPos(i));
        }
    
        if (dataPos.Count == 0)
            Debug.Log("No Target particles");
        else
            SavePointCloud.Vec3sToBytes(filename, dataPos.ToArray());
    }

    /// <summary>
    /// Saves particle data as a PLY file
    /// </summary>
    /// <param name="filename">Name of the file to save</param>
    public void SaveAsPly(string filename)
    {
        List<Vector3> dataPos = new List<Vector3>();
        for (int i = 0; i < this.GetParticlenum(); i++)
        {
            dataPos.Add(GetParticleObjectPos(i));
        }
    
        if (dataPos.Count == 0)
            Debug.Log("No Target particles");
        else
            SavePointCloud.Vec3sToPly(filename, dataPos.ToArray());
    }

    /// <summary>
    /// Saves particle data as a TXT file
    /// </summary>
    /// <param name="filename">Name of the file to save</param>
    public void SaveAsTxt(string filename)
    {
        List<Vector3> dataPos = new List<Vector3>();
        for (int i = 0; i < this.GetParticlenum(); i++)
        {
            dataPos.Add(GetParticleObjectPos(i));
        }
    
        if (dataPos.Count == 0)
            Debug.Log("No Target particles");
        else
            SavePointCloud.Vec3sToTxt(filename, dataPos.ToArray());
    }

    /// <summary>
    /// Saves particle data as a PCD file
    /// </summary>
    /// <param name="filename">Name of the file to save</param>
    public void SaveAsPcd(string filename)
    {
        List<Vector3> dataPos = new List<Vector3>();
        for (int i = 0; i < this.GetParticlenum(); i++)
        {
            dataPos.Add(GetParticleObjectPos(i));
        }
    
        if (dataPos.Count == 0)
            Debug.Log("No Target particles");
        else
            SavePointCloud.Vec3sToPcd(filename, dataPos.ToArray());
    }
    #endregion
}

[System.Serializable]
public class Particle
{
    #region Variables and Constructor
    [SerializeField]
    private Vector3 particlePosition;
    [SerializeField]
    private double particleDensity;
    [SerializeField]
    private Vector3 my_SmoothLength;
    [SerializeField]
    private bool isSelected;
    [SerializeField]
    private bool isTarget;
    [SerializeField]
    private int targetType;
    [FormerlySerializedAs("gradiant")] [SerializeField]
    private Vector3 gradient;
    [SerializeField]
    private Vector3 primaryCurvature;
    [SerializeField]
    private Vector3 secondaryCurvature;
    [SerializeField]
    private Vector3 flowEnd;
    [SerializeField] 
    private Color color;

    /// <summary>
    /// Constructor for Particle class
    /// </summary>
    /// <param name="v">Initial position of the particle</param>
    public Particle(Vector3 v)
    {
        this.particlePosition = v;
        particleDensity = 0;
        isSelected = false;
        gradient = Vector3.zero;
        color = new Color(30f / 255f, 144f / 255f, 255f / 255f);
    }
    #endregion
    
    #region Get Methods
    /// <summary>
    /// Gets the flow end position
    /// </summary>
    /// <returns>Flow end position vector</returns>
    public Vector3 GetFlowEnd()
    {
        return flowEnd;
    }

    /// <summary>
    /// Gets the particle position
    /// </summary>
    /// <returns>Position vector</returns>
    public Vector3 GetPosition()
    {
        return particlePosition;
    }

    /// <summary>
    /// Gets the particle density
    /// </summary>
    /// <returns>Density value</returns>
    public double GetParticleDensity()
    {
        return particleDensity;
    }

    /// <summary>
    /// Gets the smooth length
    /// </summary>
    /// <returns>Smooth length vector</returns>
    public Vector3 GetMySmoothLength()
    {
        return my_SmoothLength;
    }

    /// <summary>
    /// Gets the selection flag
    /// </summary>
    /// <returns>Selection flag</returns>
    public bool GetIsSelected()
    {
        return isSelected;
    }

    /// <summary>
    /// Gets the target flag
    /// </summary>
    /// <returns>Target flag</returns>
    public bool GetTarget()
    {
        return isTarget;
    }

    /// <summary>
    /// Gets the target type
    /// </summary>
    /// <returns>Target type</returns>
    public int GetTargetType()
    {
        return targetType;
    }

    /// <summary>
    /// Gets the gradient
    /// </summary>
    /// <returns>Gradient vector</returns>
    public Vector3 GetGradient()
    {
        return gradient;
    }

    /// <summary>
    /// Gets the primary curvature
    /// </summary>
    /// <returns>Primary curvature vector</returns>
    public Vector3 GetPrimaryCurvature()
    {
        return primaryCurvature;
    }

    /// <summary>
    /// Gets the secondary curvature
    /// </summary>
    /// <returns>Secondary curvature vector</returns>
    public Vector3 GetSecondaryCurvature()
    {
        return secondaryCurvature;
    }

    /// <summary>
    /// Gets the particle color
    /// </summary>
    /// <returns>Color value</returns>
    public Color GetColor()
    {
        return color;
    }
    #endregion
    
    #region Set Methods
    /// <summary>
    /// Sets the flow end position
    /// </summary>
    /// <param name="v">Flow end position</param>
    public void SetFlowEnd(Vector3 v)
    {
        flowEnd = v;
    }

    /// <summary>
    /// Sets the smooth length
    /// </summary>
    /// <param name="Sx">Smooth length in X</param>
    /// <param name="Sy">Smooth length in Y</param>
    /// <param name="Sz">Smooth length in Z</param>
    public void SetMySmoothLength(float Sx, float Sy, float Sz)
    {
        my_SmoothLength = new Vector3(Sx, Sy, Sz);
    }

    /// <summary>
    /// Sets the particle density
    /// </summary>
    /// <param name="density">Density value</param>
    public void SetParticleDensity(double density)
    {
        particleDensity = density;
    }

    /// <summary>
    /// Sets the selection flag
    /// </summary>
    /// <param name="flag">Flag value</param>
    public void SetIsSelected(bool flag)
    {
        isSelected = flag;
    }

    /// <summary>
    /// Sets the target flag and type
    /// </summary>
    /// <param name="t">Target flag</param>
    /// <param name="type">Target type</param>
    public void SetTarget(bool t, int type)
    {
        isTarget = t;
        targetType = type;
    }

    /// <summary>
    /// Sets the gradient
    /// </summary>
    /// <param name="grad">Gradient vector</param>
    public void SetGradient(Vector3 grad)
    {
        gradient = grad;
    }

    /// <summary>
    /// Sets the primary curvature
    /// </summary>
    /// <param name="pc">Primary curvature vector</param>
    public void SetPrimaryCurvature(Vector3 pc)
    {
        primaryCurvature = pc;
    }

    /// <summary>
    /// Sets the secondary curvature
    /// </summary>
    /// <param name="sc">Secondary curvature vector</param>
    public void SetSecondaryCurvature(Vector3 sc)
    {
        secondaryCurvature = sc;
    }

    /// <summary>
    /// Sets the particle color
    /// </summary>
    /// <param name="color">Color value</param>
    public void SetColor(Color color)
    {
        this.color = color;
    }
    #endregion
}