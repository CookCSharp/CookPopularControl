using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FutureDateValidationRule
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-06 15:27:31
 */
namespace CookPopularControl.Tools.Windows.ValidationRules
{
    /// <summary>
    /// 针对<see cref="System.Windows.Controls.Calendar"/>与<see cref="DatePicker"/>只允许选中大于等于当前日期的时间规则
    /// </summary>
    public class FutureDateValidationRule : ValidationRuleBase
    {
        private string _errorMessage = "必须选择今天之后的日期";
        public override string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }

        public override ValidationResult ValidateBase(object value, CultureInfo cultureInfo)
        {
            if(!DateTime.TryParse((value ?? string.Empty).ToString(),CultureInfo.CurrentCulture,DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out DateTime dateTime))
                return new ValidationResult(false, ErrorMessage);

            return dateTime.Date < DateTime.Now.Date ? new ValidationResult(false, ErrorMessage) : ValidationResult.ValidResult;
        }
    }
}
