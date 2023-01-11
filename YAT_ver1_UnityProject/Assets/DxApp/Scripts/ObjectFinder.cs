using UnityEngine;

namespace Assets.Scripts.Common
{
    public class ObjectFinder
    {
        public static GameObject Find(string objectName)
        {
            GameObject findObject = GameObject.Find("objectName");
            return findObject;
        }
        public static Transform FindChild(GameObject parentObject, string objectName)
        {
            Transform transform = parentObject.transform.Find(objectName);
            return transform;
        }
        public static Transform FindChild(Transform parentTransform, string objectName)
        {
            Transform transform = parentTransform.Find(objectName);
            return transform;
        }
    }
}