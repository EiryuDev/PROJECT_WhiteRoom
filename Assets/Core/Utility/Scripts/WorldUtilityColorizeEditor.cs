using UnityEngine;

namespace WEV.WhiteRoom
{
    public class WorldUtilityColorizeEditor : MonoBehaviour
    {
        // Color Example

        public static WorldUtilityColorizeEditor Red = new WorldUtilityColorizeEditor(Color.red);
        public static WorldUtilityColorizeEditor Yellow = new WorldUtilityColorizeEditor(Color.yellow);
        public static WorldUtilityColorizeEditor Green = new WorldUtilityColorizeEditor(Color.green);
        public static WorldUtilityColorizeEditor Blue = new WorldUtilityColorizeEditor(Color.blue);
        public static WorldUtilityColorizeEditor Cyan = new WorldUtilityColorizeEditor(Color.cyan);
        public static WorldUtilityColorizeEditor Magenta = new WorldUtilityColorizeEditor(Color.magenta);

        // Hex Example

        public static WorldUtilityColorizeEditor Orange = new WorldUtilityColorizeEditor("#FFA500");
        public static WorldUtilityColorizeEditor Olive = new WorldUtilityColorizeEditor("#808000");
        public static WorldUtilityColorizeEditor Purple = new WorldUtilityColorizeEditor("#800080");
        public static WorldUtilityColorizeEditor DarkRed = new WorldUtilityColorizeEditor("#8B0000");
        public static WorldUtilityColorizeEditor DarkGreen = new WorldUtilityColorizeEditor("#006400");
        public static WorldUtilityColorizeEditor DarkOrange = new WorldUtilityColorizeEditor("#FF8C00");
        public static WorldUtilityColorizeEditor Gold = new WorldUtilityColorizeEditor("#FFD700");

        private readonly string _prefix;

        private const string Suffix = "</color>";

        // Convert Color to HtmlString
        public WorldUtilityColorizeEditor(Color color)
        {
            _prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }
        // Use Hex Color
        public WorldUtilityColorizeEditor(string hexColor)
        {
            _prefix = $"<color={hexColor}>";
        }

        public static string operator %(string text, WorldUtilityColorizeEditor color)
        {
            return color._prefix + text + Suffix;
        }
    }
}
