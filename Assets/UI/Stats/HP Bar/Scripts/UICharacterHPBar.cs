using TMPro;
using UnityEngine;

namespace WEV.WhiteRoom
{
    // LOGIC: Performs identical to the UI_StatusBar, except this bar appears and disappears in world space (will always face camera)
    public class UICharacterHPBar : UIStatusBar
    {
        private CharacterManager character; // Reference to the Character Manager script
        private AICharacterManager aiCharacter; // Reference to the AI Character Manager script
        private PlayerManager playerCharacter; // Reference to the Player Manager script

        [SerializeField] private bool displayCharacterNameOnDamage = true; // Checks if the display character name on damage is true or not
        [SerializeField] private float defaultTimeBeforeBarHides = 3; // How much time it should go before the bar hides
        [SerializeField] private float hideTimer = 0; // Reference to the float for the hiding the timer
        public float currentDamageTaken = 0; // How much damage is taken by the character
        [SerializeField] private TextMeshProUGUI characterName; // Reference to the text mesh text for the character name
        [SerializeField] private TextMeshProUGUI characterDamage; // Reference to the text mesh text for the damage
        [HideInInspector] public int oldHealthValue = 0; // Reference to the old health value of the character
        
        protected override void Awake()
        {
            base.Awake();
            
            character = GetComponentInParent<CharacterManager>();

            if (character != null)
            {
                aiCharacter = character as AICharacterManager;
                playerCharacter = character as PlayerManager;
            }
        }

        protected override void Start()
        {
            base.Start();
            
            gameObject.SetActive(false);
        }

        private void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);

            if (hideTimer > 0)
            {
                hideTimer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            currentDamageTaken = 0;
        }

        public override void SetStat(int newValue)
        {
            if (displayCharacterNameOnDamage)
            {
                characterName.enabled = true;
                
                if(aiCharacter != null)
                    characterName.text = aiCharacter.characterName;
                
                if(playerCharacter != null)
                    characterName.text = playerCharacter.characterName.ToString();
            }

            // BELOW CODE: Call this here incase max health changes from a character effect/buff/etc
            slider.maxValue = character.characterStatsManager.maxHealth;
            
            // TO-DO: Run secondary bar logic (yellow bar that appears behind hp when damaged)
            
            // BELOW CODE: Total damage taken while the bar is active
            currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHealthValue - newValue));

            if (currentDamageTaken < 0)
            {
                currentDamageTaken = Mathf.Abs(currentDamageTaken);
                characterDamage.text = "+ " + currentDamageTaken.ToString();
            }
            else
            {
                characterDamage.text = "- " + currentDamageTaken.ToString();
            }
            
            slider.value = newValue;

            if (character.characterStatsManager.currentHealth !=
                character.characterStatsManager.maxHealth)
            {
                hideTimer = defaultTimeBeforeBarHides;
                gameObject.SetActive(true);
            }
        }
    }
}
