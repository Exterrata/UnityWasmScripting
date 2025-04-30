using System;
using System.Collections.Generic;
using System.Linq;
using static UI.UIElementBuilder;
using UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.U2D.Common;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;
using FlexDirection = UnityEngine.UIElements.FlexDirection;

namespace WasmScripting {
    public partial class WasmBindingGenerator : EditorWindow {
        private readonly List<VisualElement> _bindingSetTabList = new();
        private ToolbarSearchField _memberListSearchElement;
        private TextListElement<Type> _typeListElement;
        private TextListElement<string> _memberListElement;
        private TextField _bindingOverrideIdField;
        private MultiToggle _bindingSetToggle;
        private MultiToggle _bindingEnabledToggle;
        private MultiToggle _ownerContextToggle;
        private MultiToggle _scopeContextToggle;
        private Label _typeTitleElement;

        private static readonly Color Background0 = new(0.25f, 0.25f, 0.25f);
        private static readonly Color Background1 = new(0.22f, 0.22f, 0.22f);
        private static readonly Color Background2 = new(0.2f, 0.2f, 0.2f);
        private static readonly Color WhitelistedColor = new(0.4f, 1.0f, 0.5f);
        private static readonly Color PartialWhitelistColor = new(0.9f, 0.9f, 0.4f);
        private static readonly Color BlacklistedColor = new(1.0f, 0.4f, 0.5f);
        
        private bool ValidSelection => _selectedType != null && _memberListElement.HasSelection;

        public void CreateGUI() {
            BuildUI();
            _typeListElement.SetList(Types);
            _shownTypes.AddRange(Types);
            SetActiveBindingSet(new ClickEvent { target = _bindingSetTabList[0] });
        }

        private void SetActiveBindingSet(ClickEvent evt) {
            foreach (VisualElement element in _bindingSetTabList) element.SetChecked(false);
            TextElement target = (TextElement)evt.target;
            target.SetChecked(true);
            
            _activeSets.Clear();
            if (target.text == "All") {
                _activeSets.AddRange(AllSets);
            } else _activeSets.Add(target.text);
            
            UpdateMemberSettings();
        }

        private void SetOverrideId(ChangeEvent<string> value) {
            if (!ValidSelection) {
                _bindingOverrideIdField.value = "";
                return;
            }
            
            ScriptingWhitelist.ForEach(_activeSets, _selectedType, _memberListElement.SelectedItems, m => {
                m.OverrideName = value.newValue;
            });

            hasUnsavedChanges = true;
        }

        private void ToggleSelectedMember(object context, int index) {
            if (!ValidSelection) {
                _bindingEnabledToggle.SetValue(index, false);
                return;
            }

            bool value = _bindingEnabledToggle.GetValue(index);
            _bindingEnabledToggle.SetItems(new() { value ? "Enabled" : "Disabled" });
            _bindingEnabledToggle.SetValue(index, value);

            ScriptingWhitelist.ForEach(_activeSets, _selectedType, _memberListElement.SelectedItems, m => {
                m.IsBound = value;
            });
            
            _memberListElement.MarkDirtyRepaint();
            _typeListElement.MarkDirtyRepaint();
            hasUnsavedChanges = true;
        }

        private void SetOwnerContext(object context, int index) {
            if (!ValidSelection) {
                _ownerContextToggle.SetValue(index, false);
                return;
            }

            bool value = _ownerContextToggle.GetValue(index);
            ScriptingWhitelist.ForEach(_activeSets, _selectedType, _memberListElement.SelectedItems, m => {
                if (value) {
                    m.OwnerContext |= (OwnerContext)(1 << index);
                } else {
                    m.OwnerContext &= (OwnerContext)~(1 << index);
                }
            });

            hasUnsavedChanges = true;
        }

