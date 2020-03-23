﻿using System.Collections;
using UnityEngine;

public class Loading : MonoBehaviour
{
    // Ссылка на компонент
    private Transitions transitions;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
    }

    private void Start()
    {
        #region SaveData

        // Перевод интерфейса игры
        if (!PlayerPrefs.HasKey("language"))
            PlayerPrefs.SetString("language", (Application.systemLanguage == SystemLanguage.Russian) ? "ru-RU" : "en-US");

        // Игровой прогресс
        if (!PlayerPrefs.HasKey("progress")) PlayerPrefs.SetInt("progress", 1);

        // Звуки и музыка
        if (!PlayerPrefs.HasKey("sounds")) PlayerPrefs.SetString("sounds", "on");

        // Общий счет
        if (!PlayerPrefs.HasKey("total-score")) PlayerPrefs.SetInt("total-score", 0);

        // Уровень носилок
        if (!PlayerPrefs.HasKey("stretcher")) PlayerPrefs.SetInt("stretcher", 1);

        #endregion

        StartCoroutine(GoToMenu());
    }

    /// <summary>
    /// Переход в главное меню
    /// </summary>
    private IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(2.0f);
        transitions.GoToScene(1);
    }
}