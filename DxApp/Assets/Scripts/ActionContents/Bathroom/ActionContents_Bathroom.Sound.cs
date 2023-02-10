using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.UI.Popup.PopupView
{
    public partial class ActionContents_Bathroom : MonoBehaviour
    {
        private void PlaySE(AudioClip clip)
        {
            if (_SE.clip != clip)
                _SE.clip = clip;
            _SE.Play();
        }

        private void PlayRandomSE(AudioClip clip1, AudioClip clip2)
        {
            int num = Random.Range(0, 2);
            if (num == 0)
                _SE.clip = clip1;
            else
                _SE.clip = clip2;

            _SE.Play();
        }
        private void PlayBGM(AudioClip clip)
        {
            _BGM.clip = clip;
            _BGM.loop = true;
            //_BGM.loop = loop;
            _BGM.Play();
        }
        private void StopSound(AudioSource source)
        {
            _BGM.Stop();
        }
    }
}