        private void SetScopeContext(object context, int index) {
            if (!ValidSelection) {
                _scopeContextToggle.SetValue(index, false);
                return;
            }
            
            bool value = _scopeContextToggle.GetValue(index);
            ScriptingWhitelist.ForEach(_activeSets, _selectedType, _memberListElement.SelectedItems, m => {
                if (value) {
                    m.ScopeContext |= (ScopeContext)(1 << index);
                } else {
                    m.ScopeContext &= (ScopeContext)~(1 << index);
                }
            });

            hasUnsavedChanges = true;
        }

        private void SelectTypeListItem(Type type, int index) {
            _selectedType = type;
            
            _members.Clear();
            _shownMembers.Clear();
            if (_selectedType != null) {
                _members.AddRange(ScriptingWhitelist.Members("Avatar", type).Select(m => m.Name));
                _shownMembers.AddRange(_members);
            }
            
            _typeTitleElement.text = type?.FullName;
            _memberListElement.SetList(_shownMembers);
            _memberListSearchElement.SetValueWithoutNotify("");
            _bindingOverrideIdField.value = "";
            _bindingEnabledToggle.SetItems(new() { "Disabled" });
            _ownerContextToggle.SetValue(0, false);
            _ownerContextToggle.SetValue(1, false);
            _scopeContextToggle.SetValue(0, false);
            _scopeContextToggle.SetValue(1, false);
            _scopeContextToggle.SetValue(2, false);
        }

        private void SelectMemberListItem(string item, int index) {
            if (item == null) {
                _bindingOverrideIdField.value = "";
                _bindingEnabledToggle.SetItems(new() { "Disabled" });
                _ownerContextToggle.SetValue(0, false);
                _ownerContextToggle.SetValue(1, false);
                _scopeContextToggle.SetValue(0, false);
                _scopeContextToggle.SetValue(1, false);
                _scopeContextToggle.SetValue(2, false);
                return;
            }
            
            UpdateMemberSettings();
        }

        private void UpdateMemberSettings() {
            if (ValidSelection) {
                bool anyEnabled = false;
                bool anyDisabled = false;
                OwnerContext ownerOr = OwnerContext.None;
                OwnerContext ownerAnd = OwnerContext.All;
                ScopeContext scopeOr = ScopeContext.None;
                ScopeContext scopeAnd = ScopeContext.All;
                BoundMember selected = null;
                ScriptingWhitelist.ForEach(_activeSets, _selectedType, _memberListElement.SelectedItems, m => {
                    if (m.IsBound) anyEnabled = true;
                    else anyDisabled = true;
                    ownerOr  |= m.OwnerContext;
                    ownerAnd &= m.OwnerContext;
                    scopeOr  |= m.ScopeContext;
                    scopeAnd &= m.ScopeContext;
                    selected ??= m;
                });
                
                bool mixedEnabled = anyEnabled && anyDisabled;
                OwnerContext mixedOwner = ownerOr ^ ownerAnd;
                ScopeContext mixedScope = scopeOr ^ scopeAnd;
                
                _bindingOverrideIdField.SetValueWithoutNotify(selected.OverrideName);
                _bindingEnabledToggle.SetItems(new() { selected.IsBound ? "Enabled" : "Disabled" });
                _bindingEnabledToggle.SetValue(0, selected.IsBound);
                _bindingEnabledToggle.SetMixed(0, mixedEnabled);
                
                _ownerContextToggle.SetValue(0, selected.OwnerContext.HasFlag(OwnerContext.Self));
                _ownerContextToggle.SetValue(1, selected.OwnerContext.HasFlag(OwnerContext.Other));
                _ownerContextToggle.SetMixed(0, mixedOwner.HasFlag(OwnerContext.Self));
                _ownerContextToggle.SetMixed(1, mixedOwner.HasFlag(OwnerContext.Other));
                
                _scopeContextToggle.SetValue(0, selected.ScopeContext.HasFlag(ScopeContext.Self));
                _scopeContextToggle.SetValue(1, selected.ScopeContext.HasFlag(ScopeContext.ExternalContent));
                _scopeContextToggle.SetValue(2, selected.ScopeContext.HasFlag(ScopeContext.GameInternal));
                _scopeContextToggle.SetMixed(0, mixedScope.HasFlag(ScopeContext.Self));
                _scopeContextToggle.SetMixed(1, mixedScope.HasFlag(ScopeContext.ExternalContent));
                _scopeContextToggle.SetMixed(2, mixedScope.HasFlag(ScopeContext.GameInternal));
            }
        }

