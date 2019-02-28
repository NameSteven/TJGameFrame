using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace XAsset.Editor
{
    public static class AssetsMenuItem
	{
		[MenuItem ("Assets/Copy Asset Path")]
		static void CopyAssetPath ()
		{
			if (EditorApplication.isCompiling) {
				return;
			}
			string path = AssetDatabase.GetAssetPath (Selection.activeInstanceID);   
			GUIUtility.systemCopyBuffer = path;
			Debug.Log (string.Format ("systemCopyBuffer: {0}", path));
		}  

		const string kRuntimeMode = "Assets/XAsset/Bundle Mode"; 

		[MenuItem (kRuntimeMode)]
		public static void ToggleRuntimeMode ()
		{
            EditorUtility.ActiveBundleMode = !EditorUtility.ActiveBundleMode;  
		}

		[MenuItem (kRuntimeMode, true)]
		public static bool ToggleRuntimeModeValidate ()
		{
            Menu.SetChecked (kRuntimeMode, EditorUtility.ActiveBundleMode);
			return true;
		} 


        /// <summary>
        /// 设置打包需要的东西
        /// </summary>
		const string assetsManifesttxt = "Assets/Manifest.txt";

		[MenuItem ("Assets/XAsset/Build Manifest")]  
		public static void BuildAssetManifest ()
		{  
			if (EditorApplication.isCompiling) {
				return;
			}     
			List<AssetBundleBuild> builds = BuildRule.GetBuilds (assetsManifesttxt);
            BuildScript.BuildManifest (assetsManifesttxt, builds);
		}  

        /// <summary>
        /// 打包接口
        /// </summary>
		[MenuItem ("Assets/XAsset/Build AssetBundles")]  
		public static void BuildAssetBundles ()
		{  
			if (EditorApplication.isCompiling) {
				return;
			}       
			List<AssetBundleBuild> builds = BuildRule.GetBuilds (assetsManifesttxt);
            BuildScript.BuildManifest (assetsManifesttxt, builds);
			BuildScript.BuildAssetBundles (builds);
		}

       static string BuildAsetPath = "/SampleAssets";
        static string TjassetsManifesttxt = "Assets/TJManifest.txt";
        static List<AssetBundleBuild> AssetBundleBuildList = new List<AssetBundleBuild>();
        [MenuItem("Assets/XAsset/Build AssetBundles by dir")]
        public static void BuildAssetBundleByDir() {
            AssetBundleBuildList.Clear();
            AssetBundleBuild assetbundlebuild = new AssetBundleBuild();
            assetbundlebuild.assetBundleName = "TJManifest";
            assetbundlebuild.assetNames = new string[] { TjassetsManifesttxt };
            AssetBundleBuildList.Add(assetbundlebuild);

            string buildPath = Application.dataPath + BuildAsetPath;
            DoBuildAssetBundleByDir(buildPath);


            BuildScript.SaveManifest(TjassetsManifesttxt, AssetBundleBuildList);

            BuildScript.BuildAssetBundles(AssetBundleBuildList);
        }

     
        static void DoBuildAssetBundleByDir(string path) {
          

            string[] files = Directory.GetDirectories(path);
            for (int i = 0; i < files.Length; i++) {
                files[i] = files[i].Replace('\\', '/');
                int index = files[i].LastIndexOf("/");
                string DirName = files[i].Substring(index+1);
                if (DirName.StartsWith("@")) {
                    AssetBundleBuild assetBundleBuild = new AssetBundleBuild();
                    int index1 = Application.dataPath.Length;
                    string bundleName = files[i].Substring(index1 + 1);
                    assetBundleBuild.assetBundleName = bundleName;
                    string[] ChildFiles = Directory.GetFiles(files[i]);//"E:/selfLearnPlace/xasset-master/xasset-master/Assets/SampleAssets/@Cube\\Cube.prefab"
                    List<string> AssetNames = new List<string>();
                    for (int j = 0; j < ChildFiles.Length; j++) {
                        ChildFiles[j] = ChildFiles[j].Replace('\\', '/');
                       
                        if (!ChildFiles[j].EndsWith(".meta")) {
                          
                          string childName= "Assets/" + ChildFiles[j].Substring(index1+1);
                            AssetNames.Add(childName);
                         
                        }
                    }
                    assetBundleBuild.assetNames= AssetNames.ToArray();
                    AssetBundleBuildList.Add(assetBundleBuild);
                    Debug.Log(ChildFiles.Length);
                } else {
                    DoBuildAssetBundleByDir(files[i]);
                }
                Debug.Log(DirName);
            }
            Debug.Log(AssetBundleBuildList.Count);

          

          

        }

		[MenuItem ("Assets/XAsset/Copy AssetBundles to StreamingAssets")]  
		public static void CopyAssetBundlesToStreamingAssets ()
		{  
			if (EditorApplication.isCompiling) {
				return;
			}        
			BuildScript.CopyAssetBundlesTo (Path.Combine (Application.streamingAssetsPath, EditorUtility.AssetBundlesOutputPath));

            AssetDatabase.Refresh();
		}  

		[MenuItem ("Assets/XAsset/Build Player")]  
		public static void BuildPlayer ()
		{
			if (EditorApplication.isCompiling) {
				return;
			}  
			BuildScript.BuildStandalonePlayer ();
		}
	}
}