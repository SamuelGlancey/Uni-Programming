using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject Bullet;
    [SerializeField]private Transform rotatingShootPoint;
    [SerializeField]private Transform shootPoint;
    private bool canShoot;
    private int bulletNumber;
    private int shotNumber;
    void Start()
    {
        canShoot = true;
        GetComponent<SpriteRenderer>().color = gameManager.PlayerColor;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        fire();
    }
    void Aim()
    {
        Vector3 shoot = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        if (gameManager.mouseAndKeyboard)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
            rotatingShootPoint.eulerAngles = new Vector3(0f, 0f, AngleBetweenPoints(transform.position, mouseWorldPosition));
        }
        if (!(shoot.x == 0 && shoot.y == 0))
        {
            rotatingShootPoint.eulerAngles = new Vector3(0f, 0f, AngleBetweenPoints(new Vector2(0, 0), shoot));
        }
    }
    void fire()
    {
        if ((Input.GetButton("Fire1") || Input.GetAxis("Shoot") > 0) && canShoot)
        {
            canShoot = false;
            bullet BLT = Instantiate(Bullet, shootPoint.transform.position, shootPoint.rotation).GetComponent<bullet>();
            BLT.thisColor = bulletNumber;
            bulletNumber++;
            if (bulletNumber > BLT.rainbowColors.Count - 1)
            {
                bulletNumber = 0;
            }


            StartCoroutine(shootWait());
        }
    }

    public IEnumerator shootWait()
    {
        yield return new WaitForSeconds(gameManager.shootInterval);
        canShoot = true;
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public int Get_bulletNumber()
    {
        return bulletNumber;
    }

    public void Set_bulletNumber(int setToInt)
    {
        bulletNumber = setToInt;
    }

    public bool Get_canShoot()
    {
        return canShoot;
    }
    public void Set_canShoot(bool setToBool)
    {
        canShoot = setToBool;
    }

}
