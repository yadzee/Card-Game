using System.Linq;
using Extensions;
using UnityEngine;

namespace Tools.UI.Card
{
    public class UiTexturePicker : MonoBehaviour
    {
        [SerializeField] private Sprite[] Sprites;

        private SpriteRenderer MyRenderer { get; set; }

        private void Awake()
        {
            MyRenderer = GetComponent<SpriteRenderer>();

            if (Sprites.Length > 0)
                MyRenderer.sprite = Sprites.ToList().RandomItem();
        }

        public void SetSprite(Sprite sprite)
        {
            if (sprite == null)
                return;

            MyRenderer.sprite = sprite;
        }
    }
}