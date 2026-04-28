using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AoV.Menus
{
    public class FPSHandler : MonoBehaviour
    {
        private int frameRateLimit;
        [SerializeField] private Slider fpsSlider;
        [SerializeField] private TextMeshProUGUI fpstext;

        void Start()
        {
            PlayerPrefs.GetInt("fpslimitsave", frameRateLimit);
            Application.targetFrameRate = frameRateLimit;
            fpsSlider.value = frameRateLimit;
            fpstext.text = fpsSlider.value.ToString("");
        }

        public void SetFPSCap()
        {
            frameRateLimit = (int)fpsSlider.value;
            fpstext.text = fpsSlider.value.ToString("");
        }

        public void ApplyFPS()
        {
            Application.targetFrameRate = frameRateLimit;
            PlayerPrefs.SetInt("fpslimitsave", frameRateLimit);
        }
    }

}