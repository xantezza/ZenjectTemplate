using UnityEngine;

namespace Utils.Extensions
{
    public static class SpriteRenderersExtensions
    {
        public static void SwitchUI(this SpriteRenderer[] spriteRenderers, bool state)
        {
            foreach (var spriteRenderer in spriteRenderers) spriteRenderer.SwitchUI(state);
        }

        public static void SwitchUI(this SpriteRenderer spriteRenderer, bool state)
        {
            if (spriteRenderer.enabled == state) return;

            spriteRenderer.enabled = state;
        }
    }
}