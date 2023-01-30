using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Reconstructable<T> : ScriptableObject
    {
        public abstract T Construct(GameObject gameObject);
        public abstract void Deconstruct(GameObject gameObject);
    }
}