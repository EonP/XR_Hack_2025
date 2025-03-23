using UnityEngine;

public class SkyBoxChanger : MonoBehaviour
{
    [Tooltip("Assign one or more skybox materials here.")]
        [SerializeField] private Material lightMode;
        [SerializeField] private Material darkMode;
    
        private int currentIndex = 0;
    
        private void Start()
        {
            // Set the initial skybox (optional)
            RenderSettings.skybox = lightMode;
            
        }
        
        public void LightMode()
        {

            RenderSettings.skybox = lightMode;
        }

        public void DarkMode()
        {
            RenderSettings.skybox = darkMode;
        }

}