        private Color SetTypeItemColor(Type item, int index) {
            bool anyEnabled = false;
            bool anyDisabled = false;
            IEnumerable<string> members = ScriptingWhitelist.Members("Avatar", item).Select(m => m.Name);
            ScriptingWhitelist.ForEach(AllSets, item, members, m => {
                if (m.IsBound) anyEnabled = true;
                else anyDisabled = true;
            });

            if (anyEnabled && anyDisabled) return PartialWhitelistColor;
            return anyEnabled ? WhitelistedColor : BlacklistedColor;
        }
        
        private Color SetMemberItemColor(string item, int index) {
            bool anyEnabled = false;
            bool anyDisabled = false;
            ScriptingWhitelist.ForEach(AllSets, _selectedType, new List<string> { item }, m => {
                if (m.IsBound) anyEnabled = true;
                else anyDisabled = true;
            });

            if (anyEnabled && anyDisabled) return PartialWhitelistColor;
            return anyEnabled ? WhitelistedColor : BlacklistedColor;
        }

        private void SearchTypeList(ChangeEvent<string> value) {
            SearchList(Types, _shownTypes, value.newValue);
            _typeListElement.SetList(_shownTypes);
        }

        private void SearchMemberList(ChangeEvent<string> value) {
            SearchList(_members, _shownMembers, value.newValue);
            _memberListElement.SetList(_shownMembers);
        }

