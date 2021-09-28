using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Spelling
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-28 16:25:53
 */
namespace CookPopularControl.Controls.Menus
{
    public static class Spelling
    {
        public static ResourceKey SuggestionMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(Spelling), ResourceKeyId.SpellingSuggestionMenuItemStyle);
        public static ResourceKey IgnoreAllMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(Spelling), ResourceKeyId.SpellingIgnoreAllMenuItemStyle);
        public static ResourceKey NoSuggestionsMenuItemStyleKey { get; } = new ComponentResourceKey(typeof(Spelling), ResourceKeyId.SpellingNoSuggestionsMenuItemStyle);
        public static ResourceKey SeparatorStyleKey { get; } = new ComponentResourceKey(typeof(Spelling), ResourceKeyId.SpellingSeparatorStyle);
    }

    internal enum ResourceKeyId
    {
        SpellingSuggestionMenuItemStyle,
        SpellingIgnoreAllMenuItemStyle,
        SpellingNoSuggestionsMenuItemStyle,
        SpellingSeparatorStyle,
    }
}
