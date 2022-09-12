using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 5;

    [SerializeField] Transform _projectilePoint;
    [SerializeField] GameObject _projectile;

    private bool _canFire = true;
    private void Update()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time * speed, 26) -13.5f, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (_canFire)
        {
            BossBasicAttack();
            _canFire = false;
            StartCoroutine(Timer());
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        _canFire = true;
    }

    private void BossBasicAttack()
    {
        Instantiate(_projectile, _projectilePoint.transform.position, transform.rotation, _projectilePoint);
    }
}

