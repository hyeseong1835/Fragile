using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject dummyPrefab;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
    }
    void Start()
    {
        if (PlayerController.instance == null)
        {
            Instantiate(playerPrefab);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(dummyPrefab);
        }
    }
}
