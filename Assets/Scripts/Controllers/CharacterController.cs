using UnityEngine;
using Cubra.Levels;

namespace Cubra.Controllers
{
    public class CharacterController : MonoBehaviour
    {
        [Header("Инвертирование управления")]
        [SerializeField] private bool _inverted;

        private int Inverted => _inverted ? -1 : 1;

        // Скорость персонажей
        private readonly float _speed = 19.5f;

        // Высота прыжка персонажей
        private readonly float _jump = 5.5f;

        // Переключают ли персонажи носилки
        public bool IsSwitched { get; set; }

        // Находятся ли персонажи на земле
        public bool IsGroung { get; set; }

        // Направление движения персонажей
        public Vector2 Direction { get; private set; }

        // Ограничители для персонажей
        private float[] _limiters;

        private Animator _animator;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _limiters = new float[2];
            // Получаем границы экрана и устанавливаем ограничители для персонажей
            _limiters[0] = Camera.main.ViewportToWorldPoint(new Vector2(-0.01f, 0)).x;
            _limiters[1] = Camera.main.ViewportToWorldPoint(new Vector2(0.89f, 0)).x;
        }

        // Управление на клавиатуре для тестирования уровней
        #if UNITY_EDITOR || UNITY_STANDALONE
        private void Update()
        {
            if (Input.GetKey("left")) Direction = Vector2.left / 10;
            else if (Input.GetKey("right")) Direction = Vector2.right / 10;
            else Direction = Vector2.zero;

            if (Input.GetKeyDown("space"))
            {
                if (IsGroung == true && IsSwitched == false)
                {
                    _rigidbody.AddForce(new Vector2(0.5f * Direction.x, 1 * _jump), ForceMode2D.Impulse);
                    IsGroung = false;
                }
            }

            MoveCharacters();
        }
        #endif

        #if UNITY_ANDROID
        private void FixedUpdate()
        {
            SetMotionVector();
            MoveCharacters();
        }
        #endif

        /// <summary>
        /// Установка направления персонажей
        /// </summary>
        private void SetMotionVector()
        {
            if (_inverted == false)
            {
                // Если позиция персонажей выходит за ограничители
                if ((transform.position.x < _limiters[0] && Input.acceleration.x < 0)
                || (transform.position.x > _limiters[1] && Input.acceleration.x > 0))
                {
                    Direction *= 0;
                    return;
                }
            }
            else
            {
                // Если используется инвертирование, проверяем обратные ограничители
                if ((transform.position.x < _limiters[0] && Input.acceleration.x > 0)
                || (transform.position.x > _limiters[1] && Input.acceleration.x < 0))
                {
                    Direction *= 0;
                    return;
                }
            }

            // Устанавливаем направление в зависимости от наклона смартфона
            Direction = new Vector2(Input.acceleration.x * Inverted, 0);
            // Если длина вектора превышает единицу, округляем
            if (Direction.sqrMagnitude > 1) Direction.Normalize();
        }

        /// <summary>
        /// Перемещение персонажей
        /// </summary>
        private void MoveCharacters()
        {
            // Если направление нулевое (с погрешностью)
            if (Direction.x < 0.015f && Direction.x > -0.015f)
            {
                ChangeAnimation((int)Characters.Animations.Idle);
            }
            else
            {
                _rigidbody.transform.Translate(Direction * _speed * Time.fixedDeltaTime);
                ChangeAnimation((int)Characters.Animations.Run);
            }
        }

        /// <summary>
        /// Прыжок персонажей
        /// </summary>
        private void OnMouseDown()
        {
            // Если персонаж на земле и не обменивается
            if (IsGroung == true && IsSwitched == false)
            {
                _rigidbody.AddForce(new Vector2(0.5f * Direction.x, 1 * _jump), ForceMode2D.Impulse);
                IsGroung = false;
            }
        }

        /// <summary>
        /// Переключение анимации персонажей
        /// </summary>
        /// <param name="animation">номер анимации</param>
        public void ChangeAnimation(int animation)
        {
            _animator.SetInteger("State", animation);
        }
    }
}