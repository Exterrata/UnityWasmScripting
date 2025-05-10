using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Koneko.UIBuilder;
using UnityEngine;
using UnityEngine.U2D.Common;
using UnityEngine.UIElements;

namespace WasmScripting {
	[PublicAPI]
	public class MultiToggle : VisualElement {
		private readonly List<TextElement> _elements = new();
		private readonly List<object> _items = new();
		private readonly List<bool> _toggled = new();
		private readonly List<bool> _mixed = new();
		public Action<object, int> OnToggle;
		public bool MultiSelect = true;

		public MultiToggle(List<object> items) {
			styleSheets.Add(StyleSheetBuilder.Build(b => b.Add(".multi-toggle TextElement", s => s.Height(new Length(100, LengthUnit.Percent)).FlexGrow(1).FlexBasis(0).UnityTextAlign(TextAnchor.MiddleCenter).MarginLeft(2).BorderTopLeftRadius(4).BorderTopRightRadius(4).BorderBottomLeftRadius(4).BorderBottomRightRadius(4).BackgroundColor(new(0.345f, 0.345f, 0.345f))).Add(".multi-toggle TextElement:hover", s => s.BackgroundColor(new(0.4f, 0.4f, 0.4f))).Add(".multi-toggle TextElement:checked", s => s.BackgroundColor(new(0.2f, 0.35f, 0.5f)))));

			AddToClassList("multi-toggle");
			style.alignItems = Align.Center;
			style.flexDirection = FlexDirection.Row;
			style.flexGrow = 1;
			style.paddingTop = 2;
			style.paddingRight = 2;
			style.paddingBottom = 2;
			style.borderTopLeftRadius = 5;
			style.borderTopRightRadius = 5;
			style.borderBottomLeftRadius = 5;
			style.borderBottomRightRadius = 5;
			style.backgroundColor = new Color(0.19f, 0.19f, 0.19f);

			SetItems(items);
		}

		public void SetItems(List<object> items) {
			Clear();
			_items.Clear();
			_mixed.Clear();
			_toggled.Clear();
			_elements.Clear();
			for (var i = 0; i < items.Count; i++) {
				_items.Add(items[i]);
				_mixed.Add(false);
				_toggled.Add(false);
				TextElement element = new TextElement { text = items[i].ToString() };
				int i1 = i;
				element.RegisterCallback<ClickEvent>(e => OnClick(e, i1));
				Add(element);
				_elements.Add(element);
			}
		}

		public void SetMixed(int index, bool mixed) {
			if (mixed)
				_elements[index].text = _items[index].ToString() + '*';
			else
				_elements[index].text = _items[index].ToString();
		}

		public void SetValue(int index, bool value) {
			if (!MultiSelect) {
				for (int i = 0; i < _toggled.Count; i++) {
					if (_toggled[i]) {
						_toggled[i] = false;
						_elements[i].SetChecked(false);
					}
				}
			}

			_elements[index].text = _items[index].ToString();
			_mixed[index] = false;

			_toggled[index] = value;
			_elements[index].SetChecked(value);
			MarkDirtyRepaint();
		}

		public bool GetValue(int index) => _toggled[index];

		private void OnClick(ClickEvent evt, int index) {
			if (!MultiSelect) {
				for (int i = 0; i < _toggled.Count; i++) {
					if (_toggled[i]) {
						_toggled[i] = false;
						_elements[i].SetChecked(false);
						OnToggle(_items[i], i);
					}
				}
			}

			_elements[index].text = _items[index].ToString();
			_mixed[index] = false;

			_toggled[index] = !_toggled[index];
			OnToggle(_items[index], index);
			((VisualElement)evt.currentTarget).SetChecked(_toggled[index]);
		}
	}
}