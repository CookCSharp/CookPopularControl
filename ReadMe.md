[![Fork me on Gitee](CookPopularControl/Resources/Images/CookCSharp.png)](https://gitee.com/cook-csharp/CookPopularControl)

# Welcome to CookPopularControl 

[![Nuget Publish](https://github.com/chancezheng/CookPopularControl/actions/workflows/nuget-push.yml/badge.svg)](https://github.com/chancezheng/CookPopularControl/actions/workflows/nuget-push.yml) [![qq群](https://img.shields.io/badge/qq-658794308-red.svg)](https://jq.qq.com/?_wv=1027&k=hVVHKJ1V) 

[![](https://img.shields.io/badge/Author-写代码的厨子-brightgreen.svg)](https://gitee.com/cook-csharp/CookPopularControl) [![license](https://img.shields.io/badge/license-Apache2.0-brightgreen.svg)](https://gitee.com/cook-csharp/CookPopularControl/blob/chance_dev/LICENSE) [![nuget](https://img.shields.io/nuget/v/CookPopularControl.svg)](https://www.nuget.org/packages/CookPopularControl) [![nuget](https://img.shields.io/nuget/dt/CookPopularControl.svg)](https://www.nuget.org/packages/CookPopularControl) [![Build Status](https://dev.azure.com/407042815/vue-mapvgl/_apis/build/status/vue-mapvgl-Node.js%20With%20Grunt-CI?branchName=master)]()

[![Gitee stars](https://gitee.com/cook-csharp/CookPopularControl/badge/star.svg?theme=dark)](https://gitee.com/cook-csharp/CookPopularControl) [![Gitee forks](https://gitee.com/cook-csharp/CookPopularControl/badge/fork.svg?theme=dark)](https://gitee.com/cook-csharp/CookPopularControl) 
[![Github stars](https://img.shields.io/github/stars/chancezheng/CookPopularControl.svg?color=red&&logo=github)](https://github.com/chancezheng/CookPopularControl) [![Github forks](https://img.shields.io/github/forks/chancezheng/CookPopularControl.svg?color=red&&logo=github)](https://github.com/chancezheng/CookPopularControl)

## 介绍
CookPopularControl是支持.NetFramework4.6.1与.Net5.0的WPF控件库，其中参考了一些资料，目前提供了近70款左右的控件，还在更新中，感兴趣的可以持续关注下，如果你的项目用到此库，不要忘记点个赞，有问题可加QQ群交流：658794308，欢迎大家参与开发和指出问题，谢谢！
***

## 使用
- Install-Package CookPopularControl -Version 1.0.1.1

- 添加如下代码即可全部引用
    ```
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/CookPopularControl;component/Themes/DefaultPopularControl.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/CookPopularControl;component/Themes/DefaultPopularColor.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
    ```

- **Themes**
    ![效果](MvvmTestDemo/Resources/Effect/Theme.gif)

- **Animations**
    ![效果](MvvmTestDemo/Resources/Effect/Animations.gif)

- **Button**
    ![效果](MvvmTestDemo/Resources/Effect/Button.gif)

- **BlockBars**
    ![效果](MvvmTestDemo/Resources/Effect/BlockBars.png)   

- **CarouselView**
    ![效果](MvvmTestDemo/Resources/Effect/CarouselView.gif)

- **CheckBox**
    ![效果](MvvmTestDemo/Resources/Effect/CheckBox.gif)

- **ComboBox**
    ![效果](MvvmTestDemo/Resources/Effect/ComboBox.gif)

- **DataGrid**
    ![效果](MvvmTestDemo/Resources/Effect/DataGrid.png)

- **DateTimePicker**
    ![效果](MvvmTestDemo/Resources/Effect/DateTimePicker.png) 

- **DialogBox**
    ![效果](MvvmTestDemo/Resources/Effect/DialogBox.gif)

- **Expander**
    ![效果](MvvmTestDemo/Resources/Effect/Expander.gif)

- **Fields**
    ![效果](MvvmTestDemo/Resources/Effect/Fields.png)

- **Grid**
    ![效果](MvvmTestDemo/Resources/Effect/Grid.png)

- **GroupBox**
    ![效果](MvvmTestDemo/Resources/Effect/GroupBox.png)

- **GroupControls**
    ![效果](MvvmTestDemo/Resources/Effect/GroupControls.png)
        
- **Labels**
    ![效果](MvvmTestDemo/Resources/Effect/Labels.png)
    
- **Lists**
    ![效果](MvvmTestDemo/Resources/Effect/Lists.png)

- **Loading**
    ![效果](MvvmTestDemo/Resources/Effect/Loading.gif)

- **MediaPlayer**
    ![效果](MvvmTestDemo/Resources/Effect/MediaPlayer.gif)

- **Menu**
    ![效果](MvvmTestDemo/Resources/Effect/Menu.gif)

- **MessageDialog**
    ![效果](MvvmTestDemo/Resources/Effect/MessageDialog.png)

- **Notify**
    ![效果](MvvmTestDemo/Resources/Effect/Notify.png)
    ![效果](MvvmTestDemo/Resources/Effect/NotifyIcon.png)

- **OtherControls**
    ![效果](MvvmTestDemo/Resources/Effect/OtherControls.gif)

- **PasswordBox**
    ![效果](MvvmTestDemo/Resources/Effect/PasswordBox.gif)

- **ProgressBar**
    ![效果](MvvmTestDemo/Resources/Effect/ProgressBar.gif)

- **QRCode**
    ![效果](MvvmTestDemo/Resources/Effect/QRCode.gif)

- **RadioButton**
    ![效果](MvvmTestDemo/Resources/Effect/RadioButton.png)

- **ScrollViewer**
    ![效果](MvvmTestDemo/Resources/Effect/ScrollViewer.png)

- **Slider**
    ![效果](MvvmTestDemo/Resources/Effect/Slider.gif)
    
- **Star**
    ![效果](MvvmTestDemo/Resources/Effect/Star.png)

- **Swiper**
    ![效果](MvvmTestDemo/Resources/Effect/Swiper.gif)

- **TabControl**
    ![效果](MvvmTestDemo/Resources/Effect/TabControl.png)

- **ThreeDimensionalControls**
    ![效果](MvvmTestDemo/Resources/Effect/ThreeDimensionalControls.gif)

- **ToggleButton**
    ![效果](MvvmTestDemo/Resources/Effect/ToggleButton.gif)
    
- **TreeView**
    ![效果](MvvmTestDemo/Resources/Effect/TreeView.png)

<!-- ### &#8627; Stargazers
[![Stargazers repo roster for @cook-csharp/CookPopularControl](https://reporoster.com/stars/cook-csharp/CookPopularControl)](https://gitee.com/cook-csharp/CookPopularControl/stargazers)

### &#8627; Forkers
[![Forkers repo roster for @cook-csharp/CookPopularControl](https://reporoster.com/forks/cook-csharp/CookPopularControl)](https://gitee.com/cook-csharp/CookPopularControl/members) -->

<!-- ## ‎‍💻 Code Contributors

<img src="https://opencollective.com/CookPopularControl/contributors.svg?width=890&button=false" alt="Code Contributors" style="max-width:100%;"> -->

## ⭐️ Stargazers

<img src="https://starchart.cc/chancezheng/CookPopularControl.svg" alt="Stargazers over time" style="max-width: 100%">

<!-- ## 🏆 Forkers

<img src="https://forkchart.cc/chancezheng/CookPopularControl.svg" alt="Fork over time" style="max-width: 100%"> -->