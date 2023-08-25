using ParticleProperty;
using ScalarField;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Utility 
{
    static public void q_sort(float[] f, int begin, int end)
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
    static public double InterpolateVector(UnityEngine.Vector3 pos, ParticleGroup pG, DensityField dF)
    {
        double x = pos.x, y = pos.y, z = pos.z;
        if (x < dF.GetNodedPos(0).x || y < dF.GetNodedPos(0).y || z < dF.GetNodedPos(0).z)
            return 0;

        double x_scaled = (x - dF.GetNodedPos(0).x) / dF.XSTEP;
        int xbin = (int)x_scaled;
        double xratio = x_scaled - xbin;

        double y_scaled = (y - dF.GetNodedPos(0).y) / dF.YSTEP;
        int ybin = (int)y_scaled;
        double yratio = y_scaled - ybin;

        double z_scaled = (z - dF.GetNodedPos(0).z) / dF.ZSTEP;
        int zbin = (int)z_scaled;
        double zratio = z_scaled - zbin;

        double dens1, dens2, dens3, dens4;

        if (zbin >= dF.ZNUM - 1 || ybin >= dF.YNUM - 1 || xbin >= dF.XNUM - 1)
            return 0f;
        else
        {
            dens1 = dF.GetNodeDensity(dF.NodePosToIndex(zbin, ybin, xbin)) + (dF.GetNodeDensity(dF.NodePosToIndex(zbin, ybin, xbin + 1)) - dF.GetNodeDensity(dF.NodePosToIndex(zbin, ybin, xbin))) * xratio;
            dens2 = dF.GetNodeDensity(dF.NodePosToIndex(zbin, ybin + 1, xbin)) + (dF.GetNodeDensity(dF.NodePosToIndex(zbin, ybin + 1, xbin + 1)) - dF.GetNodeDensity(dF.NodePosToIndex(zbin, ybin + 1, xbin))) * xratio;
            dens3 = dens1 + (dens2 - dens1) * yratio;

            dens1 = dF.GetNodeDensity(dF.NodePosToIndex(zbin + 1, ybin, xbin)) + (dF.GetNodeDensity(dF.NodePosToIndex(zbin + 1, ybin, xbin + 1)) - dF.GetNodeDensity(dF.NodePosToIndex(zbin + 1, ybin, xbin))) * xratio;
            dens2 = dF.GetNodeDensity(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)) + (dF.GetNodeDensity(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin + 1)) - dF.GetNodeDensity(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin))) * xratio;
            dens4 = dens1 + (dens2 - dens1) * yratio;

            return dens3 + (dens4 - dens3) * zratio;
        }
    }

    static public Vector3 InterpolateGradient(UnityEngine.Vector3 v, ParticleGroup pG, DensityField dF) //for gradient interpolation
    {
        double x_ = v.x, y_ = v.y, z_ = v.z;
        if (x_ < dF.GetNodedPos(0).x || y_ < dF.GetNodedPos(0).y || z_ < dF.GetNodedPos(0).z)
            return Vector3.zero;

        double x_scaled = (x_ - dF.GetNodedPos(0).x) / dF.XSTEP;
        int xbin = (int)x_scaled;
        double xratio = x_scaled - xbin;

        double y_scaled = (y_ - dF.GetNodedPos(0).y) / dF.YSTEP;
        int ybin = (int)y_scaled;
        double yratio = y_scaled - ybin;

        double z_scaled = (z_ - dF.GetNodedPos(0).z) / dF.ZSTEP;
        int zbin = (int)z_scaled;
        double zratio = z_scaled - zbin;
        double dens1, dens2, dens3, dens4;
        if (zbin >= dF.ZNUM - 1 || ybin >= dF.YNUM - 1 || xbin >= dF.XNUM - 1 || zbin <= 0 || xbin <= 0 || ybin <= 0)
        { return Vector3.zero; }
        else
        {
            dens1 = dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin)).x + (dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin + 1)).x - dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin)).x) * xratio;
            dens2 = dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin)).x + (dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin + 1)).x - dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin)).x) * xratio;
            dens3 = dens1 + (dens2 - dens1) * yratio;

            dens1 = dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin)).x + (dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin + 1)).x - dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin)).x) * xratio;
            dens2 = dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)).x + (dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin + 1)).x - dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)).x) * xratio;
            dens4 = dens1 + (dens2 - dens1) * yratio;
            double x = dens3 + (dens4 - dens3) * zratio;
            //--------------------------------------------------------

            dens1 = dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin)).y + (dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin + 1)).y - dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin)).y) * xratio;
            dens2 = dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin)).y + (dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin + 1)).y - dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin)).y) * xratio;
            dens3 = dens1 + (dens2 - dens1) * yratio;

            dens1 = dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin)).y + (dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin + 1)).y - dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin)).y) * xratio;
            dens2 = dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)).y + (dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin + 1)).y - dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)).y) * xratio;
            dens4 = dens1 + (dens2 - dens1) * yratio;
            double y = dens3 + (dens4 - dens3) * zratio;
            //----------------------------------------------------------

            dens1 = dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin)).z + (dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin + 1)).z - dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin, xbin)).z) * xratio;
            dens2 = dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin)).z + (dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin + 1)).z - dF.GetNodeGradient(dF.NodePosToIndex(zbin, ybin + 1, xbin)).z) * xratio;
            dens3 = dens1 + (dens2 - dens1) * yratio;

            dens1 = dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin)).z + (dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin + 1)).z - dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin, xbin)).z) * xratio;
            dens2 = dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)).z + (dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin + 1)).z - dF.GetNodeGradient(dF.NodePosToIndex(zbin + 1, ybin + 1, xbin)).z) * xratio;
            dens4 = dens1 + (dens2 - dens1) * yratio;
            double z = dens3 + (dens4 - dens3) * zratio;
            //----------------------------------------------------------
            return new Vector3((float)x, (float)y, (float)z);
        }
    }

    public static List<int> FloodFilling(int index, double thre, DensityField dF, ParticleGroup pG, bool confirm = false)  //return the index of box which need to implement the marching cube  //confirm means this trail is the final selection result and add the particles into the stack
    {
        List<int> targetBoxIndex = new List<int>(); int sample = 0;
        try
        {


            bool[] isVisited = new bool[dF.GetNodeNum()];
            for (int i = 0; i < isVisited.Length; i++)
            {
                isVisited[i] = false;
            }
            Stack<int> nodes = new Stack<int>();
            nodes.Push(index);
            isVisited[index] = true;
        

            List<int> selectedparticle = new List<int>();
            while (nodes.Count != 0)
            {
                sample = nodes.Pop();
                if (confirm)
                {
                    foreach (var i in dF.GetLUTUnit(sample))
                    {
                        if (pG.GetParticleDensity(i) > thre)
                        {
                           
                            selectedparticle.Add(i);
                           

                        }
                    }
                }


                targetBoxIndex.Add(sample);

                foreach (var i in Neighbour_6(sample, dF, pG))
                {
                    if (dF.GetNodeDensity(i) > thre && isVisited[i] == false)
                    {
                        isVisited[i] = true;
                        if (i >= 0 && i < dF.GetNodeNum())
                            nodes.Push(i);
                        if (i - 1 >= 0 && i - 1 < dF.GetNodeNum())
                            nodes.Push(i - 1);
                        if (i - dF.XNUM >= 0 && i - dF.XNUM < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM);
                        if (i - dF.XNUM - 1 >= 0 && i - dF.XNUM - 1 < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM - 1);
                        if (i - dF.XNUM * dF.YNUM >= 0 && i - dF.XNUM * dF.YNUM < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM);
                        if (i - dF.XNUM * dF.YNUM - 1 >= 0 && i - dF.XNUM * dF.YNUM - 1 < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM - 1);
                        if (i - dF.XNUM * dF.YNUM - dF.XNUM >= 0 && i - dF.XNUM * dF.YNUM - dF.XNUM < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM - dF.XNUM);
                        if (i - dF.XNUM * dF.YNUM - dF.XNUM - 1 >= 0 && i - dF.XNUM * dF.YNUM - dF.XNUM - 1 < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM - dF.XNUM - 1);
                    }
                }
            }
            if (confirm)
            {
                DataMemory.AddParticles(selectedparticle);
              
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.Log(sample);
        }
        return targetBoxIndex;
    }

 static List<int> targetIndex;static  List<int> selectedparticle;static bool[] isVisited;static Stack<int> nodes ;
public static List<int> ReviseFloodFilling(int index, double thre, DensityField dF, ParticleGroup pG, bool confirm = false)  //return the index of box which need to implement the marching cube  //confirm means this trail is the final selection result and add the particles into the stack
    {
        targetIndex = new List<int>(); int sample = 0;
        try
        {


            isVisited = new bool[dF.GetNodeNum()];
            for (int i = 0; i < isVisited.Length; i++)
            {
                isVisited[i] = false;
            }
            nodes = new Stack<int>();
            nodes.Push(index);
            isVisited[index] = true;
        

            selectedparticle = new List<int>();
            while (nodes.Count != 0)
            {
                sample = nodes.Pop();
                if (confirm)
                {
                    foreach (var i in dF.GetLUTUnit(sample))
                    {
                        if (pG.GetParticleDensity(i) > thre)
                        {
                           
                            selectedparticle.Add(i);
                           

                        }
                    }
                }


                targetIndex.Add(sample);

                foreach (var i in Neighbour_6(sample, dF, pG))
                {
                    if (dF.GetNodeDensity(i) > thre && isVisited[i] == false)
                    {
                        
                           isVisited[i] = true;
                        if (i >= 0 && i < dF.GetNodeNum())
                            nodes.Push(i);
                        if (i - 1 >= 0 && i - 1 < dF.GetNodeNum())
                            nodes.Push(i - 1);
                        if (i - dF.XNUM >= 0 && i - dF.XNUM < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM);
                        if (i - dF.XNUM - 1 >= 0 && i - dF.XNUM - 1 < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM - 1);
                        if (i - dF.XNUM * dF.YNUM >= 0 && i - dF.XNUM * dF.YNUM < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM);
                        if (i - dF.XNUM * dF.YNUM - 1 >= 0 && i - dF.XNUM * dF.YNUM - 1 < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM - 1);
                        if (i - dF.XNUM * dF.YNUM - dF.XNUM >= 0 && i - dF.XNUM * dF.YNUM - dF.XNUM < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM - dF.XNUM);
                        if (i - dF.XNUM * dF.YNUM - dF.XNUM - 1 >= 0 && i - dF.XNUM * dF.YNUM - dF.XNUM - 1 < dF.GetNodeNum())
                            nodes.Push(i - dF.XNUM * dF.YNUM - dF.XNUM - 1);
                    }
                }
            }
            if (confirm)
            {
                DataMemory.AddParticles(selectedparticle);
              
            }

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.Log(sample);
        }
        return targetIndex;
    }

    static public List<Vector3> Emit(Vector3 saddle_pos, Vector3 direction, DensityField dF, ParticleGroup pG)
    {
        float firststepfactor = 20f;
        float step = (dF.XSTEP + dF.YSTEP + dF.ZSTEP) / 3f / 10f;
        List<Vector3> nodes = new List<Vector3>();
        int index = 0;
        Vector3 oldPos = saddle_pos;
        Vector3 newPos = saddle_pos + direction.normalized * step * firststepfactor;
        nodes.Add(oldPos); nodes.Add(newPos);

        while (Utility.InterpolateVector(oldPos, pG, dF) <= Utility.InterpolateVector(newPos, pG, dF) && index <= 1000)
        {

            Vector3 g = Utility.InterpolateGradient(oldPos, pG, dF).normalized * step;
            oldPos = newPos;
            newPos += g;
            nodes.Add(newPos);
            index++;
        }



        return nodes;
    }

    public static List<int> GetNodesInArea(ParticleGroup pG, DensityField dF, float dRatio, int parIndex)  //Input by particle position,related to the elipse
    {
        Vector3 nodePos = pG.GetParticlePosition(parIndex);
        Vector3 sL = pG.GetMySmoothLength(parIndex);
        List<int> lint = new List<int>();
        float sLx = sL[0] * dRatio;
        float sLy = sL[1] * dRatio;
        float sLz = sL[2] * dRatio;
        float xmax, xmin, ymax, ymin, zmax, zmin; 
        xmax = nodePos.x + sLx;
        xmin = nodePos.x - sLx;
        ymax = nodePos.y + sLy;
        ymin = nodePos.y - sLy;
        zmax = nodePos.z + sLz;
        zmin = nodePos.z - sLz;

        xmax = xmax - pG.XMIN;  
        xmin = xmin - pG.XMIN;
        ymax = ymax - pG.YMIN;
        ymin = ymin - pG.YMIN;
        zmax = zmax - pG.ZMIN;
        zmin = zmin - pG.ZMIN;
        if (xmax > pG.XMAX - pG.XMIN)
            xmax = pG.XMAX - pG.XMIN - 1;
        if (ymax > pG.YMAX - pG.YMIN)
            ymax = pG.YMAX - pG.XMIN - 1;
        if (zmax > pG.ZMAX - pG.ZMIN)
            zmax = pG.ZMAX - pG.XMIN - 1;
        if (xmin < 0)
            xmin = 1;
        if (ymin < 0)
            ymin = 1;
        if (zmin < 0)
            zmin = 1;

        List<int> Lx = InternalNodes(xmin, xmax, dF.XSTEP);
        List<int> Ly = InternalNodes(ymin, ymax, dF.YSTEP);
        List<int> Lz = InternalNodes(zmin, zmax, dF.ZSTEP);
        if (Lx.Count == 0 || Ly.Count == 0 || Lz.Count == 0)
            return lint;
        else
        {

            foreach (var x in Lx)
            {
                foreach (var y in Ly)
                {
                    foreach (var z in Lz)
                    {
                        int index = dF.NodePosToIndex(z, y, x);

                        Vector3 v = nodePos - dF.GetNodedPos(index);
                        v.x = Mathf.Abs(v.x / sLx);
                        v.y = Mathf.Abs(v.y / sLy);
                        v.z = Mathf.Abs(v.z / sLz);
                        double dis = 1 - Vector3.Dot(v, v);
                        if (dis > 0)
                            lint.Add(index);

                    }
                }
            }

            return lint;

        }

    }

  
    static List<int> InternalNodes(float min, float max, float step)
    {
        List<int> L = new List<int>();
        int low_boundary = Mathf.CeilToInt(min / step);
        int high_boundary = Mathf.FloorToInt(max / step);
        if (high_boundary >= low_boundary)
        {
            for (int i = low_boundary; i <= high_boundary; i++)
            {
                L.Add(i);
            }
        }
        return L;
    }

    public static List<int> Neighbour_6(int index, DensityField dF, ParticleGroup pG)
    {

        List<int> neighbours = new List<int>();
        int neighbour;
        neighbour = index + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX)
            neighbours.Add(neighbour);
        neighbour = index - 1;
        if (dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN)
            neighbours.Add(neighbour);

        neighbour = index + dF.XNUM;
        if (dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM;
        if (dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN)
            neighbours.Add(neighbour);

        neighbour = index + dF.XNUM * dF.YNUM;
        if (dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM;
        if (dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        return neighbours;
    }

    public static List<int> Neighbour_26(int index, DensityField dF, ParticleGroup pG)
    {
        List<int> neighbours = new List<int>();
        int neighbour;
        neighbour = index + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        neighbour = index + dF.XNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        neighbour = index + dF.XNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        neighbour = index - dF.XNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        neighbour = index + dF.XNUM * dF.YNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM + dF.XNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM + dF.XNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM + dF.XNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM - dF.XNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM - dF.XNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index + dF.XNUM * dF.YNUM - dF.XNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        neighbour = index - dF.XNUM * dF.YNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM + dF.XNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM + dF.XNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM + dF.XNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM - dF.XNUM;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM - dF.XNUM + 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);
        neighbour = index - dF.XNUM * dF.YNUM - dF.XNUM - 1;
        if (dF.GetNodedPos(index).x + dF.XSTEP < pG.XMAX && dF.GetNodedPos(index).x - dF.XSTEP > pG.XMIN && dF.GetNodedPos(index).y + dF.YSTEP < pG.YMAX && dF.GetNodedPos(index).y - dF.YSTEP > pG.YMIN && dF.GetNodedPos(index).z + dF.ZSTEP < pG.ZMAX && dF.GetNodedPos(index).z - dF.ZSTEP > pG.ZMIN)
            neighbours.Add(neighbour);

        return neighbours;
    }

}



//    if (dF.GetNodeDensity(i) > thre && isVisited[i] == false)
//                     {
                        
//                            isVisited[i] = true;
//                         if (i >= 0 && i < dF.GetNodeNum()){if(isVisited[i]) nodes.Push(i);}
                           
//                         if (i - 1 >= 0 && i - 1 < dF.GetNodeNum()){if(isVisited[i-1]) nodes.Push(i - 1);}
                            
//                         if (i - dF.XNUM >= 0 && i - dF.XNUM < dF.GetNodeNum()){if(isVisited[i-dF.XNUM])  nodes.Push(i - dF.XNUM);}
                         
//                         if (i - dF.XNUM - 1 >= 0 && i - dF.XNUM - 1 < dF.GetNodeNum()){if(isVisited[i - dF.XNUM - 1])  nodes.Push(i - dF.XNUM - 1);}
                        
//                         if (i - dF.XNUM * dF.YNUM >= 0 && i - dF.XNUM * dF.YNUM < dF.GetNodeNum()){if(isVisited[i - dF.XNUM * dF.YNUM])  nodes.Push(i - dF.XNUM * dF.YNUM);}
                      
//                         if (i - dF.XNUM * dF.YNUM - 1 >= 0 && i - dF.XNUM * dF.YNUM - 1 < dF.GetNodeNum()){if(isVisited[i - dF.XNUM * dF.YNUM - 1])  nodes.Push(i - dF.XNUM * dF.YNUM - 1);}
                    
//                         if (i - dF.XNUM * dF.YNUM - dF.XNUM >= 0 && i - dF.XNUM * dF.YNUM - dF.XNUM < dF.GetNodeNum()){if(isVisited[i - dF.XNUM * dF.YNUM - dF.XNUM])  nodes.Push(i - dF.XNUM * dF.YNUM - dF.XNUM);}
                         
//                         if (i - dF.XNUM * dF.YNUM - dF.XNUM - 1 >= 0 && i - dF.XNUM * dF.YNUM - dF.XNUM - 1 < dF.GetNodeNum()){if(isVisited[i - dF.XNUM * dF.YNUM - dF.XNUM - 1])  nodes.Push(i - dF.XNUM * dF.YNUM - dF.XNUM - 1);}
                       
//                     }