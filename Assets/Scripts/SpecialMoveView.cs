using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialMoveView : MonoBehaviour
{
    public string moveName;
    public string inputs;

    bool ok;

    Text labelName;
    Transform buttonParent;
    Transform OK;

    public Transform prefabButtonItem;

    int RESET_TIME = 100;
    int resetTime;

    void Start()
    {
        labelName = transform.Find("Name").GetComponent<Text>();
        buttonParent = transform.Find("Buttons");
        OK = transform.Find("OK");
        ok = false;

        InitUI();
    }

    public void InitUI()
    {
        labelName.text = moveName;
        foreach (char c in inputs)
        {
            Sprite sp = SpriteManager.Instance.Get(c);
            Transform btnItem = Instantiate(prefabButtonItem, buttonParent);
            btnItem.GetComponent<Image>().sprite = sp;
        }
        OK.gameObject.SetActive(false);
    }

}
