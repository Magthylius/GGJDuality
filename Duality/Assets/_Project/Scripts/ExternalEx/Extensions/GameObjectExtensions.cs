using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class GameObjectEx
    {
        public static GameObject New(string name, Vector3 position)
        {
            GameObject newGameObject = new GameObject(name);
            newGameObject.transform.position = position;
            return newGameObject;
        }

        public static GameObject New(string name, Vector3 position, Quaternion rotation)
        {
            GameObject newGameObject = New(name, position);
            newGameObject.transform.rotation = rotation;
            return newGameObject;
        }

        public static GameObject New(string name, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject newGameObject = New(name, position, rotation);
            newGameObject.transform.SetParent(parent);
            return newGameObject;
        }

        public static GameObject New(string name, Vector3 position, Transform parent)
        {
            GameObject newGameObject = New(name, position, Quaternion.identity);
            newGameObject.transform.SetParent(parent);
            return newGameObject;
        }

        public static GameObject New(string name, Transform parent)
        {
            GameObject newGameObject = new GameObject(name);
            newGameObject.transform.SetParent(parent);
            return newGameObject;
        }

        public static void SetAllLayers(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;

            foreach (Transform child in gameObject.transform)
                SetAllLayers(child.gameObject, layer);
            
        }
        public static void Destroy(this GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}
