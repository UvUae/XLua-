using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.VisualScripting;

public class BuildTool : Editor
{

    [MenuItem("Tools/Build Windows Bundles")]
    static void BuildWindowsBundles()
    {
       Build(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Tools/Build Android Bundles")]
    static void BuildAndroidBundles()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/Build iOS Bundles")]
    static void BuildiOSBundles()
    {
        Build(BuildTarget.iOS);
    }

    static void Build(BuildTarget target)
   {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();


        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta")) continue;
           
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file:"+fileName);

            string assetName = PathUtil.GetUnityPath(files[i]);
            assetBundle.assetNames = new string[] { assetName };
            string bundleName = assetName.Replace("Assets/BuildResources/", "").Replace(Path.GetExtension(assetName), "").ToLower();
            assetBundle.assetBundleName = bundleName+".ab";
            assetBundleBuilds.Add(assetBundle);
        }
        if(Directory.Exists(PathUtil.BundleOutPath)==false)
        {
            Directory.CreateDirectory(PathUtil.BundleOutPath);
        }
        else
        {
            Directory.Delete(PathUtil.BundleOutPath,true);
            Directory.CreateDirectory(PathUtil.BundleOutPath);
        }
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, target);
    }

   
}
