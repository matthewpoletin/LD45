using UnityEngine;
using System.Collections.Generic;

namespace Nothing
{
    [CreateAssetMenu(menuName= "ObstacleGroups")]
    public class ObstacleGroups : ScriptableObject
    {
        public List<GameObject> obstacleGroups;
    }
}