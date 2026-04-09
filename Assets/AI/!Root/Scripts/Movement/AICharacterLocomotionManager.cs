using System.Collections;
using UnityEngine;

namespace WEV.WhiteRoom
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        AICharacterManager aiCharacter;

        [Header("GROUND CHECK & JUMPING")]
        [SerializeField] protected float gravityForce = -40; // Gravity force for the jumping
        [SerializeField] LayerMask groundLayer; // Layer mask for the ground
        [SerializeField] float groundCheckSphereRadius = 0.3f; // Radius of the sphere for the ground check
        [SerializeField] protected Vector3 yVelocity; // The force at which our character is pulled up or down (jumping or falling)
        [SerializeField] protected float groundedYVelocity = -20; // The force at which our character is sticking to the ground while they are grounded
        [SerializeField] private protected float fallStartYVelocity = -5; // The force at which our character begins to fall when they are ungrounded (rises as they fall longer)
        protected bool fallingVelocityHasBeenSet = false; // Checks if the falling velocity has been set or not
        protected float inAirTimer = 0; // Timer to depect the character in the air

        [Header("SLOPE SLIDING")]
        [SerializeField] private float slopeSlideStartPositionYOffset = 1;
        [SerializeField] private float slopeSlideSphereCastMaxDistance = 2;
        [SerializeField] private float slopeSlideSpeed = -11;
        [SerializeField] private float slopeSlideSpeedMultiplier = 3;
        [SerializeField] private float slipperySurfaceMaxAngle = 15;
        [SerializeField] private float characterCollisionCheckSphereMultiplier = 1.5f;
        [SerializeField] private float characterSlideOffHeadCollisionMaxDistanceCheck = 5f;
        private Vector3 slopeSlideVelocity;
        //private bool isSliding;
        private bool isSlidingOffCharacter = false;
        private Coroutine slideOffCharacterCoroutine;
        private bool slideUntilGrounded = false;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        protected override void Update()
        {
            UseGroundCheck();
            SetGroundVelocity();
            UseSlopeSlideCheck();

            if (isGrounded)
            {
                // BELOW CODE: If character is not attempting to jump or move upward 
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // BELOW CODE: If character is not jumping, and character falling velocity has not been set
                if (!character.characterLocomotionManager.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("inAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            // BELOW CODE: There should always be some force applied to the y velocity
            character.controller.Move(yVelocity * Time.deltaTime);
        }

        protected void UseGroundCheck()
        {
            if (isGrounded)
            {
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

                if (!isGrounded)
                    OnIsNotGrounded();
            }
            else
            {
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

                if (yVelocity.y > 0)
                {
                    isGrounded = false;
                    return;
                }

                if (isGrounded)
                    OnIsGrounded();
            }
        }

        // BELOW CODE: Draws character check sphere in scene view
        protected void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }

        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if(aiCharacter.characterLocomotionManager.isMoving)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

        // SLOPES & SLIDING
        private void UseSlopeSlideCheck()
        {
            if (slopeSlideVelocity == Vector3.zero)
                isSliding = false;

            if (!isGrounded && slideUntilGrounded)
            {
                SetSlopeSlideVelocity(CoreUtilityManager.instance.GetEnvironmentLayers());
                return;
            }

            if (!isGrounded)
                return;

            SetSlopeSlideVelocity(CoreUtilityManager.instance.GetSlipperyEnvironmentLayers());
        }

        private void SetSlopeSlideVelocity(LayerMask layers)
        {
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + slopeSlideStartPositionYOffset, transform.position.z);

            if (Physics.SphereCast(startPosition, groundCheckSphereRadius, Vector3.down, out RaycastHit hitinfo,
                    slopeSlideSphereCastMaxDistance, layers))
            {
                float angle = Vector3.Angle(hitinfo.normal, Vector3.up);
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal);

                if (angle >= slipperySurfaceMaxAngle)
                {
                    slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal);
                    return;
                }
            }
            else
            {
                slopeSlideVelocity = Vector3.zero;
            }

            if (isSliding)
            {
                slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * slopeSlideSpeedMultiplier;

                if (slopeSlideVelocity.magnitude > 1)
                    return;
            }

            slopeSlideVelocity = Vector3.zero;
        }

        private void SetGroundVelocity()
        {
            if (slopeSlideVelocity != Vector3.zero)
            {
                if (character.characterLocomotionManager.isJumping && yVelocity.y > 0)
                {
                    isSliding = false;
                }
                else
                {
                    isSliding = true;
                }
            }

            if (isSliding)
            {
                yVelocity.y += CoreUtilityManager.instance.slopeSlideForce * Time.deltaTime;
                Vector3 slideVelocity = slopeSlideVelocity;

                if (character.controller.enabled)
                    character.controller.Move(slideVelocity * Time.deltaTime);
            }

            if (isGrounded)
            {
                if (yVelocity.y <= 0 && !isSliding)
                    yVelocity.y = groundedYVelocity;
            }
            else if (!isGrounded && !isSlidingOffCharacter)
            {
                Collider[] characterColliders =
                    Physics.OverlapSphere(transform.position,
                        groundCheckSphereRadius * characterCollisionCheckSphereMultiplier,
                        CoreUtilityManager.instance.GetCharacterLayers());

                for (int i = 0; i < characterColliders.Length; i++)
                {
                    if (characterColliders[i].gameObject.transform.root == character.gameObject.transform.root)
                        continue;

                    CharacterController controller = characterColliders[i].GetComponent<CharacterController>();

                    if (controller == null)
                        continue;

                    if ((controller.collisionFlags & CollisionFlags.CollidedBelow) != 0)
                    {
                        isSlidingOffCharacter = true;
                        SlideOffCharacter();
                    }
                }
            }

            if (!character.controller.enabled)
                return;
        }
        protected virtual void SlideOffCharacter()
        {
            if (slideOffCharacterCoroutine != null)
                StopCoroutine(slideOffCharacterCoroutine);

            slideOffCharacterCoroutine = StartCoroutine(SlideOffCharacterCoroutine());
        }

        protected virtual IEnumerator SlideOffCharacterCoroutine()
        {
            while (!isGrounded)
            {
                if (Physics.SphereCast(character.transform.position,
                        groundCheckSphereRadius, Vector3.down, out RaycastHit hitinfo, characterSlideOffHeadCollisionMaxDistanceCheck,
                        CoreUtilityManager.instance.GetCharacterLayers()))
                {
                    Vector3 characterSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, yVelocity.y, 0), hitinfo.normal);
                    yVelocity.y += CoreUtilityManager.instance.slopeSlideForce * Time.deltaTime;
                    Vector3 slideVelocity = characterSlideVelocity;

                    if (character.controller.enabled)
                        character.controller.Move(slideVelocity * Time.deltaTime);

                    yield return null;
                }

                yield return null;
            }

            isSlidingOffCharacter = false;
            yield return null;
        }

        protected virtual void OnIsGrounded()
        {
            slideUntilGrounded = false;
        }

        protected virtual void OnIsNotGrounded()
        {

        }
    }
}
