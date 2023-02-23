using Game.DataSet;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/DataSet/AudioClip")]
    public class AudioDataSetSO : DataSetSO<AudioID, AudioClipSO> { }
}