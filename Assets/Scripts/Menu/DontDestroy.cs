using UnityEngine;

namespace Cubra
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            // Находим все объекты с фоновой музыкой
            BackgroundMusic[] objs = FindObjectsOfType<BackgroundMusic>();

            // Если их больше одного, уничтожаем лишние
            if (objs.Length > 1) Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}