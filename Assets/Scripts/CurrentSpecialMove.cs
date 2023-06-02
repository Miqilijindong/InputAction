using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentSpecialMove : MonoBehaviour
{
    public static CurrentSpecialMove Instance { get; private set; }

    SpecialMoveItem current;

    public Text textName;

    int delayTime;

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerMove(SpecialMoveItem move)
    {
        if (current==null || current.priority < move.priority)
        {
            delayTime = 60;
            current = move;
        }
    }

    public void LateUpdate()
    {
        delayTime--;
        if (delayTime <= 0)
        {
            current = null;
            textName.text = "无";
            return;
        }

        if (current != null)
        {
            textName.text = current.moveName;
        }
        current = null;
    }
}
