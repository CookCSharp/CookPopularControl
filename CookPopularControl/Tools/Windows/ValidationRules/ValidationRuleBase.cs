using System.Globalization;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ValidationRuleBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-22 09:04:36
 */
namespace CookPopularControl.Tools.Windows.ValidationRules
{
    /// <summary>
    /// 提供数据验证基类
    /// </summary>
    public abstract class ValidationRuleBase : ValidationRule
    {
        /// <summary>
        /// 错误提示信息
        /// </summary>
        public abstract string ErrorMessage { get; set; }

        protected ValidationRuleBase()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidateBase(value, cultureInfo);
        }

        /// <summary>
        /// 自定义验证<see cref="ValidationResult"/>方法基类
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public abstract ValidationResult ValidateBase(object value, CultureInfo cultureInfo);
    }
}
