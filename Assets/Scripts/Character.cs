using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    #region Events
    public event Action OnReady;
    public event Action StartWalking;
    public event Action EndWalking;
    public event Action StartDeath;
    public event Action EndDeath;
    public event Action StartAttack;
    public event Action StartHit;
    public event Action OnTakeDamage;
    public event Action EndHit;

    public event Action<Vector3> OnStartDodge;
    public event Action OnEndDodge;


    Trigger _attackEnd = new Trigger();
    Trigger _hitEnd = new Trigger();
    Trigger _deathEnd = new Trigger();
    void AcceptAttackEnd() => _attackEnd.Activate();
    internal void AcceptHitEnd() => _hitEnd.Activate();

    

    internal void AcceptDeathEnd() => _deathEnd.Activate();
    #endregion

    [SerializeField] Collider _hitZone;
    [SerializeField] Transform _shieldRoot = null;
    [SerializeField] protected NavMeshAgent _navMeshAgent;

    protected bool _canMove;
    public Vector3 Position => transform.position;

    

    [Header("Conf")]
    [SerializeField] float _recoverTime;

    [Header("Stat")]
    [SerializeField] int _initialHP;
    [SerializeField] int _initialAttack;
    [SerializeField] int _initialDefense;
    [SerializeField] int _initialSpeed;
    [SerializeField] int _initialShield;

    public int HP { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int Speed { get; private set; }
    public int Shield { get; private set; }

    internal Character Initialization()
    {
        // Parameters validation & assignation
        HP = _initialHP;
        Attack = _initialAttack;
        Defense = _initialDefense;
        Speed = _initialSpeed;

        // Event Preparation
        MovementEventInitialization();

        // NavMesh Initialization
        transform.localPosition = Vector3.zero;
        transform.position = NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 500f, NavMesh.AllAreas) ?
            hit.position : transform.position;

        _navMeshAgent.enabled = true;
        OnReady?.Invoke();

        return this;
    }

    #region Move

    Vector2 _lastMovement;
    bool _isWalking;

    public void BlockMovement(bool canMove)
    {
        _canMove = canMove;
    }

    void MovementEventInitialization()
    {
        OnAttack += () => { EndWalking?.Invoke(); };

        StartWalking += () =>
        {
            _isWalking = true;
            _navMeshAgent.isStopped = false;
        };

        EndWalking += () =>
        {
            _isWalking = false;
            _navMeshAgent.isStopped = true;
            _lastMovement = Vector2.zero;
        };

        OnShieldOn += () =>
        {
            if (_isWalking) EndWalking?.Invoke();
        };
    }

    public void Move(Character target) => Move(new Vector2(target.Position.x - Position.x, target.Position.z - Position.z));

    public void Move(Vector2 direction)
    {
        if ( HP<=0 )
        {
            //Debug.Log("Can't move => Already dead", this);
            return;
        }

        // if (IsShieldActivated)
        // {
        //     Dodge(direction);
        // }

        if (IsShieldActivated) return;

        // Finished Movement 
        if (direction.magnitude < 0.01f)
        {
            if (_lastMovement != Vector2.zero)
            {
                EndWalking?.Invoke();
            }
            return;
        }

        // Start walking
        if (direction.magnitude > 0.01f && _lastMovement == Vector2.zero)
        {
            StartWalking?.Invoke();
        }

        var realDirection = new Vector3(direction.x, 0, direction.y);

        _navMeshAgent.Warp(transform.position);
        _navMeshAgent.Move(realDirection * Time.deltaTime * _navMeshAgent.speed);
        transform.LookAt(transform.position + realDirection);

        _lastMovement = direction;
    }
    #endregion

    #region Attack
    public event Action OnAttack;

    bool _canAttack = true;

    public bool LaunchAttack()
    {
        if (!_canAttack ) return false;

        // Fire
        throw new NotImplementedException();

    }

    internal void StopAttack()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Shield
    public event Action OnShieldOn;
    public event Action OnShieldOff;
    public event Action OnShieldHit;
    public event Action OnShieldBreakOff;
    public event Action<int> OnShieldUpdate;

    Coroutine _shieldProcess;
    Coroutine _shieldBreakOff;
    Coroutine _shieldRecover;

    Trigger _stopShield = new Trigger();

    protected bool IsShieldActivated => _shieldProcess != null;

    public bool LaunchShield(bool isActive)
    {
        if (isActive && _shieldProcess == null && _shieldBreakOff == null)
        {
            if (_shieldRecover != null) StopCoroutine(_shieldRecover);
            _shieldProcess = StartCoroutine(ShieldProcess());
            return true;
        }
        else if (!isActive && _shieldProcess != null)
        {
            _stopShield.Activate();
            return true;
        }

        return false;
    }

    IEnumerator ShieldRecover()
    {
        while (Shield <= _initialShield)
        {
            yield return new WaitForSeconds(_recoverTime);
            Shield = Mathf.Min(Shield+1, _initialShield);
            OnShieldUpdate?.Invoke(Shield);
        }
        _shieldRecover = null;
        yield break;
    }
    
    IEnumerator ShieldProcess()
    {
        OnShieldOn?.Invoke();
        _shieldRoot.gameObject.SetActive(true);

        yield return new WaitWhile(() => !_stopShield.IsActivated());

        _shieldRoot.gameObject.SetActive(false);

        if (_shieldBreakOff == null)
        {
            _shieldRecover = StartCoroutine(ShieldRecover());
            OnShieldOff?.Invoke();
        }
        _shieldProcess = null;
        yield break;
    }

    void DecreaseShield(int amount)
    {
        while (true)
        {
            
        }
    }

    void ShieldHit(int amount)
    {
        // Send right animation event
        OnShieldHit?.Invoke();

        // Update UI
        Shield -= amount;
        OnShieldUpdate?.Invoke(Shield);
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

    void Hit(int amount)
    {
        if (_hitCoroutine != null)
        {
            StopCoroutine(_hitCoroutine);
        }

        _hitCoroutine = StartCoroutine(HitRoutine());
        IEnumerator HitRoutine()
        {
            if (IsShieldActivated)
            {
                ShieldHit(amount);
            }

            if (HP <= 0)
            {
                StartDeath?.Invoke();
                foreach (var el in GetComponentsInParent<Collider>()) el.enabled = false;
                _navMeshAgent.enabled = false;

                EndDeath?.Invoke();
            }

            else
            {
                StartHit?.Invoke();
            }

            _hitCoroutine = null;
            yield break;
        }

    }

    
#endregion

}

