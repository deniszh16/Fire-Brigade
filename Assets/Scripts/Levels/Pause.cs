﻿using UnityEngine;

public class Pause : MonoBehaviour
{
    /// <summary>
    /// Настройка игровой паузы
    /// </summary>
    /// <param name="state">Состояние паузы</param>
    public void SetPause(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }

    /// <summary>
    /// Сброс паузы при выходе с уровня
    /// </summary>
    private void OnDestroy()
    {
        SetPause(false);
    }
}