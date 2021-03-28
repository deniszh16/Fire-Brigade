using UnityEngine;

namespace Cubra.Controllers
{
    public class SoundController : MonoBehaviour
    {
        // Активность звука в игре
        public static bool Activity;

        [Header("Текст кнопки")]
        [SerializeField] private TextTranslation _soundButton;

        private BackgroundMusic _backgroundMusic;

        private void Start()
        {
            _backgroundMusic = FindObjectOfType<BackgroundMusic>();

            // Получаем текущее значение звука
            Activity = PlayerPrefs.GetString("sounds") == "on";
        }

        /// <summary>
        /// Переключение звука
        /// </summary>
        public void SwitchSound()
        {
            PlayerPrefs.SetString("sounds", Activity ? "off" : "on");
            Activity = PlayerPrefs.GetString("sounds") == "on";

            // Настраиваем фоновую музыку
            _backgroundMusic.SwitchBackgroundMusic();

            ChangeButtonText();
        }

        /// <summary>
        /// Изменение перевода на звуковой кнопке
        /// </summary>
        private void ChangeButtonText()
        {
            _soundButton.ChangeKey(Activity ? "sounds-on" : "sounds-off");
        }
    }
}