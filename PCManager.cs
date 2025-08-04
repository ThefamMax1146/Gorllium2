namespace ComputerManager
{
    using TMPro;
    using UnityEngine;
    using Photon.Pun;

    public class PCManager : MonoBehaviour
    {

        [Header("MADE BY TV14/TIGERVOl14, CREDIT ME!!!")]

        [Header("DONT MESS WITH THIS!!!")]
        public CurrentTab currentTab;
        public CurrentOption currentOption;

        [Header("Put your text for the computer here.")]
        public TextMeshPro theTextidk;
        public TextMeshPro functionSelectText;

        [Header("DONT MESS!!!")]
        public string textonscreen;
        public string urfuturename;
        public string currentname;
        public string CurrentRoom;
        public string RoomToJoin;

        private float keyRed;
        private float keyGreen;
        private float keyBlue;

        [Header("Colors")]
        public float Red;
        public float Green;
        public float Blue;

        [Header("put your snap turn stuff")]
        public GameObject SnapTurnObject;
        public GameObject SmoothTurnObject;

        [Header("For credits tab")]
        [TextArea]
        public string creditstext;

        [Header("Scrolling")]
        public int maxLinesVisible = 10;
        private int scrollOffset = 0;

        // Tab scrolling variables
        private CurrentTab[] allTabs = { CurrentTab.Room, CurrentTab.Name, CurrentTab.Color, CurrentTab.Turning, CurrentTab.Credits };
        private int currentTabIndex = 0;

        void Start()
        {
            PlayerPrefs.GetString("PhtonUsername");
            currentname = PlayerPrefs.GetString("PhotonUsername");
            keyRed = PlayerPrefs.GetFloat("KeyRed");
            keyGreen = PlayerPrefs.GetFloat("KeyGreen");
            keyBlue = PlayerPrefs.GetFloat("KeyBlue");
            Red = keyRed;
            Green = keyGreen;
            Blue = keyBlue;

            // Set initial tab index based on current tab
            for (int i = 0; i < allTabs.Length; i++)
            {
                if (allTabs[i] == currentTab)
                {
                    currentTabIndex = i;
                    break;
                }
            }
        }

        public void UpdateColor()
        {
            keyRed = Red;
            PlayerPrefs.SetFloat("KeyRed", keyRed);
            keyGreen = Green;
            PlayerPrefs.SetFloat("KeyGreen", keyGreen);
            keyBlue = Blue;
            PlayerPrefs.SetFloat("KeyBlue", keyBlue);
            Color myColour = new Color(Red, Green, Blue);
            NetworkManager.Instance.SetPlayerColor(myColour);
        }

        public void ScrollUp()
        {
            if (scrollOffset > 0)
            {
                scrollOffset--;
                UpdateDisplay();
            }
        }

        public void ScrollDown()
        {
            string fullText = GetFullTabText();
            string[] lines = fullText.Split('\n');

            if (scrollOffset < lines.Length - maxLinesVisible)
            {
                scrollOffset++;
                UpdateDisplay();
            }
        }

        // New methods for tab scrolling
        public void ScrollTabUp()
        {
            currentTabIndex--;
            if (currentTabIndex < 0)
            {
                currentTabIndex = allTabs.Length - 1; // Wrap to last tab
            }
            currentTab = allTabs[currentTabIndex];
            scrollOffset = 0; // Reset content scroll when changing tabs
            UpdateDisplay();
        }

        public void ScrollTabDown()
        {
            currentTabIndex++;
            if (currentTabIndex >= allTabs.Length)
            {
                currentTabIndex = 0; // Wrap to first tab
            }
            currentTab = allTabs[currentTabIndex];
            scrollOffset = 0; // Reset content scroll when changing tabs
            UpdateDisplay();
        }

        private string GetFullTabText()
        {
            string fullText = "";

            if (currentTab == CurrentTab.Room)
            {
                fullText = "Put in a Room code and click enter to join OR...\nIf you want to leave a lobby click option 1!\n\nCurrent Room: " + CurrentRoom + "\n\nRoom To Join: " + RoomToJoin;
            }
            else if (currentTab == CurrentTab.Name)
            {
                fullText = "Put in a new name to call yourself as, you can put your oculus name, any name you want!\n\nCurrent Name: " + currentname + "\n\nNew Name: " + urfuturename;
            }
            else if (currentTab == CurrentTab.Color)
            {
                fullText = "Use the numbers 1-10 to select a color using RGB Values. Click option 1 to edit the red values, Click Option 2 to edit the Green values, and Click option 3 to edit the blue values.\n\nRed: " + Red + "\n\nGreen: " + Green + "\n\nBlue: " + Blue;
            }
            else if (currentTab == CurrentTab.Turning)
            {
                fullText = "With this, you can change your players turn setting. Click Option 1 To use snap turning\nClick Option 2 to use smooth turning\nAnd click Option 3 to turn off turning.";
            }
            else if (currentTab == CurrentTab.Credits)
            {
                fullText = creditstext;
            }
            else
            {
                fullText = "Failed to load.";
            }

            return fullText;
        }

        private string GetCurrentTabName()
        {
            switch (currentTab)
            {
                case CurrentTab.Room:
                    return "ROOM";
                case CurrentTab.Name:
                    return "NAME";
                case CurrentTab.Color:
                    return "COLOR";
                case CurrentTab.Turning:
                    return "TURNING";
                case CurrentTab.Credits:
                    return "CREDITS";
            }
            return "UNKNOWN";
        }

        private void UpdateDisplay()
        {
            if (functionSelectText != null)
            {
                // Show current tab with navigation indicators
                string tabDisplay = "Current Tab: " + GetCurrentTabName();
                tabDisplay += " (" + (currentTabIndex + 1) + "/" + allTabs.Length + ")";
                tabDisplay += "◄ Tab Scroll ►"; // Visual indicator for tab scrolling
                functionSelectText.text = tabDisplay;
            }

            string fullText = GetFullTabText();
            string[] lines = fullText.Split('\n');

            if (lines.Length <= maxLinesVisible)
            {
                theTextidk.text = fullText;
                scrollOffset = 0;
            }
            else
            {
                string visibleText = "";
                int endLine = Mathf.Min(scrollOffset + maxLinesVisible, lines.Length);

                for (int i = scrollOffset; i < endLine; i++)
                {
                    visibleText += lines[i];
                    if (i < endLine - 1) visibleText += "\n";
                }

                if (scrollOffset > 0)
                {
                    visibleText = "▲ (Scroll Up Available)\n" + visibleText;
                }
                if (scrollOffset + maxLinesVisible < lines.Length)
                {
                    visibleText += "\n▼ (Scroll Down Available)";
                }

                theTextidk.text = visibleText;
            }
        }

        void Update()
        {
            UpdateDisplay();

            if (PhotonNetwork.InRoom)
            {
                CurrentRoom = PhotonNetwork.CurrentRoom.Name;
            }
            else
            {
                CurrentRoom = "Not in a room.";
            }

            PlayerPrefs.SetString("PhotonUsername", currentname);
        }

        public enum CurrentTab
        {
            Room,
            Name,
            Color,
            Turning,
            Credits
        }

        public enum CurrentOption
        {
            Option1,
            Option2,
            Option3
        }
    }
}