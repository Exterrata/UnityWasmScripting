using UnityEngine;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder.InternalBridge
{
	public static class MeshGenerationContextExtras
	{
		private static readonly ITextHandle TextHandle = TextCoreHandle.New();

		public static void DrawText(this MeshGenerationContext ctx, string text, Vector2 position, int fontSize, Color color)
		{
			MeshGenerationContextUtils.TextParams textParams = MeshGenerationContextUtils.TextParams.MakeStyleBased(ctx.visualElement, text);
			textParams.rect = new Rect(position, Vector2.one * 16384f);
			textParams.fontSize = fontSize;
			textParams.fontColor = color;
			ctx.Text(textParams, TextHandle, ctx.visualElement.scaledPixelsPerPoint);
		}

		public static void DrawRect(this MeshGenerationContext ctx, Rect rect, Color color)
		{
			ctx.Rectangle(MeshGenerationContextUtils.RectangleParams.MakeSolid(rect, color, ContextType.Editor));
		}
	}
}
