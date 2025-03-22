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
    }

    // Нажали на кнопку GO
    public void OnButtonDown()
    {
        if (!_isShooting)
        {
            _isCharging = true;
            _chargeStartTime = Time.time;
        }
    }

    // Отпустили кнопку GO
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

        // Если нет шаров — проверяем конец раунда
        if (gm.roundBalls.Count == 0)
        {
            gm.CheckEndRound();
        }

        // Если всё ещё есть шары
        if (gm.roundBalls.Count > 0)
        {
            _isShooting = true;

            // Берём первый тип мяча из списка
            var ballType = gm.roundBalls[0];
            gm.roundBalls.RemoveAt(0);

            // Выбираем подходящий префаб
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

            // Создаём мяч
            GameObject ball = Instantiate(chosenPrefab, transform.position, Quaternion.identity);

            // Убедимся, что в этом префабе есть скрипт Ball,
            // можно вручную прицепить, а можно на префабах.
            // Запишем в него текущий BallType
            Ball ballComponent = ball.GetComponent<Ball>();
            if (ballComponent != null)
            {
                ballComponent.ballType = ballType;
            }

            // Применяем силу в направлении вниз (пушка смотрит вверх, 
            // поэтому берём -transform.up)
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            rb.AddForce(-transform.up * power * 500f);

            yield return new WaitForSeconds(shotDelay);
            _isShooting = false;

            // Если после выстрела шаров больше не осталось
            if (gm.roundBalls.Count == 0)
            {
                gm.CheckEndRound();
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
