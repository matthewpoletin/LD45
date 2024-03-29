﻿using System.Collections;
using Module.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Module.Game.Events.EventType;

namespace Nothing
{
    public enum Line {
        Left,
        Middle,
        Right
    }

    public enum WeaponType {
        None,
        Melee,
        Ranged,
        Bazooka
    }

    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;

        public bool IsActive { get; set; } = true;
        public Line CurrentLine { get; private set; } = Line.Middle;
        public Line TargetLine { get; private set; } = Line.Middle;
        public bool IsChangingLine { get; private set; } = false;
        public bool IsJumping { get; private set; } = false;

        [SerializeField]
        private Health playerHealth = null;
        public Weapon CurrentWeapon { get; private set; } = null;

        public MeleeWeapon meleeWeapon;
        public RangedWeapon rangedWeapon;
        public BazookaWeapon bazookaWeapon;
        
        public WeaponType defaultWeaponType;
        public float lineChangeDuration = 1;
        public float jumpVelocity = 10;
        public float gravity = 9.8f;

        private float currentVelocity = 0;
        private float velocityY = 0;

        private void OnEnable() {
            playerHealth.OnHealthDepleated += OnHealthDepleated;
            playerHealth.OnDamageTaken += OnDamageTaken;
        }

        private void OnDisable() {
            playerHealth.OnHealthDepleated -= OnHealthDepleated;
            playerHealth.OnDamageTaken -= OnDamageTaken;
        }

        public void Up() {
        }

        public void Down() {
        }

        public void ChangeWeapon(WeaponType weaponType) {
            if (weaponType == WeaponType.Bazooka || weaponType == WeaponType.Ranged)
            {
                animator.SetBool("WithSword", false);
            }
            else
            {
                animator.SetBool("WithSword", true);
            }
            
            if (CurrentWeapon != null) {
                
                CurrentWeapon.SetWeaponEnabled(false);
            }

            var weapons = new Dictionary<WeaponType, Weapon> {
                { WeaponType.None, null },
                { WeaponType.Melee, meleeWeapon },
                { WeaponType.Ranged, rangedWeapon },
                { WeaponType.Bazooka, bazookaWeapon }
            };
            CurrentWeapon = weapons[weaponType];
            CurrentWeapon?.SetWeaponEnabled(true);
        }

        public void Attack() {
            if (!IsActive)
            {
                return;
            }

            animator.SetTrigger("Attack");
            CurrentWeapon?.Attack();
        } 

        public void Jump() {
            if (!IsActive)
            {
                return;
            }
            
            if (IsJumping)
                return;

            IsJumping = true;

            velocityY = jumpVelocity;

            GameModule.Instance.EventManager.ProcessEvent(EventType.PlayerJump, transform, transform.position);
        }

        public void Left() {
            if (!IsActive)
            {
                return;
            }
            
            if (CurrentLine != TargetLine)
                return;

            if (TargetLine == Line.Left)
                return;

            if (TargetLine == Line.Middle)
                TargetLine = Line.Left;
            else if (TargetLine == Line.Right)
                TargetLine = Line.Middle;

            OnLineChange();
        }

        public void Right() {
            if (!IsActive)
            {
                return;
            }
            
            if (CurrentLine != TargetLine)
                return;

            if (TargetLine == Line.Right)
                return;

            if (TargetLine == Line.Middle)
                TargetLine = Line.Right;
            else if (TargetLine == Line.Left)
                TargetLine = Line.Middle;

            OnLineChange();
        }

        public void Humiliate() {
            GameModule.Instance.EventManager.ProcessEvent(EventType.PlayerHumiliate, transform, transform.position + Vector3.up * 1f);
        }
        private void OnLineChange()
        {
            IsChangingLine = true;
        }

        public void Update() {
            if (IsChangingLine)
                UpdateLinePosition();

            if (IsJumping)
                UpdateYPosition();
        }


        private bool ApproxEqual(float a, float b, float tolerance) => Mathf.Abs(a - b) < tolerance;

        private void UpdateLinePosition() {
            var lineWidth = GameModule.Instance.GameParams.lineWidth;
            var targetX = TargetLine == Line.Left ? -lineWidth :
                        TargetLine == Line.Middle ? 0 :
                        TargetLine == Line.Right ? lineWidth : 0;

            var newX = Mathf.SmoothDamp(transform.localPosition.x, targetX, ref currentVelocity, lineChangeDuration);

            if (Mathf.Abs(transform.localPosition.x - targetX) < lineWidth / 2f)
                CurrentLine = TargetLine;


            if (ApproxEqual(transform.localPosition.x, targetX, .01f))
            {
                IsChangingLine = false;
                newX = targetX;
            }

            transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        }

        private void UpdateYPosition() {
            velocityY -= gravity * Time.deltaTime;
            var newY = transform.localPosition.y + Time.deltaTime * velocityY;

            if (newY < 0)
            {
                IsJumping = false;
                newY = 0;
            }

            transform.position = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        
        private void OnHealthDepleated()
        {
            animator.SetTrigger("Death");
            StartCoroutine(StartSceneAfter());
        }

        private IEnumerator StartSceneAfter()
        {
            yield return new WaitForSeconds(0.33f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDamageTaken(int amount) {
            GameModule.Instance.EventManager.ProcessEvent(EventType.PlayerDamaged, transform, transform.position);
        }
    }
}
