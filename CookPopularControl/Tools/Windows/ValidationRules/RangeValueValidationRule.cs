using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RangeValueValidationRule
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-13 10:08:42
 */
namespace CookPopularControl.Tools.Windows.ValidationRules
{
    /// <summary>
    /// 表示值范围的规则
    /// </summary>
    public class RangeValueValidationRule : ValidationRuleBase
    {
        private string _errorMessage = $"必须输入数字";
        public override string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public override ValidationResult ValidateBase(object value, CultureInfo cultureInfo)
        {
            if (value == null) 
                return new ValidationResult(false, ErrorMessage);
            if (!RegularPatterns.Default.IsMatchRegularPattern(value.ToString()))
                return new ValidationResult(false, ErrorMessage);
            else
            {
                double.TryParse(value.ToString(), out double v);
                if (v >= MinValue && v <= MaxValue)
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, $"值不在{MinValue}~{MaxValue}范围内");
            }
        }
    }
}
