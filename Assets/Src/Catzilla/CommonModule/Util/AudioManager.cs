using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class AudioManager: ScriptableObject {
        [System.Serializable]
        private struct Channel {
            public string Name;
            public int SimultaneousSoundsCount;
        }

        public bool IsPaused {
            get {return AudioListener.pause;}
            set {AudioListener.pause = value;}
        }

        [SerializeField]
        private Channel[] channels;

        private AudioSource[][] recentlyPlayedSources;

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
            recentlyPlayedSources = new AudioSource[channels.Length][];

            for (int i = 0; i < channels.Length; ++i) {
                recentlyPlayedSources[i] =
                    new AudioSource[channels[i].SimultaneousSoundsCount];
            }
        }
    }
}
