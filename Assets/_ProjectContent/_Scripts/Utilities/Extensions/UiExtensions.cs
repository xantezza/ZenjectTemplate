using UnityEngine.UI;

namespace Utilities.Extensions
{
    public static class UiExtensions
    {
        public static void SwitchUI(this Graphic[] graphics, bool state)
        {
            foreach (var graphic in graphics) graphic.SwitchUI(state);
        }

        public static void SwitchUI(this Graphic graphic, bool state)
        {
            if (graphic.canvasRenderer.cull == !state) return;

            graphic.canvasRenderer.cull = !state;
            if (state) graphic.Rebuild(CanvasUpdate.PreRender);
        }
    }
}