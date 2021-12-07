using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSprint : MonoBehaviour
{
    public PowerUpPick Pick;
    public ControllerScript Move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Pick.canSprint)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Move.speed += 60;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Move.speed -= 60;
            }
        }
    }
}
