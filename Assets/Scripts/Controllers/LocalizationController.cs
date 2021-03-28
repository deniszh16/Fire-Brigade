using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cubra.Heplers;

namespace Cubra.Controllers
{
    public class LocalizationController : FileProcessing
    {
        // Объект для json по переводам
        private LanguageHelper _language;

        // Словарь с переводом слов на выбранный язык
        public static Dictionary<string, string> Translations;

        // Событие по обновлению перевода
        public UnityEvent TranslationUpdated;

        [Header("Текст языковой кнопки")]
        [SerializeField] private TextTranslation _languageButton;

        private void Awake()
        {
            _language = new LanguageHelper();

            Translations = new Dictionary<string, string>();
            TranslationUpdated.AddListener(ChangeButtonText);

            GetTextTranslation();
        }

        /// <summary>
        /// Получение перевода из файла
        /// </summary>
        public void GetTextTranslation()
        {
            // Текст из json файла
            var json = ReadJsonFile(PlayerPrefs.GetString("language"));
            // Преобразовываем json в объект
            ConvertToObject(ref _language, json);

            Translations.Clear();
            WriteToDictionary();
        }

        /// <summary>
        /// Заполнение словаря переводом
        /// </summary>
        private void WriteToDictionary()
        {
            for (int i = 0; i < _language.Phrase.Length; i++)
            {
                var key = _language.Phrase[i].Key;

                // Если ключ отсутствует
                if (Translations.ContainsKey(key) == false)
                    // Добавляем значение в словарь
                    Translations.Add(key, _language.Phrase[i].Value);
            }
        }

        /// <summary>
        /// Переключение языка игры
        /// </summary>
        public void SwitchLanguage()
        {
            var language = PlayerPrefs.GetString("language");
            PlayerPrefs.SetString("language", language == "ru-RU" ? "en-US" : "ru-RU");

            GetTextTranslation();
            TranslationUpdated?.Invoke();
        }

        /// <summary>
        /// Изменение перевода на языковой кнопке
        /// </summary>
        private void ChangeButtonText()
        {
            var language = PlayerPrefs.GetString("language");
            _languageButton.ChangeKey(language == "ru-RU" ? "language-rus" : "language-eng");
        }
    }
}