#region

using UnityEngine;

#endregion

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/UsableItem")]
    public class UsableItemDataSetSO : MonoBehaviourDataSetSO<UsableItemID, UsableItem> { }
}