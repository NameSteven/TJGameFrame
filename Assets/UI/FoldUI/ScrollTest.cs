using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScrollTest : MonoBehaviour {

  public   FoldableMenu foldadleMenu;

    Dictionary<int, List<string>> DataDic;
    // Use this for initialization
    void Start () {
        DataDic = new Dictionary<int, List<string>>();

        DataDic[1] = new List<string>() { "12","222","33","23232",};
        DataDic[2] = new List<string>() { "12", "222", "sds33", "23232", };
        DataDic[3] = new List<string>() { "1sds2", "2sds22", "33", "23232", };
        DataDic[4] = new List<string>() { "1sds2", "22sd2", "33", "23232", };
        DataDic[5] = new List<string>() { "12", "2sds22", "3sdds3", "23232", };

        foldadleMenu.CreatScrool(DataDic, typeof(Parent), typeof(Child));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Parent : ParentItem {

    private Text text;
    public override void FindItem(Transform trans) {
        base.FindItem(trans);
        text = trans.Find("Text").GetComponent<Text>();
    }

    public override void FillItem(object parm) {
        base.FillItem(parm);
        text.text = ((int)parm).ToString();
    }
}

public class Child : ParentItem {
    private Text text;
    public override void FindItem(Transform trans) {
        base.FindItem(trans);
        text = trans.Find("Text").GetComponent<Text>();
    }

    public override void FillItem(object parm) {
        base.FillItem(parm);
        text.text = parm.ToString();
    }
}
