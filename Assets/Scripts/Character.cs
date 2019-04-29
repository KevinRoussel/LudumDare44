using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Character : MonoBehaviour {
    
    #region Events
    public event Action OnReady;
    public event Action StartWalking;
    public event Action EndWalking;
    public event Action StartDeath;
    public event Action EndDeath;
    public event Action StartAttack;
    public event Action StartHit;
    public event Action<int> OnTakeDamage;
    public event Action EndHit;

    public event Action<Vector3> OnStartDodge;
    public event Action OnEndDodge;

    Trigger _attackEnd = new Trigger();
    Trigger _hitEnd = new Trigger();
    Trigger _deathEnd = new Trigger();
    void AcceptAttackEnd () => _attackEnd.Activate();
    internal void AcceptHitEnd () => _hitEnd.Activate();

    internal void AcceptDeathEnd () => _deathEnd.Activate();
    #endregion

    [SerializeField] Collider _hitZone;
    [SerializeField] protected NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    [SerializeField] BaseEnemy _enemyComp;

    protected bool _canMove;
    public Vector3 Position => transform.position;
    public BaseEnemy Enemy => _enemyComp;

    [Header("Conf")]
    [SerializeField] float _recoverTime;

    [Header("Stat")]
    [SerializeField] int _initialHP;
    [SerializeField] int _initialAttack;
    [SerializeField] float _initialFireRate;
    [SerializeField] int _initialDefense;
    [SerializeField] int _initialSpeed;

    public int HPMax => _initialHP;

    [Header("Shoot")]
    [SerializeField] Vector2 _shootingSpreadRange;
    [SerializeField] Transform _shootingTransform;

    ShootingManager _shootingManager;

    public int HP { get; private set; }
    public int Attack { get; private set; }
    public float FireRate { get; set; }
    public int Defense { get; private set; }
    public int Speed { get; private set; }

    public event Action OnKeyCollected;
    public event Action<int> OnHPMaxUpdated;

    public UnityEvent OnMoveOn;
    public UnityEvent OnMoveOff;
    public UnityEvent OnFireOn;
    public UnityEvent OnHitEvent;
    public UnityEvent OnDeathEvent;
    public UnityEvent OnSHieldStart;
    public UnityEvent OnSHieldHit;
    public UnityEvent OnLaunchRage;
    public UnityEvent OnFlash;


    internal Character Initialization () {
        // Parameters validation & assignation
        HP = _initialHP;
        Attack = _initialAttack;
        FireRate = _initialFireRate;
        Defense = _initialDefense;
        Speed = _initialSpeed;

        // Setup Rage
        RageCooldown = _rageCooldown;

        // Setup Shield
        OnShieldOn += () => { _shield.SetActive(true); };
        OnShieldOff += () => { _shield.SetActive(false); };

        // Event Preparation
        MovementEventInitialization();

        // NavMesh Initialization
        transform.position = NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 500f, NavMesh.AllAreas) ?
            hit.position : transform.position;

        _navMeshAgent.enabled = true;
        _enemyComp?.Initialization();

        OnReady?.Invoke();

        _shootingManager = GameObject.Find("ShootingManager").GetComponent<ShootingManager>();

        StartCoroutine(UpdateOffsetFactor());

        return this;
    }

    #region Skill

    public enum SkillChoice { Null, Rage, Shield, Flash}
    SkillChoice _selectedSkill = SkillChoice.Null;
    public void SetSkill(SkillChoice choice)
    {
        _selectedSkill = choice;
    }

    public void LaunchSkill()
    {
        switch (_selectedSkill)
        {
            case SkillChoice.Rage:
                Debug.Log("Launch Rage");
                StartRage();
                break;
            case SkillChoice.Shield:
                Debug.Log("Launch Shield");
                StartShield();
                break;
            case SkillChoice.Flash:
                Debug.Log("Launch Flash");
                StartFlash();
                break;
            case SkillChoice.Null:
            default:
                break;
        }
    }

    #endregion

    #region Move

    Vector2 _lastMovement;
    bool _isWalking;
    public void FireWalk() => StartWalking?.Invoke();
    public void FireStopWalk() => EndWalking?.Invoke();

    public enum OffsetType {
        NONE,
        FORWARD,
        BACKWARD,
        RIGHT,
        LEFT
    }

    OffsetType _offsetType;
    public void SetOffset(OffsetType type) => _offsetType = type;

    bool _offsetActivated = false;
    public bool OffsetActivated() => _offsetActivated;

    public void BlockMovement(bool canMove)
    {
        _canMove = canMove;
    }

    void MovementEventInitialization () {
        OnAttack += () => { EndWalking?.Invoke(); };

        StartWalking += () => {
            _isWalking = true;
            _navMeshAgent.isStopped = false;
        };

        EndWalking += () => {
            _isWalking = false;
            _navMeshAgent.isStopped = true;
            _lastMovement = Vector2.zero;
        };

        OnShieldOn += () => {
            if (_isWalking) EndWalking?.Invoke();
        };
    }

    public IEnumerator UpdateOffsetFactor() {
        while (true) {
            float waitTime = 0f;
            if (_offsetActivated) {
                _offsetActivated = false;
                waitTime = UnityEngine.Random.Range(2, 5);
            } else {
                _offsetActivated = true;
                waitTime = UnityEngine.Random.Range(1, 3);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void Move (Character target) => Move(new Vector2(target.Position.x - Position.x, target.Position.z - Position.z));

    public void Move (Vector2 direction) {
        if (HP <= 0) {
            //Debug.Log("Can't move => Already dead", this);
            return;
        }

        // if (IsShieldActivated)
        // {
        //     Dodge(direction);
        // }

        // if (IsShieldActivated) return;

        // Finished Movement 
        if (direction.magnitude < 0.01f) {
            if (_lastMovement != Vector2.zero) {
                OnMoveOff?.Invoke();
                EndWalking?.Invoke();
            }
            return;
        }

        // Start walking
        if (direction.magnitude > 0.01f && _lastMovement == Vector2.zero) {
            OnMoveOn?.Invoke();
            StartWalking?.Invoke();
        }

        var realDirection = new Vector3(direction.x, 0, direction.y);

        _navMeshAgent.Warp(transform.position);
        _navMeshAgent.Move(realDirection * Time.deltaTime * _navMeshAgent.speed);

        _lastMovement = direction;
    }

    public Vector3 ComputeOffset() {
        switch (_offsetType) {
            case OffsetType.FORWARD:
                return transform.forward;
            case OffsetType.BACKWARD:
                return -transform.forward;
            case OffsetType.LEFT:
                return -transform.right;
            case OffsetType.RIGHT:
                return transform.right;
            default:
                return Vector3.zero;
        }
    }

    public void LookAt(Vector2 target) {
        Vector3? result = GetRaycastResult(target);

        if (result.HasValue) {
            transform.LookAt(new Vector3(result.Value.x, transform.position.y, result.Value.z));
        }
    }
    #endregion

    #region Attack
    public event Action OnAttack;
    public event Action OnStopAttack;

    internal void EventFire() => OnAttack?.Invoke();
    internal void EventFireStop() => OnStopAttack?.Invoke();

    bool _canAttack = true;
    bool _isAttacking = false;

    public bool LaunchAttack () {
        if (!_canAttack) return false;

        _isAttacking = true;
        StartCoroutine(CallAttack());
        return true;
    }

    internal void StopAttack () {

        OnStopAttack?.Invoke();
        _isAttacking = false;
    }

    float _shootConeAttack=0f;
    public void ActivateConeAttack(float range) => _shootConeAttack = range;

    IEnumerator CallAttack () {

        while (_isAttacking) {

            Vector3? result = GetRaycastResult(Input.mousePosition);

            if (result.HasValue) {

                OnFireOn?.Invoke();
                Shoot(0);

                if (_shootConeAttack != 0f) {

                    Shoot(_shootConeAttack);
                    Shoot(-_shootConeAttack);

                }

                OnAttack?.Invoke();
            }

            yield return new WaitForSeconds(FireRate);
        }
    }

    Vector3? GetRaycastResult (Vector2 target) {
        Ray ray = Camera.main.ScreenPointToRay(target);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Player")) {
            return hit.point;
        } else {
            return null;
        }
    }

    void Shoot(float rightOffset) {

        BaseProjectile projectile = _shootingManager.Shoot(_shootingTransform, "Enemy", Vector3.zero, Attack, rightOffset);

        projectile.Speed *= _rageOn ? _rageProjectilesSpeedMultiplier : 1;

        projectile.transform.localScale *= (Lvl3RageProjectilesSizeMultiplier != 0) ? Lvl3RageProjectilesSizeMultiplier : 1;

    }
    #endregion

    #region Rage
    [Header("Rage variables")]
    [Tooltip("Duration of the rage")]
    [SerializeField] float _rageDuration;

    [Tooltip("Cooldown of the rage")]
    [SerializeField] float _rageCooldown;
    public float RageCooldown { get; set; }

    [Tooltip("Damage multiplier while rage is on")]
    [SerializeField] int _rageMultiplier;

    [Tooltip("Speed multiplierfor the projectiles while rage is on")]
    [SerializeField] float _rageProjectilesSpeedMultiplier;

    [Tooltip("Percentage of the base fire rate while rage is on")]
    [Range(0, 10)]
    [SerializeField] float _rageFireRatePercentage;

    public Action OnRageStart;
    public Action OnRageEnd;
    public Action OnRageReady;

    bool _canRage, _rageOn;

    public float Lvl3RageProjectilesSizeMultiplier { get; set; }

    Coroutine _rageCoroutine;

    public void StartRage () {
        if (_rageCoroutine==null) _rageCoroutine=StartCoroutine(Rage());

        IEnumerator Rage()
        {
            OnLaunchRage?.Invoke();
            OnRageStart?.Invoke();
            Attack *= _rageMultiplier;
            FireRate *= _rageFireRatePercentage;

            yield return new WaitForSeconds(_rageDuration);
            OnRageEnd?.Invoke();

            Attack /= _rageMultiplier;
            FireRate /= _rageFireRatePercentage;

            yield return new WaitForSeconds(RageCooldown);

            OnRageReady?.Invoke();
            _rageCoroutine = null;
        }
    }

    #endregion

    #region Shield
    [Header("Shield variables")]
    [Tooltip("Shield gameobject")]
    [SerializeField] GameObject _shield;

    [Tooltip("Duration of the shield")]
    [SerializeField] float _shieldDuration;

    [Tooltip("Cooldown of the shield")]
    [SerializeField] float _shieldCooldown;

    Coroutine _shieldRoutine;
    public event Action OnShieldOn;
    public event Action OnShieldOff;
    public event Action OnShieldHit;


    public void StartShield () {

        if (_shieldRoutine == null) _shieldRoutine=StartCoroutine(Shield());
        IEnumerator Shield()
        {
            OnSHieldStart?.Invoke();
            OnShieldOn();
            yield return new WaitForSeconds(_shieldDuration);

            OnShieldOff();

            yield return new WaitForSeconds(_shieldCooldown);
            _shieldRoutine = null;
        }
    }
    #endregion

    #region Flash

    [Header("Flash variables")]
    [Tooltip("Flash radius")]
    [SerializeField] float _flashRadius;

    [Tooltip("Duration of the flash")]
    [SerializeField] float _flashDuration;

    [Tooltip("Flash cooldown")]
    [SerializeField] float _flashCooldown;

    public event Action OnFlashLauched;
    public event Action OnFlashReady;
    public event Action OnFlashed;
    Coroutine _flashRoutine;

    public void Flashed() => OnFlashed?.Invoke();
    public Vector2 FlashUpgradeEffect { get; set; }

    public void StartFlash () {
        if (_flashRoutine==null) _flashRoutine=StartCoroutine(Flash());

        IEnumerator Flash()
        {
            OnFlash?.Invoke();
            OnFlashLauched?.Invoke();

            foreach (Character e in transform.GetComponentInParent<Room>().Enemies)
                if (Vector3.Distance(e.transform.position, transform.position) <= _flashRadius)
                    e.Enemy?.Flashed(_flashDuration, FlashUpgradeEffect);

            NavMeshAgent.speed *= FlashUpgradeEffect.x;

            Extension.WaitSecondsAnd(FlashUpgradeEffect.y, () => { NavMeshAgent.speed /= FlashUpgradeEffect.x; });

            Debug.Log("Flash", this);
            yield return new WaitForSeconds(_flashCooldown);

            OnFlashReady?.Invoke();
            Debug.Log("Flash re-enabled", this);
            _flashRoutine = null;
        }
    }

    #endregion

#if false
    #region Dodge
    Coroutine _dodgeProcess;
    float _magnitudeStreshold = 0.2f;
    float _lastDodgeDate;
    float _dodgeCooldown = 0.4f;

    void Dodge(Vector2 direction, bool log = true)
    {
        if (_dodgeProcess != null) return;
        if (_shieldProcess == null) return;
        if (direction.magnitude < _magnitudeStreshold) return;
        if (LiveCharacter.CurrentShield < LiveCharacter.Definition.DodgeCost) return;
        if (_lastDodgeDate + _dodgeCooldown > Time.fixedTime) return;

        LiveCharacter.DecreaseShield(LiveCharacter.Definition.DodgeCost);
        _healthSlider.UpdateShield();
        _lastDodgeDate = Time.fixedTime;
        _dodgeProcess = StartCoroutine(DodgeProcess());

        IEnumerator DodgeProcess()
        {
            Keyframe destinationKey = LiveCharacter.Definition.DodgeConfiguration.keys.Last();
            var startPosition = transform.position;
            var lastOffsetApplyed = Vector3.zero;
            var realDirection = new Vector3(direction.x, 0, direction.y).normalized;
            var destinationPosition = transform.position + (realDirection * destinationKey.value);
            SpecialCountDown _timer = new SpecialCountDown(destinationKey.time);

            if (log)
            {
                Debug.Log($"Joystick : {realDirection}");
                Debug.Log($"start : {startPosition}");
                Debug.Log($"destionation : {destinationPosition}");
                Debug.Log($"destination - start : {destinationPosition - startPosition}");
                Debug.Log($"movement duration : {destinationKey.time}");
                Debug.Log("[Dodge started]");
            }

            OnStartDodge?.Invoke(destinationPosition - startPosition);

            // Freeze rotation movement
            var angularSpeedCache = _navMeshAgent.angularSpeed;
            _navMeshAgent.angularSpeed = 0f;
            while (!_timer.isDone)
            {
                var delta = (realDirection * LiveCharacter.Definition.DodgeConfiguration.Evaluate(_timer.ConsumedTime)) - lastOffsetApplyed;
                if (log) Debug.Log(delta);
                _navMeshAgent.Move(delta);
                lastOffsetApplyed += delta;
                yield return null;
            }
            if (log) Debug.Log("[Dodge finished]");

            _navMeshAgent.angularSpeed = angularSpeedCache;
            OnEndDodge?.Invoke();
            _dodgeProcess = null;
            yield break;
        }
    }


    #endregion
#endif

    #region Hit&Death IDestroyable Implementation
    bool _isInvincible = false;
    Coroutine _hitCoroutine;

    Coroutine _blockHit;
    public void LaunchAutoBlock(float cooldown)
    {
        if(_blockHit==null)
        {
            _blockHit = StartCoroutine(BlockHit(cooldown));
            IEnumerator BlockHit(float coolDown)
            {
                while (true)
                {
                    _isInvincible = true;
                    bool done = false;
                    Action<int> a = (hp) => done = true;
                    OnTakeDamage += a;
                    yield return new WaitWhile(() => !done);

                    OnTakeDamage -= a;
                    _isInvincible = false;
                    yield return new WaitForSeconds(coolDown);
                }
            }
        }
    }

    public void Hit(Character instigator, int amount)
    {
        if (_hitCoroutine != null)
        {
            StopCoroutine(_hitCoroutine);
        }

        _hitCoroutine = StartCoroutine(HitRoutine());
        IEnumerator HitRoutine()
        {
            if (_shield!=null && (_shield?.gameObject.activeInHierarchy??false))
            {
                OnShieldHit?.Invoke();
                OnSHieldHit?.Invoke();
                _hitCoroutine = null;
                yield break;
            }

            if (_isInvincible)
            {
                Debug.Log("Blocked");
                amount = 0;
            }

            HP = Mathf.Max(0, HP - (((Lvl3RageProjectilesSizeMultiplier != 0) && _rageOn) ? 0 : amount));

            OnHit?.Invoke(instigator, amount);
            OnHitEvent?.Invoke();
            print(HP);
            OnTakeDamage?.Invoke(HP);

            if (HP <= 0)
            {
                GameObject.FindObjectOfType<EndingDecision>().AddEvilPoint();
                OnDeathEvent?.Invoke();
                StartDeath?.Invoke();
                foreach (var el in GetComponentsInParent<Collider>()) el.enabled = false;
                _navMeshAgent.enabled = false;

                yield return new WaitForSeconds(2f);

                EndDeath?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                StartHit?.Invoke();
            }

            _hitCoroutine = null;
            yield break;
        }

    }

    public delegate void OnHitDelegate (Character instigator, int amount);
    public OnHitDelegate OnHit { get; set; }
    #endregion

}

