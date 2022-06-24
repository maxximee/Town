using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace MoreMountains.HighroadEngine
{
    /// <summary>
    /// This class manages a player on the lobby/menu screen.
    /// Implements IActorInput interface for UI manipulation
    /// </summary>
    public class LocalLobbyPlayerUI : MonoBehaviour, IActorInput
    {
        [Header("Slot data")]
        /// Slot number. Used for Input Management and UI
        public int Position;
        /// Use this text to specify the MainActionButton name of this slot.
        public string JoinActionInputName;
        /// Reference to menu scene manager
        public LocalLobbyGameUI MenuSceneManager;

        [Header("GUI Elements")]
        // UI player name zone
        public TextMeshProUGUI PlayerNameText;

        // UI new player zone
        public RectTransform AddPlayerZone;
        public Button NewBotButton;

        // UI choose vehicle zone
        public RectTransform ChooseVehicleZone;
        public Button RemovePlayerButton;
        public Button LeftButton;
        public Button RightButton;
        private string VehicleText;
        public Image VehicleImage;

        protected LocalLobbyManager _localLobbyManager;
        protected bool _ready; // Value indicating if player is ready to play or still choosing vehicle
        protected bool _isBot;
        protected int _currentVehicleIndex = -1;

        /// <summary>
        /// Initialisation
        /// </summary>
        public virtual void Start()
        {
            InitManagers();

            InputManager.Instance.SetPlayer(Position, this);

            InitUI();

            InitStartState();
        }

        /// <summary>
        /// Initializes managers
        /// </summary>
        protected virtual void InitManagers()
        {
            // Get reference to Local Lobby Manager
            _localLobbyManager = LocalLobbyManager.Instance;
        }

        /// <summary>
        /// Initializes links to UI elements
        /// </summary>
        protected virtual void InitUI()
        {
            OnReady(true);
        }

        /// <summary>
        /// Initializes the start state.
        /// </summary>
        protected virtual void InitStartState()
        {
            // we set the player name with its position
            PlayerNameText.text = "Player #" + (Position + 1);

            _ready = true;

            // if the player already exists, when coming back from the track to menu for instance, we load the data back and show its state
            if (_localLobbyManager.ContainsPlayer(Position))
            {
                ILobbyPlayerInfo player = _localLobbyManager.GetPlayer(Position);
                _currentVehicleIndex = player.VehicleSelectedIndex;

                if (Position == 0)
                {
                    OnAddPlayerButton();
                }
                else
                {
                    OnAddBotButton();
                }


            }

            // in mobile mode, at start, player 1 has already joined the game 
            // and other players can only be added as bot players
            if (InputManager.Instance.MobileDevice)
            {
                if (Position == 0)
                {
                    OnAddPlayerButton();
                }
                else
                {
                    OnAddBotButton();
                }
            }

            ShowSelectedVehicle();
        }

        /// <summary>
        /// Shows the selected vehicle from the LobbyManager Available Vehicles
        /// </summary>
        protected virtual void ShowSelectedVehicle()
        {
            if (_currentVehicleIndex != -1)
            {
                VehicleInformation info = _localLobbyManager.AvailableVehiclesPrefabs[_currentVehicleIndex].GetComponent<VehicleInformation>();
                if (info == null)
                {
                    VehicleText = "Dragon " + _currentVehicleIndex;
                    VehicleImage.sprite = _localLobbyManager.AvailableVehicles2dSprites[_currentVehicleIndex];
                }
                else
                {
                    // TODO still in here for the PUN online lobby
                    VehicleText = info.LobbyName;
                    VehicleImage.sprite = info.lobbySprite;
                }
                _localLobbyManager.registerLobbyPlayer(Position, _currentVehicleIndex);
            }
        }

        /// <summary>
        /// Describes what happens when the Add Player button gets pressed
        /// </summary>
        public virtual void OnAddPlayerButton()
        {
            AddLobbyPlayer(false);

            // In mobile mode, we hide the remove player button
            if (InputManager.Instance.MobileDevice)
            {
                // RemovePlayerButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Describes what happens when the Add Bot button gets pressed
        /// </summary>
        public virtual void OnAddBotButton()
        {
            AddLobbyPlayer(true);
        }

        /// <summary>
        /// Internal logic when adding new player in the lobby
        /// </summary>
        /// <param name="isBot">If set to <c>true</c> player is a bot.</param>
        protected virtual void AddLobbyPlayer(bool isBot)
        {
            _isBot = isBot;

            // we look for the next available vehicle
            if (_currentVehicleIndex == -1)
            {
                int vehicle = _localLobbyManager.FindFreeVehicle();
                _currentVehicleIndex = vehicle;
            }

            ShowSelectedVehicle();


            if (_isBot)
            {
                PlayerNameText.text = "Bot #" + (Position + 1);
                // we hide the ready action
                // ReadyButton.gameObject.SetActive(false);

                // bot is always in ready state, we add it
                AddLocalPlayerToLobby();
            }
            else
            {
                PlayerNameText.text = "Player #" + (Position + 1);
                _currentVehicleIndex = int.Parse(Manager.getSelectedDragon());
                AddLocalPlayerToLobby();
            }
        }

        /// <summary>
        /// Describes what happens when a player gets removed
        /// </summary>
        public virtual void OnRemovePlayer()
        {
            ChooseVehicleZone.gameObject.SetActive(false);
            AddPlayerZone.gameObject.SetActive(true);
            _localLobbyManager.RemovePlayer(Position);
            _localLobbyManager.unregisterLobbyPlayer(Position);
            _currentVehicleIndex = -1;
            PlayerNameText.text = "Player #" + (Position + 1);
        }

        /// <summary>
        /// Describes what happens when the left button gets pressed
        /// </summary>
        public virtual void OnLeft()
        {
            // player 1 can change track level if ready
            if (Position == 0 && _ready)
            {
                MenuSceneManager.OnLeft();
                return;
            }

            if (_currentVehicleIndex == 0)
            {
                _currentVehicleIndex = _localLobbyManager.AvailableVehiclesPrefabs.Length - 1;
            }
            else
            {
                _currentVehicleIndex -= 1;
            }

            ShowSelectedVehicle();

            if (_isBot)
            {
                // bots are always in ready state, we add it
                AddLocalPlayerToLobby();
            }
        }

        /// <summary>
        /// Describes what happens when the right button gets pressed
        /// </summary>
        public virtual void OnRight()
        {
            // player 1 can change track level if ready
            if (Position == 0 && _ready)
            {
                MenuSceneManager.OnRight();
                return;
            }

            if (_currentVehicleIndex == (_localLobbyManager.AvailableVehiclesPrefabs.Length - 1))
            {
                _currentVehicleIndex = 0;
            }
            else
            {
                _currentVehicleIndex += 1;
            }

            ShowSelectedVehicle();

            if (_isBot)
            {
                // Bot is always in ready state, we add it
                AddLocalPlayerToLobby();
            }
        }

        /// <summary>
        /// Describes what happens when the ready button is pressed
        /// </summary>
        /// <param name="fromGUI">If this parameter is set to true, we won't activate the specific case where player 1
        /// can start the game. 
        /// This allows us to separate keyboard and joystick control from Mobile Touch and mouse controls</param>
        public virtual void OnReady(bool fromGUI)
        {
            if (!_ready)
            {
                // Player goes to ready state
                // LeftButton.gameObject.SetActive(false);
                // RightButton.gameObject.SetActive(false);
                // RemovePlayerButton.gameObject.SetActive(false);
                _ready = true;

                //  ReadyButton.transform.Find("Text").GetComponent<Text>().text = "- ready -";
                //  ReadyButton.image.sprite = CancelReadyButtonSprite;

                AddLocalPlayerToLobby();
            }
            else
            {
                if (!fromGUI)
                {
                    if (Position == 0 && _ready)
                    {
                        MenuSceneManager.OnStartGame();
                        return;
                    }
                }

                CancelPlayer();
            }
        }

        /// <summary>
        /// Adds a local player to lobby.
        /// </summary>
        protected virtual void AddLocalPlayerToLobby()
        {
            LocalLobbyPlayer p = new LocalLobbyPlayer
            {
                Position = Position,
                Name = _isBot ? VehicleText : PlayerNameText.text,
                VehicleName = VehicleText,
                VehicleSelectedIndex = _currentVehicleIndex,
                IsBot = _isBot
            };
            _localLobbyManager.AddPlayer(p);
        }

        /// <summary>
        /// Cancel player selection
        /// </summary>
        protected virtual void CancelPlayer()
        {
            // Player cancels ready state
            //LeftButton.gameObject.SetActive(true);
            //RightButton.gameObject.SetActive(true);
            // RemovePlayerButton.gameObject.SetActive(true);
            //  ReadyButton.gameObject.SetActive(true);

            _ready = false;
            string buttonText = "";

            if (_isBot)
            {
                buttonText = "Bot Ready?";
            }
            else
            {
                buttonText = "Ready?";
            }
            // ReadyButton.transform.Find("Text").GetComponent<Text>().text = buttonText;

            //  ReadyButton.image.sprite = ReadyButtonSprite;
            _localLobbyManager.RemovePlayer(Position);
        }

        #region IPlayerInput implementation

        /// <summary>
        /// Main Action button
        /// </summary>
        public virtual void MainActionDown()
        {
            if (AddPlayerZone.gameObject.activeSelf)
            {
                // Player use its main action button to join game
                OnAddPlayerButton();
            }
            else
            {
                // Player use its main action button to validate vehicle choosen
                OnReady(false);
            }
        }

        /// <summary>
        /// Alt button : Cancel Button.
        /// </summary>
        public virtual void AltActionDown()
        {
            if (_ready)
            {
                CancelPlayer();
            }
            else if (ChooseVehicleZone.gameObject.activeSelf)
            {
                OnRemovePlayer();
            }
        }

        /// <summary>
        /// Stopper boolean for vehicle selector
        /// </summary>
        protected bool okToMove;

        /// <summary>
        /// Input management from keyboard / joystick.
        /// </summary>
        /// <param name="value">Value.</param>
        public virtual void HorizontalPosition(float value)
        {
            // We use a stopper boolean to avoid a rolling selection effect. 
            // Each time the player gameobject gets changed, the user must release the button to change again.
            if (Mathf.Abs(value) <= 0.1)
            {
                okToMove = true;
            }

            if (okToMove)
            {
                if (Mathf.Abs(value) > 0.1)
                {
                    if (value < 0)
                    {
                        OnLeft();
                    }
                    else
                    {
                        OnRight();
                    }
                    okToMove = false;
                }
            }
        }

        public virtual void VerticalPosition(float value) { }
        public void RespawnActionDown() { }
        public virtual void AltActionReleased() { }
        public virtual void RespawnActionReleased() { }
        public void MobileJoystickPosition(Vector2 value) { }
        public virtual void MainActionReleased() { }
        public virtual void LeftPressed() { }
        public virtual void RightPressed() { }

        public virtual void GyroAngled(float tiltedAmount) { }
        public virtual void UpPressed() { }
        public virtual void DownPressed() { }
        public void AltActionPressed() { }
        public void RespawnActionPressed() { }
        public virtual void MainActionPressed() { }
        public void LeftReleased() { }
        public void RightReleased() { }
        public void UpReleased() { }
        public void DownReleased() { }

        #endregion
    }
}