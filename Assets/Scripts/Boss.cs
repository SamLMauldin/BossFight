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
    [SerializeField] AudioClip _fireSound2;
    [SerializeField] Transform _spinningPosistion;
    [SerializeField] Transform _pingPongPosistion;
    [SerializeField] Transform _attack2Posistion;
    [SerializeField] GameObject[] _secondAttacks;
    private bool _atSpawn = false;
    private bool _atFirstBase = false;
    private bool _atSecondAttack = false;
    private bool _canFire = true;
    public bool _currentMovement = false;
    public bool _currentAttack;
    private Health _bossCurrentHealth;
    Rigidbody _rb;
    public Quaternion originalRotationValue;

    private int _secondAttackIndex = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        originalRotationValue = transform.rotation;
        _bossCurrentHealth = this.GetComponent<Health>();
        _currentAttack = true;
    }
    private void Update()
    {
        SwitchingMovement();
        //Attack2Movement();
        //if (_currentMovement)
        {
            //Movement1();
        }
        //else
        {
            //Movement2();
        }
    }

    private void FixedUpdate()
    {
        if (_canFire)
        {
            if (_currentAttack == true)
            {
                BossBasicAttack();
            }
            else
            {
                BossAttack2();
            }
            _canFire = false;
            StartCoroutine(Timer());
        }
        //StartCoroutine(MoveTimer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
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
        if (_atSecondAttack)
        {
            Instantiate(_secondAttacks[_secondAttackIndex], _projectilePoint2.transform.position, transform.rotation, _projectilePoint2);
            Feedback2();
            _secondAttackIndex++;
            if(_secondAttackIndex >= _secondAttacks.Length)
            {
                _secondAttackIndex = 0;
            }
        }
    }

    private void Movement1()
    {
        if (_atSpawn)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * speed);
            _atSpawn = false;
        }
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

    private void Attack2Movement()
    {
        if (_atSecondAttack == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _attack2Posistion.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _attack2Posistion.position) <= 1f)
            {
                _atSecondAttack = true;
            }
        }
    }

    private void Feedback()
    {
        if (_fireSound != null)
        {
            AudioHelper.PlayClip2D(_fireSound, 1f);
        }
    }

    private void Feedback2()
    {
        if (_fireSound != null)
        {
            AudioHelper.PlayClip2D(_fireSound2, 1f);
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

    private void SwitchingMovement()
    {
        if(_bossCurrentHealth._currentHealth >= _bossCurrentHealth._maxHealth-150f)
        {
            Movement1();
        }
        else if (_bossCurrentHealth._currentHealth >= _bossCurrentHealth._maxHealth-450f)
        {
            Movement2();
        }
        else if (_bossCurrentHealth._currentHealth >= _bossCurrentHealth._maxHealth-600f)
        {
            Movement1();
        }
        else if(_bossCurrentHealth._currentHealth >= (_bossCurrentHealth._maxHealth -1000f))
        {
            _currentAttack = false;
            Attack2Movement();
        }
    }
}

