using UnityEngine;
using UnityEngine.UI;

namespace WEV.WhiteRoom
{
    public class UI_StatusBar : MonoBehaviour
    {
        protected Slider slider; // Reference to the slider component
        protected RectTransform rectTransform; // Reference to the rect transform component

        // LOGIC: Variable to scale bar size depending on stat(higher stat = longer bar across screen)
        [Header("BAR OPTIONS")]
        [SerializeField] protected bool scaleBarLengthWithStats = true; // Bool to scale the bar length according to stats
        [SerializeField] protected float widthScaleMultiplier = 1; // Multiplier value for the width of the scale 
        // LOGIC: Secondary bar behind may bar for polish effect (orange bar will show how much action/damage takes away from current stat)

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }
        protected virtual void Start()
        {
            
        }
        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if(scaleBarLengthWithStats)
            {
                // BELOW CODE: Scale the transform of this object
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                
                // BELOW CODE: Resets the positon of the bars on their layout group's settings 
                PlayerUIManager.instance.playerHUDManager.RefreshHUD();
            }
        }
    }
}