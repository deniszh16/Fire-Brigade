﻿using UnityEngine;

public class Training : MonoBehaviour
{
    [Header("Маска для выделения")]
    [SerializeField] private GameObject mask;

    [Header("Панель выбора")]
    [SerializeField] private GameObject stretcher;

    [Header("Игровые персонажи")]
    [SerializeField] private GameObject characters;

    [Header("Огоньки на носилках")]
    [SerializeField] private GameObject fire;

    [Header("Огоньки на дороге")]
    [SerializeField] private GameObject fireRoad;

    [Header("Компонент перевода")]
    [SerializeField] private TextTranslation textTranslation;

    // Этап обучения
    private int stage = 0;

    /// <summary>
    /// Обновления этапа обучения
    /// </summary>
    public void RefreshStage()
    {
        stage++;

        if (stage <= 8)
        {
            // Выводим следующий текст обучения
            textTranslation.ChangeKey("training-" + stage.ToString());

            switch (stage)
            {
                case 3:
                    mask.SetActive(true);
                    SetObjectPositions(mask.transform.position, new Vector3(3, characters.transform.position.y, 0));
                    break;
                case 4:
                    SetObjectPositions(new Vector3(-2, 0.04f, 0), characters.transform.position);
                    break;
                case 5:
                    stretcher.SetActive(true);
                    SetObjectPositions(new Vector3(-8.1f, -0.9f, 0), new Vector3(-8.1f, characters.transform.position.y, 0));
                    break;
                case 6:
                    fire.SetActive(true);
                    SetObjectPositions(new Vector3(7, -2.7f, 0), new Vector3(6.3f, characters.transform.position.y, 0));
                    break;
                case 7:
                    fireRoad.SetActive(true);
                    fire.SetActive(false);
                    SetObjectPositions(new Vector3(1.8f, -3.35f, 0), new Vector3(-0.3f, characters.transform.position.y, 0));
                    break;
                case 8:
                    mask.SetActive(false);
                    fireRoad.SetActive(false);
                    break;
            }
        }
        else
        {
            // Записываем прохождение обучения
            PlayerPrefs.SetString("training", "yes");

            // Возвращаемся в список уровней
            Camera.main.GetComponent<Transitions>().GoToScene(Transitions.Scenes.Levels);
        } 
    }

    /// <summary>
    /// Установка позиции обучающих объектов
    /// </summary>
    /// <param name="mask">позиция маски</param>
    /// <param name="characters">позиция персонажей</param>
    private void SetObjectPositions(Vector3 mask, Vector3 characters)
    {
        this.mask.transform.position = mask;
        this.characters.transform.position = characters;
    }
}