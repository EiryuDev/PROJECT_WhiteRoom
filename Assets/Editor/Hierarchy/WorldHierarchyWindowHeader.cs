using UnityEngine;
using UnityEditor;
    
// LOGIC: Simply re-styles a gameObject name in the Hiearchy window to be black and all caps.
// LOGIC: Allows us to seperate our gameObjects and not lose our minds hehe :)

namespace WEV.WhiteRoom
{
    [InitializeOnLoad]
    public static class WorldHierarchyWindowHeader
    {
        // BELOW CODE: The highlight color for better customization
        private static readonly Color HeaderColor = Color.lightBlue; // Solid red background
        private static readonly Color TextColor = Color.black; // Black text for readability

        static WorldHierarchyWindowHeader()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.EntityIdToObject(instanceID) as GameObject;
            if (gameObject == null || !gameObject.name.StartsWith("//", System.StringComparison.Ordinal))
                return;

            // BELOW CODE: Remove "//" and convert to uppercase
            string formattedName = gameObject.name.TrimStart('/').ToUpperInvariant();

            // BELOW CODE: Draw background highlight
            EditorGUI.DrawRect(selectionRect, HeaderColor);

            // BELOW CODE: Create a custom style to ensure no overlapping text
            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = TextColor },
                alignment = TextAnchor.MiddleLeft
            };

            // BELOW CODE: Suppress Unity's default text by drawing an empty content first
            EditorGUI.LabelField(selectionRect, GUIContent.none, EditorStyles.label);

            // BELOW CODE: Draw custom text
            EditorGUI.LabelField(selectionRect, formattedName, labelStyle);
        }
    }
}