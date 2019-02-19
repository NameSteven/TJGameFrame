using System;
using System.Collections.Generic;
using UnityEngine;

namespace XAsset
{
	public class TestPanel : UIPanel
	{
		protected override void OnLoaded ()
		{
			base.OnLoaded (); 
			Log ("OnLoaded");
		}

		protected override void OnUnload ()
		{
			base.OnUnload ();
			Log ("OnUnload");
		}
	}

   
    public class UIPanelTest : MonoBehaviour
	{
		UIPanel panel;

        [SerializeField]
        string assetPath = "Assets/SampleAssets/Logo.prefab";
        void Start()
		{
            Assets.InitializeAsync(LoadPrefab);
            if (! Assets.Initialize ()) {
				Debug.LogError ("Initialize failed!");
			} 
		}


        void LoadPrefab() {
            var asset = Assets.Load<GameObject>(assetPath);
            if (asset != null) {

                var prefab = asset.asset;
                if (prefab != null) {
                    var go = Instantiate(prefab) as GameObject;
                    // ReleaseAssetOnDestroy.Register(go, asset);
                    //  GameObject.Destroy(go, 10);
                }
            }
        }
        void OnGUI()
		{
			using (var h = new GUILayout.HorizontalScope (GUILayout.Width(Screen.width), GUILayout.Height(Screen.height))) {
				GUILayout.FlexibleSpace ();
				using (var v = new GUILayout.VerticalScope ()) {
					GUILayout.FlexibleSpace ();

					if (GUILayout.Button("Load")) {
						panel = UIPanel.Create <TestPanel>("Assets/SampleAssets/Logo.prefab");
					}
					
					if (GUILayout.Button("Unload")) {
						panel.Unload (false);
						panel = null;
					} 

					GUILayout.FlexibleSpace (); 
				}
				GUILayout.FlexibleSpace ();
			}
		}
	} 
}
