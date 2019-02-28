using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XAsset;

public class TestPanel : MonoBehaviour {

    public Image sprite;
    public Button button;
    private Sprite spriteImage;
    List<string> spriteName = new List<string>() {"Assets/SampleAssets/UiSprite/@CommonUI/ui-common_xuetiao_0_0.png",

    "Assets/SampleAssets/UiSprite/@CommonUI/ui-common_xuetiao_1.png",

    "Assets/SampleAssets/UiSprite/@CommonUI/ui-common_xuetiao_10.png",

   };
	// Use this for initialization
	void Start () {
        button.onClick.AddListener(buttonClick);

    }
    int index = 0;
    List<Asset> AssetList = new List<Asset>();
    private void buttonClick() {
      
        var asset = Assets.Load<Sprite>(spriteName[index]);
        //  if (!AssetList.Contains(asset))
        AssetList.Add(asset);
        if (asset != null) {

            Sprite prefab =(Sprite)asset.asset;
            if (prefab != null) {

               // Sprite temp = Sprite.Create(prefab, new Rect(0, 0, prefab.width, prefab.height), new Vector2(0, 0));
                sprite.sprite = prefab;
                sprite.SetNativeSize();
                // spriteImage = Instantiate(prefab) as Sprite;
                //  testpanel.transform.SetParent(GameObject.Find("Canvas").transform);
                //   ReleaseAssetOnDestroy.Register(p, asset);

                // GameObject.Destroy(go, 10);

            }
        }
        index++;
        if (index >= spriteName.Count) index = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDestroy() {
        for (int i = 0; i < AssetList.Count; i++) {
            AssetList[i].Release();
        }
    }
}
