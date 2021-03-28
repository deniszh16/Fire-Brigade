using System;
using System.Collections;
using UnityEngine;

namespace Cubra.Levels
{
    public class LevelManager : MonoBehaviour
    {
        // Перечисление режимов игры
        public enum GameModes { Play, Victory, Lose }

        // Текущий игровой режим
        public static GameModes GameMode;

        [Header("Всего жителей")]
        [SerializeField] private int _victims;

        public int Victims
        {
            get => _victims;
            set => _victims = value;
        }

        // Текущее количество жителей
        public int CurrentVictims { get; private set; }

        // Количество спасенных жителей
        public int SavedVictims { get; set; }

        // Событие по изменению количества жителей
        public event Action VictimsChanged;

        [Header("Максимум ошибок")]
        [SerializeField] private int _errors;

        // Текущее количество ошибок
        public int CurrentErrors { get; private set; }

        // Текущий счет уровня
        public int Score { get; private set; } 

        // Событие по изменению игрового счета
        public event Action<int> ScoresChanged;

        private Results _results;

        private void Awake()
        {
            GameMode = GameModes.Play;
            CurrentVictims = _victims;

            _results = Camera.main.GetComponent<Results>();
        }

        /// <summary>
        /// Уменьшение количества жильцов
        /// </summary>
        public void ReduceQuantityVictims()
        {
            CurrentVictims--;
            VictimsChanged?.Invoke();

            // Если жильцы закончились и ошибок меньше допустимого
            if (CurrentVictims <= 0 && CurrentErrors < _errors)
                // Завершаем текущий уровень победой
                _ = StartCoroutine(CompleteLevel(GameModes.Victory));
        }

        /// <summary>
        /// Изменение количества ошибок
        /// </summary>
        /// <param name="value">значение</param>
        public void ChangeErrors(int value)
        {
            CurrentErrors += value;

            // Если набрано максимум ошибок
            if (CurrentErrors >= _errors)
                // Завершаем уровень проигрышем
                _ = StartCoroutine(CompleteLevel(GameModes.Lose));
        }

        /// <summary>
        /// Изменение игрового счета
        /// </summary>
        /// <param name="value">значение</param>
        public void ChangeScore(int value)
        {
            Score += value;

            if (Score < 0) Score = 0;

            // Сообщаем об изменении
            ScoresChanged?.Invoke(value);
        }

        /// <summary>
        /// Завершение текущего уровня
        /// </summary>
        /// <param name="mode">режим завершения</param>
        private IEnumerator CompleteLevel(GameModes mode)
        {
            if (GameMode == GameModes.Play)
            {
                yield return new WaitForSeconds(1.2f);

                GameMode = mode;
                _results.ShowResult();
            }
        }

        private void OnDestroy()
        {
            VictimsChanged = null;
            ScoresChanged = null;
        }
    }
}