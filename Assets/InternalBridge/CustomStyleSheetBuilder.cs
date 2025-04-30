using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.StyleSheets;
using Cursor = UnityEngine.UIElements.Cursor;

namespace UI.Bridge {
	public class CustomStyleSheetBuilder {
		private static readonly PropertyInfo[] StyleProperties = typeof(IStyle).GetProperties();
		private readonly StyleSheetBuilder _builder = new();

		public void Add(string selector, IStyle style) {
			_builder.BeginRule(0);

			List<(StyleSelectorPart[], StyleSelectorRelationship)> parts = ParseSelector(selector);
			using (_builder.BeginComplexSelector(CSSSpec.GetSelectorSpecificity(selector))) {
				foreach ((StyleSelectorPart[], StyleSelectorRelationship) part in parts) {
					_builder.AddSimpleSelector(part.Item1, part.Item2);
				}
			}

			foreach (PropertyInfo prop in StyleProperties) {
				object value = prop.GetValue(style);
				object keyword = value.GetType().GetProperty("keyword")?.GetValue(value);
				if ((StyleKeyword)keyword! != StyleKeyword.Undefined) continue;
				
				_builder.BeginProperty(PropertyNameMap[prop.Name]);
				ProcessStyleProperty(value);
				_builder.EndProperty();
			}
			
			_builder.EndRule();
		}
		
		public void BuildTo(StyleSheet sheet) => _builder.BuildTo(sheet);

		private void ProcessStyleProperty(object styleValue) {
			switch (styleValue.GetType().GetProperty("value")?.GetValue(styleValue)) {
				case Color color:
					_builder.AddValue(color);
					break;
				case Length length:
					_builder.AddValue(new Dimension(length.value, ConvertUnit(length.unit)));
					break;
				case float f:
					_builder.AddValue(f);
					break;
				case int i:
					_builder.AddValue(i);
					break;
				case Rotate rotate:
					_builder.AddValue(new Dimension(rotate.angle.value, ConvertUnit(rotate.angle.unit)));
					break;
				case Scale scale:
					_builder.AddValue(scale.value.x);
					_builder.AddValue(scale.value.y);
					_builder.AddValue(scale.value.z);
					break;
				case Translate translate:
					_builder.AddValue(new Dimension(translate.x.value, ConvertUnit(translate.x.unit)));
					_builder.AddValue(new Dimension(translate.y.value, ConvertUnit(translate.y.unit)));
					_builder.AddValue(translate.z);
					break;
				case TransformOrigin transformOrigin:
					_builder.AddValue(new Dimension(transformOrigin.x.value, ConvertUnit(transformOrigin.x.unit)));
					_builder.AddValue(new Dimension(transformOrigin.y.value, ConvertUnit(transformOrigin.y.unit)));
					_builder.AddValue(transformOrigin.z);
					break;
				case Background background:
					if (background.vectorImage != null) _builder.AddValue(background.vectorImage);
					if (background.sprite != null) _builder.AddValue(background.sprite);
					if (background.renderTexture != null) _builder.AddValue(background.renderTexture);
					if (background.texture != null) _builder.AddValue(background.texture);
					break;
				case TextShadow textShadow:
					_builder.AddValue(new Dimension(textShadow.offset.x, Dimension.Unit.Pixel));
					_builder.AddValue(new Dimension(textShadow.offset.y, Dimension.Unit.Pixel));
					_builder.AddValue(textShadow.blurRadius);
					_builder.AddValue(textShadow.color);
					break;
				case Cursor cursor:
					if (cursor.texture != null) _builder.AddValue(cursor.texture);
					_builder.AddValue(new Dimension(cursor.hotspot.x, Dimension.Unit.Pixel));
					_builder.AddValue(new Dimension(cursor.hotspot.y, Dimension.Unit.Pixel));
					break;
				case Font font:
					_builder.AddValue(font);
					break;
				case Enum e:
					_builder.AddValue(ToKebabCase(e.ToString()), StyleValueType.Enum);
					break;
				case IEnumerable enumerable:
					
					break;
				default:
					Debug.LogWarning($"Unsupported style type: {styleValue.GetType()}");
					break;
			}
		}

		private List<(StyleSelectorPart[], StyleSelectorRelationship)> ParseSelector(string selector) {
			List<(StyleSelectorPart[], StyleSelectorRelationship)> parts = new();
			List<StyleSelectorPart> currentParts = new();
			StyleSelectorRelationship currentRelationship = StyleSelectorRelationship.None;
			bool currentIsPseudo = false;
			
			int index = 0;
			while (index < selector.Length) {
				char currentChar = selector[index];
				
				if (currentChar == '>') {
					if (currentParts.Count > 0) {
						parts.Add((currentParts.ToArray(), currentRelationship));
						currentParts.Clear();
					}
					currentRelationship = StyleSelectorRelationship.Child;
					index++;
					continue;
				}
				
				if (char.IsWhiteSpace(currentChar)) {
					if (currentParts.Count > 0) {
						parts.Add((currentParts.ToArray(), currentRelationship));
						currentParts.Clear();
					}
					currentRelationship = StyleSelectorRelationship.Descendent;
					index++;
					continue;
				}
				
				if (selector[index] == ':') {
					currentIsPseudo = true;
					index++;
					continue;
				}
				
				int start = index;
				while (index < selector.Length && selector[index] != ':' && selector[index] != '>' && !char.IsWhiteSpace(selector[index])) {
					index++;
				}

				string token = selector[start..index];
				if (string.IsNullOrEmpty(token)) continue;
				
				if (currentIsPseudo) {
					currentParts.Add(StyleSelectorPart.CreatePseudoClass(token));
					currentIsPseudo = false;
				} else {
					if (token.StartsWith(".")) {
						currentParts.Add(StyleSelectorPart.CreateClass(token[1..]));
					} else if (token.StartsWith("#")) {
						currentParts.Add(StyleSelectorPart.CreateId(token[1..]));
					} else {
						currentParts.Add(StyleSelectorPart.CreateType(token));
					}
				}
			}
			
			if (currentParts.Count > 0) parts.Add((currentParts.ToArray(), currentRelationship));
			return parts;
		}
		
