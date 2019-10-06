using System.Collections.Generic;
using UnityEngine;

namespace Module.Game.Level.Obstacles
{
    [CreateAssetMenu(fileName = "New Obstacle Group Params", menuName = "Params/ObstacleGroupParams", order = 0)]
    public class ObstacleGroupParams : ScriptableObject
    {
        [SerializeField] private List<GameObject> obstacleGroups = null;

        public List<GameObject> ObstacleGroups => obstacleGroups;
    }
}