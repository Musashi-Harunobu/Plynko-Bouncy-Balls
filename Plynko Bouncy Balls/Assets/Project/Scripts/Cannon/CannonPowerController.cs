using System;
using UnityEngine;
using UnityEngine.UI;
public class CannonPowerController : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private GameObject cannonBall;

    private float _chargeStartTime = 0f;
    private float _maxChargeTime = 1f;
    private float _power;
    private bool _isCharging = false;
    private void Start()
    {
        powerSlider.minValue = 0f;
        powerSlider.maxValue = 1f;
    }

    private void Update()
    {
        if (_isCharging)
        {
            UpdatePower();
        }
    }
    
    public void OnButtonDown()
    {
        _isCharging = true;
        StartCharging();
    }

    public void OnButtonUp()
    {
        _isCharging = false;
    }
    
    
    public void FireCannon()
    {
        _power = Input.GetTouch(0).deltaTime;

        if (_power > 1f)
        {
            _power = 1f;
        }

        GameObject ball = Instantiate(cannonBall, transform.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(-transform.up * _power * 500f);
        Debug.Log(_power);
    }
    
    private void StartCharging()
    {
        _isCharging = true;
        _chargeStartTime = Time.time;
    }
    
    private void UpdatePower()
    {
        float elapsedTime = Time.time - _chargeStartTime;
        float power = Mathf.Clamp01(elapsedTime / _maxChargeTime);

        powerSlider.value = power;
    }

}