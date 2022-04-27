using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetReadyPlayer : MonoBehaviour
{
    private Animator anim;
    private void OnEnable()
    {
        Time.timeScale = 1;
        anim = GetComponent<Animator>();
        anim.Play("FadeOut", -1, 0.0f);
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void Play()
    {
        UIManager.INSTANCE.CustomPlay();
    }
}
