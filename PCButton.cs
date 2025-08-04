namespace ComputerManager
{
    using UnityEngine;
    using Photon.Pun;
    using PlayFab;
    using PlayFab.ClientModels;
    using static ComputerManager.PCManager;

    public class PCButton : MonoBehaviour
    {
        public PCManager pcManager;
        public bool isEnter;
        public bool Backspace;
        public string Letter;
        public bool IsOption1;
        public bool IsOption2;
        public bool IsOption3;
        public bool TestPress;
        public string HandTag;

        public bool IsJoinPublicLobby;
        public ChangeTab changeTab;

        public bool IsScrollUp;
        public bool IsScrollDown;

        // New tab scrolling booleans
        public bool IsTabScrollUp;
        public bool IsTabScrollDown;

        public void Update()
        {
            if (TestPress)
            {
                PressButton();
                TestPress = false;
            }
        }

        public void PressButton()
        {
            if (changeTab != ChangeTab.NoSwitch)
            {
                switch (changeTab)
                {
                    case ChangeTab.SwitchRoom:
                        pcManager.currentTab = PCManager.CurrentTab.Room;
                        break;
                    case ChangeTab.SwitchName:
                        pcManager.currentTab = PCManager.CurrentTab.Name;
                        break;
                    case ChangeTab.SwitchColor:
                        pcManager.currentTab = PCManager.CurrentTab.Color;
                        break;
                    case ChangeTab.SwitchTurning:
                        pcManager.currentTab = PCManager.CurrentTab.Turning;
                        break;
                    case ChangeTab.SwitchCredits:
                        pcManager.currentTab = PCManager.CurrentTab.Credits;
                        break;
                }
                return;
            }

            // Handle tab scrolling
            if (IsTabScrollUp)
            {
                pcManager.ScrollTabUp();
                return;
            }

            if (IsTabScrollDown)
            {
                pcManager.ScrollTabDown();
                return;
            }

            if (IsScrollUp)
            {
                pcManager.ScrollUp();
                return;
            }

            if (IsScrollDown)
            {
                pcManager.ScrollDown();
                return;
            }

            if (pcManager.currentTab == PCManager.CurrentTab.Room && !string.IsNullOrEmpty(Letter))
            {
                pcManager.RoomToJoin += Letter;
            }

            if (pcManager.currentTab == PCManager.CurrentTab.Name && !string.IsNullOrEmpty(Letter))
            {
                pcManager.urfuturename += Letter;
            }

            if (Backspace)
            {
                if (pcManager.currentTab == PCManager.CurrentTab.Room)
                {
                    if (pcManager.RoomToJoin.Length > 0)
                    {
                        pcManager.RoomToJoin = pcManager.RoomToJoin.Remove(pcManager.RoomToJoin.Length - 1);
                    }
                }
                else if (pcManager.currentTab == PCManager.CurrentTab.Name)
                {
                    if (pcManager.urfuturename.Length > 0)
                    {
                        pcManager.urfuturename = pcManager.urfuturename.Remove(pcManager.urfuturename.Length - 1);
                    }
                }
            }

            if (isEnter)
            {
                if (pcManager.currentTab == PCManager.CurrentTab.Room)
                {
                    string roomCode = pcManager.RoomToJoin;
                    int MaxPlayers = 10;
                    NetworkManager.JoinPrivate(roomCode, MaxPlayers.ToString());
                }
                else if (pcManager.currentTab == PCManager.CurrentTab.Name)
                {
                    string name = pcManager.urfuturename;

                    NetworkManager.Instance.SetPlayerName(name);
                    pcManager.currentname = name;

                    var request = new UpdateUserTitleDisplayNameRequest
                    {
                        DisplayName = name
                    };

                    PlayFabClientAPI.UpdateUserTitleDisplayName(request,
                        result =>
                        {
                            Debug.Log($"✅ Display name updated in PlayFab to: {result.DisplayName}");
                        },
                        error =>
                        {
                            Debug.LogError($"❌ Failed to update name in PlayFab: {error.GenerateErrorReport()}");
                        });
                }

            }

            if (IsOption1)
            {
                if (pcManager.currentTab == PCManager.CurrentTab.Room)
                {
                    PhotonNetwork.LeaveRoom();
                }
                else if (pcManager.currentTab == PCManager.CurrentTab.Turning)
                {
                    pcManager.SnapTurnObject.SetActive(true);
                    pcManager.SmoothTurnObject.SetActive(false);
                }
                pcManager.currentOption = CurrentOption.Option1;
            }

            if (IsOption2)
            {
                if (pcManager.currentTab == PCManager.CurrentTab.Turning)
                {
                    pcManager.SnapTurnObject.SetActive(false);
                    pcManager.SmoothTurnObject.SetActive(true);
                }
                pcManager.currentOption = CurrentOption.Option2;
            }

            if (IsOption3)
            {
                if (pcManager.currentTab == PCManager.CurrentTab.Turning)
                {
                    pcManager.SnapTurnObject.SetActive(false);
                    pcManager.SmoothTurnObject.SetActive(false);
                }
                pcManager.currentOption = CurrentOption.Option3;
            }

            if (IsJoinPublicLobby)
            {
                NetworkManager.JoinRandom();
            }

            if (pcManager.currentTab == PCManager.CurrentTab.Color)
            {
                if (Letter == "1")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.1f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.1f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.1f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "2")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.2f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.2f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.2f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "3")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.3f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.3f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.3f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "4")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.4f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.4f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.4f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "5")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.5f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.5f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.5f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "6")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.6f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.6f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.6f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "7")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.7f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.7f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.7f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "8")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.8f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.8f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.8f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "9")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0.9f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0.9f;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0.9f;
                    }
                    pcManager.UpdateColor();
                }
                if (Letter == "0")
                {
                    if (pcManager.currentOption == PCManager.CurrentOption.Option1)
                    {
                        pcManager.Red = 0;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option2)
                    {
                        pcManager.Green = 0;
                    }
                    if (pcManager.currentOption == PCManager.CurrentOption.Option3)
                    {
                        pcManager.Blue = 0;
                    }
                    pcManager.UpdateColor();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(HandTag))
            {
                PressButton();
            }
        }

        public enum ChangeTab
        {
            NoSwitch,
            SwitchRoom,
            SwitchName,
            SwitchColor,
            SwitchTurning,
            SwitchCredits
        }
    }
}