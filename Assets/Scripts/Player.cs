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

    public class Player : MonoBehaviour, IPlayerControllable
    {
        [field: SerializeField, HideInInspector]
        public Line CurrentLine { get; private set; } = Line.Middle;
        [field: SerializeField, HideInInspector]
        public bool IsChangingLine { get; private set; } = false;

        public float lineWidth = 3;
        public float lineChangeDuration = 1;

        [SerializeField, HideInInspector]
        private float currentVelocity = 0;

        public PlayerInput input;

        private void Awake()
        {
            input.controllable = this;
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
            if (CurrentLine == Line.Left)
                return;

            if (CurrentLine == Line.Middle)
                CurrentLine = Line.Left;
            else if (CurrentLine == Line.Right)
                CurrentLine = Line.Middle;

            OnLineChange();
        }

        public void Right()
        {
            if (CurrentLine == Line.Right)
                return;

            if (CurrentLine == Line.Middle)
                CurrentLine = Line.Right;
            else if (CurrentLine == Line.Left)
                CurrentLine = Line.Middle;

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
            var targetX = CurrentLine == Line.Left ? -lineWidth :
                        CurrentLine == Line.Middle ? 0 :
                        CurrentLine == Line.Right ? lineWidth : 0;

            var newX = Mathf.SmoothDamp(transform.localPosition.x, targetX, ref currentVelocity, lineChangeDuration);

            if (ApproxEqual(transform.localPosition.x, targetX, .01f))
            {
                IsChangingLine = false;
                newX = targetX;
            }

            transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
