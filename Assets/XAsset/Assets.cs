using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XAsset
{

    //提供上层逻辑接口 不用知道具体的assetbundle加载
    public sealed class Assets : MonoBehaviour
    {
        static Assets instance; 

        static Manifest manifest = new Manifest();
        //所有资源
        public static string[] allAssetNames { get { return manifest.allAssets; } }
        //所有包
        public static string[] allBundleNames { get { return manifest.allBundles; } }
        public static string GetBundleName(string assetPath) { return manifest.GetBundleName(assetPath); }
        public static string GetAssetName(string assetPath) { return manifest.GetAssetName(assetPath); }

        static readonly List<Asset> assets = new List<Asset>();//加载的asset

        private static void CheckInstance()
        {
            if (instance == null)
            {
                var go = new GameObject("Assets");
                DontDestroyOnLoad(go);
                instance = go.AddComponent<Assets>();
            }
        }

        //初始化
        public static bool Initialize()
        {
            CheckInstance();

#if UNITY_EDITOR
            if (Utility.ActiveBundleMode)
            {
                return InitializeBundle();//初始化manifest
            }
            return true;
#else
			return InitializeBundle();
#endif
        }

        /// <summary>
        /// 加载manifest
        /// </summary>
        /// <returns></returns>
        static bool InitializeBundle() {
            string relativePath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            var url =
#if UNITY_EDITOR
               // relativePath + "/";
            Path.Combine(Application.streamingAssetsPath, relativePath) + "/";
#else
				Path.Combine(Application.streamingAssetsPath, relativePath) + "/"; 
#endif
            if (Bundles.Initialize(url)) {
                var bundle = Bundles.Load("manifest");
                if (bundle != null) {
                    var asset = bundle.LoadAsset<TextAsset>("Manifest.txt");
                    if (asset != null) {
                        using (var reader = new StringReader(asset.text)) {
                            manifest.Load(reader);
                            reader.Close();
                        }
                        bundle.Release();
                        Resources.UnloadAsset(asset);
                        asset = null;
                    }
                    return true;
                }
                throw new FileNotFoundException("assets manifest not exist.");
            }
            throw new FileNotFoundException("bundle manifest not exist.");
        }


        public static void InitializeAsync(System.Action onComplete)
        {
            CheckInstance();

            instance.InitializeBundleAsync(onComplete);
        }

        public static Asset Load<T>(string path) where T : Object
        {
            return Load(path, typeof(T));
        }

        public static Asset Load(string path, System.Type type)
        {
            return LoadInternal(path, type, false);
        }

        public static Asset LoadAsync<T>(string path)
        {
            return LoadAsync(path, typeof(T));
        }

        public static Asset LoadAsync(string path, System.Type type)
        {
            return LoadInternal(path, type, true);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="asyncMode"></param>
        /// <returns></returns>
        static Asset LoadInternal(string path, System.Type type, bool asyncMode) {
            Asset asset = assets.Find(obj => { return obj.assetPath == path; });
            if (asset == null) {
#if UNITY_EDITOR
                if (Utility.ActiveBundleMode) {
                    asset = CreateAssetRuntime(path, type, asyncMode);
                } else {
                    asset = new Asset(path, type);
                }
#else
				asset = CreateAssetRuntime (path, type, asyncMode);
#endif
                assets.Add(asset);
                asset.Load();
            }
            asset.Retain();
            for (int i = 0; i < assets.Count; i++) {
                Debug.Log("-------------" + assets[i].assetPath + "-----");
            }
            return asset;


        }

         void OnGUI() {

            string tex = "";
            int count = 0;
            GUIStyle sty = new GUIStyle();
            sty.fontSize = 20;
            foreach (var item in Bundles.bundles) {
                tex = string.Format("{0}----{1}", item.Value.path, item.Value.references);
                GUI.Label(new Rect(Screen.width / 12, Screen.height / 2-count*20-400, 500, 300), tex, sty);
                //if (GUILayout.Button(string.Format("{0}-{1}",item.Value.path, item.Value.references))) {
                count++;
                //}
            }
           
        }
        static Asset CreateAssetRuntime(string path, System.Type type, bool asyncMode) {
            if (asyncMode)
                return new BundleAssetAsync(path, type);
            return new BundleAsset(path, type);
        }

        public static void Unload(Asset asset)
        {
            asset.Release();
        }


     

        /// <summary>
        /// 初始化加载的
        /// </summary>
        /// <param name="onComplete"></param>
        void InitializeBundleAsync(System.Action onComplete)
        {
            string relativePath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            var url =
#if UNITY_EDITOR
                relativePath + "/";
#else
				Path.Combine(Application.streamingAssetsPath, relativePath) + "/"; 
#endif

            StartCoroutine(Bundles.InitializeAsync(url, bundle =>
            {
                if (bundle != null)
                {
                    var asset = bundle.LoadAsset<TextAsset>("Manifest.txt");
                    if (asset != null)
                    {
                        using (var reader = new StringReader(asset.text))
                        {
                            manifest.Load(reader);
                            reader.Close();
                        }
                        bundle.Release();
                        Resources.UnloadAsset(asset);
                        asset = null;
                    }
                }

                if (onComplete != null)
                {
                    onComplete.Invoke();
                }
            }));
        }

     

     

     


        System.Collections.IEnumerator gc = null;
        System.Collections.IEnumerator GC()
        {
			System.GC.Collect ();
            yield return 0;
            yield return Resources.UnloadUnusedAssets();
        }

        void Update()
        {
            bool removed = false;
            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                if (! asset.Update() && asset.references <= 0)
                {
                    asset.Unload();
                    asset = null;
                    assets.RemoveAt(i);
                    i--;
                    removed = true;
                }
            }

            if (removed)
            {
                if (gc != null)
                {
                    StopCoroutine(gc); 
                }
                gc = GC();
                StartCoroutine(gc);
            }

            Bundles.Update();
        }
    }
}
