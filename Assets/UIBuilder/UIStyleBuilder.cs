using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder
{
    public static partial class UIElementBuilder
    {
        public static StyleStruct AlignContent(Align value) =>
            new(StyleType.AlignContent) { Align = value };

        public static StyleStruct AlignItems(Align value) =>
            new(StyleType.AlignItems) { Align = value };

        public static StyleStruct AlignSelf(Align value) =>
            new(StyleType.AlignSelf) { Align = value };

        public static StyleStruct BackgroundColor(Color value) =>
            new(StyleType.BackgroundColor) { Color = value };

        public static StyleStruct BackgroundImage(StyleBackground value) =>
            new(StyleType.BackgroundImage) { Background = value };

        public static StyleStruct BorderBottomColor(Color value) =>
            new(StyleType.BorderBottomColor) { Color = value };

        public static StyleStruct BorderBottomLeftRadius(Length value) =>
            new(StyleType.BorderBottomLeftRadius) { Length = value };

        public static StyleStruct BorderBottomRightRadius(Length value) =>
            new(StyleType.BorderBottomRightRadius) { Length = value };

        public static StyleStruct BorderBottomWidth(float value) =>
            new(StyleType.BorderBottomWidth) { Float = value };

        public static StyleStruct BorderLeftColor(Color value) =>
            new(StyleType.BorderLeftColor) { Color = value };

        public static StyleStruct BorderLeftWidth(float value) =>
            new(StyleType.BorderLeftWidth) { Float = value };

        public static StyleStruct BorderRightColor(Color value) =>
            new(StyleType.BorderRightColor) { Color = value };

        public static StyleStruct BorderRightWidth(float value) =>
            new(StyleType.BorderRightWidth) { Float = value };

        public static StyleStruct BorderTopColor(Color value) =>
            new(StyleType.BorderTopColor) { Color = value };

        public static StyleStruct BorderTopLeftRadius(Length value) =>
            new(StyleType.BorderTopLeftRadius) { Length = value };

        public static StyleStruct BorderTopRightRadius(Length value) =>
            new(StyleType.BorderTopRightRadius) { Length = value };

        public static StyleStruct BorderTopWidth(float value) =>
            new(StyleType.BorderTopWidth) { Float = value };

        public static StyleStruct Bottom(Length value) => new(StyleType.Bottom) { Length = value };

        public static StyleStruct Color(Color value) => new(StyleType.Color) { Color = value };

        public static StyleStruct Cursor(StyleCursor value) =>
            new(StyleType.Cursor) { Cursor = value };

        public static StyleStruct Display(DisplayStyle value) =>
            new(StyleType.Display) { DisplayStyle = value };

        public static StyleStruct FlexBasis(Length value) =>
            new(StyleType.FlexBasis) { Length = value };

        public static StyleStruct FlexDirection(FlexDirection value) =>
            new(StyleType.FlexDirection) { FlexDirection = value };

        public static StyleStruct FlexGrow(float value) =>
            new(StyleType.FlexGrow) { Float = value };

        public static StyleStruct FlexShrink(float value) =>
            new(StyleType.FlexShrink) { Float = value };

        public static StyleStruct FlexWrap(Wrap value) => new(StyleType.FlexWrap) { Wrap = value };

        public static StyleStruct FontSize(Length value) =>
            new(StyleType.FontSize) { Length = value };

        public static StyleStruct Height(Length value) => new(StyleType.Height) { Length = value };

        public static StyleStruct JustifyContent(Justify value) =>
            new(StyleType.JustifyContent) { Justify = value };

        public static StyleStruct Left(Length value) => new(StyleType.Left) { Length = value };

        public static StyleStruct LetterSpacing(Length value) =>
            new(StyleType.LetterSpacing) { Length = value };

        public static StyleStruct MarginBottom(Length value) =>
            new(StyleType.MarginBottom) { Length = value };

        public static StyleStruct MarginLeft(Length value) =>
            new(StyleType.MarginLeft) { Length = value };

        public static StyleStruct MarginRight(Length value) =>
            new(StyleType.MarginRight) { Length = value };

        public static StyleStruct MarginTop(Length value) =>
            new(StyleType.MarginTop) { Length = value };

        public static StyleStruct MaxHeight(Length value) =>
            new(StyleType.MaxHeight) { Length = value };

        public static StyleStruct MaxWidth(Length value) =>
            new(StyleType.MaxWidth) { Length = value };

        public static StyleStruct MinHeight(Length value) =>
            new(StyleType.MinHeight) { Length = value };

        public static StyleStruct MinWidth(Length value) =>
            new(StyleType.MinWidth) { Length = value };

        public static StyleStruct Opacity(float value) => new(StyleType.Opacity) { Float = value };

        public static StyleStruct Overflow(Overflow value) =>
            new(StyleType.Overflow) { Overflow = value };

        public static StyleStruct PaddingBottom(Length value) =>
            new(StyleType.PaddingBottom) { Length = value };

        public static StyleStruct PaddingLeft(Length value) =>
            new(StyleType.PaddingLeft) { Length = value };

        public static StyleStruct PaddingRight(Length value) =>
            new(StyleType.PaddingRight) { Length = value };

        public static StyleStruct PaddingTop(Length value) =>
            new(StyleType.PaddingTop) { Length = value };

        public static StyleStruct Position(Position value) =>
            new(StyleType.Position) { Position = value };

        public static StyleStruct Right(Length value) => new(StyleType.Right) { Length = value };

        public static StyleStruct Rotate(Rotate value) => new(StyleType.Rotate) { Rotate = value };

        public static StyleStruct Scale(Scale value) => new(StyleType.Scale) { Scale = value };

        public static StyleStruct TextOverflow(TextOverflow value) =>
            new(StyleType.TextOverflow) { TextOverflow = value };

        public static StyleStruct TextShadow(TextShadow value) =>
            new(StyleType.TextShadow) { TextShadow = value };

        public static StyleStruct Top(Length value) => new(StyleType.Top) { Length = value };

        public static StyleStruct TransformOrigin(TransformOrigin value) =>
            new(StyleType.TransformOrigin) { TransformOrigin = value };

        public static StyleStruct TransitionDelay(List<TimeValue> value) =>
            new(StyleType.TransitionDelay) { TimeValue = value };

        public static StyleStruct TransitionDuration(List<TimeValue> value) =>
            new(StyleType.TransitionDuration) { TimeValue = value };

        public static StyleStruct TransitionProperty(StyleList<StylePropertyName> value) =>
            new(StyleType.TransitionProperty) { PropertyName = value };

        public static StyleStruct TransitionTimingFunction(StyleList<EasingFunction> value) =>
            new(StyleType.TransitionTimingFunction) { EasingFunction = value };

        public static StyleStruct Translate(Translate value) =>
            new(StyleType.Translate) { Translate = value };

        public static StyleStruct UnityBackgroundImageTintColor(Color value) =>
            new(StyleType.UnityBackgroundImageTintColor) { Color = value };

        public static StyleStruct UnityBackgroundScaleMode(StyleEnum<ScaleMode> value) =>
            new(StyleType.UnityBackgroundScaleMode) { ScaleMode = value };

        public static StyleStruct UnityFont(Font value) =>
            new(StyleType.UnityFont) { Font = value };

        public static StyleStruct UnityFontDefinition(FontDefinition value) =>
            new(StyleType.UnityFontDefinition) { FontDefinition = value };

        public static StyleStruct UnityFontStyleAndWeight(FontStyle value) =>
            new(StyleType.UnityFontStyleAndWeight) { FontStyle = value };

        public static StyleStruct UnityOverflowClipBox(OverflowClipBox value) =>
            new(StyleType.UnityOverflowClipBox) { OverflowClipBox = value };

        public static StyleStruct UnityParagraphSpacing(Length value) =>
            new(StyleType.UnityParagraphSpacing) { Length = value };

        public static StyleStruct UnitySliceBottom(int value) =>
            new(StyleType.UnitySliceBottom) { Int = value };

        public static StyleStruct UnitySliceLeft(int value) =>
            new(StyleType.UnitySliceLeft) { Int = value };

        public static StyleStruct UnitySliceRight(int value) =>
            new(StyleType.UnitySliceRight) { Int = value };

        public static StyleStruct UnitySliceTop(int value) =>
            new(StyleType.UnitySliceScale) { Int = value };

        public static StyleStruct UnitySliceScale(float value) =>
            new(StyleType.UnitySliceTop) { Float = value };

        public static StyleStruct UnityTextAlign(TextAnchor value) =>
            new(StyleType.UnityTextAlign) { TextAnchor = value };

        public static StyleStruct UnityTextOutlineColor(Color value) =>
            new(StyleType.UnityTextOutlineColor) { Color = value };

        public static StyleStruct UnityTextOutlineWidth(float value) =>
            new(StyleType.UnityTextOutlineWidth) { Float = value };

        public static StyleStruct UnityTextOverflowPosition(TextOverflowPosition value) =>
            new(StyleType.UnityTextOverflowPosition) { TextOverflowPosition = value };

        public static StyleStruct Visibility(Visibility value) =>
            new(StyleType.Visibility) { Visibility = value };

        public static StyleStruct WhiteSpace(WhiteSpace value) =>
            new(StyleType.WhiteSpace) { WhiteSpace = value };

        public static StyleStruct Width(Length value) => new(StyleType.Width) { Length = value };

        public static StyleStruct WordSpacing(Length value) =>
            new(StyleType.WordSpacing) { Length = value };

        private static void ApplyStyle(VisualElement element, StyleStruct styleStruct)
        {
            IStyle style = element.style;
            switch (styleStruct.Type)
            {
                case StyleType.AlignContent:
                    style.alignContent = styleStruct.Align;
                    break;
                case StyleType.AlignItems:
                    style.alignItems = styleStruct.Align;
                    break;
                case StyleType.AlignSelf:
                    style.alignSelf = styleStruct.Align;
                    break;
                case StyleType.BackgroundColor:
                    style.backgroundColor = styleStruct.Color;
                    break;
                case StyleType.BackgroundImage:
                    style.backgroundImage = styleStruct.Background;
                    break;
                case StyleType.BorderBottomColor:
                    style.borderBottomColor = styleStruct.Color;
                    break;
                case StyleType.BorderBottomLeftRadius:
                    style.borderBottomLeftRadius = styleStruct.Length;
                    break;
                case StyleType.BorderBottomRightRadius:
                    style.borderBottomRightRadius = styleStruct.Length;
                    break;
                case StyleType.BorderBottomWidth:
                    style.borderBottomWidth = styleStruct.Float;
                    break;
                case StyleType.BorderLeftColor:
                    style.borderLeftColor = styleStruct.Color;
                    break;
                case StyleType.BorderLeftWidth:
                    style.borderLeftWidth = styleStruct.Float;
                    break;
                case StyleType.BorderRightColor:
                    style.borderRightColor = styleStruct.Color;
                    break;
                case StyleType.BorderRightWidth:
                    style.borderRightWidth = styleStruct.Float;
                    break;
                case StyleType.BorderTopColor:
                    style.borderTopColor = styleStruct.Color;
                    break;
                case StyleType.BorderTopLeftRadius:
                    style.borderTopLeftRadius = styleStruct.Length;
                    break;
                case StyleType.BorderTopRightRadius:
                    style.borderTopRightRadius = styleStruct.Length;
                    break;
                case StyleType.BorderTopWidth:
                    style.borderTopWidth = styleStruct.Float;
                    break;
                case StyleType.Bottom:
                    style.bottom = styleStruct.Length;
                    break;
                case StyleType.Color:
                    style.color = styleStruct.Color;
                    break;
                case StyleType.Cursor:
                    style.cursor = styleStruct.Cursor;
                    break;
                case StyleType.Display:
                    style.display = styleStruct.DisplayStyle;
                    break;
                case StyleType.FlexBasis:
                    style.flexBasis = styleStruct.Length;
                    break;
                case StyleType.FlexDirection:
                    style.flexDirection = styleStruct.FlexDirection;
                    break;
                case StyleType.FlexGrow:
                    style.flexGrow = styleStruct.Float;
                    break;
                case StyleType.FlexShrink:
                    style.flexShrink = styleStruct.Float;
                    break;
                case StyleType.FlexWrap:
                    style.flexWrap = styleStruct.Wrap;
                    break;
                case StyleType.FontSize:
                    style.fontSize = styleStruct.Length;
                    break;
                case StyleType.Height:
                    style.height = styleStruct.Length;
                    break;
                case StyleType.JustifyContent:
                    style.justifyContent = styleStruct.Justify;
                    break;
                case StyleType.Left:
                    style.left = styleStruct.Length;
                    break;
                case StyleType.LetterSpacing:
                    style.letterSpacing = styleStruct.Length;
                    break;
                case StyleType.MarginBottom:
                    style.marginBottom = styleStruct.Length;
                    break;
                case StyleType.MarginLeft:
                    style.marginLeft = styleStruct.Length;
                    break;
                case StyleType.MarginRight:
                    style.marginRight = styleStruct.Length;
                    break;
                case StyleType.MarginTop:
                    style.marginTop = styleStruct.Length;
                    break;
                case StyleType.MaxHeight:
                    style.maxHeight = styleStruct.Length;
                    break;
                case StyleType.MaxWidth:
                    style.maxWidth = styleStruct.Length;
                    break;
                case StyleType.MinHeight:
                    style.minHeight = styleStruct.Length;
                    break;
                case StyleType.MinWidth:
                    style.minWidth = styleStruct.Length;
                    break;
                case StyleType.Opacity:
                    style.opacity = styleStruct.Float;
                    break;
                case StyleType.Overflow:
                    style.overflow = styleStruct.Overflow;
                    break;
                case StyleType.PaddingBottom:
                    style.paddingBottom = styleStruct.Length;
                    break;
                case StyleType.PaddingLeft:
                    style.paddingLeft = styleStruct.Length;
                    break;
                case StyleType.PaddingRight:
                    style.paddingRight = styleStruct.Length;
                    break;
                case StyleType.PaddingTop:
                    style.paddingTop = styleStruct.Length;
                    break;
                case StyleType.Position:
                    style.position = styleStruct.Position;
                    break;
                case StyleType.Right:
                    style.right = styleStruct.Length;
                    break;
                case StyleType.Rotate:
                    style.rotate = styleStruct.Rotate;
                    break;
                case StyleType.Scale:
                    style.scale = styleStruct.Scale;
                    break;
                case StyleType.TextOverflow:
                    style.textOverflow = styleStruct.TextOverflow;
                    break;
                case StyleType.TextShadow:
                    style.textShadow = styleStruct.TextShadow;
                    break;
                case StyleType.Top:
                    style.top = styleStruct.Length;
                    break;
                case StyleType.TransformOrigin:
                    style.transformOrigin = styleStruct.TransformOrigin;
                    break;
                case StyleType.TransitionDelay:
                    style.transitionDelay = styleStruct.TimeValue;
                    break;
                case StyleType.TransitionDuration:
                    style.transitionDuration = styleStruct.TimeValue;
                    break;
                case StyleType.TransitionProperty:
                    style.transitionProperty = styleStruct.PropertyName;
                    break;
                case StyleType.TransitionTimingFunction:
                    style.transitionTimingFunction = styleStruct.EasingFunction;
                    break;
                case StyleType.Translate:
                    style.translate = styleStruct.Translate;
                    break;
                case StyleType.UnityBackgroundImageTintColor:
                    style.unityBackgroundImageTintColor = styleStruct.Color;
                    break;
                case StyleType.UnityBackgroundScaleMode:
                    style.unityBackgroundScaleMode = styleStruct.ScaleMode;
                    break;
                case StyleType.UnityFont:
                    style.unityFont = styleStruct.Font;
                    break;
                case StyleType.UnityFontDefinition:
                    style.unityFontDefinition = styleStruct.FontDefinition;
                    break;
                case StyleType.UnityFontStyleAndWeight:
                    style.unityFontStyleAndWeight = styleStruct.FontStyle;
                    break;
                case StyleType.UnityOverflowClipBox:
                    style.unityOverflowClipBox = styleStruct.OverflowClipBox;
                    break;
                case StyleType.UnityParagraphSpacing:
                    style.unityParagraphSpacing = styleStruct.Length;
                    break;
                case StyleType.UnitySliceBottom:
                    style.unitySliceBottom = styleStruct.Int;
                    break;
                case StyleType.UnitySliceLeft:
                    style.unitySliceLeft = styleStruct.Int;
                    break;
                case StyleType.UnitySliceRight:
                    style.unitySliceRight = styleStruct.Int;
                    break;
                case StyleType.UnitySliceScale:
                    style.unitySliceTop = styleStruct.Int;
                    break;
                case StyleType.UnityTextAlign:
                    style.unityTextAlign = styleStruct.TextAnchor;
                    break;
                case StyleType.UnityTextOutlineColor:
                    style.unityTextOutlineColor = styleStruct.Color;
                    break;
                case StyleType.UnityTextOutlineWidth:
                    style.unityTextOutlineWidth = styleStruct.Float;
                    break;
                case StyleType.UnityTextOverflowPosition:
                    style.unityTextOverflowPosition = styleStruct.TextOverflowPosition;
                    break;
                case StyleType.Visibility:
                    style.visibility = styleStruct.Visibility;
                    break;
                case StyleType.WhiteSpace:
                    style.whiteSpace = styleStruct.WhiteSpace;
                    break;
                case StyleType.Width:
                    style.width = styleStruct.Length;
                    break;
                case StyleType.WordSpacing:
                    style.wordSpacing = styleStruct.Length;
                    break;
            }
        }
    }
}
