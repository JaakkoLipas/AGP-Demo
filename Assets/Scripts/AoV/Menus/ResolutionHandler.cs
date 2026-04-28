using UnityEngine;
using TMPro;

namespace AoV.Menus
{
    public class ResolutionHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionMode;
        [SerializeField] private TMP_Dropdown displayMode;
        private FullScreenMode screenMode;

        private int height;
        private int width;

        public void UpdateResolution()
        {
            switch (resolutionMode.value)
            {
                case 0:
                    {
                        width = 1920;
                        height = 1080;
                        break;
                    }
                case 1:
                    {
                        width = 640;
                        height = 360;
                        break;
                    }
                case 2:
                    {
                        width = 1280;
                        height = 720;
                        break;
                    }
                case 3:
                    {
                        width = 2560;
                        height = 1440;
                        break;
                    }
                case 4:
                    {
                        width = 3840;
                        height = 2160;
                        break;
                    }
            }
        }

        public void UpdateWindow()
        {
            if (displayMode.value == 0)
            {
                screenMode = FullScreenMode.ExclusiveFullScreen;
            }

            if (displayMode.value == 1)
            {
                screenMode = FullScreenMode.FullScreenWindow;
            }

            if (displayMode.value == 2)
            {
                screenMode = FullScreenMode.Windowed;
            }
        }

        public void ApplyResolutionAndWindow()
        {
            Screen.SetResolution(width, height, screenMode);
        }
    }

}