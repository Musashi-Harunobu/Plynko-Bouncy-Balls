using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CannonPowerController : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    
    // Вместо одного префаба мяча — 4 разных (если у вас они есть),
    // либо можете хранить их в массиве, но в данном примере явно:
    [SerializeField] private GameObject redBallPrefab;
    [SerializeField] private GameObject purpleBallPrefab;
    [SerializeField] private GameObject yellowBallPrefab;
    [SerializeField] private GameObject greenBallPrefab;

    [SerializeField] private float shotDelay = 0.5f;

    private float _chargeStartTime;
    private float _maxChargeTime = 1f;
    private bool _isCharging = false;
    private bool _isShooting = false;

    private void Start()
    {
        // Инициируем новый раунд (опционально)
        GameManager.Instance.StartNewRound();
    }

    private void Update()
    {
        if (_isCharging)
        {
            UpdatePower();
        }
        
        FindObjectOfType<BallsList2D>().RefreshUI();
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
        var gm = GameManager.Instance;

        if (gm.IsBallInFlight)
        {
            Debug.Log("Мяч уже в полёте! Дождитесь его попадания в лунку.");
            yield break;
        }

        if (gm.roundBalls.Count == 0)
        {
            gm.CheckEndRound();
            yield break;
        }

        gm.SetBallInFlight(true);

        _isShooting = true;

        var ballType = gm.roundBalls[0];
        gm.roundBalls.RemoveAt(0);

        GameObject chosenPrefab = null;
        switch (ballType)
        {
            case GameManager.BallType.Red:
                chosenPrefab = redBallPrefab;
                break;
            case GameManager.BallType.Purple:
                chosenPrefab = purpleBallPrefab;
                break;
            case GameManager.BallType.Yellow:
                chosenPrefab = yellowBallPrefab;
                break;
            case GameManager.BallType.Green:
                chosenPrefab = greenBallPrefab;
                break;
        }

        float power = Mathf.Clamp01(Time.time - _chargeStartTime);
        GameObject ball = Instantiate(chosenPrefab, transform.position, Quaternion.identity);
        Ball ballComponent = ball.GetComponent<Ball>();
        if (ballComponent != null)
        {
            ballComponent.ballType = ballType;
        }

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.AddForce(-transform.up * power * 500f);

        yield return new WaitForSeconds(shotDelay);
        _isShooting = false;
    }




    private void UpdatePower()
    {
        float elapsedTime = Time.time - _chargeStartTime;
        float power = Mathf.Clamp01(elapsedTime / _maxChargeTime);
        powerSlider.value = power;
    }
}
