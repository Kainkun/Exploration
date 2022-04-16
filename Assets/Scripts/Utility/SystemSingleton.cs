using UnityEngine;

public class SystemSingleton<T> : MonoBehaviour where T : SystemSingleton<T>
{
    static protected T g_instance = null;
    public static T Get()
    {
        Init();
        return g_instance;
    }

    public static void Init()
    {
        if (g_instance) return;

        System.Type singletonType = typeof(T);
        GameObject singletonObject = new GameObject(singletonType.ToString(), singletonType);
        g_instance = singletonObject.GetComponent<T>();
    }

    protected virtual void Awake()
    {
        if (g_instance != null)
        {
            // If there is already and instance created,
            // Destory this extra instance
            Destroy(gameObject);
            return;
        }
        else
        {
            g_instance = (T)this;
        }
    }
}