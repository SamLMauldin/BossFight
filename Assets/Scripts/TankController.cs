using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] float _maxSpeed = .25f;
    [SerializeField] float _turnSpeed = 2f;
    [SerializeField] Transform _projectilePoint;
    [SerializeField] GameObject _projectile;
    [SerializeField] AudioClip _fireSound;
    [SerializeField] float _jumpForce = 10f;
    [SerializeField] GameObject _ground;
    float _offset;

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _offset = (transform.position.y - _ground.transform.position.y)+1f;
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            TankFire();
        }
        Jump();
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _maxSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    public void TankFire()
    {
        Instantiate(_projectile, _projectilePoint.transform.position, transform.rotation, _projectilePoint);
        Feedback();
    }

    private void Feedback()
    {
        //audio. TODO - consider Object Pooling for performance
        if (_fireSound != null)
        {
            AudioHelper.PlayClip2D(_fireSound, 1f);
        }
    }

    private void Jump()
    {
        if (transform.position.y - _ground.transform.position.y < _offset)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _rb.AddForce(Vector3.up * _jumpForce);
            }
        }
    }
}
