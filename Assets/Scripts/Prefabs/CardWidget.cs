using UnityEngine;

namespace ShadowCraft
{
    public class CardWidget : MonoBehaviour
    {
        Card card { get; set; }

        public void OnHoverEnter()
        {
            Debug.Log("On Hover");
        }

        public void OnHoverExit()
        {
            Debug.Log("On Stop Hover");
        }

        public void OnClick()
        {
            Debug.Log("On Click");
        }

        public void OnClickRelease()
        {
            Debug.Log("On Click Release");
        }

        public void OnDrag(Vector2 newLocation)
        {
            Debug.Log("On Drag");
        }
    }
}
