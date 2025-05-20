using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder {
	[PublicAPI]
	public static partial class UIElementBuilder {
		public static VisualElement Element(params UIOperation[] operations) {
			VisualElement element = new();
			ApplyOperation(element, operations);
			return element;
		}

		public static VisualElement Element(out VisualElement element, params UIOperation[] operations) {
			element = new();
			ApplyOperation(element, operations);
			return element;
		}

		public static T Element<T>(params UIOperation[] operations)
			where T : VisualElement, new() {
			T element = new();
			ApplyOperation(element, operations);
			return element;
		}

		public static T Element<T>(out T element, params UIOperation[] operations)
			where T : VisualElement, new() {
			element = new();
			ApplyOperation(element, operations);
			return element;
		}

		public static SetName Name(string name) => new() { Name = name };

		public static SetEnabled Enabled(bool enabled) => new() { Enabled = enabled };

		public static SetFocusable Focusable(bool focusable) => new() { Focusable = focusable };

		public static AddClass AddClass(string @class) => new() { Class = @class };

		public static Subscribe Subscribe<TEvent>(EventCallback<TEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown)
			where TEvent : EventBase<TEvent>, new() => new() { Callback = new UICallback<TEvent>(callback, trickleDown) };

		private static void ApplyOperation(VisualElement element, UIOperation[] operations) {
			foreach (UIOperation operation in operations) {
				switch (operation.Type) {
					case OperationType.Element:
						element.Add((VisualElement)operation.Value);
						break;
					case OperationType.AddClass:
						element.AddToClassList((string)operation.Value);
						break;
					case OperationType.Name:
						element.name = (string)operation.Value;
						break;
					case OperationType.Enabled:
						element.SetEnabled((bool)operation.Value);
						break;
					case OperationType.Focusable:
						element.focusable = (bool)operation.Value;
						break;
					case OperationType.Subscribe:
						((Subscribe)operation.Value).Callback.Register(element);
						break;
					case OperationType.Style:
						ApplyStyle(element, (StyleStruct)operation.Value);
						break;
				}
			}
		}
	}
}