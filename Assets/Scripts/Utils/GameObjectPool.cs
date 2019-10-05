using System.Collections.Generic;
using System.Linq;
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
            var poolGameObject = _pool.FirstOrDefault(item => item == prefab);
            if (poolGameObject != default(GameObject))
            {
                _pool.Remove(poolGameObject);
                poolGameObject.SetActive(true);
                return poolGameObject;
            }

            // create object if object not found
            return Instantiate(prefab, container);
        }

        public void UtilizeObject(GameObject utilizedGameObject)
        {
            _pool.Add(utilizedGameObject);
            utilizedGameObject.gameObject.SetActive(false);
            utilizedGameObject.transform.parent = _utilizationContainer;
        }
    }
}