using UnityEditor;
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Text;

public class CreateAssetBundles {
    [MenuItem("Assets/Build 2D Skybox")]
    static void Build2DAssetBundles() {
        CreateAssets(true);
    }

    [MenuItem("Assets/Build 3D Skybox")]
    static void Build3DAssetBundles()
    {
        CreateAssets(false);
    }

    static void CreateAssets(bool is2D) {
        string assetBundleDirectory = is2D ? "Assets/Output/2D" : "Assets/Output/3D";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        Cleanup(is2D);
    }

    static void Cleanup(bool is2D) {
        string in_folder = is2D ? "Assets/Input/2D" : "Assets/Input/3D";
        string folder = is2D ? "Assets/Output/2D" : "Assets/Output/3D";
        string target_file = is2D ? "custom_2d" : "custom_3d";
        string trash_file_1 = is2D ? "custom_3d" : "custom_2d";
        string trash_file_2 = is2D ? "2d" : "3d";

        if (File.Exists(Path.Combine(folder, target_file)))
        {
            string infoFilePath = Path.Combine(folder, "info.json");
            if (File.Exists(infoFilePath))
            {
                File.Delete(infoFilePath);
            }

            File.Copy(Path.Combine(in_folder, "info.json"), infoFilePath);

            if (File.Exists(Path.Combine(folder, trash_file_1)))
            {
                File.Delete(Path.Combine(folder, trash_file_1));
                File.Delete(Path.Combine(folder, trash_file_1 + ".manifest"));
            }

            if (File.Exists(Path.Combine(folder, trash_file_2)))
            {
                File.Delete(Path.Combine(folder, trash_file_2));
                File.Delete(Path.Combine(folder, trash_file_2 + ".manifest"));
            }

            string zipFilePath = Path.Combine(folder, "skybox.zip");

            if (File.Exists(zipFilePath)) {
                File.Delete(zipFilePath);
            }

            byte[] buffer = new byte[4096];

            using (ZipOutputStream zos = new ZipOutputStream(File.Create(zipFilePath))) {
                zos.SetLevel(9);
                zos.IsStreamOwner = true;
                zos.NameTransform = null;

                ZipEntry ze;

                ze = new ZipEntry("uid.txt");
                ze.DateTime = DateTime.Now;
                zos.PutNextEntry(ze);

                string uniqueUID = System.Guid.NewGuid().ToString();
                byte[] bytes = Encoding.ASCII.GetBytes(uniqueUID);
                zos.Write(bytes);

                ze = new ZipEntry("info.json");
                ze.DateTime = DateTime.Now;
                zos.PutNextEntry(ze);
                using (FileStream fs = File.OpenRead(infoFilePath))
                {
                    // Using a fixed size buffer here makes no noticeable difference for output
                    // but keeps a lid on memory usage.
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        zos.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }

                ze = new ZipEntry("sky.asset");
                ze.DateTime = DateTime.Now;
                zos.PutNextEntry(ze);
                using (FileStream fs = File.OpenRead(Path.Combine(folder, target_file)))
                {
                    // Using a fixed size buffer here makes no noticeable difference for output
                    // but keeps a lid on memory usage.
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        zos.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }

                
            }          
        }
        else {
            Debug.LogError("Did not find output, try running it again");
        }
    }
}