		private Dimension.Unit ConvertUnit(LengthUnit unit) => (Dimension.Unit)(unit + 1);
		private Dimension.Unit ConvertUnit(TimeUnit unit) => (Dimension.Unit)(unit + 3);
		private Dimension.Unit ConvertUnit(AngleUnit unit) => (Dimension.Unit)(unit + 5);
		private static string ToKebabCase(string input) => Regex.Replace(input, "(?<!^)([A-Z])", "-$1").ToLower();
		
		private static readonly Dictionary<string, string> PropertyNameMap = new() {
			["alignContent"] = "align-content",
			["alignItems"] = "align-items",
			["alignSelf"] = "align-self",
			["bottom"] = "bottom",
			["display"] = "display",
			["flex"] = "flex",
			["flexBasis"] = "flex-basis",
			["flexDirection"] = "flex-direction",
			["flexGrow"] = "flex-grow",
			["flexShrink"] = "flex-shrink",
			["flexWrap"] = "flex-wrap",
			["height"] = "height",
			["justifyContent"] = "justify-content",
			["left"] = "left",
			["margin"] = "margin",
			["marginBottom"] = "margin-bottom",
			["marginLeft"] = "margin-left",
			["marginRight"] = "margin-right",
			["marginTop"] = "margin-top",
			["maxHeight"] = "max-height",
			["maxWidth"] = "max-width",
			["minHeight"] = "min-height",
			["minWidth"] = "min-width",
			["overflow"] = "overflow",
			["padding"] = "padding",
			["paddingBottom"] = "padding-bottom",
			["paddingLeft"] = "padding-left",
			["paddingRight"] = "padding-right",
			["paddingTop"] = "padding-top",
			["position"] = "position",
			["right"] = "right",
			["top"] = "top",
			["width"] = "width",
			["backgroundColor"] = "background-color",
			["backgroundImage"] = "background-image",
			["borderBottomColor"] = "border-bottom-color",
			["borderBottomLeftRadius"] = "border-bottom-left-radius",
			["borderBottomRightRadius"] = "border-bottom-right-radius",
			["borderBottomWidth"] = "border-bottom-width",
			["borderColor"] = "border-color",
			["borderLeftColor"] = "border-left-color",
			["borderLeftWidth"] = "border-left-width",
			["borderRadius"] = "border-radius",
			["borderRightColor"] = "border-right-color",
			["borderRightWidth"] = "border-right-width",
			["borderTopColor"] = "border-top-color",
			["borderTopLeftRadius"] = "border-top-left-radius",
			["borderTopRightRadius"] = "border-top-right-radius",
			["borderTopWidth"] = "border-top-width",
			["borderWidth"] = "border-width",
			["color"] = "color",
			["opacity"] = "opacity",
			["visibility"] = "visibility",
			["fontSize"] = "font-size",
			["letterSpacing"] = "letter-spacing",
			["whiteSpace"] = "white-space",
			["wordSpacing"] = "word-spacing",
			["unitySliceLeft"] = "-unity-slice-left",
			["unitySliceRight"] = "-unity-slice-right",
			["unitySliceTop"] = "-unity-slice-top",
			["unitySliceBottom"] = "-unity-slice-bottom",
			["unityBackgroundScaleMode"] = "-unity-background-scale-mode",
			["unityBackgroundImageTintColor"] = "-unity-background-image-tint-color",
			["unityFont"] = "-unity-font",
			["unityFontStyleAndWeight"] = "-unity-font-style-and-weight",
			["unityParagraphSpacing"] = "-unity-paragraph-spacing",
			["unityTextAlign"] = "-unity-text-align",
			["unityTextOutlineColor"] = "-unity-text-outline-color",
			["unityTextOutlineWidth"] = "-unity-text-outline-width",
			["rotate"] = "rotate",
			["scale"] = "scale",
			["transformOrigin"] = "transform-origin",
			["translate"] = "translate",
			["transition"] = "transition",
			["transitionDelay"] = "transition-delay",
			["transitionDuration"] = "transition-duration",
			["transitionProperty"] = "transition-property",
			["transitionTimingFunction"] = "transition-timing-function",
			["cursor"] = "cursor",
			["textOverflow"] = "text-overflow",
			["textShadow"] = "text-shadow"
		};
	}
}