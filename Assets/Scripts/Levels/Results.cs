using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cubra.Levels
{
    public class Results : MonoBehaviour
    {
        [Header("Элементы меню")]
        [SerializeField] private GameObject[] _menuItems;

        private enum MenuItems { Blackout, Characters, Menu, Results, Victory, Lose }

        [Header("Итоговый счет")]
        [SerializeField] private Text _score;

        [Header("Фоновая музыка")]
        [SerializeField] private BackgroundMusic _backgroundMusic;

        [Header("Звуки результата")]
        [SerializeField] private AudioClip[] _audioClips;

        private enum AudioClips { Victory, Lose }

        private AudioSource _audioResults;
        private PlayingSound _playingResults;

        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = Camera.main.GetComponent<LevelManager>();

            _audioResults = _menuItems[(int)MenuItems.Results].GetComponent<AudioSource>();
            _playingResults = _menuItems[(int)MenuItems.Results].GetComponent<PlayingSound>();
        }

        /// <summary>
        /// Отображение результата уровня
        /// </summary>
        public void ShowResult()
        {
            // Отключаем фоновую музыку
            _ = StartCoroutine(_backgroundMusic.ChangeVolume(-0.1f, x => x > 0));

            // Отображаем необходимые элементы меню
            _menuItems[(int)MenuItems.Blackout].SetActive(true);
            _menuItems[(int)MenuItems.Characters].SetActive(true);
            _menuItems[(int)MenuItems.Menu].SetActive(true);
            _menuItems[(int)MenuItems.Results].SetActive(true);

            // Обновляем общее количество пойманных персонажей
            PlayerPrefs.SetInt("victims", PlayerPrefs.GetInt("victims") + _levelManager.SavedVictims);

            if (LevelManager.GameMode == LevelManager.GameModes.Victory)
            {
                // Удваиваем набранные очки
                _levelManager.ChangeScore(_levelManager.Score);

                // Отображаем панель победы
                _menuItems[(int)MenuItems.Victory].SetActive(true);

                // Устанавливаем звук и воспроизводим
                _audioResults.clip = _audioClips[(int)AudioClips.Victory];
                _playingResults.PlaySound();

                var progress = PlayerPrefs.GetInt("progress");
                // Если прогресс викторины не превышает номер уровня
                if (progress <= LevelSelection.LevelNumber)
                    // Увеличиваем прогресс
                    PlayerPrefs.SetInt("progress", progress + 1);

                // Увеличиваем общий и текущий счет
                PlayerPrefs.SetInt("total-score", PlayerPrefs.GetInt("total-score") + _levelManager.Score);
                PlayerPrefs.SetInt("current-score", PlayerPrefs.GetInt("current-score") + _levelManager.Score);

                AchievementPerLevel();

                // Отображаем счетчик очков
                _ = StartCoroutine(ScoreCounter());
            }
            else
            {
                // Отображаем панель проигрыша
                _menuItems[(int)MenuItems.Lose].SetActive(true);

                // Устанавливаем звук и воспроизводим
                _audioResults.clip = _audioClips[(int)AudioClips.Lose];
                _playingResults.PlaySound();
            }
        }

        /// <summary>
        /// Достижение за уровень без ошибок
        /// </summary>
        private void AchievementPerLevel()
        {
            if (_levelManager.CurrentErrors == 0)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                    // Открываем достижение по прохождению без ошибок
                    GooglePlayServices.UnlockingAchievement(GPGSIds.achievement_5);
            }
        }

        /// <summary>
        /// Счетчик набранных очков
        /// </summary>
        private IEnumerator ScoreCounter()
        {
            // Счетчик очков
            var counter = 0;
            // Шаг увеличения
            var counterStep = 10;
            // Набранные очки за уровень
            var scoreLevel = _levelManager.Score;

            while (scoreLevel > 0)
            {
                yield return new WaitForSeconds(0.01f);

                if (scoreLevel >= counterStep)
                {
                    counter += counterStep;
                    scoreLevel -= counterStep;
                }
                else
                {
                    counter += scoreLevel;
                    scoreLevel = 0;
                }

                _score.text = counter.ToString();
            }
        }
    }
}