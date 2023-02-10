using UnityEngine;
public class PinchObject : MonoBehaviour
{
    private bool _canPinch;
    [SerializeField] private Transform _characterTransform;
    private float _scaleValue = 700f;
    private Vector3 _saveScale;
    private float _saveDist;
    private float _max = 900f;
    private float _min = 500f;
    float _dist;

    private Vector2 _touchPosA;
    private Vector2 _touchPosB;
    private void Start()
    {
        _characterTransform.transform.localScale = _scaleValue * Vector3.one;
        _canPinch = false;
        _saveScale = Vector3.one * 700f;
    }
    void Update()
    {

#if UNITY_EDITOR
        return;
#endif
        Touch();
        if (!_canPinch) return;
        PinchZoom();
        SizeSetting();
    }
    private void Touch()
    {

#if UNITY_EDITOR
        return;
#endif

        if (Input.touchCount == 2)
            valueChange = true;
        else

            valueChange = false;
    }

    private bool valueChange
    {
        get { return _canPinch; }
        set
        {
            if (_canPinch != value)
            {
                if (value == true) // ÀúÀå
                {
                    _saveDist = SaveDist();
                    //_textSaveValue.text = _saveDist.ToString();
                }
                else
                {
                    _saveScale = _characterTransform.localScale;
                }
                _canPinch = value;
            }
        }
    }

    private float SaveDist()
    {
        return Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
    }

    private void PinchZoom()
    {
        _touchPosA = Input.GetTouch(0).position;
        _touchPosB = Input.GetTouch(1).position;
        _dist = Vector3.Distance(_touchPosB, _touchPosA);

        _scaleValue = _dist - _saveDist;
    }

    private void SizeSetting()
    {
        _characterTransform.localScale = _saveScale + (_scaleValue * Vector3.one);
        if (_characterTransform.localScale.x >= _max)
            _characterTransform.localScale = Vector3.one * _max;
        else if (_characterTransform.localScale.x <= _min)
            _characterTransform.localScale = Vector3.one * _min;
    }
}