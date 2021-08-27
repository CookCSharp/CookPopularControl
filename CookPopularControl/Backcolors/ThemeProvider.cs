using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Baml2006;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ThemeProvider
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-13 16:21:47
 */
namespace CookPopularControl.Backcolors
{
    public class ThemeProvider
    {
        public ThemeProvider(Assembly assembly)
        {
            var resourcesName = assembly.GetName().Name + ".g";
            var manager = new ResourceManager(resourcesName, assembly);
            var resourceSet = manager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            var dictionaryEntries = resourceSet.OfType<DictionaryEntry>().ToList();
            string? assemblyName = assembly.GetName().Name;

            var regex = new Regex(@"^themes\/materialdesigncolor\.(?<name>[a-z]+)\.(?<type>primary|accent)\.baml$");

            //Swatches =
            //    dictionaryEntries
            //    .Select(x => new { key = x.Key.ToString(), match = regex.Match(x.Key.ToString()) })
            //    .Where(x => x.match.Success && x.match.Groups["name"].Value != "black")
            //    .GroupBy(x => x.match.Groups["name"].Value)
            //    .Select(x =>
            //    CreateSwatch
            //    (
            //        x.Key,
            //        Read(assemblyName, x.SingleOrDefault(y => y.match.Groups["type"].Value == "primary")?.key),
            //        Read(assemblyName, x.SingleOrDefault(y => y.match.Groups["type"].Value == "accent")?.key)
            //    ))
            //    .ToList();
        }

        /// <summary>
        /// Creates a new swatch provider based on standard Material Design colors.
        /// </summary>
        public ThemeProvider() : this(Assembly.GetExecutingAssembly())
        { }

        //public IEnumerable<Swatch> Swatches { get; }

        //private static Swatch CreateSwatch(string name, ResourceDictionary? primaryDictionary, ResourceDictionary? accentDictionary)
        //{
        //    return new Swatch(name, GetHues(primaryDictionary), GetHues(accentDictionary));

        //    static List<Hue> GetHues(ResourceDictionary? resourceDictionary)
        //    {
        //        var hues = new List<Hue>();
        //        if (resourceDictionary != null)
        //        {
        //            foreach (var entry in resourceDictionary.OfType<DictionaryEntry>()
        //                .OrderBy(de => de.Key)
        //                .Where(de => !(de.Key.ToString() ?? "").EndsWith("Foreground", StringComparison.Ordinal)))
        //            {

        //                hues.Add(GetHue(resourceDictionary, entry));
        //            }
        //        }
        //        return hues;
        //    }

        //    static Hue GetHue(ResourceDictionary dictionary, DictionaryEntry entry)
        //    {
        //        if (!(entry.Value is Color colour))
        //        {
        //            throw new InvalidOperationException($"Entry {entry.Key} was not of type {nameof(Color)}");
        //        }
        //        string foregroundKey = entry.Key?.ToString() + "Foreground";
        //        if (!(dictionary.OfType<DictionaryEntry>()
        //                .Single(de => string.Equals(de.Key.ToString(), foregroundKey, StringComparison.Ordinal))
        //                .Value is Color foregroundColour))
        //        {
        //            throw new InvalidOperationException($"Entry {foregroundKey} was not of type {nameof(Color)}");
        //        }

        //        return new Hue(entry.Key?.ToString() ?? "", colour, foregroundColour);
        //    }
        //}

        private static ResourceDictionary? Read(string? assemblyName, string? path)
        {
            if (assemblyName is null || path is null)
                return null;

            return (ResourceDictionary)Application.LoadComponent(new Uri(
                $"/{assemblyName};component/{path.Replace(".baml", ".xaml")}",
                UriKind.RelativeOrAbsolute));
        }

        private void ReadXaml(Stream stream)
        {
            Baml2006Reader read = new Baml2006Reader(stream);
            var s = read.Value is Window;
        }
    }
}
