using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class GameObjectPool : MonoBehaviour
    {
        private Transform _utilizationContainer;

        private static List<Tuple<GameObject, GameObject>> _pool = new List<Tuple<GameObject, GameObject>>();

        public void Init(Transform utilizationContainer = null)
        {
            _utilizationContainer = utilizationContainer != null ? utilizationContainer : gameObject.transform;
        }

        public GameObject GetObject(GameObject prefab, Transform container = null)
        {
            // find object if exists
            foreach (var item in _pool)
            {
                var itemPrefab = item.Item1;
                var itemGameObject = item.Item2;
                if (!itemGameObject.activeInHierarchy && itemGameObject.transform.parent == _utilizationContainer && itemPrefab == prefab)
                {
                    itemGameObject.SetActive(true);
                    itemGameObject.transform.parent = container;
                    return itemGameObject;
                }
            }

            // create object if object not found
            return AddObject(prefab, container);
        }

        public GameObject AddObject(GameObject prefab, Transform container)
        {
            var instance = Instantiate(prefab, container);
            _pool.Add(Tuple.Create(prefab, instance));
            return instance;
        }

        public void UtilizeObject(GameObject utilizedGameObject)
        {
            utilizedGameObject.gameObject.SetActive(false);
            utilizedGameObject.transform.parent = _utilizationContainer;
        }
    }
}