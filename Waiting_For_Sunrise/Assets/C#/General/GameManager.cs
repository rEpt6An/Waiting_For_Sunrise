using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ����ģʽ
    public static GameManager Instance { get; private set; }

    // --- ��Ϸ״̬���� ---
    public int Day { get; private set; } = 1; 
    private const int MAX_DAYS = 20; 

    void Awake()
    {
        // --- ����ʵ�� ---
        if (Instance != null && Instance != this)
        {
            // ����������Ѵ���һ��GameManager����������µ�
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // --- �糡������ ---
        // �����GameManager�����ڼ����³���ʱ��������
        DontDestroyOnLoad(gameObject);

        // --- �������������¼� ---
        // �����ڳ������غ�ִ���߼��Ĺؼ�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ��GameManager������ʱ��ȡ���¼���������ֹ�ڴ�й©
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ÿ��һ���³����������ʱ����������ͻᱻ����
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"���� '{scene.name}' �Ѽ���. ��ǰ�ǵ� {Day} ��.");

        // �����߼����ж��Ƿ��Ǵ�Night�����л�����Shop����
        // ������Ҫ֪����һ��������ʲô������Ƚϸ��ӡ�
        // һ�����򵥿ɿ��ķ����ǣ�ֻ�ڽ���Shop����ʱ����������
        if (scene.name == "Shop")
        {
            // �����̵꣬����+1
            IncrementDay();
        }

        // �����һ��������Night��������Ҫ����Ƿ�ʤ��
        if (scene.name == "Night")
        {
            CheckForVictory();
        }
    }

    private void IncrementDay()
    {
        if (Day < MAX_DAYS)
        {
            Day++;
            Debug.Log($"�µ�һ�쿪ʼ��! �����ǵ� {Day} ��.");
        }
        else
        {
            // ��ͨ����Ӧ�÷�������Ϊ�ڽ���Night����ǰ��Ӧ��ʤ����
            Debug.Log("�Ѿ��ﵽ�������������������������");
        }
    }

    private void CheckForVictory()
    {
        // ��day�ﵽ20��ʱ�� night ʱ�����������Ϸʤ��
        // ����ζ�ţ�����20���Night����ʱ����������Shopʱ��������Ϊ21��
        // ��������Ӧ�����ڽ����21���Night֮ǰʤ����Ҳ���ǵ�20�����ʱ��
        // ������߼��ǣ������ǰ�����Ѿ��ﵽ20���������Ǽ�����ʼNight��������ʤ����
        if (Day >= MAX_DAYS)
        {
            // ��Ϸʤ�����߼�
            HandleGameVictory();
        }
        else
        {
            Debug.Log($"�� {Day} ���ҹ��ʼ...");
            // �������Ҫ���������������õ���ʱ����
            // FindObjectOfType<CountdownTimer>()?.ResetTimer();
        }
    }

    private void HandleGameVictory()
    {
        // TODO: ������ʵ����Ϸʤ�����߼�
        // ���磺
        // 1. ������ҿ���
        // 2. ��ʾʤ��UI���
        // 3. ����ʤ�����������˵�
        Debug.LogWarning("��Ϸʤ������ɹ������ " + MAX_DAYS + " �죡");

        // ʾ������ת��ʤ������
        // SceneManager.LoadScene("VictoryScene");
    }

    // ��������һ����������ȡ��ǰ�������ַ���������UI��ʾ
    public string GetDayString()
    {
        return $"Day: {Day}";
    }
}