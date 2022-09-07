using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inheritance
{
    [RequireComponent(typeof(BallMotor))]
    public class Player : MonoBehaviour
    {
        BallMotor _ballMotor;

        private void Awake()
        {
            _ballMotor = GetComponent<BallMotor>();
        }

        private void FixedUpdate()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

            _ballMotor.Move(movement);
        }
    }
}

