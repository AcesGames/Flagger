using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIButtonScript : MonoBehaviour
{
    public int buttonId;

    public void DrawFromTheTop()
    {
        UIManager.INSTANCE.DrawFromTheTop();
    }

    public void RandomFlag()
    {
     UIManager.INSTANCE.DrawRandomFlag();
    }

    public void ButtonClickAnswer()
    {
        UIManager.INSTANCE.AnswerFlag(gameObject);
    }

    public void WipeEverything()
    {
        UIManager.INSTANCE.WipeEverything();
    }
}
