using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;

public class ZipUtil : MonoBehaviour
{
    public static void Extract(string FileZipPath, string OutputFolder, Action on_done = null, Action on_error = null)
    {
        if (!Directory.Exists(OutputFolder))
            Directory.CreateDirectory(OutputFolder);
        try
        {

            if (File.Exists(FileZipPath))
                ZipFile.ExtractToDirectory(FileZipPath, OutputFolder, true);
            on_done?.Invoke();

        }
        catch (Exception e)
        {
            on_error?.Invoke();
        }
    }

    public static void Zip(string folder, string fileName)
    {
        ZipFile.CreateFromDirectory(folder, fileName);
    }
}
