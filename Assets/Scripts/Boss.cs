using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 5;

    [SerializeField] Transform _projectilePoint;
    [SerializeField] Transform _projectilePoint2;
    [SerializeField] GameObject _projectile;
    [SerializeField] AudioClip _fireSound;
    [SerializeField] Transform _spinningPosistion;
    [SerializeField] Transform _pingPongPosistion;
    [SerializeField] Transform _attack2Posistion;
    [SerializeField] GameObject[] _secondAttacks;
    private bool _atSpawn = false;
    private bool _atFirstBase = false;
    private bool _atSecondAttack = false;
    private bool _canFire = true;
    public bool _currentMovement = false;
    Rigidbody _rb;
    public Quaternion originalRotationValue;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        originalRotationValue = transform.rotation; 
        
    }
    private void Update()
    {
        if (_currentMovement)
        {
            //Movement1();
        }
        else
        {
            //Movement2();
        }
        StartCoroutine(MoveTimer());
    }

    private void FixedUpdate()
    {
        BossAttack2();
        if (_canFire)
        {
            //BossBasicAttack();
            _canFire = false;
            StartCoroutine(Timer());
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.75f);
        _canFire = true;
    }
    IEnumerator MoveTimer()
    {
        yield return new WaitForSeconds(20);
        if (_atSpawn)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * speed);
            _atSpawn = false;
        }
        if (_atFirstBase)
        {
            _atFirstBase = false;
        }
        _currentMovement = !_currentMovement;
    }

    private void BossBasicAttack()
    {
        Instantiate(_projectile, _projectilePoint.transform.position, transform.rotation, _projectilePoint);
        Feedback();
    }

    private void BossAttack2()
    {
        if (_atSecondAttack == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _attack2Posistion.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, _attack2Posistion.position) <= 1f)
            {
                _atSecondAttack = true;
            }
        }
        else
        {
            for(int i = 0; i < _secondAttacks.Length; i++)
            {
                Instantiate(_secondAttacks[i], _projectilePoint2.transform.position, transform.rotation, _projectilePoint2);
            }
        }


    }

    private void Movement1()
    {
        if (_atSpawn == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pingPongPosistion.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _pingPongPosistion.position) <= 1f)
            {
                _atFirstBase = true;
            }
            transform.position = new Vector3(Mathf.PingPong(Time.time * speed, 26) - 13.5f, transform.position.y, transform.position.z);
        }
    }

    private void Movement2()
    {
        if (_atSpawn == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _spinningPosistion.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _spinningPosistion.position) <= 1f)
            {
                _atSpawn = true;
            }
        }
        else
        {
            Quaternion turnOffset = Quaternion.Euler(0, speed, 0);
            _rb.MoveRotation(_rb.rotation * turnOffset);
        }
    }
    private void Feedback()
    {
        //audio. TODO - consider Object Pooling for performance
        if (_fireSound != null)
        {
            AudioHelper.PlayClip2D(_fireSound, 1f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health damageableOject = collision.gameObject.GetComponent<Health>();
        if (damageableOject != null)
        {
            damageableOject.TakeDamage(50);
        }
    }
}

