#region

using Game.DataSet;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Game
{
    /**
     * The base class for game mode logics
     */
    public abstract class GameModeLogicSO : ScriptableObject, IDataId<GameModeID>
    {
        /**
         * Invoked when the condition of game ends of the game mode is satisfied
         */
        public event UnityAction<PlayerID> OnGameEnded = delegate {  };

        public GameModeID ID { get => _id; }
        [SerializeField] private GameModeID _id;

        // the subclass should override this function and use it to
        // hook events to check for winning condition
        protected abstract void HookEvents();
        
        // the subclass should override this function and use it to
        // unhook events that are used to check for winning condition
        // since this is a SO object and will not reset itself
        protected abstract void UnHookEvents();

        public void Initialize()
        {
            // clear the OnGameEnded event for new game,
            // since this is a SO object and will not reset itself
            OnGameEnded = (_) => { };
            HookEvents();
        }

        // warped function for invoking OnGameEnded event
        // should be called when the winning condition is satisfied
        protected void InvokeGameEndedEvent(PlayerID winnerId)
        {
            UnHookEvents();
            OnGameEnded.Invoke(winnerId);
        }
    }
}