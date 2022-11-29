using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EvoCube.Player
{
    public class PlayerFlyingTest : MonoBehaviour
    {
        float flySpeed = 0.5f;
        public GameObject playerObj;
        float accelerationAmount = 3f;
        float accelerationRatio = 1f;
        float slowDownRatio = 0.5f;
        bool shift = false;
        bool ctrl = false;

        [Inject]
        void Construct(Player player)
        {
            playerObj = player.gameObject;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                shift = true;
                flySpeed *= accelerationRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                shift = false;
                flySpeed /= accelerationRatio;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                ctrl = true;
                flySpeed *= slowDownRatio;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                ctrl = false;
                flySpeed /= slowDownRatio;
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.Translate(-playerObj.transform.forward * flySpeed * Input.GetAxis("Vertical"));
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(-playerObj.transform.right * flySpeed * Input.GetAxis("Horizontal"));
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(playerObj.transform.up * flySpeed * 0.5f);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(-playerObj.transform.up * flySpeed * 0.5f);
            }
        }
    }
}


