﻿using UnityEngine;

public class ReturnBack : MonoBehaviour
{
    [Header("Сцена для возврата")]
    [SerializeField] private Transitions.Scenes scene;

    // Ссылка на компонент
    private Transitions transitions;

    private void Awake()
    {
        transitions = Camera.main.GetComponent<Transitions>();
    }

    private void Update()
    {
        // Если нажата кнопка возврата, выполняем переход на сцену
        if (Input.GetKey(KeyCode.Escape)) transitions.GoToScene(scene);
    }
}