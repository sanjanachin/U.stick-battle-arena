#region

using Game.DataSet;
using UnityEngine;

#endregion

namespace Game
{
    [CreateAssetMenu(menuName = "Game/DataSet/AudioClip")]
    public class AudioDataSetSO : DataSetSO<AudioID, AudioClipSO> { }
}