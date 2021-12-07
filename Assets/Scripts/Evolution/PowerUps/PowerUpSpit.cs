using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpit : MonoBehaviour
{
    public GameObject playerObj;

    public PowerUpPick Pick;
    public GameObject Spit;

    public GameObject bulletSpawnPoint;
    public float waitTime;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = CameraInterface.camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
        }
        gameObject.transform.LookAt(hit.point);

        Spit.SetActive(Pick.canSpit);
        if (Pick.canSpit)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Spit");
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position, playerObj.gameObject.transform.rotation);
        //playerObj.gameObject.transform.rotation);
    }
}
