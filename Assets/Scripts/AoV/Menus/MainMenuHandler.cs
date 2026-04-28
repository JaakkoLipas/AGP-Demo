using UnityEngine;

namespace AoV.Menus
{
    public class MainMenuHandler : MonoBehaviour
    {
        public void ActivatePanel(GameObject panel)
        {
            panel.SetActive(true);
        }

        public void DeactivatePanel(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void LoadScene(int sceneIndex)
        {
            AoV.System.SceneLoader.Instance.LoadSceneWithFade(sceneIndex);
        }

        public void CloseApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }

}