using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class AudioManager: ScriptableObject {
        [Serializable]
        private struct Channel {
            public string Name;
            public int Id;
            public int SimultaneousSoundsCount;
        }

        private struct RecentPlay {
            public AudioSource Source;
            public float Time;
        }

        public bool IsPaused {
            get {return AudioListener.pause;}
            set {AudioListener.pause = value;}
        }

        [SerializeField]
        private Channel[] channels;

        [SerializeField]
        [Tooltip("In seconds")]
        private float soundMergePeriod;

        [NonSerialized]
        private IDictionary<int, RecentPlay[]> recentPlays =
            new Dictionary<int, RecentPlay[]>(8);

        public void Play(AudioClip sound, AudioSource audioSource,
            int channelId, float pitch = 1f) {
            // DebugUtils.Log("AudioManager.Play(); sound={0};", sound);
            int freeSlot = GetSuitableChannelSlot(channelId, sound);

            if (freeSlot == -1) {
                return;
            }

            audioSource.clip = sound;
            audioSource.pitch = pitch;
            audioSource.Play();
            recentPlays[channelId][freeSlot] = new RecentPlay{
                Source = audioSource, Time = Time.realtimeSinceStartup};
        }

        /**
         * @return -1 if no suitable slot
         */
        private int GetSuitableChannelSlot(int channelId, AudioClip sound) {
            // DebugUtils.Log("AudioManager.GetSuitableChannelSlot()");
            RecentPlay[] channelRecentPlays = recentPlays[channelId];
            int suitableSlot = -1;

            for (int i = 0; i < channelRecentPlays.Length; ++i) {
                var recentPlay = channelRecentPlays[i];
                AudioSource recentSource = recentPlay.Source;

                if (recentSource == null) {
                    suitableSlot = i;
                } else if (recentSource.clip == sound
                           && recentPlay.Time + soundMergePeriod >=
                               Time.realtimeSinceStartup) {
                    return -1;
                } else if (!recentSource.isPlaying) {
                    suitableSlot = i;
                }
            }

            return suitableSlot;
        }

        private void OnEnable() {
            // because of unity's bug
            IsPaused = false;

            for (int i = 0; i < channels.Length; ++i) {
                Channel channel = channels[i];
                recentPlays.Add(channel.Id,
                    new RecentPlay[channel.SimultaneousSoundsCount]);
            }
        }
    }
}
