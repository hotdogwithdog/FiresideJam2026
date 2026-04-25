using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menus
{
    public class EndLevelController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _massText;
        [SerializeField] private GameObject _nextLevelButton;
        
        private PlayerController _playerController;
        
        private void Awake()
        {
            foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
            {
                _playerController = playerObject.GetComponent<PlayerController>();
                if (_playerController != null) break;
            }
            
            _titleText.text = "Level " + LevelControl.LevelManager.Instance.CurrentLevel + " Completed";
            
            _massText.text += _playerController.GetMass().ToString("F2") + " / " + _playerController.MaxMass.ToString("F2");

            if (LevelControl.LevelManager.Instance.CurrentLevel == 6)
            {
                // Disable the Next Level Button
                _nextLevelButton.SetActive(false);
            }
        }
    }
}