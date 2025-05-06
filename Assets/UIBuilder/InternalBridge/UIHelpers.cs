using System;
using UnityEngine.UIElements;

namespace Koneko.UIBuilder.InternalBridge {
	public static class UIHelpers {
		public static void SetPseudoState(this VisualElement element, PseudoStates pseudoStates) {
			element.pseudoStates = (UnityEngine.UIElements.PseudoStates)(int)pseudoStates;
		}
	}
	
	[Flags]
	public enum PseudoStates {
		None = 0,
		Active = 1,
		Hover = 2,
		Checked = 8,
		Disabled = 32,
		Focus = 64
	}
}