using System.Collections.Generic;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using RhythmicVR.Files;
using UnityEditor;
using UnityEngine;

namespace RhythmicVR
{
    internal static class ModBuilder
    {
        internal const int SDKVersion = 2;

        internal static BundleAssets GetAssetAndScenes(params Object[] assets)
        {
            List<string> AssetPaths = new List<string>();
            List<string> Scenes = new List<string>();
            List<Object> AssetObjects = new List<Object>();

            AssetObjects.AddRange(assets);
            foreach (Object asset in assets)
            {
                var deps = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(asset));
                foreach (var dep in deps)
                {
                    if (!dep.EndsWith("LightingData.asset"))
                    {
                        switch (Path.GetExtension(dep).ToLower())
                        {
                            case ".dll":
                            case ".cs":
                            case ".js":
                                break;
                            case ".unity":
                                Scenes.Add(dep);
                                AssetObjects.Add(AssetDatabase.LoadAssetAtPath<Object>(dep));
                                break;
                            default:
                                AssetPaths.Add(dep);
                                AssetObjects.Add(AssetDatabase.LoadAssetAtPath<Object>(dep));
                                break;
                        }
                    }
                }
            }

            return new BundleAssets
            {
                AssetObjects = AssetObjects.ToArray(),
                AssetPaths = AssetPaths.ToArray(),
                ScenePaths = Scenes.ToArray()
            };
        }

