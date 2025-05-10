using System;
using Koneko.UIBuilder.InternalBridge;
using UnityEngine;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder
{
	public class StyleSheetBuilder
	{
		private readonly CustomStyleSheetBuilder _builder = new();

		public static StyleSheet Build(Action<StyleSheetBuilder> builder)
		{
			StyleSheetBuilder styleSheetBuilder = new();
			builder(styleSheetBuilder);
			StyleSheet sheet = ScriptableObject.CreateInstance<StyleSheet>();
			styleSheetBuilder._builder.BuildTo(sheet);
			return sheet;
		}

		public StyleSheetBuilder Add(string selector, Action<StyleBuilder> style)
		{
			StyleHolder holder = new();
			StyleBuilder styleBuilder = new(holder);
			style(styleBuilder);
			_builder.Add(selector, holder);
			return this;
		}
	}
}
