using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : Utilities.Singleton<AudioManager>
    {
        #region VolumeControl
        public AudioMixer audioControl;

        private float _masterVol = 0.5f;
        private float _musicVol = 0.5f;
        private float _SFXVol = 0.5f;

        public float MasterVol { get { return _masterVol; } private set { _masterVol = value; } }
        public float MusicVol { get { return _musicVol; } private set { _musicVol = value; } }
        public float SFXVol { get { return _SFXVol; } private set { _SFXVol = value; } }

        private void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            audioControl.SetFloat("Master", ConvertToLogValue(_masterVol));
            audioControl.SetFloat("Music", ConvertToLogValue(_musicVol));
            audioControl.SetFloat("SFX", ConvertToLogValue(_SFXVol));
            _playOneShootAudioSource = gameObject.AddComponent<AudioSource>();
            _playOneShootAudioSource.loop = false;
            _playOneShootAudioSource.playOnAwake = false;
            _playOneShootAudioSource.spatialBlend = 0.0f;
            _playOneShootAudioSource.outputAudioMixerGroup = audioControl.FindMatchingGroups("SFX")[0];
        }

        public void SetVolume(AudioChannel channel, float value)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    audioControl.SetFloat("Master", ConvertToLogValue(value));
                    _masterVol = value;
                    break;
                case AudioChannel.Music:
                    audioControl.SetFloat("Music", ConvertToLogValue(value));
                    _musicVol = value;
                    break;
                case AudioChannel.SFX:
                    audioControl.SetFloat("SFX", ConvertToLogValue(value));
                    _SFXVol = value;
                    break;
                default:
                    Debug.LogError("AudioManager::SetVolume: ERROR IN SET VOLUME UNKOWN AUDIO CHANNEL: " + channel);
                    return;
            }
        }

        public float GetVolumeByChannel(AudioChannel channel)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    return MasterVol;
                case AudioChannel.Music:
                    return MusicVol;
                case AudioChannel.SFX:
                    return SFXVol;
                default:
                    Debug.LogError($"ERROR_UNKNOWN_CHANNEL: {channel}");
                    return 0;
            }
        }

        private float ConvertToLogValue(float value)
        {
            return Mathf.Log10(value) * 20;
        }
        #endregion

        #region PlayUtilitiesForSmallSFX
        private AudioSource _playOneShootAudioSource;
        /// <summary>
        /// Play the sound on the SFX Channel
        /// </summary>
        /// <param name="clipToPlay">The audioClip to play</param>
        /// <param name="scale">The Scale of the volume default 1 that is equal to the SFX volume channel</param>
        public void PlayOneShot2D(AudioClip clipToPlay, float scale = 1.0f)
        {
            if (clipToPlay == null)
            {
                Debug.LogError("AudioManager::PlayOneShot: The audioClip is null");
                return;
            }
            _playOneShootAudioSource.PlayOneShot(clipToPlay, scale);
        }
        #endregion
    }
}