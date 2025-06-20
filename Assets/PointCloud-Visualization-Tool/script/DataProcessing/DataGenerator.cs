
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;



public class DataGenerator
    {

    //please add your defined mathmetical method with the function template
    //
    //public Vector3[] YourDatasetName()
    //{
    //Vector3[] points;
    //....Your code here
    //return points;
    //}

    //After save the file, it will be automatically appear in "Custom dataset" in the Unity inspector for you to select in runtime.

    public Vector3[] Sphere3D()
        {
            int numberOfPoints =  (int)NumParticles.NUM_64K;
            float radius = 1.0f; 
            Vector3[] points = new Vector3[numberOfPoints];
    
            for (int i = 0; i < numberOfPoints; i++)
            {
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points[i]=new Vector3(x, y, z);
            }
    
            return points;
        }
    
        public Vector3[] Cube3D() 
        {
            
            Random.InitState(2);
    
            int num = 100000;
    
            int i = 0;
    
            Vector3[] v = new Vector3[num];
    
            while (i < num)
    
            {
    
                v[i] = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    
                i++;
    
            }
    
            return v;
    
        }
    
        public Vector3[] Triangle3D()
        {
            Vector3 p1 = new Vector3(-Random.Range(0f, 1.0f), -Random.Range(0f, 1.0f), 0f);
            Vector3 p2 = new Vector3(Random.Range(0f, 1.0f), -Random.Range(0f, 1.0f), 0f);
            Vector3 p3 = new Vector3(-Random.Range(0f, 1.0f), Random.Range(0f, 1.0f), 0f);
            int numberOfPoints =  (int)NumParticles.NUM_16K;
        Vector3[] points = new Vector3[numberOfPoints];
    
    
            for (int i = 0; i < numberOfPoints; i++)
            {
                double r1 = Random.Range(0f, 1.0f);
                double r2 = Random.Range(0f, 1.0f);
    
                double sqrtR1 = Mathf.Sqrt((float)r1);
                double b1 = 1 - sqrtR1;
                double b2 = r2 * sqrtR1;
                double b3 = 1 - b1 - b2;
    
                float x = (float)(b1 * p1.x + b2 * p2.x + b3 * p3.x);
                float y = (float)(b1 * p1.y + b2 * p2.y + b3 * p3.y);
                points[i]=new Vector3(x,y,Random.Range(0f, 1.0f));
            }
            
        return points;
        }
        
        public Vector3[] Three_Sphere_different_size()
        {
            int numberOfPoints =  (int)NumParticles.NUM_16K  *10;
            float radius = 1.0f; 
            List<Vector3> points = new List<Vector3>();
    
            for (int i = 0; i < numberOfPoints; i++)
            { 
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points.Add(new Vector3(x, y, z));
            }
    
            
             numberOfPoints = (int)NumParticles.NUM_16K*8  *4;
             radius = 2f; 
    
            for (int i = 0; i < numberOfPoints; i++)
            {
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points.Add(new Vector3(x, y, z+6f));
            }
            
            
            numberOfPoints = (int)((float)NumParticles.NUM_16K*3.375f);
            radius = 1.5f; 
    
            for (int i = 0; i < numberOfPoints; i++)
            {
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points.Add(new Vector3(x+6f, y, z+7f));
            }
            
            
            int num = 10000;
    
            int ii = 0;
            while (ii < num)
    
            {
                points.Add (new Vector3(Random.Range(-4.0f, 8.0f), Random.Range(-6f, 6f),Random.Range(-2.0f, 10.0f)));
                ii++;
            }
    
            
            return points.ToArray();
        }
         
        public Vector3[] Three_Sphere_sameSize()
        {
            int numberOfPoints =  (int)NumParticles.NUM_16K  *10;
            float radius = 1.0f; 
            List<Vector3> points = new List<Vector3>();
    
            for (int i = 0; i < numberOfPoints; i++)
            { 
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points.Add(new Vector3(x, y, z));
            }
    
            
             numberOfPoints = (int)NumParticles.NUM_16K  *4;
             radius = 1f; 
    
            for (int i = 0; i < numberOfPoints; i++)
            {
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points.Add(new Vector3(x, y, z+6f));
            }
            
            
            numberOfPoints = (int)((float)NumParticles.NUM_16K);
            radius = 1f; 
    
            for (int i = 0; i < numberOfPoints; i++)
            {
                float r = radius * (float)Mathf.Pow(Random.Range(0f, 1.0f), (float)(1f/3f)); 
    
                float theta = 2 * (float)Mathf.PI * Random.Range(0f, 1.0f); 
                float phi = (float)Mathf.Acos(2 * Random.Range(0f, 1.0f) - 1); 
    
                float x = r * (float)(Mathf.Sin(phi) * Mathf.Cos(theta));
                float y = r * (float)(Mathf.Sin(phi) * Mathf.Sin(theta));
                float z = r * (float)(Mathf.Cos(phi));
    
                points.Add(new Vector3(x+6f, y, z+7f));
            }
            
            
            int num = 10000;
    
            int ii = 0;
            while (ii < num)
    
            {
                points.Add (new Vector3(Random.Range(-3.0f, 8.0f), Random.Range(-6f, 5f),Random.Range(-2.0f, 9.0f)));
                ii++;
            }
    
            
            return points.ToArray();
        }
    }
    
    
    

