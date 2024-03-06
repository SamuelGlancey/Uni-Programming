using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonManager : MonoBehaviour
{

    public Slider moveSpeed;
    public Slider turnSpeed;
    public PlayerController_ pc;

    public upgrade moveSpeedUpgrade;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController_>();
    }

    private void OnEnable()
    {
        moveSpeedUpgrade.upgraded += UpgradeMoveSpeed;
    }
    private void OnDisable()
    {
        moveSpeedUpgrade.upgraded -= UpgradeMoveSpeed;
    }

    void UpgradeMoveSpeed(int level)
    {
        moveSpeed.maxValue += level;
    }
    // Update is called once per frame
    void Update()
    {
        pc.SetGroundSpeed(moveSpeed.value);
        pc.SetTurnSpeed(turnSpeed.value);
    } 
}
