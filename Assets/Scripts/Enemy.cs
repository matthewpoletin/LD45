using System;
using UnityEngine;
using System.Collections;
using Module.Game;
using Module.Game.Level.Obstacles;
using EventType = Module.Game.Events.EventType;
using Random = UnityEngine.Random;

namespace Nothing
{
    [RequireComponent(typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        public int damage = 1;
        public bool dieInColissionWithObstacles = true;
        public bool dieInColissionWithPlayer = true;
        public bool avoidObstacles = true;
        public float maxDistanceToObstacleWhenChangingLine = 3;
        public float lineChangeDuration = .3f;

        [SerializeField]
        private Health health;
        [SerializeField, HideInInspector]
        private int currentLine;
        [SerializeField, HideInInspector]
        private int targetLine;
        [SerializeField, HideInInspector]
        private float currentVelocity;
        [SerializeField, HideInInspector]
        private bool isChangingLine = false;

        public Health Health => health;

        private void Awake() {
            health = GetComponent<Health>();

            health.OnHealthDepleated += OnEnemyDestroyed;

            currentLine = targetLine = CalculateCurrentLine();
        }

        public virtual void OnEnemyDestroyed()
        {
            GameModule.Instance.EventManager.ProcessEvent(EventType.EnemyDeath);
            Destroy(gameObject);
        }

        private int CalculateCurrentLine() {
            return (int)(transform.position.x / GameModule.Instance.GameParams.lineWidth);
        }

        private void Update() {
            if (!avoidObstacles)
                return;

            if (isChangingLine)
                UpdateLinePosition();
            else {
                for (float y = 0; y < 3; y += 0.3f) {
                    if (Physics.Raycast(transform.position + new Vector3(0, y, 0),
                        -transform.forward, out var hit, maxDistanceToObstacleWhenChangingLine)) {
                        var obstacle = hit.collider.gameObject.GetComponent<Obstacle>();
                        if (obstacle != null) {
                            var currentLine = (int)(transform.position.x / GameModule.Instance.GameParams.lineWidth);
                            targetLine = 0;
                            if (currentLine == 0)
                                targetLine = Random.value > .5f ? -1 : 1;
                            isChangingLine = true;
                        }
                    }
                }
            }
        }

        private bool ApproxEqual(float a, float b, float tolerance) => Mathf.Abs(a - b) < tolerance;

        private void UpdateLinePosition() {
            var lineWidth = GameModule.Instance.GameParams.lineWidth;
            var targetX = targetLine == -1 ? -lineWidth :
                        targetLine == 0 ? 0 :
                        targetLine == 1 ? lineWidth : 0;

            var newX = Mathf.SmoothDamp(transform.localPosition.x, targetX, ref currentVelocity, lineChangeDuration);

            if (Mathf.Abs(transform.localPosition.x - targetX) < lineWidth / 2f)
                currentLine = targetLine;


            if (ApproxEqual(transform.localPosition.x, targetX, .01f)) {
                isChangingLine = false;
                newX = targetX;
            }

            transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        }

        private void OnDrawGizmosSelected() {
            for (float y = 0; y < 3; y += 0.3f) {
                Gizmos.DrawLine(transform.position + new Vector3(0, y, 0),
                    transform.position + new Vector3(0, y, 0) + (-transform.forward) * maxDistanceToObstacleWhenChangingLine);
            }
        }

        public void OnTriggerEnter(Collider other) {
            var obstacleGroup = other.gameObject.GetComponent<Obstacle>();

            if (obstacleGroup != null && dieInColissionWithObstacles) {
                health.Kill();
                return;
            }

            var player = other.gameObject.GetComponent<Player>();

            if (player == null)
                return;

            player.GetComponent<Health>().Damage(damage);

            if (dieInColissionWithPlayer)
                health.Kill();
        }
    }
}
