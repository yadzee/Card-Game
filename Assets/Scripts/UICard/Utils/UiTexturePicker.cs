using UnityEngine;

namespace Tools.UI.Card
{
    public class UiTexturePicker : MonoBehaviour
    {
        private SpriteRenderer MyRenderer { get; set; }

        private void Awake()
        {
            MyRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(Sprite sprite)
        {
            if (sprite == null)
                return;

            MyRenderer.sprite = sprite;
        }
    }
}