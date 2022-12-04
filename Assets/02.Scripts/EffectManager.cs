using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private ParticleSystem fx_Touch;


    private void Awake()
    {
        fx_Touch = Resources.Load<ParticleSystem>("#HAM_Programming/Prefabs/TouchEffect");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fx_Touch.Play();
        }
    }
}
