using System.Collections.Generic;
using Module.Game.Events;
using Module.Game.Level;
using UnityEngine;

namespace Module.Game
{
    [CreateAssetMenu(fileName = "New Game Params", menuName = "Params/GameParams", order = 0)]
    public class GameParams : ScriptableObject
    {
        [SerializeField] private List<LevelParams> levels = null;
        [SerializeField] private AudioParams audioParams = null;
        [SerializeField] private List<EventParams> events = null;

        public List<LevelParams> Levels => levels;
        public AudioParams AudioParams => audioParams;
        public List<EventParams> Events => events;

        public float lineWidth = 3;
    }
}