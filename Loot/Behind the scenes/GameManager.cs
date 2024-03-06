using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void pauseEvent();
public class GameManager : MonoBehaviour
{
    public static event pauseEvent pause;
    public static event pauseEvent unPause;
    public GameObject Map;
    public Transform cameraModeCamera;
    public List<GameObject> GamePlayButtons;
    public List<GameObject> MapButtons;
    public List<GameObject> CamButtons;
    public List<GameObject> ToiletButtons;
    public Transform destination;
    public GameObject endscreen;
    bool moveCam;

    [Header("Digsites and golden stuff")]
    public List<dropObject> DigSites;
    public List<GameObject> goldenObjects;
    


    [Header("shovel")]
    public GameObject playerShovel, playerGoldShovel, goldShovel, enemyShovel;

    [Header("upgrades")]
    public upgrade attackRange;
    [Header("cave")]
    public Animator cave;
    public enum GameState
    {
        GAMEPLAY,
        MAP,
        CAMERA,
        TOILET,
        FINISH
    }
    public static GameState currentState = GameState.GAMEPLAY;

    public void Start()
    {
        currentState = GameState.GAMEPLAY;
        allocateGoldenObjects();
    }

    public void growShovel(int level)
    {
        playerShovel.transform.localScale *= 1 + level / 2;
    }
    void allocateGoldenObjects()
    {
        for(int i = 0; i < 3; i++)
        {
            dropObject d = DigSites[Random.Range(0, DigSites.Count)];
            d.drops.Add(goldenObjects[i]);
            d.dropChances.Add(100);
            DigSites.Remove(d);
        }
    }

    void takeShovel()
    {
        playerShovel.SetActive(false);
        playerGoldShovel.SetActive(true);
        goldShovel.SetActive(false);
        enemyShovel.SetActive(true);
    }

    void closeCave()
    {

        StartCoroutine(closeCaveAfterTime());
    }

    IEnumerator closeCaveAfterTime()
    {
        yield return new WaitForSeconds(10f);
        Camera.main.transform.parent = cave.transform;
        cave.SetTrigger("close");
        currentState = GameState.FINISH;
        moveCam = true;
    }
    void moveCamera()
    {

        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, destination.position, 5 * Time.deltaTime);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, destination.rotation, 1 * Time.deltaTime);
    }

    public void Update()
    {
        switch (currentState)
        {
            case GameState.MAP:
                Map.SetActive(true);
                changeButtons(GamePlayButtons, false);
                changeButtons(MapButtons, true);
                break;
            case GameState.GAMEPLAY:
                Map.SetActive(false);
                changeButtons(GamePlayButtons, true);
                changeButtons(MapButtons, false);
                changeButtons(CamButtons, false);
                changeButtons(ToiletButtons, false);
                break;
            case GameState.CAMERA:
                changeButtons(GamePlayButtons, false);
                changeButtons(CamButtons, true);
                break;
            case GameState.TOILET:
                changeButtons(GamePlayButtons, false);
                changeButtons(ToiletButtons, true);
                break;
            case GameState.FINISH:
                changeButtons(GamePlayButtons, false);
                changeButtons(MapButtons, false);
                changeButtons(CamButtons, false);
                changeButtons(ToiletButtons, false);
                endscreen.SetActive(true);
                ToiletButtons[0].transform.parent.parent.gameObject.SetActive(false);
                break;
        }
        if (moveCam)
        {
            moveCamera();
        }
    }
    public void openMap()
    {
        pause();
        currentState = GameManager.GameState.MAP;
    }
    public void backToGameplay()
    {
        unPause();
        currentState = GameManager.GameState.GAMEPLAY;
    }
    public void enterCameraMode()
    {
        pause();
        cameraModeCamera.localRotation = Quaternion.identity;
        currentState = GameManager.GameState.CAMERA;
    }
    public static void sitOnToilet()
    {
        pause();
        GameManager.currentState = GameManager.GameState.TOILET;
    }
    public static void getOffToilet()
    {
        unPause();
        GameManager.currentState = GameManager.GameState.GAMEPLAY;
    }

    public void changeButtons(List<GameObject> btns, bool trfalsue)
    {
        foreach (GameObject btn in btns)
        {
            btn.SetActive(trfalsue);
        }
    }

    private void OnEnable()
    {
        pirateBoss.bossDied += takeShovel;
        pirateBoss.bossDied += closeCave;
        attackRange.upgraded += growShovel;

    }

    private void OnDisable()
    {
        pirateBoss.bossDied -= takeShovel;
        pirateBoss.bossDied -= closeCave;
        attackRange.upgraded += growShovel;
    }
}
