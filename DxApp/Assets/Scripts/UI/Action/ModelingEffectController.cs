using Assets.Scripts.Common.Models;
using Assets.Scripts.PrefabModel;
using DXApp_AppData.Enum;
using DXApp_AppData.Item;
using DXApp_AppData.Table;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelingEffectController : MonoBehaviour
{
    [SerializeField] private ModelingEffect _body;
    [SerializeField] private ModelingEffect _head;
    [SerializeField] private ModelingEffect _deco;
    [SerializeField] private ModelingEffect _property;

    public void PlayPartsEffect(PartsType type, ModelPrefabBase model, List<PartsInstance> partsArchive)
    {
        bool isActive;

        // 액티브 되어있는지 아닌지 체크
        if (type == PartsType.Predeco || type == PartsType.Postdeco)
        {
            isActive = null != partsArchive.FirstOrDefault(v => v.CustomData.PartsType == PartsType.Predeco || v.CustomData.PartsType == PartsType.Postdeco);
        }
        else isActive = null != partsArchive.FirstOrDefault(v => v.CustomData.PartsType == type);

        switch (type)
        {
            case PartsType.Body:
                _body.ModelObj?.gameObject.SetActive(false);
                if (model != null)
                {
                    model.transform.SetParent(_body.transform);
                    model.GetComponent<BoxCollider>().enabled = false;
                    _body.ModelObj = model.transform;
                    _body.SetTransform(model.OriginPos, model.OriginScale);
                    _body.PlayPartsEffect(isActive);
                    var animator = model.GetComponent<Animator>();
                    animator.SetTrigger("Empty");
                }
                break;

            case PartsType.Head:
                _head.ModelObj?.gameObject.SetActive(false);
                if (model != null)
                {
                    model.transform.SetParent(_head.transform);
                    _head.ModelObj = model.transform;
                    _head.SetTransform(model.OriginPos, model.OriginScale);
                    _head.PlayPartsEffect(isActive);
                }
                break;

            case PartsType.Predeco:
            case PartsType.Postdeco:
                _deco.ModelObj?.gameObject.SetActive(false);
                if (model != null)
                {
                    model.transform.SetParent(_deco.transform);
                    _deco.ModelObj = model.transform;
                    _deco.SetTransform(model.OriginPos, model.OriginScale);
                    _deco.PlayPartsEffect(isActive);
                }
                break;
        }
    }

    public void PlayPropertyEffect(FigureType type, FigureTypeTable figureTypeTable)
    {
        bool isActive = false;
        // 액티브 되어있는지 아닌지 체크
        if (null == figureTypeTable)
        {
            isActive = false;
            _property.PlayPropertyEffect(isActive, type);
        }
        else if (type == figureTypeTable.FigureType)
        {
            isActive = true;
            _property.PlayPropertyEffect(isActive, type);
        }
    }
}
