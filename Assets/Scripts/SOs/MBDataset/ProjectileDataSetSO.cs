#region

using UnityEngine;

#endregion

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/Projectile")]
    public class ProjectileDataSetSO : MonoBehaviourDataSetSO<ProjectileID, Projectile> { }
}