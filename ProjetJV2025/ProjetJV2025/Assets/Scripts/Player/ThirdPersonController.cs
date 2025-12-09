 using System;
 using UnityEngine;
 using UnityEngine.Animations.Rigging;
 using UnityEngine.UI;
 using Random = UnityEngine.Random;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{

    public class ThirdPersonController : MonoBehaviour, IDamageable
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Réglages")]
        public Camera cam;                     // Caméra isométrique
        public LayerMask groundMask;           // Le layer du sol (à définir dans l’inspecteur)
        public float rotationSpeed = 10f;      // Vitesse de rotation du joueur


        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDInputX;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDInputY;

        private Animator _animator;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        public int health;
        public Slider healthSlider;

        public int maxStamina = 200;
        public int stamina;
        public Slider staminaSlider;
        private float _staminaRegenCooldown;
        
        private bool _hasAnimator;
        
        private Vector2 input;
        
        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            health = 100;
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _input = GetComponent<StarterAssetsInputs>();
            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
        
            //JumpAndGravity();
            GroundedCheck();
            Move();

            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        private void LateUpdate()
        {
            RotateTowardsMouse();
        }

        private void AssignAnimationIDs()
        {
            _animIDInputX = Animator.StringToHash("InputX");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDInputY = Animator.StringToHash("InputY");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void Move()
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
            {
                if (stamina > 0)
                {
                    input.y *= 2;
                    input.x *= 2;

                    stamina -= 1;
                    _staminaRegenCooldown = 1f;
                }
            }
            else if (_staminaRegenCooldown <= 0f)
            {
                stamina += 1;
            }
            else
            {
                _staminaRegenCooldown -= Time.deltaTime;
            }

            stamina = Math.Clamp(stamina, 0, maxStamina);
            staminaSlider.value = stamina / (float)maxStamina;
            
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDInputX, input.x);
                _animator.SetFloat(_animIDInputY, input.y);
            }
            
        }
        void RotateTowardsMouse()
        {
            Ray ray =  cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = transform.position.y;

                Vector3 direction = (targetPoint - transform.position).normalized;

                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        targetRotation,
                        Time.deltaTime * rotationSpeed
                    );
                }
            }
        }
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    //AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                //AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            healthSlider.value = Mathf.Clamp(health / 100f, 0f, 1f);
            Debug.Log($"Player have {health}hp (Slide value: {healthSlider.value})");
        }
    }
}