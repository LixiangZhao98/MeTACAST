using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData 
{
    static public void Vec3sToFile(string filename,Vector3[] vs)
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
            Debug.Log("Write success");
        }
        catch
        {
            Debug.LogError("Write fail");
        }

           
           

    }

    static public void FlagsToFile(string filename,int[] indexes)
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
