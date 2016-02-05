using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace Catzilla.CommonModule.Util {
    [CreateAssetMenuAttribute]
    public class AudioManager: ScriptableObject {
        public bool IsPaused {
            get {return AudioListener.pause;}
            set {AudioListener.pause = value;}
        }

        [System.Serializable]
        private struct ChannelParams {
            public string Name;
            public int SimultaneousSoundsCount;
        }

        private class Channel {
            public int NextSourceIndex;
            public AudioSource[] RecentSources;
        }

        [SerializeField]
        private ChannelParams[] channelsParams;

        private Channel[] channels;

        public void Play(
            AudioClip sound, AudioSource audioSource, int channelId) {
            // DebugUtils.Log("AudioManager.Play(); sound={0};", sound);
            Channel channel = channels[channelId];
            AudioSource[] recentSources = channel.RecentSources;
            float volumeAttenuation = 1f / recentSources.Length;

            for (int i = 0; i < recentSources.Length; ++i) {
                if (recentSources[i] != null) {
                    recentSources[i].volume -= volumeAttenuation;
                }
            }

            int nextSourceIndex = channel.NextSourceIndex;
            // AudioSource recentSource = recentSources[nextSourceIndex];

            // if (recentSource != null) {
            //     recentSource.Stop();
            // }

            recentSources[nextSourceIndex] = audioSource;
            ++nextSourceIndex;

            if (nextSourceIndex >= recentSources.Length) {
                channel.NextSourceIndex = 0;
            }

            audioSource.clip = sound;
            audioSource.volume = 1f;
            audioSource.Play();
        }

        private void OnEnable() {
            // because of unity's bug
            IsPaused = false;
            channels = new Channel[channelsParams.Length];

            for (int i = 0; i < channelsParams.Length; ++i) {
                var recentSources =
                    new AudioSource[channelsParams[i].SimultaneousSoundsCount];
                channels[i] = new Channel{RecentSources = recentSources};
            }
        }
    }
}
