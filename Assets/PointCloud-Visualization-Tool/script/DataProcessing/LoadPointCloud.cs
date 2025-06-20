using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LoadPointCloud
{
    public static Vector3[] LoadPly(string filename)
    {
        var pointList = new List<Vector3>();
        try
        {
            if (!File.Exists(filename))
            {
                Debug.LogError("file does not exist: " + filename);
                return null;
            }

            using (var fs = new FileStream(filename, FileMode.Open))
            {
                using (var br = new BinaryReader(fs, Encoding.ASCII))
                {
                    var vertexCount = 0;
                    var isBinary = false;

                    // Read and parse the header
                    var headerCount = 0;
                    while (true)
                    {
                        var line = ReadLine(br);
                        headerCount++;
                        if (line.StartsWith("format"))
                        {
                            if (line.Contains("binary_big_endian 1.0"))
                                isBinary = true;
                            else if (line.Contains("ascii"))
                                isBinary = false;
                            else
                                throw new Exception("Unsupported PLY format");
                        }
                        else if (line.StartsWith("element vertex"))
                        {
                            var tokens = line.Split(' ');
                            vertexCount = int.Parse(tokens[2]);
                        }
                        else if (line.StartsWith("end_header"))
                        {
                            break;
                        }
                    }

                    if (isBinary)
                    {
                        // Process binary format
                        for (var i = 0; i < vertexCount; i++)
                        {
                            var x = ReadBigEndianFloat(br);
                            var y = ReadBigEndianFloat(br);
                            var z = ReadBigEndianFloat(br);
                            pointList.Add(new Vector3(x, y, z));
                        }
                    }
                    else
                    {
                        // Process ASCII format
                        br.BaseStream.Seek(0, SeekOrigin.Begin); // Reset stream to beginning
                        using (var sr = new StreamReader(fs, Encoding.ASCII))
                        {
                            // Skip header lines
                            for (var i = 0; i < vertexCount + headerCount; i++)
                            {
                                var line = sr.ReadLine();
                                if (i >= headerCount) // Start reading vertices after header
                                {
                                    var tokens = line.Split(' ');
                                    var x = float.Parse(tokens[0]);
                                    var y = float.Parse(tokens[1]);
                                    var z = float.Parse(tokens[2]);
                                    pointList.Add(new Vector3(x, y, z));
                                }
                            }
                        }
                    }
                }
            }

            return pointList.ToArray();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public static Vector3[] LoadPcd(string filename)
    {
        if (!File.Exists(filename)) throw new FileNotFoundException($"File not found: {filename}");

        var points = new List<Vector3>();
        var isAscii = false;
        var isBinary = false;
        var pointCount = 0;
        var headerLength = 0;

        using (var reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                headerLength += line.Length + 1; // 加1是因为换行符
                line = line.Trim();

                if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;

                if (line.StartsWith("DATA ascii"))
                {
                    isAscii = true;
                    break;
                }

                if (line.StartsWith("DATA binary"))
                {
                    isBinary = true;
                    break;
                }

                // 获取点的数量
                if (line.StartsWith("POINTS"))
                {
                    var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && int.TryParse(parts[1], out var count)) pointCount = count;
                }
            }
        }

        using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            if (isAscii)
            {
                // ASCII 格式处理
                fs.Seek(headerLength, SeekOrigin.Begin);
                using (var reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length < 3) continue;

                        if (float.TryParse(parts[0], out var x) &&
                            float.TryParse(parts[1], out var y) &&
                            float.TryParse(parts[2], out var z))
                            points.Add(new Vector3(x, y, z));
                    }
                }
            }
            else if (isBinary)
            {
                // Binary 格式处理
                fs.Seek(headerLength, SeekOrigin.Begin);
                var buffer = new byte[pointCount * 12]; // 每个点包含3个float，每个float占4字节
                fs.Read(buffer, 0, buffer.Length);

                for (var i = 0; i < pointCount; i++)
                {
                    var x = BitConverter.ToSingle(buffer, i * 12);
                    var y = BitConverter.ToSingle(buffer, i * 12 + 4);
                    var z = BitConverter.ToSingle(buffer, i * 12 + 8);

                    points.Add(new Vector3(x, y, z));
                }
            }
            else
            {
                throw new FormatException("Unsupported PCD data format: Only ASCII or binary formats are supported.");
            }
        }

        return points.ToArray();
    }

    public static Vector3[] LoadBin(string filename)
    {
        return FloatsToVec3s(BytesToFloats(LoadBytes(filename)));
    }

    /// <summary>
    ///     从TXT文件加载点云数据
    /// </summary>
    /// <param name="filename">TXT文件路径</param>
    /// <returns>点云数据数组</returns>
    public static Vector3[] LoadTxt(string filename)
    {
        if (!File.Exists(filename)) throw new FileNotFoundException($"File not found: {filename}");

        var points = new List<Vector3>();

        using (var reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue; // 跳过空行

                var parts = line.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 3) continue; // 如果行不包含至少三个值，跳过

                if (float.TryParse(parts[0], out var x) &&
                    float.TryParse(parts[1], out var y) &&
                    float.TryParse(parts[2], out var z))
                    points.Add(new Vector3(x, y, z));
            }
        }

        return points.ToArray();
    }

    public static int[] LoadFlags(string filename)
    {
        return BytesToInts(LoadBytes(filename));
    }


    private static byte[] LoadBytes(string filename)
    {
        try
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);

            return new byte[0];
        }
    }

    public static int[] LoadInt(string filename)
    {
        try
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var byteArray = new byte[fs.Length];
                fs.Read(byteArray, 0, byteArray.Length);
                return BytesToInts(byteArray);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            Debug.Log(e);
            return new int[0];
        }
    }

    private static float[] BytesToFloats(byte[] bs)
    {
        var floatArray = new float[bs.Length / sizeof(float)];
        for (var i = 0; i < floatArray.Length; i++)
        {
            var byteArray = new byte[sizeof(float)];
            for (var j = 0; j < sizeof(float); j++) byteArray[j] = bs[i * sizeof(float) + j];

            floatArray[i] = BitConverter.ToSingle(byteArray, 0);
        }

        return floatArray;
    }

    private static Vector3[] FloatsToVec3s(float[] fs)
    {
        var vectorArray = new Vector3[fs.Length / 3];
        for (var i = 0; i < vectorArray.Length; i++)
            vectorArray[i] = new Vector3(fs[i * 3], fs[i * 3 + 1], fs[i * 3 + 2]);

        return vectorArray;
    }

    private static int[] BytesToInts(byte[] bs)
    {
        var intArray = new int[bs.Length / sizeof(int)];
        for (var i = 0; i < intArray.Length; i++)
        {
            var byteArray = new byte[sizeof(int)];
            for (var j = 0; j < sizeof(int); j++) byteArray[j] = bs[i * sizeof(int) + j];

            intArray[i] = BitConverter.ToInt32(byteArray, 0);
        }

        return intArray;
    }

    private static string ReadLine(BinaryReader br)
    {
        var byteList = new List<byte>();
        byte readByte;
        while ((readByte = br.ReadByte()) != '\n')
            if (readByte != '\r') // Ignore carriage return if present
                byteList.Add(readByte);

        return Encoding.ASCII.GetString(byteList.ToArray());
    }

    private static float ReadBigEndianFloat(BinaryReader br)
    {
        var bytes = br.ReadBytes(4);
        Array.Reverse(bytes); // Convert to little endian
        return BitConverter.ToSingle(bytes, 0);
    }
}

