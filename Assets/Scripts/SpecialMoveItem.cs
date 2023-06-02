using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpecialMoveData
{
    public int curIndex = 0;
    public int time = 0;
}

/// <summary>
/// 特殊移动类
/// 如搓技能的那种
/// </summary>
public class SpecialMoveItem : MonoBehaviour
{
    public string moveName;
    /// <summary>
    /// 预定的指令输入
    /// 写法的话，是按照小键盘的方式来判断方向的，5则是无输入
    /// </summary>
    public string inputs;
    /// <summary>
    /// 简易指令
    /// 1:就是可以通过对指令相近的按键进行判断是否正确
    /// 0:就是必须按对指令
    /// </summary>
    public string simples;
    public int moveTime;
    /// <summary>
    /// 优先级，越大越高
    /// </summary>
    public int priority;

    bool ok;

    Text labelName;
    Transform buttonParent;
    Transform OK;

    SpecialMoveData moveData;

    public Transform prefabButtonItem;

    int RESET_TIME = 100;
    int resetTime;

    void Start()
    {
        labelName = transform.Find("Name").GetComponent<Text>();
        buttonParent = transform.Find("Buttons");
        OK = transform.Find("OK");

        moveData = new SpecialMoveData();

        simples += new string('0', inputs.Length - simples.Length);

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

    public void RefreshUI()
    {
        int i = 0;
        for (i=0; i<moveData.curIndex+1; i++)
        {
            Image im = buttonParent.GetChild(i).GetComponent<Image>();
            im.color = Color.green;
        }
        for (; i<buttonParent.childCount; i++)
        {
            Image im = buttonParent.GetChild(i).GetComponent<Image>();
            im.color = Color.white;
        }
        OK.gameObject.SetActive(ok);
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
        return 5;
    }

    public int CurrentInputKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return 8;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            return 2;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            return 4;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            return 6;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            return 11;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            return 12;
        }
        return 0;
    }

    public int CurrentInputHit()
    {
        if (Input.GetKey(KeyCode.K))
        {
            return 1;
        }
        if (Input.GetKey(KeyCode.J))
        {
            return 2;
        }
        return 0;
    }

    int _Char2Dir(char c)
    {
        c = char.ToLower(c);
        if (c == 'p' || c == 'k')
        {
            return -1;
        }
        // 判断是否数字
        if (char.IsDigit(c))
        {
            return int.Parse(c.ToString());
        }
        return -1;
    }

    int _Char2Hit(char c)
    {
        c = char.ToLower(c);
        if (c == 'p')
        {
            return 1;
        }
        if (c == 'k')
        {
            return 2;
        }
        return -1;
    }

    /// <summary>
    /// 相近的摇杆方向
    /// </summary>
    Dictionary<int, int[]> dictSimpleDir = new Dictionary<int, int[]>
    {
        {8, new int[] {7,9} },
        {9, new int[] {8,6} },
        {6, new int[] {9,3} },
        {3, new int[] {6,2} },
        {2, new int[] {1,3} },
        {1, new int[] {4,2} },
        {4, new int[] {7,1} },
        {7, new int[] {4,8} },
    };

    bool IsWrongDir(int a, int b)
    {
        if (a == b) { return false; }
        if (a==0 || b==0 || a==-1 || b==-1) { return false; }

        if (dictSimpleDir.ContainsKey(a))
        {
            int[] nearDir = dictSimpleDir[a];
            if (nearDir.Contains(b))
            {
                return false;
            }
        }
        Debug.LogFormat($"{moveName} Wrong {a}=={b} : {moveData.curIndex}");
        return true;
    }

    bool IsCorrectDir(int a, int b)
    {
        if (a==0 || b==0 || a==-1 || b==-1) { return false; }
        if (a == b) {return true; }

        if (dictSimpleDir.ContainsKey(a))
        {
            int[] nearDir = dictSimpleDir[a];
            if (nearDir.Contains(b))
            {
                return true;
            }
        }
        return false;
    }

    void FixedUpdate()
    {
        if (ok)
        {
            resetTime--;
            if (resetTime == 0)
            {
                moveData.curIndex = 0;

                print("zero! " + 1111);
                moveData.time = 0;
                ok = false;
                RefreshUI();
            }
            return;
        }

        if (moveData.curIndex> 0 && moveData.time < 0)
        {
            //print("move time < 0 " + moveData.time);
            print("zero! " + 2222 + " " + Time.time);
            moveData.time = 0;
            moveData.curIndex = 0;
            RefreshUI();
            //return;
        }

        int requireDir = _Char2Dir(inputs[moveData.curIndex]);
        int inputDir = CurrentInputDir();
        bool match = false;
        if (simples[moveData.curIndex] == '1')
        {
            if (IsCorrectDir(requireDir, inputDir))
            {
                match = true;
            }
        }
        else
        {
            if (requireDir == inputDir)
            {
                match = true;
            }
        }

        int requireHit = _Char2Hit(inputs[moveData.curIndex]);
        int inputHit = CurrentInputHit();
        if (requireHit == inputHit)
        {
            match = true;
        }

        if (!match)
        {
            int keyDown = CurrentInputKeyDown();
            //if (keyDown != 0)
            //{
            //    Debug.Log("Key down:" + keyDown + "  inputDir:" + inputDir);
            //}
            if (keyDown > 0)
            {
                if (keyDown > 10 || IsWrongDir(requireDir, keyDown))
                {
                    Debug.LogFormat($"Wrong {keyDown} {requireDir}");
                    print("zero! " + 3333);
                    moveData.curIndex = 0;
                    moveData.time = 0;
                    ok = false;
                    RefreshUI();
                    return;
                }
            }
        }

        if (match)
        {
            Debug.LogFormat($"match {moveData.curIndex} {Time.time}");
            moveData.curIndex++;
            moveData.time = moveTime;
            RefreshUI();

            // check ok
            if (moveData.curIndex == inputs.Length)
            {
                ok = true;
                resetTime = RESET_TIME;
                RefreshUI();

                CurrentSpecialMove.Instance.TriggerMove(this);
            }
        }
        else
        {
            moveData.time--;
        }
    }
}