        internal static bool BuildBundleFromAsset(Bundle bundle)
        {
            Debug.Log("Building bundle");

            bundle.AssetsInBundle = null;

            var unityAssetsInBundle = new List<Object>();
            unityAssetsInBundle.Add(bundle);
            if (bundle.ExtraAssetIncludes != null) unityAssetsInBundle.AddRange(bundle.ExtraAssetIncludes);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string ModFolder = "bundle";
            string ModPath = Path.Combine(Directory.GetCurrentDirectory(), ModFolder);
            Debug.Log($"Gathering Asset and Scenes from the bundle");
            BundleAssets assets = GetAssetAndScenes(bundle);

            bundle.AssetsInBundle = assets;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Assets included in bundle:");
            foreach (string asset in assets.AssetPaths)
            {
                sb.AppendLine(asset);
            }
            sb.AppendLine("Scenes included in bundle:");
            foreach (string scene in assets.ScenePaths)
            {
                sb.AppendLine(scene);
            }
            Debug.Log(sb);

            HashSet<string> BundleNames = new HashSet<string>();

            //Intenal unique name for the bundle so we can load it later without conflicts
            EditorUtility.DisplayProgressBar("Generating Unique name", "Generating Unique name", 0.05f);
            System.Guid GUID = System.Guid.NewGuid();
            AssetBundleBuild abb = new AssetBundleBuild
            {
                assetBundleName = $"{GUID}.data",
                addressableNames = assets.AssetPaths,
                assetNames = assets.AssetPaths,
            };
            AssetBundleBuild sbb = new AssetBundleBuild
            {
                assetBundleName = $"{GUID}.scene",
                addressableNames = assets.ScenePaths,
                assetNames = assets.ScenePaths,
            };
            BundleNames.Add(abb.assetBundleName);
            BundleNames.Add(sbb.assetBundleName);

            try
            {
                EditorUtility.DisplayProgressBar("Checking for temp folder", "Checking for temp folder to see if theres a old mod to clear out", 0.1f);
                Debug.Log($"Checking for temp folder to see if theres a old mod to clear out");
                if (Directory.Exists(ModPath))
                {
                    EditorUtility.DisplayProgressBar("Temp folder exists, removing it.", "Temp folder exists, removing it.", 0.11f);
                    Debug.Log("Temp folder exists, removing it.");

                    Directory.Delete(ModPath, true);
                }

                EditorUtility.DisplayProgressBar($"Creating {ModPath}", "Creating folder to place files into.", 0.12f);
                Debug.Log($"Creating {ModPath}, {ModPath}/win64 and {ModPath}/lin64");
                Directory.CreateDirectory(ModPath);
                Directory.CreateDirectory(ModPath + "/win64");
                Directory.CreateDirectory(ModPath + "/lin64");

                EditorUtility.ClearProgressBar();
                Debug.Log($"Building bundles");
                List<AssetBundleManifest> buildManifests = new List<AssetBundleManifest>();
                buildManifests.Add(BuildPipeline.BuildAssetBundles(ModFolder + "/win64", new AssetBundleBuild[] { abb, sbb },
                     BuildAssetBundleOptions.None,
                     BuildTarget.StandaloneWindows64));
                buildManifests.Add(BuildPipeline.BuildAssetBundles(ModFolder + "/lin64", new AssetBundleBuild[] { abb, sbb },
                                                                   BuildAssetBundleOptions.None,
                                                                   BuildTarget.StandaloneLinux64));

                if (buildManifests.Count == 0)
                {
                    EditorUtility.DisplayDialog("Errors while building bundle", "While building the bundle there were script or unity errors that prevented it from building. Please check the console for scripts that are broken and retry after fixing.", "Ok");

                    return false;
                }

                EditorUtility.DisplayProgressBar($"Reading manifest", "", 0.8f);
                Debug.Log($"Manifest:{File.ReadAllText(Path.Combine(ModPath + "/win64", abb.assetBundleName + ".manifest"))}");
                Debug.Log($"Manifest:{File.ReadAllText(Path.Combine(ModPath + "/lin64", abb.assetBundleName + ".manifest"))}");

                EditorUtility.DisplayProgressBar($"Removing unused manifest files.", "", 0.81f);
                Debug.Log($"Removing unused manifest files.");
                List<string> filesWin = new List<string>();
                filesWin.AddRange(Directory.GetFiles(ModPath + "/win64"));
                foreach (string file in filesWin)
                {
                    //If this file is not inside of the set of actual bundle files, delete it.
                    if (!BundleNames.Contains(Path.GetFileName(file)))
                    {
                        File.Delete(file);
                    }
                }
                
                List<string> filesLin = new List<string>();
                filesLin.AddRange(Directory.GetFiles(ModPath + "/lin64"));
                foreach (string file in filesLin)
                {
                    //If this file is not inside of the set of actual bundle files, delete it.
                    if (!BundleNames.Contains(Path.GetFileName(file)))
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                string ErrorMessage = $"Unable to build assetbundles of your content, please check your assets for errors, or broken scripts.\nIf the error persists, Contact the devs.\nInternal Error:{e.Message}";
                EditorUtility.DisplayDialog("Unable to build bundle.", ErrorMessage, "Ok");
                Debug.Log($"<b>{ErrorMessage}</b>");

                return false;
            }

            var invalidCharacters = Path.GetInvalidFileNameChars();
            HashSet<char> invalidSet = new HashSet<char>();
            foreach (var c in invalidCharacters)
                invalidSet.Add(c);
            StringBuilder cleanOutput = new StringBuilder();
            for (int i = 0; i < bundle.Name.Length; i++)
            {
                if (!invalidSet.Contains(bundle.Name[i]))
                    cleanOutput.Append(bundle.Name[i]);
            }

            string name = $"{cleanOutput}.plugin";

            EditorUtility.DisplayProgressBar($"Bundling up bundle", "", 0.9f);
            var zip = new FastZip();
            zip.CreateZip(name, ModFolder, true, "");

            //Remove the temp folder
            EditorUtility.DisplayProgressBar($"Removing temp folder", "", 0.99f);
            Directory.Delete(ModPath, true);

            EditorUtility.ClearProgressBar();
            string message = $"Finished building bundle successfully. Your packaged bundle can now be uploaded to the lavender website.\nLocation:{Path.GetFullPath(name)}";
            EditorUtility.DisplayDialog("Finished building bundle sucessfuly.", message, "Ok");
            Debug.Log($"<b>{message}</b>");

            return true;
        }
    }
}
