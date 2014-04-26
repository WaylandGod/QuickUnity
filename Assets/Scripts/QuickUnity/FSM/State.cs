using System.Collections;
using UnityEngine;

namespace QuickUnity.FSM
{
    /// <summary>
    /// A abstract state class.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// When state machine enter this state call.
        /// </summary>
        /// <param name="entity">An entity instance.</param>
        public abstract void OnEnter(Entity entity);

        /// <summary>
        /// When state machine exit this state call.
        /// </summary>
        /// <param name="entity">An entity instance.</param>
        public abstract void OnExit(Entity entity);
    }
}