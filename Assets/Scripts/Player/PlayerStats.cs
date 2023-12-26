using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Variables

    [Header("Main Player Stats")]
    public int PlayerHealth;
    private int maxPlayerHealth = 4;
    public bool isPlayerDead = false;
    [field: SerializeField] public float playerSpeed { get; private set; }

    public float energyLeft
    {
        get
        {
            return _energyLeft;
        }
        set
        {
            _energyLeft = value;

            if (_energyLeft > maxEnergy)
            {
                _energyLeft = maxEnergy;
            }
            else if (_energyLeft < 0)
            {
                _energyLeft = 0;
            }
        }
    }
    private float _energyLeft;
    [SerializeField] private int maxEnergy;
    [field: SerializeField] public float dashPower { get; private set; }

    [Header("UI Controlls")]
    [SerializeField] UIAnimationController UIAnimationController;

    [field: Header("Primary Weapon")]
    [field: SerializeField] public float attackSpeed { get; private set; }
    public float attackCooldown
    {
        get { return _attackCooldown; }

        set
        {
            _attackCooldown = value;

            if (_attackCooldown < 0)
            {
                _attackCooldown = 0;
            }
        }
    }
    private float _attackCooldown { get; set; }
    [field: SerializeField] public float attackRadius { get; private set; }
    [field: SerializeField] public int primaryWeaponDamage { get; private set; }
    [field: SerializeField] public float primaryWeaponKnockback { get; private set; }

    [field: Header("Secondary Weapon")]
    [field: SerializeField] public int secondaryWeaponDamage { get; private set; }
    [field: SerializeField] public float secondaryWeaponProjectileSpeed { get; private set; }
    [field: SerializeField] public float secondaryWeaponProjectileDistance { get; private set; }
    
    #endregion

    private void Awake()
    {
        PlayerHealth = maxPlayerHealth;
        energyLeft = maxEnergy;
    }

    private void Update()
    {
        energyLeft += Time.deltaTime;
        attackCooldown -= Time.deltaTime * attackSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (PlayerHealth > 0)
        {
            UIAnimationController.TakeDamageAnimation();
            PlayerHealth -= damage;
        }
    }

    public void GainHealth(int health)
    {
        if (PlayerHealth < maxPlayerHealth)
        {
            UIAnimationController.GainHealthAnimation();
            PlayerHealth += health;
        }
    }
}
