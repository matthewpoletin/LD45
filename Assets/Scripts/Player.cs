﻿using Module.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using Module.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public class Player : MonoBehaviour {
        [field: SerializeField, HideInInspector]
        public Line CurrentLine { get; private set; } = Line.Middle;
        [field: SerializeField, HideInInspector]
        public Line TargetLine { get; private set; } = Line.Middle;
        [field: SerializeField, HideInInspector]
        public bool IsChangingLine { get; private set; } = false;
        [field: SerializeField, HideInInspector]
        public bool IsJumping { get; private set; } = false;


        [SerializeField]
        private Health playerHealth = null;

        [field: SerializeField, HideInInspector]
        public Weapon CurrentWeapon { get; private set; } = null;

        public MeleeWeapon meleeWeapon;
        public RangedWeapon rangedWeapon;
        public BazookaWeapon bazookaWeapon;

        public float lineWidth = 3;
        public WeaponType defaultWeaponType;
        public float lineChangeDuration = 1;
        public float jumpVelocity = 10;
        public float gravity = 9.8f;

        [SerializeField, HideInInspector]
        private float currentVelocity = 0;

        [SerializeField, HideInInspector]
        private float velocityY = 0;

        private void Awake() {
            ChangeWeapon(defaultWeaponType);
        }

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
            if (CurrentWeapon != null) {
                CurrentWeapon.gameObject.SetActive(false);
            }

            var weapons = new Dictionary<WeaponType, Weapon> {
                { WeaponType.None, null },
                { WeaponType.Melee, meleeWeapon },
                { WeaponType.Ranged, rangedWeapon },
                { WeaponType.Bazooka, bazookaWeapon }
            };
            CurrentWeapon = weapons[weaponType];
            CurrentWeapon?.gameObject.SetActive(true);
        }

        public void Attack() {
            CurrentWeapon?.Attack();
        } 

        public void Jump() {
            if (IsJumping)
                return;

            IsJumping = true;

            velocityY = jumpVelocity;

            GameModule.Instance.SoundManager.PlaySfx(SfxType.Jump);
        }

        public void Left() {
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
            GameModule.Instance.SoundManager.PlaySfx(SfxType.Humiliate);
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
            velocityY -= gravity;
            var newY = transform.localPosition.y + Time.deltaTime * velocityY;

            if (newY < 0)
            {
                IsJumping = false;
                newY = 0;
            }

            transform.position = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        
        private void OnHealthDepleated() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnDamageTaken(int amount) {
            GameModule.Instance.SoundManager.PlaySfx(SfxType.Damaged);
        }
    }
}
