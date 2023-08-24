using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float walkSoundDuration = 0.5f;
    float nextSoundTime;
    bool isRight;
    public AudioClipsSO leftClips;
    public AudioClipsSO rightClips;
    public AudioClipsSO fallClips;
    AudioSource audioSource;

    [Header("Movement")]
    [SerializeField] private MovementStats movementStats;

    private float speed;
    public float Speed => speed;

    public Action<Vector2> OnMove;
    public Action OnJump;

    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundDistance = 0.3f;
    private bool isGrounded = true;

    Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        speed = movementStats.moveSpeed;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        PlayerHealth.Instance.OnPlayerDied += DisableMovement;
        audioSource.volume = AudioManager.Instance.SoundVolume;
    }

    void Update()
    {
        GroundCheck();

        if (isGrounded)
        {
            speed = movementStats.moveSpeed;
            if (velocity.y < 0)
                velocity.y = -2f;
        }
        else
            speed = movementStats.airControl * movementStats.moveSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Set the speed based on how far the joystick is pushed
        float axisMultiplier = move.magnitude > 1 ? 1 : move.magnitude;

        // Normalize the move vector and multiply by the speed
        move.Normalize();
        move *= axisMultiplier;

        controller.Move(move * speed * Time.deltaTime);
        OnMove?.Invoke(move);
        if (isGrounded)
        {
            if (move.magnitude > 0 && Time.time >= nextSoundTime)
            {
                nextSoundTime = Time.time + walkSoundDuration;
                isRight = !isRight;
                if (isRight)
                {
                    audioSource.PlayOneShot(rightClips.RandomAudioClip);
                }
                else
                {
                    audioSource.PlayOneShot(leftClips.RandomAudioClip);
                }
            }

            if (Input.GetAxis("Jump") > 0)
            {
                velocity.y = Mathf.Sqrt(movementStats.jumpHeight * -2f * movementStats.gravity);
                OnJump?.Invoke();
            }
        }


        velocity.y += movementStats.gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(groundCheckTransform.position, groundDistance, whatIsGround))
        {
            if (!isGrounded)
            {
                audioSource.PlayOneShot(fallClips.RandomAudioClip);
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void DisableMovement()
    {
        enabled = false;
    }

}