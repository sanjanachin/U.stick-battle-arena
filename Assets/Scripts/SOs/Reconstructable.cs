using UnityEngine;

namespace Game
{
    /**
     * Represent a reconstructable gameObject data that can constructed a given
     * gameObject to certain state by code and deconstructed back to the original given gameObject
     */
    public abstract class Reconstructable<T> : ScriptableObject
    {
        /**
         * Preform the construction on the given gameObject
         * Should ONLY be called by relevant manager or object pools
         */
        public abstract T Construct(GameObject gameObject);
        
        /**
         * Preform the deconstruction on the given gameObject
         * Should ONLY be called by relevant manager or object pools
         */
        public abstract void Deconstruct(GameObject gameObject);
    }
}