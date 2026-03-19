using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance { get; private set; }
    private string previousScene;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // ��ȡ��ǰ��������
        previousScene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        // ����Ƿ��� X��Y��A �� B ��
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.B))
        {
            // ������һ����
            SceneManager.LoadScene(previousScene);
        }
    }

    // ���°�ťʱ�л�����һ������
    public void SwitchToScene(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (sceneName == currentScene)
        {
            return;
        }

        previousScene = currentScene;  // ���µ�ǰ����Ϊ��һ������
        SceneManager.LoadScene(sceneName);  // ����Ŀ�曲��
    }

    // ������һ������
    public void BackToPreviousScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (string.IsNullOrEmpty(previousScene) || previousScene == currentScene)
        {
            return;
        }

        SceneManager.LoadScene(previousScene);
    }
}