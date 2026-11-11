using UnityEngine;

namespace Tools.UI.Card
{
    public class UiTexturePicker : MonoBehaviour
    {
        private SpriteRenderer MyRenderer => GetComponent<SpriteRenderer>();

        
        public void SetSprite(Sprite sprite)
        {
            if (sprite == null)
                return;

            MyRenderer.sprite = sprite;
        }
    }
}