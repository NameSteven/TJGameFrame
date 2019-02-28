using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ParentMenu : MonoBehaviour
{
    private GameObject childMenu;//子菜单的parent
    private RectTransform[] childs;//所有子菜单的rect
    private RectTransform itemRect;//子菜单的prefab
    private Vector3 offset;//单个子菜单的高度
    private int count;//子菜单的个数
    public bool isOpening { get; private set; }//父菜单是否展开
    public bool isCanClick { get; set; }//父菜单是否可以点击

    public void Init(RectTransform rect, int count,Type t, IList dataList=null)
    {
        if (!typeof(ParentItem).IsAssignableFrom(t)) throw new Exception("传入类型必须是ParentItem的派生类");
        childMenu = transform.Find("childMenu").gameObject;
        itemRect = rect;
        this.count = count;
        childs = new RectTransform[this.count];
        offset = new Vector3(0, itemRect.rect.height);
        for (int i = 0; i < this.count; i++)
        {
            childs[i] = Instantiate(itemRect, childMenu.transform);

            ParentItem parentComponent = childs[i].GetComponent(t)as ParentItem;
            if (parentComponent == null) {
                parentComponent = childs[i].gameObject.AddComponent(t) as ParentItem;
            }
            parentComponent.FindItem(childs[i]);
            parentComponent.FillItem(dataList[i]);
            childs[i].gameObject.SetActive(true);
        }
        childMenu.gameObject.SetActive(false);
        isOpening = false;
        isCanClick = true;
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (!isCanClick) return;
        if (!isOpening)
            StartCoroutine(ShowChildMenu());
        else
            StartCoroutine(HideChildMenu());
    }

    IEnumerator ShowChildMenu()
    {
        childMenu.gameObject.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            childs[i].localPosition -= i * offset;
            yield return new WaitForSeconds(0.01f);
        }
        isCanClick = true;
        isOpening = true;
    }

    IEnumerator HideChildMenu()
    {
        for (int i = count - 1; i >= 0; i--)
        {
            childs[i].localPosition += i * offset;
            yield return new WaitForSeconds(0.01f);
        }
        childMenu.gameObject.SetActive(false);
        isCanClick = true;
        isOpening = false;
    }
}
