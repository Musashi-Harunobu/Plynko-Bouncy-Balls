using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CannonPowerController : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private GameObject cannonBallPrefab;
    [SerializeField] private float shotDelay = 0.5f;

    private float _chargeStartTime;
    private float _maxChargeTime = 1f;
    private bool _isCharging = false;
    private bool _isShooting = false; // 🔹 Блокируем двойные выстрелы

    private Queue<GameManager.BallType> ballQueue = new Queue<GameManager.BallType>();
    

    private void Start()
    {
        GameManager.Instance.StartNewRound();
        LoadBallsFromGameManager();
    }

    private void LoadBallsFromGameManager()
    {
        ballQueue.Clear();
        foreach (var ball in GameManager.Instance.playerBalls)
        {
            ballQueue.Enqueue(ball);
        }
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
        if (!_isShooting) 
        {
            _isCharging = true;
            _chargeStartTime = Time.time;
        }
    }

    public void OnButtonUp()
    {
        if (!_isShooting) 
        {
            _isCharging = false;
            StartCoroutine(FireCannon());
        }
    }

    private IEnumerator FireCannon()
    {
        if (ballQueue.Count == 0)
        {
            LoadBallsFromGameManager();
        }

        if (ballQueue.Count > 0)
        {
            _isShooting = true;
            GameManager.BallType ballType = ballQueue.Dequeue();
            GameManager.Instance.playerBalls.Remove(ballType);

            float power = Mathf.Clamp01(Time.time - _chargeStartTime);
            GameObject ball = Instantiate(cannonBallPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            rb.AddForce(-transform.up * power * 500f);

            Debug.Log($"Выстрел мячом: {ballType}");

            yield return new WaitForSeconds(shotDelay);
            _isShooting = false;

            if (ballQueue.Count == 0 && GameManager.Instance.playerBalls.Count == 0)
            {
                GameManager.Instance.CheckEndRound();
            }
        }
        else
        {
            Debug.Log("Нет мячей для выстрела!");
            _isShooting = false;
        }
    }

    private void UpdatePower()
    {
        float elapsedTime = Time.time - _chargeStartTime;
        float power = Mathf.Clamp01(elapsedTime / _maxChargeTime);
        powerSlider.value = power;
    }
}
