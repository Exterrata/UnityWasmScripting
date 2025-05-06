using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder {
	public struct UIOperation {
		public OperationType Type { get; }
		public object Value { get; }

		private UIOperation(VisualElement element) {
			Type = OperationType.Element;
			Value = element;
		}

		private UIOperation(StyleStruct style) {
			Type = OperationType.Style;
			Value = style;
		}

		private UIOperation(SetName name) {
			Type = OperationType.Name;
			Value = name;
		}

		private UIOperation(SetEnabled enabled) {
			Type = OperationType.Enabled;
			Value = enabled;
		}

		private UIOperation(SetFocusable focusable) {
			Type = OperationType.Focusable;
			Value = focusable;
		}

		private UIOperation(AddClass @class) {
			Type = OperationType.AddClass;
			Value = @class.Class;
		}

		private UIOperation(Subscribe subscribe) {
			Type = OperationType.Subscribe;
			Value = subscribe;
		}
		
		public static implicit operator UIOperation(VisualElement element) => new(element);
		public static implicit operator UIOperation(StyleStruct style) => new(style);
		public static implicit operator UIOperation(SetName name) => new(name);
		public static implicit operator UIOperation(SetEnabled enabled) => new(enabled);
		public static implicit operator UIOperation(SetFocusable focusable) => new(focusable);
		public static implicit operator UIOperation(AddClass @class) => new(@class);
		public static implicit operator UIOperation(Subscribe callback) => new(callback);
	}

	public enum OperationType {
		Element,
		Style,
		Name,
		Enabled,
		Focusable,
		AddClass,
		Subscribe,
	}

	public struct SetName { public string Name; }
	public struct SetEnabled { public bool Enabled; }
	public struct SetFocusable { public bool Focusable; }
	public struct AddClass { public string Class; }
	public struct Subscribe { public IUICallback Callback; }
	
	public readonly struct UICallback<TEvent> : IUICallback where TEvent : EventBase<TEvent>, new() {
		private EventCallback<TEvent> Handler { get; }
		private TrickleDown TrickleDown { get; }

		public UICallback(EventCallback<TEvent> handler, TrickleDown trickleDown = TrickleDown.NoTrickleDown) {
			Handler = handler;
			TrickleDown = trickleDown;
		}

		public void Register(VisualElement element) {
			element.RegisterCallback(Handler, TrickleDown);
		}
	}
	
	public interface IUICallback {
		void Register(VisualElement element);
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct StyleStruct {
		public StyleStruct(StyleType type) {
			Type = type;
			Length = default;
			Color = default;
			Float = default;
			Int = default;
			Rotate = default;
			Scale = default;
			Translate = default;
			TransformOrigin = default;
			Cursor = default;
			Background = default;
			Font = default;
			FontDefinition = default;
			TextShadow = default;
			Align = default;
			DisplayStyle = default;
			FlexDirection = default;
			Wrap = default;
			Justify = default;
			Overflow = default;
			Position = default;
			TextOverflow = default;
			FontStyle = default;
			OverflowClipBox = default;
			TextAnchor = default;
			TextOverflowPosition = default;
			Visibility = default;
			WhiteSpace = default;
			ScaleMode = default;
			TimeValue = default;
			PropertyName = default;
			EasingFunction = default;
		}
		
		[FieldOffset(0)] public StyleType Type;
		
		[FieldOffset(1)] public StyleLength Length;
		[FieldOffset(1)] public StyleColor Color;
		[FieldOffset(1)] public StyleFloat Float;
		[FieldOffset(1)] public StyleInt Int;
		[FieldOffset(1)] public StyleRotate Rotate;
		[FieldOffset(1)] public StyleScale Scale;
		[FieldOffset(1)] public StyleTranslate Translate;
		[FieldOffset(1)] public StyleTransformOrigin TransformOrigin;
		[FieldOffset(8)] public StyleCursor Cursor;
		[FieldOffset(8)] public StyleBackground Background;
		[FieldOffset(8)] public StyleFont Font;
		[FieldOffset(8)] public StyleFontDefinition FontDefinition;
		[FieldOffset(1)] public StyleTextShadow TextShadow;
		[FieldOffset(1)] public StyleEnum<Align> Align;
		[FieldOffset(1)] public StyleEnum<DisplayStyle> DisplayStyle;
		[FieldOffset(1)] public StyleEnum<FlexDirection> FlexDirection;
		[FieldOffset(1)] public StyleEnum<Wrap> Wrap;
		[FieldOffset(1)] public StyleEnum<Justify> Justify;
		[FieldOffset(1)] public StyleEnum<Overflow> Overflow;
		[FieldOffset(1)] public StyleEnum<Position> Position;
		[FieldOffset(1)] public StyleEnum<TextOverflow> TextOverflow;
		[FieldOffset(1)] public StyleEnum<FontStyle> FontStyle;
		[FieldOffset(1)] public StyleEnum<OverflowClipBox> OverflowClipBox;
		[FieldOffset(1)] public StyleEnum<TextAnchor> TextAnchor;
		[FieldOffset(1)] public StyleEnum<TextOverflowPosition> TextOverflowPosition;
		[FieldOffset(1)] public StyleEnum<Visibility> Visibility;
		[FieldOffset(1)] public StyleEnum<WhiteSpace> WhiteSpace;
		[FieldOffset(1)] public StyleEnum<ScaleMode> ScaleMode;
		[FieldOffset(8)] public StyleList<TimeValue> TimeValue;
		[FieldOffset(8)] public StyleList<StylePropertyName> PropertyName;
		[FieldOffset(8)] public StyleList<EasingFunction> EasingFunction;
	}

	public enum StyleType : byte {
		AlignContent,
		AlignItems,
		AlignSelf,
		BackgroundColor,
		BackgroundImage,
		BackgroundPositionX,
		BackgroundPositionY,
		BackgroundRepeat,
		BackgroundSize,
		BorderBottomColor,
		BorderBottomLeftRadius,
		BorderBottomRightRadius,
		BorderBottomWidth,
		BorderLeftColor,
		BorderLeftWidth,
		BorderRightColor,
		BorderRightWidth,
		BorderTopColor,
		BorderTopLeftRadius,
		BorderTopRightRadius,
		BorderTopWidth,
		Bottom,
		Color,
		Cursor,
		Display,
		FlexBasis,
		FlexDirection,
		FlexGrow,
		FlexShrink,
		FlexWrap,
		FontSize,
		Height,
		JustifyContent,
		Left,
		LetterSpacing,
		MarginBottom,
		MarginLeft,
		MarginRight,
		MarginTop,
		MaxHeight,
		MaxWidth,
		MinHeight,
		MinWidth,
		Opacity,
		Overflow,
		PaddingBottom,
		PaddingLeft,
		PaddingRight,
		PaddingTop,
		Position,
		Right,
		Rotate,
		Scale,
		TextOverflow,
		TextShadow,
		Top,
		TransformOrigin,
		TransitionDelay,
		TransitionDuration,
		TransitionProperty,
		TransitionTimingFunction,
		Translate,
		UnityBackgroundImageTintColor,
		UnityBackgroundScaleMode,
		UnityEditorTextRenderingMode,
		UnityFont,
		UnityFontDefinition,
		UnityFontStyleAndWeight,
		UnityOverflowClipBox,
		UnityParagraphSpacing,
		UnitySliceBottom,
		UnitySliceLeft,
		UnitySliceRight,
		UnitySliceScale,
		UnitySliceTop,
		UnityTextAlign,
		UnityTextGenerator,
		UnityTextOutlineColor,
		UnityTextOutlineWidth,
		UnityTextOverflowPosition,
		Visibility,
		WhiteSpace,
		Width,
		WordSpacing,
	}
}