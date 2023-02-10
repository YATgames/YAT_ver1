using UnityEngine;
using DG.Tweening;
using DXApp_AppData.Enum;

public class ModelingEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
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

        if(!isActive)
        {
            _particle.Stop();
        }
        else
        {
            var main = _particle.main;
            switch(type)
            {
                case FigureType.Counter:
                    main.startColor = new Color((float)88 / 255, (float)158 / 255, (float)255 / 255, 1);
                    break;
                case FigureType.Strong:
                    main.startColor = new Color((float)255 / 255, (float)255 / 255, (float)88 / 255, 1);
                    break;
                case FigureType.Fast:
                    main.startColor = new Color((float)255 / 255, (float)88 / 255, (float)88 / 255, 1);
                    break;
                default:
                    Debug.LogError("FigureType Error! FigureType = " + type);
                    break;
            }

            _particle.Play();
        }
    }
}