        private void BuildUI() {
            rootVisualElement.styleSheets.Add(StyleSheetBuilder.Build(b => b
                .Add(".tab-list TextElement:checked", s => s
                    .BorderTopLeftRadius(3)
                    .BorderTopRightRadius(3)
                    .BackgroundColor(new(0.345f, 0.345f, 0.345f))
                )
                .Add(".section-box", s => s
                    .Margin(2, 4)
                    .BorderTopLeftRadius(5)
                    .BorderTopRightRadius(5)
                    .BorderBottomLeftRadius(5)
                    .BorderBottomRightRadius(5)
                    .BackgroundColor(new(0.19f, 0.19f, 0.19f))
                )
                .Add(".unity-base-field__label", s => s
                    .MinWidth(100)
                    .FlexShrink(1)
                )
            ));
            
            rootVisualElement.Add(Element(
                FlexDirection(FlexDirection.Row),
                FlexGrow(1),
                Element( // left side type list
                    Width(new Length(25, LengthUnit.Percent)),
                    BackgroundColor(Background1),
                    Element( // type list header
                        FlexDirection(FlexDirection.Row),
                        PaddingTop(2),
                        PaddingLeft(3),
                        PaddingRight(3),
                        PaddingBottom(2),
                        BackgroundColor(Background0),
                        Element<ToolbarSearchField>(
                            Subscribe<ChangeEvent<string>>(SearchTypeList),
                            MarginLeft(0),
                            MarginRight(0),
                            FlexShrink(1),
                            Width(new Length(100, LengthUnit.Percent))
                        )
                    ),
                    Element( // type list
                        out VisualElement typeListElement,
                        FlexGrow(1),
                        PaddingLeft(2),
                        PaddingRight(2),
                        Element(
                            FlexGrow(1),
                            new TextListElement<Type>(SetTypeItemColor, SelectTypeListItem)
                        )
                    )
                ),
                Element( // center member list
                    FlexGrow(1),
                    BorderLeftWidth(1),
                    BorderRightWidth(1),
                    BorderLeftColor(Background0),
                    BorderRightColor(Background0),
                    BackgroundColor(Background2),
                    Element( // member list header
                        FlexDirection(FlexDirection.Row),
                        PaddingTop(2),
                        PaddingLeft(3),
                        PaddingRight(3),
                        PaddingBottom(2),
                        BackgroundColor(Background0),
                        Element(out _typeTitleElement),
                        Element(FlexGrow(1)),
                        Element(
                            out _memberListSearchElement,
                            Subscribe<ChangeEvent<string>>(SearchMemberList),
                            MarginLeft(0),
                            MarginRight(0),
                            FlexShrink(1),
                            Width(150)
                        )
                    ),
                    Element( // member list
                        out VisualElement memberListElement,
                        FlexGrow(1),
                        PaddingLeft(2),
                        PaddingRight(2),
                        Element(
                            FlexGrow(1),
                            new TextListElement<string>(SetMemberItemColor, SelectMemberListItem) { MultiSelect = true }
                        )
                    )
                ),
                Element( // right side binding settings
                    BackgroundColor(Background1),
                    Width(new Length(30, LengthUnit.Percent)),
                    Element(
                        out VisualElement bindingSetTabList,
                        AddClass("tab-list"),
                        FlexDirection(FlexDirection.Row),
                        BackgroundColor(Background0),
                        Height(24),
                        new TextElement { text = "All", style = { flexGrow = 1, flexBasis = 0, unityTextAlign = TextAnchor.MiddleCenter }},
                        new TextElement { text = "Avatar", style = { flexGrow = 1, flexBasis = 0, unityTextAlign = TextAnchor.MiddleCenter }},
                        new TextElement { text = "World", style = { flexGrow = 1, flexBasis = 0, unityTextAlign = TextAnchor.MiddleCenter }},
                        new TextElement { text = "Prop", style = { flexGrow = 1, flexBasis = 0, unityTextAlign = TextAnchor.MiddleCenter }}
                    ),
                    Element(
                        out VisualElement bindingOverrideIdField,
                        AddClass("section-box"),
                        MarginTop(4),
                        PaddingTop(2),
                        PaddingBottom(2),
                        new TextField("Override ID:") { style = {
                            flexGrow = 1
                        }}
                    ),
                    Element(
                        AddClass("section-box"),
                        FlexDirection(FlexDirection.Row),
                        new Label("Binding Enabled:") { style = { unityTextAlign = TextAnchor.MiddleCenter, marginLeft = 4, minWidth = 100 }},
                        Element(out VisualElement bindingEnabledToggle, Height(24), FlexGrow(1), new MultiToggle(new() { "Disabled" }) { OnToggle = ToggleSelectedMember })
                    ),
                    Element(
                        AddClass("section-box"),
                        FlexDirection(FlexDirection.Row),
                        new Label("Owner Context:") { style = { unityTextAlign = TextAnchor.MiddleCenter, marginLeft = 4, minWidth = 100 }},
                        Element(out VisualElement ownerContextToggle, Height(24), FlexGrow(1), new MultiToggle(new() { "Self", "Other" }) { OnToggle = SetOwnerContext })
                    ),
                    Element(
                        AddClass("section-box"),
                        FlexDirection(FlexDirection.Row),
                        new Label("Scope Context:") { style = { unityTextAlign = TextAnchor.MiddleCenter, marginLeft = 4, minWidth = 100 }},
                        Element(out VisualElement scopeContextToggle, Height(24), FlexGrow(1), new MultiToggle(new() { "Self", "External Content", "Game Internal" }) { OnToggle = SetScopeContext })
                    )
                )
            ));

            foreach (VisualElement element in bindingSetTabList.Children()) {
                element.RegisterCallback<ClickEvent>(SetActiveBindingSet);
                _bindingSetTabList.Add(element);
            }

            _bindingOverrideIdField = bindingOverrideIdField.Q<TextField>();
            _bindingEnabledToggle = bindingEnabledToggle.Q<MultiToggle>();
            _ownerContextToggle = ownerContextToggle.Q<MultiToggle>();
            _scopeContextToggle = scopeContextToggle.Q<MultiToggle>();
            _typeListElement = typeListElement.Q<TextListElement<Type>>();
            _memberListElement = memberListElement.Q<TextListElement<string>>();
            _bindingOverrideIdField.RegisterValueChangedCallback(SetOverrideId);
        }
    }
}