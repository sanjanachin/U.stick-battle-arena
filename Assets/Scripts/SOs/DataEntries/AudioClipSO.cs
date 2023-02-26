#region

using System;
using Game.DataSet;
using UnityEngine;

#endregion

namespace Game
{
    public enum AudioID
    {
        None = -1,
        // TODO: Add ID
        Death = 0,
        Damage = 1,
        PistolUse = 2,
        PistolSwitch = 3,
        SMGUse = 4,
        SMGSwitch = 5,
        ShotgunUse = 6,
        ShotgunSwitch = 7,
        SniperUse = 8,
        SniperSwitch = 9,
        BowPull = 10,
        BowUse = 11,
        BowSwitch = 12,
        DaggerUse = 13,
        DaggerSwitch = 14,
    }
    
    [CreateAssetMenu(menuName = "Game/DataEntry/AudioClip")]
    public class AudioClipSO : ScriptableObject, IDataId<AudioID>
    {
        public AudioClip AudioClip { get => _audioClip; }
        public AudioID ID { get => _id; }
        public AudioClipPlaySetting DefaultSetting { get => _defaultSetting; }
        
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private AudioID _id;
        [SerializeField] private AudioClipPlaySetting _defaultSetting;
    }

    [Serializable]
    public struct AudioClipPlaySetting
    {
        public float Pitch { get => _pitch; }
        public float Volume { get => _volume; }
        public bool Loop { get => _loop; }

        [Header("Note that pitch cannot be 0")]
        [Range(-5f, 5f)]
        [SerializeField] private float _pitch;
        [Range(0f, 3f)]
        [SerializeField] private float _volume;
        [SerializeField] private bool _loop;

        public AudioClipPlaySetting Create(float pitch, float volume, bool loop)
        {
            return new AudioClipPlaySetting()
            {
                _pitch = pitch,
                _volume = volume,
                _loop = loop,
            };
        }
    }
}