using CookPopularControl.Communal.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Xml.Linq;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ThemeProvider
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-13 16:21:47
 */
namespace CookPopularControl.Themes
{
    public class ThemeProvider
    {
        private static ResourceDictionary _lastInsertResourceDictionary;
        private const string Pattern = @"^themes\/cookcolors\/(?<colorname>[a-z]+)color\.baml$";

        public Dictionary<string, ResourceDictionary> Themes { get; }
        public static event ThemeEventHandler<ResourceDictionary> ThemeChanged;

        public ThemeProvider() : this(Assembly.GetExecutingAssembly())
        {

        }

        public ThemeProvider(Assembly assembly)
        {
            Themes = new Dictionary<string, ResourceDictionary>();
            GetThemes(assembly);
        }

        private void GetThemes(Assembly assembly, string pattern = Pattern)
        {
            var resourcesName = assembly.GetName().Name + ".g";
            var manager = new ResourceManager(resourcesName, assembly);
            var resourceSet = manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            var dictionaryEntries = resourceSet.OfType<DictionaryEntry>().ToList();
            string? assemblyName = assembly.GetName().Name;

            var regex = new Regex(pattern);
            dictionaryEntries.Select(x => new { key = x.Key.ToString(), match = regex.Match(x.Key.ToString()) })
                             .Where(x => x.match.Success && x.match.Groups["colorname"].Value != "base")
                             .ToList()
                             .ForEach(x => Read(assemblyName, x.match.Groups["colorname"].Value, x.match.Value));
        }

        private void Read(string? assemblyName, string? key, string? path)
        {
            if (assemblyName is null || key is null || path is null)
                return;

            var resourceDictionary = (ResourceDictionary)Application.LoadComponent(new Uri($"/{assemblyName};component/{path.Replace(".baml", ".xaml")}", UriKind.RelativeOrAbsolute));
            Themes.Add(key, resourceDictionary);
        }

        public void AddThemes(string pattern, params Assembly[] assemblys)
        {
            Assembly assembly;
            if (assemblys == null)
            {
                assembly = Assembly.GetEntryAssembly();
                GetThemes(assembly, pattern);
            }
            else
            {
                foreach (var asm in assemblys)
                {
                    GetThemes(asm, pattern);
                }
            }
        }

        public void SetAppTheme(string key, int replaceResourceDictionaryIndex = 1)
        {
            if (Themes == null || Themes.Count <= 0)
                return;

            if (Themes.ContainsKey(key.ToLower()))
            {
                if (_lastInsertResourceDictionary != null)
                    Application.Current.Resources.MergedDictionaries.Remove(_lastInsertResourceDictionary);
                Application.Current.Resources.MergedDictionaries.Insert(replaceResourceDictionaryIndex, Themes[key.ToLower()]);
                _lastInsertResourceDictionary = Themes[key.ToLower()];

                ThemeChanged?.Invoke(new Control(), new ThemeChangedArg<ResourceDictionary>(Themes[key.ToLower()]));
            }
        }


        private void ReadXaml(Stream stream)
        {
            Baml2006Reader read = new Baml2006Reader(stream);
            var s = read.Value is Window;
        }
    }
}
