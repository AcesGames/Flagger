using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonAutoClick : MonoBehaviour
{
    public bool startClick;
    private void Start()
    {
        if (startClick)
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
