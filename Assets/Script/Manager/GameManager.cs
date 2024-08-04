using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject dummyPrefab;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 1000;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(dummyPrefab);
        }
    }
}
