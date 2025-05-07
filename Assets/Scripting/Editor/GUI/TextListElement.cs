using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Koneko.UIBuilder.InternalBridge;
using UnityEngine;
using UnityEngine.UIElements;

namespace WasmScripting
{
    [PublicAPI]
    public class TextListElement<TItem> : VisualElement
    {
        private readonly Func<TItem, int, Color> _colorEvaluator;
        private readonly Action<TItem, int> _onSelect;
        private List<TItem> _list = new();
        private bool _reversedSelection;
        private int _selectedIndex0;
        private int _selectedIndex1;
        private float _targetScroll;
        private float _currentScroll;
        public int FontSize = 12;
        public float LineHeight = 18f;
        public float ScrollSpeed = 15f;
        public float ScrollSmoothing = 20f;
        public bool MultiSelect;

        public List<TItem> List => _list;

        public IEnumerable<TItem> SelectedItems
        {
            get
            {
                if (_reversedSelection && _selectedIndex0 != -1)
                    return _list.GetRange(_selectedIndex1, _selectedIndex0 - _selectedIndex1 + 1);
                if (_selectedIndex1 != -1)
                    return _list.GetRange(_selectedIndex0, _selectedIndex1 - _selectedIndex0 + 1);
                return _list.GetRange(_selectedIndex0, 1);
            }
        }

        public bool HasSelection => _selectedIndex0 != -1 || _selectedIndex1 != -1;

        public TextListElement(Func<TItem, int, Color> colorEvaluator, Action<TItem, int> onSelect)
        {
            _colorEvaluator = colorEvaluator;
            _onSelect = onSelect;
            style.flexGrow = 1;
            style.overflow = Overflow.Hidden;
            RegisterCallback<ClickEvent>(OnClick);
            RegisterCallback<WheelEvent>(OnScroll);
            generateVisualContent += OnGenerateVisualContent;
            schedule.Execute(Update).Every(10);
        }

        public void SetList(List<TItem> list)
        {
            _list = list;
            _targetScroll = 0;
            _currentScroll = 0;
            _selectedIndex0 = -1;
            _selectedIndex1 = -1;
            MarkDirtyRepaint();
        }

        private void OnClick(ClickEvent evt)
        {
            float startLine = Mathf.Max(0, (-_currentScroll + 2) / LineHeight);
            float selectedLine = evt.localPosition.y / LineHeight;
            int index = (int)(selectedLine + startLine);
            if (index < _list.Count)
            {
                if (MultiSelect && evt.shiftKey)
                {
                    _selectedIndex1 = index;
                    _reversedSelection = _selectedIndex1 < _selectedIndex0;
                }
                else
                {
                    _reversedSelection = false;
                    _selectedIndex0 = index;
                    _selectedIndex1 = -1;
                }
                _onSelect(_list[index], index);
            }
            else
            {
                if (MultiSelect && evt.shiftKey)
                {
                    _selectedIndex1 = _list.Count;
                }
                else
                {
                    _selectedIndex0 = -1;
                    _selectedIndex1 = -1;
                }
                _onSelect(default, -1);
            }
            MarkDirtyRepaint();
            evt.StopPropagation();
        }

        private void OnScroll(WheelEvent evt)
        {
            if (resolvedStyle.height > _list.Count * LineHeight)
            {
                _currentScroll = 0;
                _targetScroll = 0;
            }
            else
            {
                float maxScroll = _list.Count * LineHeight - resolvedStyle.height;
                _targetScroll = Mathf.Clamp(
                    _targetScroll - evt.delta.y * ScrollSpeed,
                    -maxScroll,
                    0
                );
            }
            MarkDirtyRepaint();
            evt.StopPropagation();
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            float height = resolvedStyle.height;
            int startLine = Mathf.Max(0, Mathf.FloorToInt(-_currentScroll / LineHeight) - 1);
            int endLine = Mathf.Min(_list.Count, startLine + Mathf.CeilToInt(height / LineHeight));

            for (int i = startLine; i <= endLine + 1; i++)
            {
                if (i > _list.Count - 1)
                    return;
                float y = i * LineHeight + _currentScroll;
                if (i == _selectedIndex0)
                {
                    ctx.DrawRect(
                        new Rect(0, y - 2, resolvedStyle.width, LineHeight),
                        new(0.35f, 0.35f, 0.35f)
                    );
                }
                else if (MultiSelect)
                {
                    if (_reversedSelection)
                    {
                        if (i >= _selectedIndex1 && i < _selectedIndex0)
                        {
                            ctx.DrawRect(
                                new Rect(0, y - 2, resolvedStyle.width, LineHeight),
                                new(0.35f, 0.35f, 0.35f)
                            );
                        }
                    }
                    else
                    {
                        if (i > _selectedIndex0 && i <= _selectedIndex1)
                        {
                            ctx.DrawRect(
                                new Rect(0, y - 2, resolvedStyle.width, LineHeight),
                                new(0.35f, 0.35f, 0.35f)
                            );
                        }
                    }
                }
                Color color = _colorEvaluator(_list[i], i);
                ctx.DrawText(_list[i]?.ToString() ?? "", new Vector2(0, y), FontSize, color);
            }
        }

        private void Update()
        {
            if (Math.Abs(_currentScroll - _targetScroll) < 0.01f)
                return;
            _currentScroll = Mathf.Lerp(_currentScroll, _targetScroll, ScrollSmoothing * 0.01f);
            MarkDirtyRepaint();
        }
    }
}
