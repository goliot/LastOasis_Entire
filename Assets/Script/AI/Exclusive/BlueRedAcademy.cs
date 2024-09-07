using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.MLAgents;

public class BlueRedAcademy : MonoBehaviour
{
    public static BlueRedAcademy instance = null;
    public static ActionsButton[] btn;

    private static int round = 0;
    public float frequency;
    private FPS timeControl;

    public BlueAgent blue;
    public RedAgent red;

    public int resetCount = 0;
    private bool needsReset = true; // �� ��ε� �ʿ� ���θ� ��Ÿ���� �÷���

    private const string ResetKey = "ResetCount";
    public void Awake()
    {
        instance = this;
        this.timeControl = GetComponent<FPS>();
        needsReset = true; // Awake���� �÷��� �ʱ�ȭ
        resetCount = PlayerPrefs.GetInt(ResetKey, 0);
        BlueRedAcademy.round++;

        BlueActionSpace b_Btn = this.gameObject.AddComponent<BlueActionSpace>();
        RedActionSpace r_Btn = this.gameObject.AddComponent<RedActionSpace>();

        btn = new ActionsButton[2] { b_Btn, r_Btn };
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (this.blue.Terminal && this.red.Terminal && BlueRedAcademy.round > 0)
        {
            this.EndEpisode();
            float epsilon = Random.Range(0.0f, 1.0f);

            bool normalSpeed;
            if (epsilon < this.frequency)
            {
                normalSpeed = true;
            }
            else
            {
                normalSpeed = false;
            }
            this.ResetAll();
            this.timeControl.SetTimeScale(normalSpeed);
        }
    }

    public void ResetAll()
    {
        resetCount = 1;
        PlayerPrefs.SetInt(ResetKey, resetCount);
        PlayerPrefs.Save();
        if (needsReset)
        {
            needsReset = false; // �� ��ε� �� �÷��׸� false�� ����
            UnityEngine.SceneManagement.SceneManager.LoadScene("SceneType1");
        }
    }

    void EndEpisode()
    {
        this.blue.EndEpisode();
        this.red.EndEpisode();
    }

    // �� �ε� �Ϸ� �� ȣ��Ǵ� �޼���
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        needsReset = true; // �� �� �ε� �� �÷��� �ٽ� Ȱ��ȭ
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �̺�Ʈ ������ ���
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �� �ε� �̺�Ʈ ������ ����
    }
}