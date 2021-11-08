using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// 在此类的 SDK 样式项目中，现在，在此文件中早前定义的几个程序集属性将在生成期间自动添加，并使用在项目属性中定义的值进行填充。有关包含的属性以及如何定制此过程的详细信息，请参阅
// https://aka.ms/assembly-info-properties


// 将 ComVisible 设置为 false 会使此程序集中的类型对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，请将该类型的 ComVisible
// 属性设置为 true。
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("CookPopularControl")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")] //, ConfigFileExtension = "config", Watch = true

// 如果此项目向 COM 公开，则下列 GUID 用于 typelib 的 ID。
[assembly: Guid("969067b6-6d85-40df-b024-7d9eca442855")]

[assembly: XmlnsPrefix("https://Chance.CookPopularCSharpToolkit/2021/xaml", "toolkit")]
[assembly: XmlnsDefinition("https://Chance.CookPopularCSharpToolkit/2021/xaml", "CookPopularCSharpToolkit")]
[assembly: XmlnsDefinition("https://Chance.CookPopularCSharpToolkit/2021/xaml", "CookPopularCSharpToolkit.Communal")]
[assembly: XmlnsDefinition("https://Chance.CookPopularCSharpToolkit/2021/xaml", "CookPopularCSharpToolkit.Windows")]
[assembly: XmlnsDefinition("https://Chance.CookPopularCSharpToolkit/2021/xaml", "CookPopularCSharpToolkit.Windows.Expression")]
[assembly: XmlnsDefinition("https://Chance.CookPopularCSharpToolkit/2021/xaml", "CookPopularCSharpToolkit.Windows.Transitions")]
