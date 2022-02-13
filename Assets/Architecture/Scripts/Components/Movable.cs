using System;
using UnityEngine;

namespace Components
{
    [Serializable] public struct Movable
    {
        public float Speed;
        [HideInInspector] public MovableState State;
        [HideInInspector] public bool IsMovable;
    }

    public enum MovableState
    {
        Stand, Walk, Run
    }
}