using Assets.Scripts.Common;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SoundManager : UnitySingleton<SoundManager>
    {
        private Dictionary<string, string> _soundTable = new Dictionary<string, string>();
        private Dictionary<string, AudioClip> _sfxs = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> _bgms = new Dictionary<string, AudioClip>();

        private List<AudioSource> _sfxPool = new List<AudioSource>();
        private AudioSource _bgmPool;

        private float _volume;
        public override void Initialize()
        {
            base.Initialize();
            /*
            var data = Resources.Load("Tables/sound_table") as TextAsset;
            var csvFile = CSVReader.Read(data.text);
            for (int i = 0; i < csvFile.Count; i++)
            {
                var key = csvFile[i]["Key"].ToString();
                var fileName = csvFile[i]["FileName"].ToString();

                if (_soundTable.ContainsKey(key) == true)
                {
                    LogManager.Error("Sound Table Áßº¹ Å° : {0}", key);
                    continue;
                }

                _soundTable.Add(key, fileName);
            }*/
        }

        public void SetVolume(float value)
        {
            _volume = value;

            foreach (var sfx in _sfxPool)
            {
                sfx.volume = _volume;
            }

            if (_bgmPool != null) _bgmPool.volume = _volume;
        }

        private string GetFileName(string key)
        {
            if (_soundTable.ContainsKey(key))
                return _soundTable[key];
            else
            {
                LogManager.Error("Not Found Sound Key : {0}", key);
                return string.Empty;
            }
        }

        private AudioClip GetSfxClip(string key)
        {
            AudioClip clip = null;
            var fileName = GetFileName(key);

            if (_sfxs.TryGetValue(fileName, out clip) == false)
            {
                clip = Resources.Load<AudioClip>(string.Format("Sounds/SFX/{0}", fileName));

                if (clip == null)
                    LogManager.ServerKeepLog("SoundManager ==> GetSfxClip Error==> {0}", fileName);
                else
                    _sfxs.Add(fileName, clip);
            }

            return clip;
        }

        private AudioClip GetBGMClip(string key)
        {
            AudioClip clip = null;
            var fileName = GetFileName(key);

            if (_bgms.TryGetValue(fileName, out clip) == false)
            {
                clip = Resources.Load<AudioClip>(string.Format("Sounds/BGM/{0}", fileName));

                if (clip == null)
                    LogManager.ServerKeepLog("SoundManager ==> GetBGMClip Error==> {0}", fileName);
                else
                    _bgms.Add(fileName, clip);
            }

            return clip;
        }

        private AudioSource GetSFXPool()
        {
            foreach (var audio in _sfxPool)
            {
                if (audio.isPlaying == false) return audio;
            }

            var a = gameObject.AddComponent<AudioSource>();
            a.loop = false;
            a.volume = _volume;
            _sfxPool.Add(a);
            return a;
        }

        public void PlayBGM(string key)
        {
            if (_bgmPool == null)
            {
                _bgmPool = gameObject.AddComponent<AudioSource>();
                _bgmPool.loop = true;
                _bgmPool.volume = _volume;
            }

            _bgmPool.clip = GetBGMClip(key);
            _bgmPool.Play();
        }

        public void StopBGM()
        {
            if (_bgmPool == null)
                return;

            if (_bgmPool.isPlaying == false)
                return;

            _bgmPool.Stop();
        }

        public void Play(string key)
        {
            var source = GetSFXPool();
            source.clip = GetSfxClip(key);
            source.Play();
        }
        public void Stop(string key)
        {
            foreach (var sound in _sfxPool)
            {
                if (sound.clip.name == _sfxs[key].name && sound.isPlaying == true)
                    sound.Stop();
            }
        }
    }
}
