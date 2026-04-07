using System;
using UnityEngine;
using UnityEngine.Animations;

namespace WEV.WhiteRoom
{
    public class CharacterFootStepSFXCreator : MonoBehaviour
    {
        private PlayerManager player;

        [Header("GROUND CHECK")]
        [SerializeField] private float distanceToGround = 0.3f;
        [SerializeField] private LayerMask environmentLayers;

        [Header("STEP TIMING")]
        public float walkStepInterval = 0.5f;
        public float sprintStepInterval = 0.32f;
        public float crouchStepInterval = 0.7f;

        private float stepTimer;
        private AudioSource audioSource;
        private GameObject steppedOnObject;

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            HandleFootsteps();
        }

        private void HandleFootsteps()
        {
            if (player == null)
                return;

            if (!player.playerLocomotionManager.isGrounded)
                return;

            if (player.playerInputManager.moveAmount <= 0.1f)
                return;

            float interval = walkStepInterval;

            if (player.playerLocomotionManager.isSprinting)
                interval = sprintStepInterval;
            else if (player.playerLocomotionManager.isCrouching)
                interval = crouchStepInterval;

            stepTimer += Time.deltaTime;

            if (stepTimer >= interval)
            {
                stepTimer = 0;
                DetectGroundAndPlay();
            }
        }

        private void DetectGroundAndPlay()
        {
            RaycastHit hit;

            if (Physics.Raycast(
                    transform.position,
                    Vector3.down,
                    out hit,
                    distanceToGround,
                    environmentLayers))
            {
                steppedOnObject = hit.transform.gameObject;
                PlayFootstepSFX();
            }
        }

        private void PlayFootstepSFX()
        {
            if (steppedOnObject == null)
                return;

            AudioClip clip =
                WorldSoundFXManager.instance
                .ChooseRandomFootStepSoundBasedOnGround(steppedOnObject, player);

            if (clip != null)
                audioSource.PlayOneShot(clip);
        }
    }
}
