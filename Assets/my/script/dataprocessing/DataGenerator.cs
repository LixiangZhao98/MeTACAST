using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataGenerator
{
    public enum NumParticles
    {
        NUM_1K = 1024,
        NUM_2K = 1024 * 2,
        NUM_4K = 1024 * 4,
        NUM_8K = 1024 * 8,
        NUM_16K = 1024 * 16,
        NUM_32K = 1024 * 32,
        NUM_64K = 1024 * 64,
        NUM_128K = 1024 * 128,
        NUM_256K = 1024 * 256,
        NUM_512K = 1024 * 512
    }
    static public Vector3[] Generate(string name)
    {
        if(name == "random_sphere")
        return Generate_Random_sphere();

        return null;
    }
    static public Vector3[] Generate_Random_sphere()
    {
        float positionScale = 16f;
        Random.InitState(2);
        int numBodies = (int)NumParticles.NUM_16K;
        float scale = positionScale * Mathf.Max(1, numBodies / 65536);
        int i = 0;
        Vector3[] v = new Vector3[numBodies];
        while (i < numBodies)
        {
            Vector3 pos = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

            if (Vector3.Dot(pos, pos) > 1.0) continue;
            v[i] = pos;
            i++;
        }
        return v;
    }


}
