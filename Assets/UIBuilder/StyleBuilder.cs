using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder
{
	public class StyleBuilder
	{
		private readonly IStyle _style;

		public StyleBuilder(IStyle style)
		{
			_style = style;
		}

		public StyleBuilder AlignContent(Align value)
		{
			_style.alignContent = value;
			return this;
		}

		public StyleBuilder AlignItems(Align value)
		{
			_style.alignItems = value;
			return this;
		}

		public StyleBuilder AlignSelf(Align value)
		{
			_style.alignSelf = value;
			return this;
		}

		public StyleBuilder BackgroundColor(Color value)
		{
			_style.backgroundColor = value;
			return this;
		}

		public StyleBuilder BackgroundImage(StyleBackground value)
		{
			_style.backgroundImage = value;
			return this;
		}

		public StyleBuilder BorderBottomColor(Color value)
		{
			_style.borderBottomColor = value;
			return this;
		}

		public StyleBuilder BorderBottomLeftRadius(Length value)
		{
			_style.borderBottomLeftRadius = value;
			return this;
		}

		public StyleBuilder BorderBottomRightRadius(Length value)
		{
			_style.borderBottomRightRadius = value;
			return this;
		}

		public StyleBuilder BorderBottomWidth(float value)
		{
			_style.borderBottomWidth = value;
			return this;
		}

		public StyleBuilder BorderLeftColor(Color value)
		{
			_style.borderLeftColor = value;
			return this;
		}

		public StyleBuilder BorderLeftWidth(float value)
		{
			_style.borderLeftWidth = value;
			return this;
		}

		public StyleBuilder BorderRightColor(Color value)
		{
			_style.borderRightColor = value;
			return this;
		}

		public StyleBuilder BorderRightWidth(float value)
		{
			_style.borderRightWidth = value;
			return this;
		}

		public StyleBuilder BorderTopColor(Color value)
		{
			_style.borderTopColor = value;
			return this;
		}

		public StyleBuilder BorderTopLeftRadius(Length value)
		{
			_style.borderTopLeftRadius = value;
			return this;
		}

		public StyleBuilder BorderTopRightRadius(Length value)
		{
			_style.borderTopRightRadius = value;
			return this;
		}

		public StyleBuilder BorderTopWidth(float value)
		{
			_style.borderTopWidth = value;
			return this;
		}

		public StyleBuilder Bottom(Length value)
		{
			_style.bottom = value;
			return this;
		}

		public StyleBuilder Color(Color value)
		{
			_style.color = value;
			return this;
		}

		public StyleBuilder Cursor(StyleCursor value)
		{
			_style.cursor = value;
			return this;
		}

		public StyleBuilder Display(DisplayStyle value)
		{
			_style.display = value;
			return this;
		}

		public StyleBuilder FlexBasis(StyleLength value)
		{
			_style.flexBasis = value;
			return this;
		}

		public StyleBuilder FlexDirection(FlexDirection value)
		{
			_style.flexDirection = value;
			return this;
		}

		public StyleBuilder FlexGrow(float value)
		{
			_style.flexGrow = value;
			return this;
		}

		public StyleBuilder FlexShrink(float value)
		{
			_style.flexShrink = value;
			return this;
		}

		public StyleBuilder FlexWrap(Wrap value)
		{
			_style.flexWrap = value;
			return this;
		}

		public StyleBuilder FontSize(Length value)
		{
			_style.fontSize = value;
			return this;
		}

		public StyleBuilder Height(Length value)
		{
			_style.height = value;
			return this;
		}

		public StyleBuilder JustifyContent(Justify value)
		{
			_style.justifyContent = value;
			return this;
		}

		public StyleBuilder Left(Length value)
		{
			_style.left = value;
			return this;
		}

		public StyleBuilder LetterSpacing(Length value)
		{
			_style.letterSpacing = value;
			return this;
		}

		public StyleBuilder MarginBottom(Length value)
		{
			_style.marginBottom = value;
			return this;
		}

		public StyleBuilder MarginLeft(Length value)
		{
			_style.marginLeft = value;
			return this;
		}

		public StyleBuilder MarginRight(Length value)
		{
			_style.marginRight = value;
			return this;
		}

		public StyleBuilder MarginTop(Length value)
		{
			_style.marginTop = value;
			return this;
		}

		public StyleBuilder MaxHeight(Length value)
		{
			_style.maxHeight = value;
			return this;
		}

		public StyleBuilder MaxWidth(Length value)
		{
			_style.maxWidth = value;
			return this;
		}

		public StyleBuilder MinHeight(Length value)
		{
			_style.minHeight = value;
			return this;
		}

		public StyleBuilder MinWidth(Length value)
		{
			_style.minWidth = value;
			return this;
		}

		public StyleBuilder Opacity(float value)
		{
			_style.opacity = value;
			return this;
		}

		public StyleBuilder Overflow(Overflow value)
		{
			_style.overflow = value;
			return this;
		}

		public StyleBuilder PaddingBottom(Length value)
		{
			_style.paddingBottom = value;
			return this;
		}

		public StyleBuilder PaddingLeft(Length value)
		{
			_style.paddingLeft = value;
			return this;
		}

		public StyleBuilder PaddingRight(Length value)
		{
			_style.paddingRight = value;
			return this;
		}

		public StyleBuilder PaddingTop(Length value)
		{
			_style.paddingTop = value;
			return this;
		}

		public StyleBuilder Position(Position value)
		{
			_style.position = value;
			return this;
		}

		public StyleBuilder Right(Length value)
		{
			_style.right = value;
			return this;
		}

		public StyleBuilder Rotate(Rotate value)
		{
			_style.rotate = value;
			return this;
		}

		public StyleBuilder Scale(Scale value)
		{
			_style.scale = value;
			return this;
		}

		public StyleBuilder TextOverflow(TextOverflow value)
		{
			_style.textOverflow = value;
			return this;
		}

		public StyleBuilder TextShadow(TextShadow value)
		{
			_style.textShadow = value;
			return this;
		}

		public StyleBuilder Top(Length value)
		{
			_style.top = value;
			return this;
		}

		public StyleBuilder TransformOrigin(TransformOrigin value)
		{
			_style.transformOrigin = value;
			return this;
		}

		public StyleBuilder TransitionDelay(List<TimeValue> value)
		{
			_style.transitionDelay = value;
			return this;
		}

		public StyleBuilder TransitionDuration(List<TimeValue> value)
		{
			_style.transitionDuration = value;
			return this;
		}

		public StyleBuilder TransitionProperty(StyleList<StylePropertyName> value)
		{
			_style.transitionProperty = value;
			return this;
		}

		public StyleBuilder TransitionTimingFunction(StyleList<EasingFunction> value)
		{
			_style.transitionTimingFunction = value;
			return this;
		}

		public StyleBuilder Translate(Translate value)
		{
			_style.translate = value;
			return this;
		}

		public StyleBuilder UnityBackgroundImageTintColor(Color value)
		{
			_style.unityBackgroundImageTintColor = value;
			return this;
		}

		public StyleBuilder UnityFont(Font value)
		{
			_style.unityFont = value;
			return this;
		}

		public StyleBuilder UnityFontDefinition(FontDefinition value)
		{
			_style.unityFontDefinition = value;
			return this;
		}

		public StyleBuilder UnityFontStyleAndWeight(FontStyle value)
		{
			_style.unityFontStyleAndWeight = value;
			return this;
		}

		public StyleBuilder UnityOverflowClipBox(OverflowClipBox value)
		{
			_style.unityOverflowClipBox = value;
			return this;
		}

		public StyleBuilder UnityParagraphSpacing(Length value)
		{
			_style.unityParagraphSpacing = value;
			return this;
		}

		public StyleBuilder UnitySliceBottom(int value)
		{
			_style.unitySliceBottom = value;
			return this;
		}

		public StyleBuilder UnitySliceLeft(int value)
		{
			_style.unitySliceLeft = value;
			return this;
		}

		public StyleBuilder UnitySliceRight(int value)
		{
			_style.unitySliceRight = value;
			return this;
		}

		public StyleBuilder UnitySliceTop(int value)
		{
			_style.unitySliceTop = value;
			return this;
		}

		public StyleBuilder UnityTextAlign(TextAnchor value)
		{
			_style.unityTextAlign = value;
			return this;
		}

		public StyleBuilder UnityTextOutlineColor(Color value)
		{
			_style.unityTextOutlineColor = value;
			return this;
		}

		public StyleBuilder UnityTextOutlineWidth(float value)
		{
			_style.unityTextOutlineWidth = value;
			return this;
		}

		public StyleBuilder UnityTextOverflowPosition(TextOverflowPosition value)
		{
			_style.unityTextOverflowPosition = value;
			return this;
		}

		public StyleBuilder Visibility(Visibility value)
		{
			_style.visibility = value;
			return this;
		}

		public StyleBuilder WhiteSpace(WhiteSpace value)
		{
			_style.whiteSpace = value;
			return this;
		}

		public StyleBuilder Width(Length value)
		{
			_style.width = value;
			return this;
		}

		public StyleBuilder WordSpacing(Length value)
		{
			_style.wordSpacing = value;
			return this;
		}

		public StyleBuilder Size(Length width, Length height)
		{
			_style.width = width;
			_style.height = height;
			return this;
		}

		public StyleBuilder Position(Length top, Length right, Length bottom, Length left)
		{
			_style.top = top;
			_style.right = right;
			_style.bottom = bottom;
			_style.left = left;
			return this;
		}

		public StyleBuilder Margin(Length vertical, Length horizontal)
		{
			_style.marginTop = vertical;
			_style.marginBottom = vertical;
			_style.marginLeft = horizontal;
			_style.marginRight = horizontal;
			return this;
		}

		public StyleBuilder Padding(Length vertical, Length horizontal)
		{
			_style.paddingTop = vertical;
			_style.paddingBottom = vertical;
			_style.paddingLeft = horizontal;
			_style.paddingRight = horizontal;
			return this;
		}
	}
}
