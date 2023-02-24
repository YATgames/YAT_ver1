using UnityEngine;
using DG.Tweening;
using DXApp_AppData.Enum;
using Assets.Scripts.Managers;

public class ModelingEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private ParticleSystem[] _propertyEffects;

    public Transform ModelObj { get; set; }

    private bool isActive = false;

    private Vector3 _originPos;
    private Vector3 _originScale;

    public void SetTransform(Vector3 pos, Vector3 scale)
    {
        _originPos = pos;
        _originScale = scale;
    }

    public void PlayPartsEffect(bool active)
    {
        isActive = active;

        _particle.Play();
        if (ModelObj == null) return;

        ModelObj.gameObject.SetActive(true);
        ModelObj.transform.localPosition = _originPos;

        if (isActive)
        {
            ModelObj.localScale = Vector3.zero;
            ModelObj.DOScale(_originScale, 0.1f);
            isActive = !isActive;
        }
        else
        {
            ModelObj.localScale = _originScale;
            ModelObj.DOScale(Vector3.zero, 0.1f);
            isActive = !isActive;
        }
    }

    public void PlayPropertyEffect(bool active, FigureType type)
    {
        isActive = active;

        if (!isActive)
        {
            switch (type)
            {
                case FigureType.Strong:
                    _propertyEffects[(int)FigureType.Strong].Stop();
                    break;
                case FigureType.Fast:
                    _propertyEffects[(int)FigureType.Fast].Stop();
                    break;
                case FigureType.Counter:
                    _propertyEffects[(int)FigureType.Counter].Stop();
                    break;
                default:
                    Debug.LogError("FigureType Error! FigureType = " + type);
                    break;
            }

            SoundManager.Instance.Stop("PropertyEffectSFX");
        }
        else
        {
            SoundManager.Instance.Stop("PropertyEffectSFX");

            switch (type)
            {
                case FigureType.Strong:
                    _propertyEffects[(int)FigureType.Strong].Play();
                    _propertyEffects[(int)FigureType.Fast].Stop();
                    _propertyEffects[(int)FigureType.Counter].Stop();
                    break;
                case FigureType.Fast:
                    _propertyEffects[(int)FigureType.Fast].Play();
                    _propertyEffects[(int)FigureType.Strong].Stop();
                    _propertyEffects[(int)FigureType.Counter].Stop();
                    break;
                case FigureType.Counter:
                    _propertyEffects[(int)FigureType.Counter].Play();
                    _propertyEffects[(int)FigureType.Strong].Stop();
                    _propertyEffects[(int)FigureType.Fast].Stop();
                    break;
                default:
                    Debug.LogError("FigureType Error! FigureType = " + type);
                    break;
            }
            SoundManager.Instance.Play("PropertyEffect_SFX");
        }
    }
}
