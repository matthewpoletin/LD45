using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nothing
{
    public enum Line
    {
        Left,
        Middle,
        Right
    }

    public class Player : MonoBehaviour
    {
        [field: SerializeField, HideInInspector]
        public Line CurrentLine { get; private set; } = Line.Middle;
        [field: SerializeField, HideInInspector]
        public Line TargetLine { get; private set; } = Line.Middle;
        [field: SerializeField, HideInInspector]
        public bool IsChangingLine { get; private set; } = false;
        [field: SerializeField, HideInInspector]
        public int Money { get; set; } = 0;
        [field: SerializeField, HideInInspector]
        public Weapon Weapon { get; set; }

        public float lineWidth = 3;
        public Weapon defaultWeapon;
        public float lineChangeDuration = 1;

        [SerializeField, HideInInspector]
        private float currentVelocity = 0;

        private void Awake()
        {
            Weapon = defaultWeapon;
        }

        public void Up()
        {
        }

        public void Down()
        {
        }

        public void Jump()
        {
        }

        public void Left()
        {
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

        public void Right()
        {
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


        private void OnLineChange()
        {
            IsChangingLine = true;
        }

        public void Update()
        {
            if (IsChangingLine)
                UpdateLinePosition();
        }


        private bool ApproxEqual(float a, float b, float tolerance) => Mathf.Abs(a - b) < tolerance;

        private void UpdateLinePosition()
        {
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
    }
}
