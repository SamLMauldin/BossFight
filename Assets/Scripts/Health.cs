using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] public int _maxHealth = 100;
    [SerializeField] public int _currentHealth = 100;
    [SerializeField] Material _damagedMaterial;
    [SerializeField] Material _originalMaterial;
    [SerializeField] ParticleSystem _damagedParticles;
    [SerializeField] AudioClip _damagedSound;
    [SerializeField] ParticleSystem _killParticles;
    [SerializeField] AudioClip _killSound;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] GameObject _damagedPanel = null;


    [SerializeField] Camera _gameCamera = null;
    [SerializeField] Vector3 _originalPosOfCam;
    [SerializeField] float _shakeFrequncy = 5;
    private  bool _damageTaken = false;


    void Start()
    {
        _currentHealth = _maxHealth;
        if(_healthBar != null)
        {
            _healthBar.SetMaxHealth(_maxHealth);
        }
        if(_damagedPanel != null)
        {
            _damagedPanel.SetActive(false);
        }
        if(_gameCamera != null)
        {
            _originalPosOfCam = _gameCamera.transform.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(_currentHealth <= 0)
        {
            Kill();
        }
         if(_gameCamera != null && _damageTaken)
        {
            _gameCamera.transform.position = _originalPosOfCam + Random.insideUnitSphere * _shakeFrequncy;
            if (_gameCamera != null)
            {
                StartCoroutine(CameraShake());
            }
        }

    }
    public void Kill()
    {
        KillFeedback();
        this.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if(_damagedMaterial != null)
        {
            GetComponentInChildren<Renderer>().material = _damagedMaterial;
        }
        _currentHealth -= damage;
        if (_healthBar != null)
        {
            _healthBar.SetHealth(_currentHealth);
        }
        if(_damagedPanel != null)
        {
            StartCoroutine(DamagePanel());
        }
        _damageTaken = true;
        Feedback();
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        GetComponentInChildren<Renderer>().material = _originalMaterial;
    }

    private void Feedback()
    {
        //particles 
        if (_damagedParticles != null)
        {
            _damagedParticles = Instantiate(_damagedParticles, transform.position, Quaternion.identity);
        }
        //audio. TODO - consider Object Pooling for performance
        if (_damagedSound != null)
        {
            AudioHelper.PlayClip2D(_damagedSound, 1f);
        }
    }

    private void KillFeedback()
    {
        //particles 
        if (_killParticles != null)
        {
            _damagedParticles = Instantiate(_killParticles, transform.position, Quaternion.identity);
        }
        //audio. TODO - consider Object Pooling for performance
        if (_killSound != null)
        {
            AudioHelper.PlayClip2D(_killSound, 1f);
        }
    }

    IEnumerator DamagePanel()
    {
        _damagedPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        _damagedPanel.SetActive(false);
    }

    IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(1);
        _gameCamera.transform.position = _originalPosOfCam;
        _damageTaken = false;
    }
}
