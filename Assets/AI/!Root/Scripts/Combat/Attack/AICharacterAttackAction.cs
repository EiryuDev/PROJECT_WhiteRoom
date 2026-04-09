using UnityEngine;

namespace WEV.WhiteRoom
{
    [CreateAssetMenu(menuName = "White Room/A.I/Actions/Attack Actions")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("A.I CHARACTER DATA")]
        [Tooltip("Every attack will have an attack score, The higher the score, The higher the score is, the more chance to play that attack.For example: If A.I Character have 5 attacks, but bite attack is the highest attack score, then it is more likely, that attack will play more and play repeatedly, whatever the case.")]
        public int attackWeight = 50; // Weight for the attack, the more weight the more chance to perform
        [Tooltip("Type of the attack to perform.")]
        [SerializeField] AttackType attackType; 
        [Tooltip("Which attack animation to perform.")]
        [SerializeField] private string attackAnimation; 
        // Attack can be repeated
        [Tooltip("How long A.I Character should wait after attacking to recover.")]
        public float actionRecoveryTimer = 1.5f;
        [Tooltip("The combo action for this attack action")]
        public AICharacterAttackAction comboAction; 

        [Header("A.I CHARACTER ATTACK SETTINGS")]
        [Tooltip("Minimum angle for the particular attack.")]
        public float minimumAttackAngle = -35; 
        [Tooltip("Maximum angle for the particular attack.")]
        public float maximumAttackAngle = 35; 

        [Header("A.I CHARACTER MOVEMENT SETTINGS")]
        [Tooltip("Minimum distance required by the A.I Character to attack.")]
        public float minimumDistanceNeededToAttack = 0; 
        [Tooltip("Maximum distance required by the A.I Character to attack.")]
        public float maximumDistanceNeededToAttack = 2; 
        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            // BELOW CODE: Does AI act like a player character (like an invader a.I?) if so use this
            //aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackType, attackAnimation, true);

            //  BELOW CODE: Does AI use simple attacks that are purely animation based (not equipment / item based) if so use this
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackAnimation, true);
        }
    }
}
