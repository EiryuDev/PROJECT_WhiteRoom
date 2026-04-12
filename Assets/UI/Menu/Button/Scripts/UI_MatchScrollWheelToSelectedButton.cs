using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WEV.WhiteRoom
{
    public class UI_MatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected; // Current selected game object
        [SerializeField] GameObject previouslySelected; // Previously selected game object
        [SerializeField] RectTransform currentSelectedTransform; // Current selected rect transform for the game object


        [SerializeField] RectTransform contentPanel; // Reference to the content panel rect transform
        [SerializeField] ScrollRect scrollRect; // Reference to the scroll rect game object

        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previouslySelected)
                {
                    previouslySelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            // BELOW CODE: It will only lock the position of the y axis (up & down)
            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }
}