using System.Collections.Generic;
using Module.Game.Level;
using UnityEngine;

namespace Module.Game
{
    [CreateAssetMenu(fileName = "New Game Params", menuName = "Params/GameParams", order = 0)]
    public class GameParams : ScriptableObject
    {
        [SerializeField] private List<LevelParams> levels = null;

        public List<LevelParams> Levels => levels;
    }
}