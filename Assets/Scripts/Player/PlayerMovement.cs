using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _crouchSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _turnSmoothVel;
    [SerializeField] private LayerMask groundMask;
    private bool _isCrouching = false;
    private bool _isWalking;
    private bool _hasWalked;
    private bool _isGrounded;
    private float _walkingSpeed;
    private Rigidbody rb;
    private PhotonView _view;
    private CapsuleCollider _collider;

    [SerializeField] private UnityEvent WalkingEnter;
    [SerializeField] private UnityEvent WalkingExit;
    [SerializeField] private UnityEvent JumpEnter;
    [SerializeField] private UnityEvent JumpExit;
    [SerializeField] public UIManager _UImanager;
    [SerializeField] public ChatManager _Chatmanager;

    public void SetCamera(GameObject cam)
    {
        _cam = cam.transform;
    }

    public bool GetCrouchStatus()
    {
        return _isCrouching;
    }

    public bool GetWalkingStatus()
    {
        return _isWalking;
    }

    public bool IsInAir() 
    {
        return !_isGrounded;
    }

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            _walkingSpeed = _speed;
            rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_isGrounded);
        if (_view.IsMine)
        {
            if (_UImanager.GetUISwitch() == false || _Chatmanager.GetIsTyping() == false)
            {
                PlayerControl();
                UpdateGroundedStatus();
            }
        }
    }

    private void PlayerControl()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool playerCrouch = Input.GetButtonDown("Fire2");
        bool playerJump = Input.GetButtonDown("Jump");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        _isWalking = direction.magnitude > 0.01f || direction.magnitude < -0.01f;

        if (_isWalking)
        {
            if (!_hasWalked)
            {
                WalkingEnter.Invoke();
                _hasWalked = true;
            }
        }
        else
        {
            if (_hasWalked)
            {
                WalkingExit.Invoke();
                _hasWalked = false;
            }
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;
            float angle = (Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVel, _turnSmoothTime));
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.velocity = new Vector3(moveDir.normalized.x * GetPlayerSpeed(), rb.velocity.y, moveDir.normalized.z * GetPlayerSpeed());
        }

        if (playerCrouch)
        {
            Debug.Log("button pressed");
            _isCrouching = !_isCrouching;
            Debug.Log(_speed);
        }

        if (playerJump && _isGrounded)
        {
            Debug.Log("Has Jumped");
            JumpEnter.Invoke();
            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        if (!_isGrounded)
        {
            JumpExit.Invoke(); 
        }
    }

    public float GetPlayerSpeed()
    {
        if (!_isCrouching)
        {
            return _walkingSpeed;
        }
        else
        {
            return _crouchSpeed;
        }
    }

    private void UpdateGroundedStatus()
    {
        float raycastDistance = _collider.bounds.extents.y + 0.1f; // Adjust the raycast distance as needed

        // Raycast from slightly above the bottom of the capsule collider downward
        bool grounded = Physics.Raycast(_collider.bounds.center, Vector3.down, raycastDistance, groundMask, QueryTriggerInteraction.Ignore);

        _isGrounded = grounded;
    }
}
