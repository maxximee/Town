using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace MoreMountains.HighroadEngine
{
    /// <summary>
    /// This class manages game scene choice and game state in the local lobby scene.
    /// </summary>
    public class LocalLobbyGameUI : MonoBehaviour
    {
        [Header("GUI Elements")]

        /// the "select previous scene" button
        public Button LeftButton;
        /// the "select next scene" button
        public Button RightButton;
        /// the text object used to display the scene name
        public TextMeshProUGUI SceneName;
        /// the image object used to display a picture of the target scene
        public Image SceneImage;
        /// the start game button
        public Button StartGameButton;
        // UI back button
        public Button BackButton;

        protected LocalLobbyManager _localLobbyManager;
        protected int _currentSceneSelected;

        /// <summary>
        /// Initializes states.
        /// </summary>
        protected virtual void Start()
        {
            InitManagers();

            InitUI();

            InitStartState();
        }

        /// <summary>
        /// Initializes managers.
        /// </summary>
        protected virtual void InitManagers()
        {
            // Find global menu manager
            _localLobbyManager = LocalLobbyManager.Instance;
        }

        /// <summary>
        /// Initializes links to UI elements.
        /// </summary>
        protected virtual void InitUI()
        {
            // Init buttons actions
            LeftButton.onClick.AddListener(OnLeft);
            RightButton.onClick.AddListener(OnRight);
            StartGameButton.onClick.AddListener(OnStartGame);
            BackButton.onClick.AddListener(_localLobbyManager.ReturnToStartScreen);
        }

        /// <summary>
        /// Initializes the start state.
        /// </summary>
        protected virtual void InitStartState()
        {
            // First scene or last used scene by default
            _currentSceneSelected = _localLobbyManager.TrackSelected;
            ShowSelectedScene();
        }


        /// <summary>
        /// Shows the selected scene.
        /// </summary>
        protected virtual void ShowSelectedScene()
        {
            SceneName.text = _localLobbyManager.AvailableTracksSceneName[_currentSceneSelected];
            SceneImage.sprite = _localLobbyManager.AvailableTracksSprite[_currentSceneSelected];
        }

        /// <summary>
        /// Left button action
        /// </summary>
        public virtual void OnLeft()
        {
            if (_currentSceneSelected == 0)
            {
                _currentSceneSelected = _localLobbyManager.AvailableTracksSceneName.Length - 1;
            }
            else
            {
                _currentSceneSelected -= 1;
            }
            _localLobbyManager.TrackSelected = _currentSceneSelected;
            ShowSelectedScene();
        }

        /// <summary>
        /// Right button action
        /// </summary>
        public virtual void OnRight()
        {
            if (_currentSceneSelected == (_localLobbyManager.AvailableTracksSceneName.Length - 1))
            {
                _currentSceneSelected = 0;
            }
            else
            {
                _currentSceneSelected += 1;
            }
            _localLobbyManager.TrackSelected = _currentSceneSelected;
            ShowSelectedScene();
        }

        /// <summary>
        /// Describes what happens when the game starts
        /// </summary>
        public void OnStartGame()
        {
            LoadingSceneManager.LoadScene(SceneName.text);
        }
    }
}