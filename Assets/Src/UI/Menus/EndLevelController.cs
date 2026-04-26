using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class EndLevelController : MonoBehaviour
    {
        [SerializeField] private GameObject _nextLevelButton;
        
        private PlayerController _playerController;
        
        private void Awake()
        {
            foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                _playerController = playerObject.GetComponent<PlayerController>();
                if (_playerController != null) break;
            }

            if (LevelControl.LevelManager.Instance.CurrentLevel == 6)
            {
                // Disable the Next Level Button
                _nextLevelButton.SetActive(false);
            }
        }
    }
}