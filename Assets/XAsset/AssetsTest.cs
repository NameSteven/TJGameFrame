using System.Collections;
using UnityEngine;
using XAsset;

public class AssetsTest : MonoBehaviour
{
    //  [SerializeField] string assetPath = "Assets/SampleAssets/Logo.prefab";
   // [SerializeField]
    string assetPath = "Assets/SampleAssets/@Prefab/TestPanel.prefab";

    string cubePath = "Assets/SampleAssets/@Cube/Cube.prefab";
    string sypherePath = "Assets/SampleAssets/@Sphere/Sphere.prefab";

    void Start()
    {
      //  Assets.InitializeAsync(LoadPrefab);
        Assets.Initialize();
        LoadPrefab();

        /*
#if UNITY_WEBGL
        Assets.InitializeAsync(() =>
        {
            StartCoroutine(Load());
        });
#else
        if (!Assets.Initialize())
        {
            Debug.LogError("Assets.Initialize falied.");
        }
        StartCoroutine(Load());
#endif
*/

    }
    GameObject testpanel = null;
    GameObject cubeObj = null;
    GameObject syphereobj = null;
    void LoadPrefab() {
        var asset = Assets.Load<GameObject>(assetPath);
        if (asset != null) {

            var prefab = asset.asset;
            if (prefab != null) {
                testpanel = Instantiate(prefab, GameObject.Find("Canvas").transform) as GameObject;
              //  testpanel.transform.SetParent(GameObject.Find("Canvas").transform);
                ReleaseAssetOnDestroy.Register(testpanel, asset);
               // GameObject.Destroy(go, 10);
            }
        }
    }
    void LoadCube() {
        var asset = Assets.Load<GameObject>(cubePath);
        if (asset != null) {

            var prefab = asset.asset;
            if (prefab != null) {
                cubeObj = Instantiate(prefab) as GameObject;
                ReleaseAssetOnDestroy.Register(cubeObj, asset);
                // GameObject.Destroy(go, 10);
            }
        }
    }

    void LoadSyphere() {
        var asset = Assets.Load<GameObject>(sypherePath);
        if (asset != null) {

            var prefab = asset.asset;
            if (prefab != null) {
                syphereobj = Instantiate(prefab) as GameObject;
                ReleaseAssetOnDestroy.Register(syphereobj, asset);
                // GameObject.Destroy(go, 10);
            }
        }
    }

    private void OnGUI() {
        if (GUILayout.Button("加载cube")) {
            LoadCube();
        }
        if (GUILayout.Button("加载syphere")) {
            LoadSyphere();
        }
        if (GUILayout.Button("加载panel")) {
            LoadPrefab();
        }
        if (GUILayout.Button("卸载syphere")) {
            if (syphereobj != null) {
                Destroy(syphereobj);
                syphereobj = null;
            }
        }
        if (GUILayout.Button("卸载cube")) {
            if (cubeObj != null) {
                Destroy(cubeObj);
                cubeObj = null;
            }
        }
        if (GUILayout.Button("卸载panel")) {
            if (testpanel != null) {
                Destroy(testpanel);
                testpanel = null;
            }
        }
    }
    IEnumerator Load()
    {

        //string assetPath = "Assets/SampleAssets/MyCube.prefab";
        ///// 同步模式用路径加载资源
        //var asset = Assets.Load<GameObject>(assetPath);
        //if (asset != null && asset.asset != null) {
        //    var go = GameObject.Instantiate(asset.asset);
        //    GameObject.Destroy(go, 1);
        //}
        ///// 卸载
        //asset.Unload();
        //asset = null;

        ///// 异步模式加载
        //var assetAsync = Assets.LoadAsync<GameObject>(assetPath);
        //if (assetAsync != null) {
        //    yield return assetAsync;
        //    if (assetAsync.asset != null) {
        //        var go = GameObject.Instantiate(assetAsync.asset);
        //        GameObject.Destroy(go, 1);
        //    } else {
        //        Debug.LogError(assetAsync.error);
        //    }
        //    assetAsync.Unload();
        //    assetAsync = null;
        //}

        //var asset = Assets.LoadAsync<GameObject>(assetPath);
        //if (asset != null)
        //{
        //    yield return asset;
        //    var prefab = asset.asset;
        //    if (prefab != null)
        //    {
        //        var go = Instantiate(prefab) as GameObject;
        //        ReleaseAssetOnDestroy.Register(go, asset);
        //        GameObject.Destroy(go, 10);
        //    }
        //}

      yield return new WaitForSeconds(11);

        //asset = Assets.Load<GameObject>(assetPath);
        //if (asset != null)
        //{
        //    var prefab = asset.asset;
        //    if (prefab != null)
        //    {
        //        var go = Instantiate(prefab) as GameObject;
        //        ReleaseAssetOnDestroy.Register(go, asset);
        //        GameObject.Destroy(go, 3);
        //    }
        //}
    }
}
