using System.Collections;
using UnityEngine;

namespace QuickUnity.FSM
{
    /// <summary>
    /// State controller.
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        private State currentState = null;

        /// <summary>
        /// The current state.
        /// </summary>
        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }
    }
}