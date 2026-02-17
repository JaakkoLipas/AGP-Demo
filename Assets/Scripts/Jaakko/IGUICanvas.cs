using UnityEngine;

namespace AoV
{
    public class IGUICanvas : MonoBehaviour
    {
        public static IGUICanvas Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else { Destroy(this.gameObject); }
        }
    }

}