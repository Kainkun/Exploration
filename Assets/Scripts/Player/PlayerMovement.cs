//define MOVEMENT_PRINT_DEBUG
//#define CLIMB_PRINT_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask environmentLayerMask;
    private Gravity gravity;

    private void Awake()
    {
        yawTransform = cameraSystem.GetChild(0);
        pitchTransform = yawTransform.GetChild(0);
        rollTransform = pitchTransform.GetChild(0);

        gravity = GetComponent<Gravity>();
        rb = GetComponent<Rigidbody>();
        prev = rb.position;

        currentJetpackFuel = maxJetpackFuel;

        jumpBuffer = new Buffer(StartJump, CanJump, jumpBufferTime);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        sprinting = Input.GetKey(KeyCode.LeftShift);

        HandleCamera();

        if (noclip)
        {
            HandleNoclip();
            return;
        }
        else
        {
            HandleClimb();
            HandleFuel();
            HandleBoost();
            if (!climbing && Input.GetKeyDown(KeyCode.Space))
                jumpBuffer.Queue();

            jumpBuffer.Tick(Time.deltaTime);
        }

        HandleUi();

        //GameObject.Find("GraphVelocityY").GetComponent<Graph>().SetValue(transform.position.y);

        #region Dev Tools

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            maxJetpackFuel = 0;
            currentJetpackFuel = maxJetpackFuel;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            maxJetpackFuel = 1;
            currentJetpackFuel = maxJetpackFuel;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            maxJetpackFuel = 2;
            currentJetpackFuel = maxJetpackFuel;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            maxJetpackFuel = 3;
            currentJetpackFuel = maxJetpackFuel;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            maxJetpackFuel = 4;
            currentJetpackFuel = maxJetpackFuel;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            maxJetpackFuel = 5;
            currentJetpackFuel = maxJetpackFuel;
        }

        #endregion
    }

    private void FixedUpdate()
    {
        if (!climbing && !noclip)
        {
            HandleGlide();
            HandleJumpMovement();
            HandleMovement();
        }
    }


    #region Noclip

    private bool noclip;

    public void ToggleNoclip()
    {
        if (!noclip)
            EnableNoclip();
        else
            DisableNoclip();
    }

    public void EnableNoclip()
    {
        rb.isKinematic = true;
        noclip = true;
    }

    public void DisableNoclip()
    {
        rb.isKinematic = false;
        noclip = false;
    }

    void HandleNoclip()
    {
        float speed = 5;
        float sprint = 20;
        transform.position += ((PlayerManager.playerCamera.transform.forward * movementInput.y) +
                               (PlayerManager.playerCamera.transform.right * movementInput.x)) *
                              ((sprinting ? sprint : speed) * Time.deltaTime);
        if (Input.GetKey(KeyCode.Space))
            transform.position += (PlayerManager.playerCamera.transform.up) *
                                  ((sprinting ? sprint : speed) * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftControl))
            transform.position += (-PlayerManager.playerCamera.transform.up) *
                                  ((sprinting ? sprint : speed) * Time.deltaTime);
    }

    #endregion


    #region Movement Variables

    [Header("Movement")] public float walkForwardSpeed = 3.5f;
    public float walkSidewaysSpeed = 3f;
    public float walkBackwardsSpeed = 1.8f;
    public float sprintMultiplier = 1.7f;
    public float acceleration;

    private Rigidbody rb;
    private bool sprinting;
    private Vector2 movementInput;

    Vector2 targetMovementLocalVelocity;
    private Vector2 moveTowardsTargetMovementLocalVelocity;

    private Vector3 prev;
    private Vector3 globalVelocity;
    private Vector3 localVelocity;

    #endregion


    void HandleMovement()
    {
        //Input to target velocity
        targetMovementLocalVelocity.y = movementInput.y * (movementInput.y > 0 ? walkForwardSpeed : walkBackwardsSpeed);
        targetMovementLocalVelocity.x = movementInput.x * walkSidewaysSpeed;
        if (sprinting)
            targetMovementLocalVelocity *= sprintMultiplier;

        //Move(targetMovementVelocity);
        Move(targetMovementLocalVelocity);

        //Calculate velocity
        globalVelocity = (rb.position - prev) / Time.fixedDeltaTime;
        localVelocity = transform.InverseTransformVector(globalVelocity);
        prev = rb.position;
    }

    void Move(Vector2 targetMovementVelocity)
    {
        //Acceleration
        moveTowardsTargetMovementLocalVelocity =
            Vector2.MoveTowards(moveTowardsTargetMovementLocalVelocity, targetMovementVelocity,
                Time.deltaTime * acceleration);

        //TODO: Turning midair is weird. no deceleration

        //Stick to floor
        Vector3 targetVelocityGlobal = new Vector3(
            moveTowardsTargetMovementLocalVelocity.x,
            0,
            moveTowardsTargetMovementLocalVelocity.y);
        targetVelocityGlobal = transform.TransformVector(targetVelocityGlobal);

        if (grounded && targetVelocityGlobal.sqrMagnitude > 0 && Physics.Raycast(
                transform.position + (Vector3.up * 0.1f),
                Vector3.down,
                out RaycastHit hit,
                2,
                environmentLayerMask,
                QueryTriggerInteraction.Ignore))
        {
            Vector3 tangent1 = Vector3.Cross(targetVelocityGlobal, hit.normal);
            Vector3 tangent2 = Vector3.Cross(hit.normal, tangent1);

#if MOVEMENT_PRINT_DEBUG
            Debug.DrawRay(transform.position, tangent1, Color.red);
            Debug.DrawRay(transform.position, tangent2, Color.green);
#endif
            targetVelocityGlobal = tangent2;
        }

        //Change position
        rb.MovePosition(rb.position + (targetVelocityGlobal * Time.deltaTime));
    }


    #region Jump Variables

    [Header("Jump")] public float jumpVelocity;
    public float maxJumpHoldTime = 0.4f;

    private float jumpHoldTime;
    private bool grounded;
    private bool linearJump;
    private float jumpCooldown = 0.2f;
    private float timeSinceJump;

    private Buffer jumpBuffer;
    public float jumpBufferTime;
    private float timeSinceLeaveGround;
    public float coyoteTime;

    #endregion

    void HandleJumpMovement()
    {
        //Jump cooldown
        timeSinceJump += Time.deltaTime;

        //Ground check
        bool hit = Physics.SphereCast(transform.position + (Vector3.up * 0.2f),
            0.199f,
            Vector3.down,
            out RaycastHit _,
            0.01f,
            environmentLayerMask,
            QueryTriggerInteraction.Ignore);

        //Hit ground
        if (!grounded && hit && timeSinceJump > jumpCooldown)
        {
            grounded = true;
            //print(timeSinceJump);
        }

        //TODO: HANDLE HEAD BUMP

        //Leave ground (Excludes Jumping)
        if (grounded && !hit)
        {
            grounded = false;
            timeSinceLeaveGround = 0;
        }

        //Jump hold
        if (linearJump && Input.GetKey(KeyCode.Space))
        {
            Vector3 velocity = rb.velocity;
            velocity.y = jumpVelocity;
            rb.velocity = velocity;
            jumpHoldTime += Time.deltaTime;
        }

        //Jump peak
        if (linearJump && (jumpHoldTime >= maxJumpHoldTime || !Input.GetKey(KeyCode.Space)))
        {
            jumpHoldTime = 0;
            linearJump = false;
            //print(timeSinceJump);
        }

        timeSinceLeaveGround += Time.deltaTime;
    }

    bool CanJump()
    {
        return !climbing &&
               timeSinceJump > jumpCooldown &&
               (grounded || timeSinceLeaveGround <= coyoteTime);
    }

    void StartJump()
    {
        timeSinceJump = 0;
        grounded = false;
        linearJump = true;

        // Vector3 velocity = rb.velocity;
        // velocity.y = jumpVelocity;
        // rb.velocity = velocity;
    }

    #region Camera Variables

    [Header("Camera")] public float mouseSensitivity;
    public float mouseSmoothing;
    private Vector2 mouseInput;

    [Range(0f, 90f)] public float cameraLookUpLimit = 90f;
    [Range(0f, 90f)] public float cameraLookDownLimit = 90f;

    public Transform cameraSystem;

    private Transform yawTransform;
    private Transform pitchTransform;
    private Transform rollTransform;
    private float currentPitch;

    #endregion

    void HandleCamera()
    {
        BodyYaw(mouseInput.x * mouseSensitivity * Time.deltaTime);

        CameraRotate(
            0,
            mouseInput.y * mouseSensitivity * Time.deltaTime,
            0);
    }

    public void CameraRotate(float yaw, float pitch, float roll)
    {
        currentPitch += pitch;
        currentPitch = Mathf.Clamp(currentPitch, -cameraLookDownLimit, cameraLookUpLimit);
        pitchTransform.localEulerAngles = new Vector3(-currentPitch, 0, 0);

        yawTransform.Rotate(Vector3.up, yaw);

        rollTransform.localEulerAngles += new Vector3(0, 0, -roll);
    }

    public void HeadRotate()
    {
    }

    public void BodyYaw(float yaw)
    {
        transform.Rotate(Vector3.up, yaw);
    }


    #region Climb Variables

    [Header("Climbing")] public float maxClimbableForwardDistance = 1f;
    public float maxClimbReachAboveHead = 1.5f;
    public float minClimbableHeight = 0.5f;
    public float climbSpeed;
    public float maxClimbTimeBeforeSnapping;

    private bool climbing;
    private bool jumpButtonReleasedAfterClimb;
    private float timeClimbing;
    private Vector3 climbDestination;

    private float playerRadius = 0.25f;
    private float playerHeight = 1.8f;

    private Vector3 climbRaycastForwardStartTop;
    private Vector3 climbRaycastForwardStartBottom;
    private RaycastHit climbRaycastForwardHit;
    private float climbRaycastForwardRadius;

    private Vector3 climbRaycastUpStart;
    private RaycastHit climbRaycastUpHit;
    private Vector3 climbUpSafe;

    private Vector3 climbRaycastTopStart;
    private RaycastHit climbRaycastTopHit;
    private Vector3 climbTopSafe;
    private float climbRaycastTopDistance;

    private Vector3 climbRaycastDownStart;
    private RaycastHit climbRaycastDownHit;
    private Vector3 climbDownSafe;
    private float climbRaycastDownDistance;

    private Vector3 climbFinalCapsuleBottom;
    private Vector3 climbFinalCapsuleTop;
    private float capsuleDistance;

    #endregion

    void HandleClimb()
    {
        //TODO: climbing slopes buggy?

        if (!climbing &&
            (Input.GetKeyDown(KeyCode.Space) || (!grounded && Input.GetKey(KeyCode.Space))))
        {
            TryClimb();
        }

        //Climb animation
        if (climbing)
        {
            if (Vector3.Distance(transform.position, this.climbDestination) > 0.01f &&
                timeClimbing < maxClimbTimeBeforeSnapping)
            {
                Vector3 targetPosition = Vector3.MoveTowards(transform.position, climbDestination,
                    Time.deltaTime * climbSpeed);

                Vector3 positionDelta = targetPosition - transform.position;
                positionDelta.y = 0;

                if (Physics.SphereCast(
                        PlayerManager.playerCamera.transform.position,
                        0.075f,
                        positionDelta,
                        out RaycastHit h,
                        positionDelta.magnitude,
                        environmentLayerMask,
                        QueryTriggerInteraction.Ignore))
                {
                    targetPosition.x = transform.position.x;
                    targetPosition.z = transform.position.z;
                }

                transform.position = targetPosition;
            }
            else
            {
                transform.position = climbDestination;
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                climbing = false;
                timeClimbing = 0;
            }

            timeClimbing += Time.deltaTime;
        }
    }

    void TryClimb()
    {
        capsuleDistance = playerHeight - (playerRadius * 2);

        climbRaycastForwardStartBottom = transform.position + (Vector3.up * playerRadius);
        climbRaycastForwardStartTop = climbRaycastForwardStartBottom + (Vector3.up * capsuleDistance);
        climbRaycastForwardRadius = playerRadius - 0.05f;
        bool climbRaycastForwardIntersects = Physics.CapsuleCast(
            climbRaycastForwardStartTop,
            climbRaycastForwardStartBottom,
            climbRaycastForwardRadius,
            transform.forward,
            out climbRaycastForwardHit,
            maxClimbableForwardDistance,
            environmentLayerMask,
            QueryTriggerInteraction.Ignore);

        if (!climbRaycastForwardIntersects)
        {
#if CLIMB_PRINT_DEBUG
            print("nothing in front to climb");
#endif
            return;
        }

        climbRaycastUpStart = transform.position + (Vector3.up * (playerHeight - playerRadius));
        bool climbRaycastUpIntersects = Physics.SphereCast(
            climbRaycastUpStart,
            playerRadius,
            Vector3.up,
            out climbRaycastUpHit,
            maxClimbReachAboveHead,
            environmentLayerMask,
            QueryTriggerInteraction.Ignore);

        if (climbRaycastUpIntersects)
            climbUpSafe = climbRaycastUpStart + (Vector3.up * climbRaycastUpHit.distance);
        else
            climbUpSafe = climbRaycastUpStart + (Vector3.up * maxClimbReachAboveHead);

        climbRaycastTopDistance = climbRaycastForwardHit.distance + (playerRadius * 2);
        bool climbRaycastTopIntersects = Physics.SphereCast(
            climbUpSafe,
            playerRadius,
            transform.forward,
            out climbRaycastTopHit,
            climbRaycastTopDistance,
            environmentLayerMask,
            QueryTriggerInteraction.Ignore);

        if (climbRaycastTopIntersects)
            climbTopSafe = climbUpSafe + (transform.forward * climbRaycastTopHit.distance);
        else
            climbTopSafe = climbUpSafe + (transform.forward * (climbRaycastForwardHit.distance + (playerRadius * 2)));

        if (climbRaycastTopIntersects)
        {
#if CLIMB_PRINT_DEBUG
            print("no way to climb on top");
#endif
            return;
        }


        climbRaycastDownDistance = maxClimbReachAboveHead + playerHeight - playerRadius - minClimbableHeight;

        //Find best top surface position
        int hits = 0;
        float totalDistance = 0;
        float totalDepth = 0;
        int interations = 10;
        for (int i = 0; i < interations; i++)
        {
            float distance = playerRadius * ((float)i / (interations - 1));
            Color color = Color.grey;

            if (Physics.Raycast(
                    climbTopSafe + (-transform.forward * distance),
                    Vector3.down,
                    out climbRaycastDownHit,
                    climbRaycastDownDistance,
                    environmentLayerMask,
                    QueryTriggerInteraction.Ignore))
            {
                hits++;
                totalDistance += distance;
                totalDepth += climbRaycastDownHit.distance;
                color = Color.white;
            }
#if CLIMB_PRINT_DEBUG
            Debug.DrawRay(climbTopSafe + (-transform.forward * distance), Vector3.down * climbRaycastDownDistance,
                color, 1f);
#endif
        }

        if (hits > 0)
        {
            float averageDistance = totalDistance / hits;
            float averageDepth = totalDepth / hits;
            climbDownSafe = climbTopSafe + (-transform.forward * averageDistance) + (Vector3.down * averageDepth);
#if CLIMB_PRINT_DEBUG
            Debug.DrawRay(climbTopSafe + (-transform.forward * averageDistance), (Vector3.down * averageDepth),
                Color.magenta, 2);
#endif
        }
        else
        {
#if CLIMB_PRINT_DEBUG
            print("what ever is in front might be too short to climb?");
#endif
            return;
        }


        climbFinalCapsuleBottom = climbDownSafe + (Vector3.up * (playerRadius + 0.01f));
        climbFinalCapsuleTop = climbFinalCapsuleBottom + (Vector3.up * capsuleDistance);
        Collider[] climbCapsuleIntersects = Physics.OverlapCapsule(
            climbFinalCapsuleBottom,
            climbFinalCapsuleTop,
            playerRadius,
            environmentLayerMask,
            QueryTriggerInteraction.Ignore);

        if (climbCapsuleIntersects.Length > 0)
        {
#if CLIMB_PRINT_DEBUG
            print("Final climb destination is blocked by: " + climbCapsuleIntersects[0].name);
#endif
            return;
        }

        //Start climb
        climbDestination = climbFinalCapsuleBottom + (Vector3.down * playerRadius);
        rb.isKinematic = true;
        climbing = true;
    }


    #region Jetpack Variables

    [Header("Jetpack")] public float maxJetpackFuel;
    private float currentJetpackFuel;
    private float glideFuelCost = 1;
    public float boostFuelCost;
    public float FuelRechargeRate;
    public bool canGlide;
    public bool canBoost;

    private bool gliding;
    public float glideFallSpeed;
    public float upwardBoostForce;

    public Image imageLeft;
    public Image imageRight;
    public Image imageCenter;
    public Image imageDot1;
    public Image imageDot2;
    public Image imageDot3;
    public Image imageCenterMini;

    #endregion

    void HandleFuel()
    {
        if (grounded)
        {
            currentJetpackFuel += FuelRechargeRate * Time.deltaTime;
            currentJetpackFuel = Mathf.Clamp(currentJetpackFuel, 0, maxJetpackFuel);
        }
    }

    void HandleGlide()
    {
        if (!canGlide)
            return;

        if (currentJetpackFuel > 0 && rb.velocity.y <= glideFallSpeed &&
            (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl)))
        {
            gravity.enabled = false;
            gliding = true;
            Vector3 v = rb.velocity;
            v.y = glideFallSpeed;
            rb.velocity = v;

            currentJetpackFuel -= glideFuelCost * Time.deltaTime;
            currentJetpackFuel = Mathf.Clamp(currentJetpackFuel, 0, maxJetpackFuel);
        }
        else
        {
            gravity.enabled = true;
            gliding = false;
        }
    }

    void HandleBoost()
    {
        if (!canBoost)
            return;

        //TODO: coyote jump dosnt work well with fuel or boosts

        //if (currentJetpackFuel > 0 && !grounded && Input.GetKeyDown(KeyCode.Space))
        if (currentJetpackFuel >= boostFuelCost && !CanJump() && Input.GetKeyDown(KeyCode.Space))
        {
            // rb.AddForce(Vector3.up * upwardBoostForce, ForceMode.Impulse);
            Vector3 v = rb.velocity;
            v.y = upwardBoostForce;
            rb.velocity = v;
            currentJetpackFuel -= boostFuelCost;
            currentJetpackFuel = Mathf.Clamp(currentJetpackFuel, 0, maxJetpackFuel);
            // if (currentJetpackFuel % boostFuelCost == 0)
            //     currentJetpackFuel -= boostFuelCost;
            // else
            //     currentJetpackFuel -= currentJetpackFuel % boostFuelCost;
        }

        // if (!grounded && Input.GetKeyDown(KeyCode.LeftShift) && currentBoosts > 0)
        // {
        //     rb.AddForce(transform.forward * forwardBoostForce, ForceMode.Impulse);
        //     currentBoosts--;
        // }
    }

    void HandleUi()
    {
        //UI
        if (imageCenter)
        {
            imageLeft.fillAmount = currentJetpackFuel / maxJetpackFuel;
            imageRight.fillAmount = currentJetpackFuel / maxJetpackFuel;

            if (currentJetpackFuel == 0)
                imageCenter.fillAmount = 0;
            else if (currentJetpackFuel % boostFuelCost == 0)
                imageCenter.fillAmount = 1;
            else
                imageCenter.fillAmount = (currentJetpackFuel % boostFuelCost) / boostFuelCost;

            if (Mathf.CeilToInt(currentJetpackFuel / boostFuelCost) >= 1)
                imageDot1.enabled = true;
            else
                imageDot1.enabled = false;

            if (Mathf.CeilToInt(currentJetpackFuel / boostFuelCost) >= 2)
                imageDot2.enabled = true;
            else
                imageDot2.enabled = false;

            if (Mathf.CeilToInt(currentJetpackFuel / boostFuelCost) >= 3)
                imageDot3.enabled = true;
            else
                imageDot3.enabled = false;

            imageCenterMini.fillAmount = currentJetpackFuel / maxJetpackFuel;
        }
    }

    private void OnDrawGizmos()
    {
#if CLIMB_PRINT_DEBUG
        Gizmos.color = Color.HSVToRGB(0.00f, 0.6f, 0.9f);
        Vector3 ForwardCapsuleMaxTop = climbRaycastForwardStartTop + (transform.forward * maxClimbableForwardDistance);
        Vector3 ForwardCapsuleMaxBottom =
            climbRaycastForwardStartBottom + (transform.forward * maxClimbableForwardDistance);
        Gizmos.DrawWireSphere(ForwardCapsuleMaxTop, playerRadius);
        Gizmos.DrawWireSphere(ForwardCapsuleMaxBottom, playerRadius);
        Gizmos.DrawLine(ForwardCapsuleMaxTop, ForwardCapsuleMaxBottom);
        Gizmos.color = Color.HSVToRGB(0.00f, 0.8f, 0.9f);
        Gizmos.DrawSphere(climbRaycastForwardStartTop, playerRadius);
        Gizmos.DrawSphere(climbRaycastForwardStartBottom, playerRadius);
        Gizmos.DrawLine(climbRaycastForwardStartTop, climbRaycastForwardStartBottom);
        Vector3 ForwardCapsuleEndTop =
            climbRaycastForwardStartTop + (transform.forward * climbRaycastForwardHit.distance);
        Vector3 ForwardCapsuleEndBottom =
            climbRaycastForwardStartBottom + (transform.forward * climbRaycastForwardHit.distance);
        Gizmos.DrawSphere(ForwardCapsuleEndTop, playerRadius);
        Gizmos.DrawSphere(ForwardCapsuleEndBottom, playerRadius);
        Gizmos.DrawLine(ForwardCapsuleEndTop, ForwardCapsuleEndBottom);
        Gizmos.color = Color.HSVToRGB(0.08f, 0.5f, 0.9f);
        Gizmos.DrawWireSphere(climbRaycastUpStart + (Vector3.up * maxClimbReachAboveHead), playerRadius + 0.01f);
        Gizmos.color = Color.HSVToRGB(0.08f, 0.8f, 0.9f);
        Gizmos.DrawSphere(climbRaycastUpStart, playerRadius);
        Gizmos.color = Color.HSVToRGB(0.13f, 0.8f, 0.9f);
        Gizmos.DrawSphere(climbUpSafe, playerRadius);
        Gizmos.color = Color.HSVToRGB(0.17f, 0.4f, 0.9f);
        Gizmos.DrawWireSphere(climbUpSafe + (transform.forward * climbRaycastTopDistance), playerRadius + 0.01f);
        Gizmos.color = Color.HSVToRGB(0.17f, 0.8f, 0.9f);
        Gizmos.DrawSphere(climbTopSafe, playerRadius);
        Gizmos.color = Color.HSVToRGB(0.30f, 0.8f, 0.4f);
        Gizmos.DrawRay(climbTopSafe, Vector3.down * climbRaycastDownDistance);
        Gizmos.color = Color.HSVToRGB(0.30f, 0.8f, 0.9f);
        Gizmos.DrawRay(climbTopSafe, Vector3.down * climbRaycastDownHit.distance);
        Gizmos.color = Color.HSVToRGB(0.60f, 0.8f, 0.9f);
        Gizmos.DrawWireSphere(climbFinalCapsuleTop, playerRadius);
        Gizmos.DrawWireSphere(climbFinalCapsuleBottom, playerRadius);
        Gizmos.DrawLine(climbFinalCapsuleTop, climbFinalCapsuleBottom);
#endif
    }
}