using System;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Audio
{
    public class PikoTheAdventureSongController : Utilities.Singleton<PikoTheAdventureSongController> // Probably a bad idea but it will work for his purpose xd
    {
        [SerializeField] private AudioClip[] _audioClipsLow;
        [SerializeField] private AudioClip[] _audioClipsMid;
        [SerializeField] private AudioClip[] _audioClipsHigh;

        private AudioClip[][] _audioClips;
        
        private AudioSource _audioSource;

        private Random _random;

        private PlayerController _playerController;
        
        private void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _audioSource.spatialBlend = 0f;
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.audioControl.FindMatchingGroups("Music")[0];
            
            _random = new System.Random();
            
            _audioClips = new AudioClip[3][];
            _audioClips[0] = _audioClipsLow;
            _audioClips[1] = _audioClipsMid;
            _audioClips[2] = _audioClipsHigh;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (GameObject playerObject in scene.GetRootGameObjects())
            {
                _playerController = playerObject.GetComponent<PlayerController>();
                if (_playerController != null) break;
            }
        }

        private void Update()
        {
            if (_audioSource.isPlaying) return;

            int index;
            if (_playerController == null)
            {
                // Just randomize in the mid music (this is when the Scene is main menu
                index = _random.Next(0, _audioClipsMid.Length);
                _audioSource.clip = _audioClipsMid[index];
                _audioSource.Play();
                return;
            }

            int groupIndex = GetGroupIndex();
            
            index = _random.Next(0, _audioClips[groupIndex].Length);

            _audioSource.clip = _audioClips[groupIndex][index];
            _audioSource.Play();
        }

        private int GetGroupIndex()
        {
            float massPercentage = _playerController.GetMass() / _playerController.MaxMass;

            if (massPercentage <= 0.33f) return 2;
            if (massPercentage <= 0.66f) return 1;
            return 0;
        }
    }
}