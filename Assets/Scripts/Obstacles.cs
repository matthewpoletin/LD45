using UnityEngine;
using System.Collections.Generic;

namespace Nothing
{
    [CreateAssetMenu(menuName="Obstacles")]
    public class Obstacles : ScriptableObject
    {
        public List<GameObject> obstacles;
    }
}