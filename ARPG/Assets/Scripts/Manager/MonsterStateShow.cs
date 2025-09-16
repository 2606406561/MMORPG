using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateShow : MonoBehaviour
{
    [SerializeField, Header("生物的位置")] public Transform ts;
    [SerializeField, Header("文本框的位置")] public TextMeshProUGUI tmp;
    private RectTransform ts1;//UI位置
    private Vector3 nowPos;
    Vector3 offset = new Vector3(0, 3, 0);
    [Header("血条设置")]
    [SerializeField] private Image healthFill;       // 血条填充图像
    [SerializeField] private Image damageEffectFill; // 掉血动画效果图像
    [SerializeField] private float damageEffectSpeed = 5f; // 掉血动画速度

    [Header("颜色设置")]
    [SerializeField] private Color fullHealthColor = Color.green;   // 满血颜色
    [SerializeField] private Color mediumHealthColor = Color.yellow;// 中等血量颜色
    [SerializeField] private Color lowHealthColor = Color.red;      // 低血量颜色
    [SerializeField] private float mediumHealthThreshold = 0.5f;    // 中等血量阈值(百分比)
    [SerializeField] private float lowHealthThreshold = 0.2f;       // 低血量阈值(百分比)

    public float _currentHealth;      // 当前血量
    private float _targetHealth;       // 目标血量(受到伤害后的血量)
    private float _maxHealth;          // 最大血量
    private void Start()
    {
        ts1 = GameObject.Find("Canvas").GetComponent<RectTransform>();
        transform.SetParent(ts1);
    }
    private void Update()
    {
        if (ts == null)
        {
            Destroy(gameObject);
            return;
        }

        nowPos = Camera.main.WorldToScreenPoint(ts.position + offset);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ts1, nowPos, null, out localPos);
        transform.localPosition = localPos;
        // 实现掉血动画效果(延迟追赶当前血量)
        if (damageEffectFill.fillAmount > healthFill.fillAmount)
        {
            damageEffectFill.fillAmount = Mathf.Lerp(
                damageEffectFill.fillAmount,
                healthFill.fillAmount,
                Time.deltaTime * damageEffectSpeed
            );
        }
        else
        {
            damageEffectFill.fillAmount = healthFill.fillAmount;
        }
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 初始化血条
    /// </summary>
    /// <param name="maxHealth">最大血量</param>
    public void Initialize(float maxHealth,float nowHealth)
    {
        if (nowHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        _maxHealth = maxHealth;
        _currentHealth = nowHealth;
        _targetHealth = nowHealth;
        tmp.text = _currentHealth.ToString() + "/" + maxHealth.ToString();
        UpdateHealthBarVisuals();
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        // 计算新的血量(确保不会低于0)
        _targetHealth = Mathf.Max(0, _targetHealth - damage);
        tmp.text = (_currentHealth - damage <= 0 ? 0 : _currentHealth - damage).ToString() + "/" + _maxHealth.ToString();


        // 平滑过渡到新的血量
        StartCoroutine(SmoothlyUpdateHealth());

        // 更新颜色
        UpdateHealthColor();
    }

    /// <summary>
    /// 平滑更新血量显示
    /// </summary>
    private System.Collections.IEnumerator SmoothlyUpdateHealth()
    {
        // 当当前血量还没达到目标血量时，持续更新
        while (Mathf.Abs(_currentHealth - _targetHealth) > 0.1f)
        {
            _currentHealth = Mathf.Lerp(_currentHealth, _targetHealth, Time.deltaTime * 5f);
            healthFill.fillAmount = _currentHealth / _maxHealth;
            yield return null;
        }

        // 最终精确设置到目标值
        _currentHealth = _targetHealth;
        healthFill.fillAmount = _currentHealth / _maxHealth;
    }

    /// <summary>
    /// 更新血条颜色
    /// </summary>
    private void UpdateHealthColor()
    {
        float healthPercentage = _currentHealth / _maxHealth;

        // 根据血量百分比设置颜色
        if (healthPercentage <= lowHealthThreshold)
        {
            healthFill.color = lowHealthColor;
        }
        else if (healthPercentage <= mediumHealthThreshold)
        {
            healthFill.color = mediumHealthColor;
        }
        else
        {
            healthFill.color = fullHealthColor;
        }
    }

    /// <summary>
    /// 初始化血条视觉效果
    /// </summary>
    private void UpdateHealthBarVisuals()
    {
        float fillAmount = _currentHealth / _maxHealth;
        healthFill.fillAmount = fillAmount;
        damageEffectFill.fillAmount = fillAmount;
        UpdateHealthColor();
    }

    /// <summary>
    /// 恢复生命值
    /// </summary>
    public void Heal(float amount)
    {
        if (amount <= 0) return;

        _targetHealth = Mathf.Min(_maxHealth, _targetHealth + amount);
        StartCoroutine(SmoothlyUpdateHealth());
        UpdateHealthColor();
    }
}
