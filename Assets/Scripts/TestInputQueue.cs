using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInputQueue : MonoBehaviour
{
    Transform buttonParent;
    public Transform prefabButtonItem;
    public int removeTime = 30;
    public bool autoRemoveButton = false;

    void Start()
    {
        buttonParent = transform.Find("Buttons");
    }

    public void AddButton(char c)
    {
        Sprite sp = SpriteManager.Instance.Get(c);
        Transform btnItem = Instantiate(prefabButtonItem, buttonParent);
        btnItem.GetComponent<Image>().sprite = sp;
    }

    public void RemoveButton()
    {
        if (buttonParent.childCount > 0)
        {
            DestroyImmediate(buttonParent.GetChild(0).gameObject);
        }
    }

    public void ClearButtons()
    {
        while (buttonParent.childCount > 0)
        {
            DestroyImmediate(buttonParent.GetChild(0).gameObject);
        }
    }

    public int CurrentInputDir()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.A))
            {
                return 7;
            }
            if (Input.GetKey(KeyCode.D))
            {
                return 9;
            }
            return 8;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A))
            {
                return 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                return 3;
            }
            return 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            return 4;
        }
        if (Input.GetKey(KeyCode.D))
        {
            return 6;
        }
        return 0;
    }

    public char CurrentInputHit()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            return 'p';
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            return 'k';
        }
        return '\0';
    }

    int lastInputDir;
    int removeCounter = 0;

    void Update()
    {
        int inputDir = CurrentInputDir();
        if (inputDir > 0)
        {
            if (inputDir != lastInputDir)
            {
                lastInputDir = inputDir;
                AddButton(inputDir.ToString()[0]);
            }
        }

        char hit = CurrentInputHit();
        if (hit != '\0')
        {
            AddButton(hit);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearButtons();
        }

        if (autoRemoveButton)
        {
            if (buttonParent.childCount > 25)
            {
                RemoveButton();
                removeCounter = removeTime;
            }

            if (buttonParent.childCount > 0)
            {
                removeCounter--;
                Debug.LogFormat($"{removeCounter}  {Time.time} ");
            }

            if (removeCounter <= 0)
            {
                RemoveButton();
                removeCounter = removeTime;
            }
        }

    }
}
