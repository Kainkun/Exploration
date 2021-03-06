using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DaveFPS
{
    public class PlayerMovementAdvanced : MonoBehaviour
    {
        [Header("Movement")]
        private float moveSpeed;

        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;
        public float walkSpeed;
        public float sprintSpeed;
        public float slideSpeed;
        public float wallrunSpeed;

        public float speedIncreaseMultiplier;
        public float slopeIncreaseMultiplier;

        public float groundDrag;

        [Header("Jumping")]
        public bool useGravity = true;
        public float gravity = 30;
        private float currentGravity;
        public float jumpForce;

        public float jumpCooldown;
        public float airMultiplier;
        bool readyToJump;

        [Header("Crouching")]
        public float crouchSpeed;

        public float crouchYScale;
        private float startYScale;

        [Header("Keybinds")]
        public KeyCode jumpKey = KeyCode.Space;

        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Ground Check")]
        public float playerHeight;

        public LayerMask whatIsGround;
        bool grounded;

        [Header("Slope Handling")]
        public float maxSlopeAngle;

        private RaycastHit slopeHit;
        private bool exitingSlope;


        public Transform orientation;

        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        Rigidbody rb;

        public MovementState state;
        
        [Header("Boost")]
        public float upBoost = 100;
        public float forwardBoost = 100;
        public float forwardUpBoost = 100;
        private bool doubleJump = true;
        
        [Header("Teleporter")]
        public Rigidbody teleporter;
        public enum MovementState
        {
            walking,
            sprinting,
            wallrunning,
            crouching,
            sliding,
            air
        }

        public bool sliding;
        public bool crouching;
        public bool wallrunning;

        //public TextMeshProUGUI text_speed;
        //public TextMeshProUGUI text_mode;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            readyToJump = true;

            startYScale = transform.localScale.y;

            currentGravity = gravity;
        }

        private void Update()
        {
            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            MyInput();
            SpeedControl();
            StateHandler();
            TextStuff();

            // handle drag
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;
            
            //boost
            if (grounded)
                doubleJump = true;
            
            if (Input.GetKeyDown(KeyCode.Space) && state == MovementState.air)
            {
                Vector3 v = Camera.main.transform.forward;
                v.y = 0;
                v = v.normalized * 0.25f;
                Debug.DrawRay(transform.position - v, -v, Color.red, 1);
                if(Physics.Raycast(transform.position - v, -v, 1))
                {
                    Vector3 f = Camera.main.transform.forward;
                    f.y = 0;
                    f = f.normalized * forwardBoost;
                    f.y = forwardBoost;
                    GetComponent<Rigidbody>().velocity = f;
                    //GetComponent<Rigidbody>().AddForce(f, ForceMode.Impulse);
                }
                else
                {
                    if(!doubleJump)
                        return;
                    
                    Vector3 vel = GetComponent<Rigidbody>().velocity;
                    vel.y = upBoost;
                    GetComponent<Rigidbody>().velocity = vel;
                    //GetComponent<Rigidbody>().AddForce(Vector3.up * upBoost, ForceMode.Impulse);
                    doubleJump = false;
                }
            }

            //teleport
            if (teleporter && Input.GetMouseButtonDown(1))
            {
                transform.position = teleporter.position + Vector3.up;
            }
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // when to jump
            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }

            // start crouch
            if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

                crouching = true;
            }

            // stop crouch
            if (Input.GetKeyUp(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

                crouching = false;
            }
        }

        private void StateHandler()
        {
            // Mode - Wallrunning
            if (wallrunning)
            {
                state = MovementState.wallrunning;
                desiredMoveSpeed = wallrunSpeed;
            }

            // Mode - Sliding
            else if (sliding)
            {
                state = MovementState.sliding;

                // increase speed by one every second
                if (OnSlope() && rb.velocity.y < 0.1f)
                    desiredMoveSpeed = slideSpeed;

                else
                    desiredMoveSpeed = sprintSpeed;
            }

            // Mode - Crouching
            else if (crouching)
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }

            // Mode - Sprinting
            else if (grounded && Input.GetKey(sprintKey))
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }

            // Mode - Walking
            else if (grounded)
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }

            // Mode - Air
            else
            {
                state = MovementState.air;
            }

            // check if desired move speed has changed drastically
            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;
        }

        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            // smoothly lerp movementSpeed to desired value
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

                if (OnSlope())
                {
                    float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                    time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
                }
                else
                    time += Time.deltaTime * speedIncreaseMultiplier;

                yield return null;
            }

            moveSpeed = desiredMoveSpeed;
        }

        private void MovePlayer()
        {
            // calculate movement direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // on slope
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

            // on ground
            else if (grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            // in air
            else if (!grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

            // turn gravity off while on slope
            if (!wallrunning) SetGravity(!OnSlope());

            if(useGravity)
                rb.AddForce(Vector3.down * currentGravity, ForceMode.Acceleration);

        }

        private void SpeedControl()
        {
            // limiting speed on slope
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                    rb.velocity = rb.velocity.normalized * moveSpeed;
            }

            // // limiting speed on ground or in air
            // else
            // {
            //     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //
            //     // limit velocity if needed
            //     if (flatVel.magnitude > moveSpeed)
            //     {
            //         Vector3 limitedVel = flatVel.normalized * moveSpeed;
            //         rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            //     }
            // }
        }

        private void Jump()
        {
            exitingSlope = true;

            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyToJump = true;

            exitingSlope = false;
        }
        
        public void SetGravity(bool b)
        {
            useGravity = b;
        }

        public void SetGravity(float newGravity)
        {
            currentGravity = newGravity;
        }
        
        public void ResetGravity()
        {
            currentGravity = gravity;
        }

        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }

        private void TextStuff()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // if (OnSlope())
            //     text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1));
            //
            // else
            //     text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1));
            //
            // text_mode.SetText(state.ToString());
        }

        public static float Round(float value, int digits)
        {
            float mult = Mathf.Pow(10.0f, (float) digits);
            return Mathf.Round(value * mult) / mult;
        }
    }
}