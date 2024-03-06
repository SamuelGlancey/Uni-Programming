using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class progress : MonoBehaviour
{
    private Text progressText;
    [SerializeField]private Text timeText;
    private float time;
    private float startBlocks;
    public static float progressPercent;
    [SerializeField] private GameObject winScreen;
    // Start is called before the first frame update
    void Start()
    {
        progressText = GetComponent<Text>();
        startBlocks = gameManager.numOfBlocks;
        progressPercent = (int)((gameManager.numOfBlocks / startBlocks) * 100);

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(progressPercent < 2)
        {
            progressPercent = ((gameManager.numOfBlocks / startBlocks) * 100);
        }
        else
        {
            progressPercent = (int)((gameManager.numOfBlocks / startBlocks) * 100);
        }
        timeText.text = $"{time.ToString("F2")}";
        progressText.text = $"%{progressPercent}";
        transform.parent.GetComponent<Image>().color = gameManager.BlockColor;
        if (progressPercent == 0)
        {
            Time.timeScale = 0f;
            winScreen.SetActive(true);
            winScreen.transform.GetChild(1).GetComponent<Text>().text = $"That took {time} Seconds";
        }

    }
}
