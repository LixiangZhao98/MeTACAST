
    using System;
    using System.IO;
    using UnityEngine;
    
    public class SavePointCloud 
    {
        static public void Vec3sToPly(string filename,Vector3[] vs)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("ply");
                writer.WriteLine("format ascii 1.0");
                writer.WriteLine("element vertex " + vs.Length);
                writer.WriteLine("property float x float y float z");
                writer.WriteLine("end_header");
    
                for (int i = 0; i <  vs.Length; i++)
                {
                    writer.WriteLine($"{vs[i].x} {vs[i].y} {vs[i].z}");
                }
            }
        }
        public static void Vec3sToPcd(string filename, Vector3[] vs)
        {
            if (vs == null || vs.Length == 0)
            {
                throw new System.ArgumentException("The point cloud array is empty or null.");
            }
    
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("# .PCD v0.7 - Point Cloud Data file format");
                writer.WriteLine("VERSION 0.7");
                writer.WriteLine("FIELDS x y z");
                writer.WriteLine("SIZE 4 4 4");
                writer.WriteLine("TYPE F F F");
                writer.WriteLine("COUNT 1 1 1");
                writer.WriteLine($"WIDTH {vs.Length}");
                writer.WriteLine("HEIGHT 1");
                writer.WriteLine("VIEWPOINT 0 0 0 1 0 0 0");
                writer.WriteLine($"POINTS {vs.Length}");
                writer.WriteLine("DATA ascii");
    
                foreach (var v in vs)
                {
                    writer.WriteLine($"{v.x:F6} {v.y:F6} {v.z:F6}");
                }
            }
    
            Debug.Log($"PCD file has been written successfully: {filename}");
        }
        public static void Vec3sToTxt(string filename, Vector3[] vs)
        {
            if (vs == null || vs.Length == 0)
            {
                throw new System.ArgumentException("The point cloud array is empty or null.");
            }
    
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var v in vs)
                {
                    writer.WriteLine($"{v.x:F6} {v.y:F6} {v.z:F6}");
                }
            }
    
            Debug.Log($"TXT file has been written successfully: {filename}");
        }
        
        static public void Vec3sToBytes(string filename,Vector3[] vs)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                byte[] byteArray = new byte[sizeof(float) * 3 * vs.Length];
                for (int i = 0; i < vs.Length; i++)
                {
                    byte[] bytex = BitConverter.GetBytes(vs[i].x);
                    byte[] bytey = BitConverter.GetBytes(vs[i].y);
                    byte[] bytez = BitConverter.GetBytes(vs[i].z);
                    bytex.CopyTo(byteArray, sizeof(float) * 3 * i);
                    bytey.CopyTo(byteArray, sizeof(float) * 3 * i + 1 * sizeof(float));
                    bytez.CopyTo(byteArray, sizeof(float) * 3 * i + 2 * sizeof(float));
                }
                fs.Write(byteArray, 0, byteArray.Length);
                Debug.Log(vs.Length+"vector3s are written into "+filename+" successfully.");
            }
            catch
            {
                Debug.LogError("Write fail");
            }
        }
        
        static public void FloatsToBytes(string filename,float[] vs)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                byte[] byteArray = new byte[sizeof(float) *vs.Length];
                for (int i = 0; i < vs.Length; i++)
                {
                    byte[] byte_ = BitConverter.GetBytes(vs[i]);
                    byte_.CopyTo(byteArray, sizeof(float) * i);
                }
                fs.Write(byteArray, 0, byteArray.Length);
                Debug.Log(vs.Length+"floats are written into "+filename+" successfully.");
            }
            catch
            {
                Debug.LogError("Write fail");
            }
        }
    
        static public void FlagsToBytes(string filename,int[] indexes)
        {
          
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            FileStream fs = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);
                byte[] byteArray = new byte[sizeof(int) * indexes.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    byte[] byte_ = BitConverter.GetBytes(indexes[i]);
                    byte_.CopyTo(byteArray, sizeof(int) * i);
                }
                fs.Write(byteArray, 0, byteArray.Length);
                Debug.Log("Flag Write success:"+filename);
            fs.Flush();   
            fs.Close();     
            fs.Dispose();
        }
    }

