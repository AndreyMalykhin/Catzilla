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

        public bool IsPaused {
            get {return AudioListener.pause;}
            set {AudioListener.pause = value;}
        }

        [SerializeField]
        private Channel[] channels;

        [NonSerialized]
        private IDictionary<int, AudioSource[]> recentlyPlayedSources =
            new Dictionary<int, AudioSource[]>(8);

        public void Play(AudioClip sound, AudioSource audioSource,
            int channelId, float pitch = 1f) {
            // DebugUtils.Log("AudioManager.Play(); sound={0};", sound);
            int freeSlot = GetFreeChannelSlot(channelId);

            if (freeSlot == -1) {
                return;
            }

            recentlyPlayedSources[channelId][freeSlot] = audioSource;
            audioSource.clip = sound;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        private int GetFreeChannelSlot(int channelId) {
            AudioSource[] channelRecentlyPlayedSources =
                recentlyPlayedSources[channelId];

            for (int i = 0; i < channelRecentlyPlayedSources.Length; ++i) {
                if (channelRecentlyPlayedSources[i] != null
                    && channelRecentlyPlayedSources[i].isPlaying) {
                    continue;
                }

                return i;
            }

            return -1;
        }

        private void OnEnable() {
            // because of unity's bug
            IsPaused = false;

            for (int i = 0; i < channels.Length; ++i) {
                Channel channel = channels[i];
                recentlyPlayedSources.Add(channel.Id,
                    new AudioSource[channel.SimultaneousSoundsCount]);
            }
        }
    }
}
