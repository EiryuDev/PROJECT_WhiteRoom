using UnityEngine;

namespace WEV.WhiteRoom
{
    public class CoreUtilityColorizeEditor : MonoBehaviour
    {
        // Color Example

        public static CoreUtilityColorizeEditor Red = new CoreUtilityColorizeEditor(Color.red);
        public static CoreUtilityColorizeEditor Yellow = new CoreUtilityColorizeEditor(Color.yellow);
        public static CoreUtilityColorizeEditor Green = new CoreUtilityColorizeEditor(Color.green);
        public static CoreUtilityColorizeEditor Blue = new CoreUtilityColorizeEditor(Color.blue);
        public static CoreUtilityColorizeEditor Cyan = new CoreUtilityColorizeEditor(Color.cyan);
        public static CoreUtilityColorizeEditor Magenta = new CoreUtilityColorizeEditor(Color.magenta);

        // Hex Example

        public static CoreUtilityColorizeEditor Orange = new CoreUtilityColorizeEditor("#FFA500");
        public static CoreUtilityColorizeEditor Olive = new CoreUtilityColorizeEditor("#808000");
        public static CoreUtilityColorizeEditor Purple = new CoreUtilityColorizeEditor("#800080");
        public static CoreUtilityColorizeEditor DarkRed = new CoreUtilityColorizeEditor("#8B0000");
        public static CoreUtilityColorizeEditor DarkGreen = new CoreUtilityColorizeEditor("#006400");
        public static CoreUtilityColorizeEditor DarkOrange = new CoreUtilityColorizeEditor("#FF8C00");
        public static CoreUtilityColorizeEditor Gold = new CoreUtilityColorizeEditor("#FFD700");

        private readonly string _prefix;

        private const string Suffix = "</color>";

        // Convert Color to HtmlString
        public CoreUtilityColorizeEditor(Color color)
        {
            _prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }
        // Use Hex Color
        public CoreUtilityColorizeEditor(string hexColor)
        {
            _prefix = $"<color={hexColor}>";
        }

        public static string operator %(string text, CoreUtilityColorizeEditor color)
        {
            return color._prefix + text + Suffix;
        }
    }
}
