using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

// 在此类的 SDK 样式项目中，现在，在此文件中早前定义的几个程序集属性将在生成期间自动添加，并使用在项目属性中定义的值进行填充。有关包含的属性以及如何定制此过程的详细信息，请参阅
// https://aka.ms/assembly-info-properties


// 将 ComVisible 设置为 false 会使此程序集中的类型对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，请将该类型的 ComVisible
// 属性设置为 true。

[assembly: ComVisible(true)]
[assembly: CLSCompliant(true)]
//[assembly: InternalsVisibleTo("CookPopularControl,PublicKey=" + "")]

// 如果此项目向 COM 公开，则下列 GUID 用于 typelib 的 ID。

[assembly: Guid("4c66675d-d011-452b-a73c-b3263806c47a")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: XmlnsPrefix("https://Chance.CookPopularControl/2021/xaml", "pc")]

[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.Controls")]
[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.DialogBox")]
[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.Windows")]

[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.Controls.Dragables")]
[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.Controls.ThreeDimensional")]

[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.Communal")]
[assembly: XmlnsDefinition("https://Chance.CookPopularControl/2021/xaml", "CookPopularControl.Expression")]


