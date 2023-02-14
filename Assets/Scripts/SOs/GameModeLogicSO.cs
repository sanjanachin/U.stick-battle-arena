using Game.DataSet;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public abstract class GameModeLogicSO : ScriptableObject, IDataId<GameModeID>
    {
        public event UnityAction OnGameEnded = delegate {  };

        public GameModeID ID { get => _id; }
        
        [SerializeField] private GameModeID _id;

        protected abstract void InitializeLogic();

        public void Initialize()
        {
            OnGameEnded = () => { };
            InitializeLogic();
        }
        
        protected void InvokeGameEndedEvent()
        {
            OnGameEnded.Invoke();
        }
    }
}