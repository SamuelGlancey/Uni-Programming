using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void upgradeEvent(int level);
public class upgrade : MonoBehaviour
{
    public event upgradeEvent upgraded;
    public int baseCost;
    public int costPerLevel;
    int level;
    int currentCost;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentCost = baseCost + level * costPerLevel;
        costText.text = $"{currentCost}";
    }

    public void buy()
    {
        if(PlayerController_.rolls >= currentCost)
        {
            PlayerController_.rolls -= currentCost;
            level++;
            levelText.text = $"{level}";
            upgraded(level);
            if (level == 3)
            {
                GetComponent<Button>().interactable = false;
                levelText.text = $"MAX";
                costText.gameObject.SetActive(false);
            }
        }
    }
}
