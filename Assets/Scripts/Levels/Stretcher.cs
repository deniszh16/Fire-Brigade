using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cubra.Heplers;

namespace Cubra.Levels
{
    public class Stretcher : MonoBehaviour
    {
        // Событие по уничтожению носилок
        public UnityEvent OnDestroyStretcher;

        // Событие по изменению прочности
        public event Action StrengthChanged;

        // Прочность носилок
        public int Strength { get; private set; } = 100;

        // Уровень носилок
        public int StretcherLevel { get; private set; }

        // Цветовой статус носилок
        public ColorStatus.Statuses Status { get; private set; }

        // Предыдущий цветовой статус
        private int _previousStatus;

        // Горят ли носилки
        public bool IsBurns { get; private set; }

        // Используются ли улучшенные носилки
        public bool IsSuper { get; private set; }

        // Перечисление анимаций носилок
        private enum State { Green, Orange, Red, Yellow, Different, Destroy, Broken }

        // Ссылка на активную корутину
        public Coroutine ActiveCoroutine { get; set; }

        private Animator _animator;
        private BoxCollider2D _boxCollider;
        public PlayingSound PlayingSound { get; private set; }
        private Flames _flames;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _boxCollider = GetComponent<BoxCollider2D>();
            PlayingSound = GetComponent<PlayingSound>();

            _flames = GetComponentInChildren<Flames>();
            _flames.Extinguished += PutOutStretcher;

            // Устанавливаем зеленый статус носилок
            Status = ColorStatus.Statuses.Green;

            // Получаем уровень носилок
            StretcherLevel = PlayerPrefs.GetInt("stretcher");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Если огненная капля касается носилок
            if (collision.gameObject.GetComponent<Drop>())
            {
                // Если носилки обычные
                if (IsSuper == false)
                {
                    // Возвращаем каплю в указанный пул объектов
                    PoolsManager.PutObjectToPool(ListingPools.Pools.Twinkle.ToString(), collision.gameObject);
                    
                    // Поджигаем носилки
                    SetFireStretcher(IsBurns);
                }
            }
        }

        /// <summary>
        /// Возгорание носилок
        /// </summary>
        public void SetFireStretcher(bool isBurns)
        {
            if (isBurns == false)
                // Запускаем уменьшение прочности носилок (с учетом уровня носилок)
                ActiveCoroutine = StartCoroutine(ReduceStrength(10 - StretcherLevel));

            // Поджигаем носилки
            _flames.FlameVisibility(true);
            IsBurns = true;
        }

        /// <summary>
        /// Тушение носилок
        /// </summary>
        private void PutOutStretcher()
        {
            IsBurns = false;
            StopCoroutine(ActiveCoroutine);

            // Записываем тушение носилок
            PlayerPrefs.SetString("fire-stretcher", "yes");
        }

        /// <summary>
        /// Постепенное уменьшение прочности носилок
        /// </summary>
        /// <param name="value">значение для вычитания</param>
        private IEnumerator ReduceStrength(int value)
        {
            while (Strength > 0)
            {
                yield return new WaitForSeconds(1.0f);
                ChangeStrength(-value);
            }

            CheckStrength();
        }

        /// <summary>
        /// Постепенное увеличение прочности носилок
        /// </summary>
        /// <param name="value">значение для добавления</param>
        public IEnumerator IncreaseStrength(int value)
        {
            while (Strength < 100 && IsBurns == false)
            {
                yield return new WaitForSeconds(1.0f);
                ChangeStrength(value);
            }
        }

        /// <summary>
        /// Изменение прочности носилок
        /// </summary>
        /// <param name="value">значение</param>
        public void ChangeStrength(int value)
        {
            Strength += value;

            // Удаляем излишек прочности
            if (Strength > 100) Strength = 100;
            StrengthChanged?.Invoke();
        }

        /// <summary>
        /// Проверка прочности носилок
        /// </summary>
        public void CheckStrength()
        {
            if (Strength <= 0)
            {
                if (IsBurns == true)
                {
                    // Скрываем все огоньки
                    _flames.FlameVisibility(false);
                    // Отображаем сгоревшие носилки
                    ChangeAnimation((int)State.Destroy);
                }
                else
                {
                    // Отображаем сломанные носилки
                    ChangeAnimation((int)State.Broken);
                    _boxCollider.enabled = false;
                }

                // Сообщаем об уничтожении
                OnDestroyStretcher?.Invoke();
            }
        }

        /// <summary>
        /// Переключение анимации носилок
        /// </summary>
        /// <param name="number">номер анимации</param>
        public void ChangeAnimation(int number)
        {
            // Записываем предыдущий номер анимации
            _previousStatus = _animator.GetInteger("State");

            _animator.SetInteger("State", number);
            Status = ((ColorStatus.Statuses)number);
        }

        /// <summary>
        /// Восстановление носилок после использования улучшения
        /// </summary>
        public IEnumerator SuperiorStretcher()
        {
            IsSuper = true;
            Strength = 100;

            // Записываем использование улучшенных носилок
            PlayerPrefs.SetString("use-bonus", "yes");

            yield return new WaitForSeconds(20.0f);

            // Восстанавливаем предыдущие носилки
            ChangeAnimation(_previousStatus);
            IsSuper = false;
        }

        private void OnDestroy()
        {
            OnDestroyStretcher = null;
            StrengthChanged = null;
        }
    }
}