using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class GameObjectPool : MonoBehaviour
    {
        private Transform _utilizationContainer;

        private static List<GameObject> _pool = new List<GameObject>();

        public void Init(Transform utilizationContainer = null)
        {
            _utilizationContainer = utilizationContainer != null ? utilizationContainer : gameObject.transform;
        }

        public GameObject GetObject(GameObject prefab, Transform container = null)
        {
            // find object if exists
            foreach (var item in _pool)
            {
                if (!item.activeInHierarchy && item.transform.parent == _utilizationContainer)
                {
                    item.SetActive(true);
                    item.transform.parent = container;
                    return item;
                }
            }

            // create object if object not found
            return AddObject(prefab, container);
        }

        public GameObject AddObject(GameObject prefab, Transform container)
        {
            var instance = Instantiate(prefab, container);
            _pool.Add(instance);
            return instance;
        }

        public void UtilizeObject(GameObject utilizedGameObject)
        {
            utilizedGameObject.gameObject.SetActive(false);
            utilizedGameObject.transform.parent = _utilizationContainer;
        }
    }
}