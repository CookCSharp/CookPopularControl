using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotNullAndEmptyValidationRule
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-22 09:01:58
 */
namespace CookPopularControl.Tools.Windows.ValidationRules
{
    /// <summary>
    /// NullOrEmpty Rule
    /// </summary>
    public class NotNullAndEmptyValidationRule : ValidationRuleBase
    {
        private string _errorMessage = "不能为空";
        public override string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }

        public override ValidationResult ValidateBase(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrEmpty((value ?? "").ToString())
                    ? new ValidationResult(false, ErrorMessage)
                    : ValidationResult.ValidResult;
        }      
    }

    /// <summary>
    /// NullOrWhiteSpace Rule
    /// </summary>
    public class NotNullAndWhiteSpaceValidationRule : ValidationRuleBase
    {
        private string _errorMessage = "不能为空和空格";
        public override string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }

        public override ValidationResult ValidateBase(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                    ? new ValidationResult(false, ErrorMessage)
                    : ValidationResult.ValidResult;
        }
    }
}
