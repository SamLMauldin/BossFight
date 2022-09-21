using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 100;
    [SerializeField] int _currentHealth = 100;
    [SerializeField] Material _damagedMaterial;
    [SerializeField] Material _originalMaterial;
    [SerializeField] ParticleSystem _damagedParticles;
    [SerializeField] AudioClip _damagedSound;
    [SerializeField] ParticleSystem _killParticles;
    [SerializeField] AudioClip _killSound;

    // Update is called once per frame
    void Update()
    {
        if(_currentHealth <= 0)
        {
            Kill();
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
}
