using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenAnimEvent : MonoBehaviour
{
   public void SoundEvent()
    {
        SoundManager.Instance.Play("CaseButtonOpenSound_SFX");
    }
}