public class csvController
{
    private static csvController csv;
    public List<string[]> arrayData;

    private csvController()
    {
        arrayData = new List<string[]>();
    }

    public static csvController GetInstance()
    {
        if (csv == null) csv = new csvController();

        return csv;
    }

    public int loadFile(string fileName)
    {
        arrayData.Clear();
        StreamReader sr = null;
        try
        {
            var file_url = fileName;
            sr = File.OpenText(file_url);
            Debug.Log("File Find in " + file_url);
        }
        catch
        {
            Debug.Log("File cannot find ! ");
            return 0;
        }

        string line;
        var count = 0;
        while ((line = sr.ReadLine()) != null)
        {
            count++;
            arrayData.Add(line.Split(','));
        }

        sr.Close();
        sr.Dispose();
        return count;
    }

    public string getString(int row, int col)
    {
        return arrayData[row][col];
    }

    public int getInt(int row, int col)
    {
        return int.Parse(arrayData[row][col]);
    }

    public float getFloat(int row, int col)
    {
        return float.Parse(arrayData[row][col]);
    }

    public Vector3[] StartLoad(string filename)
    {
        var count = GetInstance().loadFile(filename);
        var vs = new Vector3[count];
        for (var i = 1; i < count; i++)
            vs[i - 1] = new Vector3(GetInstance().getFloat(i, 1),
                GetInstance().getFloat(i, 2), GetInstance().getFloat(i, 3));

        return vs;
    }


    public void WriteCsv(string[] strs, string path)
    {
        if (!File.Exists(path)) File.Create(path).Dispose();

        using (var stream = new StreamWriter(path, false, Encoding.UTF8))
        {
            for (var i = 0; i < strs.Length; i++)
                if (strs[i] != null)
                    stream.WriteLine(strs[i]);
        }
    }
}