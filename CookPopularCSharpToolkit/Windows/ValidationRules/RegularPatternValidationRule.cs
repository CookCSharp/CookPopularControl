using System;
using System.Globalization;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RegularPatternValidation
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-22 15:07:59
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 正则表达式规则校验
    /// </summary>
    public class RegularPatternValidationRule : ValidationRuleBase
    {
        private string _errorMessage = "输入错误";
        public override string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }

        /// <summary>
        /// 匹配规则
        /// </summary>
        public InputTextType? RegularPattern { get; set; }

        public override ValidationResult ValidateBase(object value, CultureInfo cultureInfo)
        {
            if (RegularPattern is null) return ValidationResult.ValidResult;

            if(ErrorMessage == "输入错误" || string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = "Please input " + Enum.GetName(typeof(InputTextType), RegularPattern);

            return RegularPatterns.Default.IsMatchRegularPattern((value ?? string.Empty).ToString(), RegularPattern.Value)
                   ? ValidationResult.ValidResult
                   : new ValidationResult(false, ErrorMessage);
        }
    }
}
