using CookPopularCSharpToolkit.Windows;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Bootstrapper
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-29 10:13:35
 */
namespace MvvmTestDemo
{
    /// <summary>
    /// 启动器
    /// </summary>
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = PrismIocExtensions.GetContainer(containerRegistry);
            var b = Container.Equals(container);

            //var assemblyName = new AssemblyName("Kitty");
            //var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            //注册View与ViewModel
            //ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }

        /// <summary>
        /// 重写View与ViewModel的解析方式
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            //配置好View与ViewModel的映射之后，prism:ViewModelLocator.AutoWireViewModel="True"才生效
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName.Replace(".UserControls.", ".DemoViewModels.");
                viewName = viewName.Replace(".DemoViews.", ".DemoViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName}ViewModel,{viewAssemblyName}";

                return Type.GetType(viewModelName);
            });
        }
    }
}
