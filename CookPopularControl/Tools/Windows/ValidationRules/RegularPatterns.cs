using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RegularPatterns
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-22 14:03:48
 */
namespace CookPopularControl.Tools.Windows.ValidationRules
{
    /// <summary>
    /// 提供一些正则表达式列表
    /// </summary>
    public sealed class RegularPatterns
    {
        public static readonly RegularPatterns Default = new RegularPatterns();
        private RegularPatterns() { }

        /// <summary>
        /// 数字规则
        /// </summary>
        public const string DigitalPattern = @"^(\-|\+?)\d+(\.\d+)?$";

        /// <summary>
        /// 正整数规则
        /// </summary>
        public const string PositiveIntPattern = @"^[1-9]\d*$";

        /// <summary>
        /// 负整数规则
        /// </summary>
        public const string NegativeIntPattern = @"^-[1-9]\d*$ ";

        /// <summary>
        /// 整数规则
        /// </summary>
        public const string IntPattern = @"^-?[1-9]\d*$";

        /// <summary>
        /// 非负整数规则
        /// </summary>
        public const string NotNegativeIntPattern = @"^[1-9]\d*|0$";

        /// <summary>
        /// 非正整数规则
        /// </summary>
        public const string NotPositveIntPattern = @"^-[1-9]\d*|0$";

        /// <summary>
        /// 正浮点数规则
        /// </summary>
        public const string PositiveDoublePattern = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";

        /// <summary>
        /// 负浮点数规则
        /// </summary>
        public const string NegativeDoublePattern = @"^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$";

        /// <summary>
        /// 浮点数规则
        /// </summary>
        public const string DoublePattern = @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$";

        /// <summary>
        /// 非负浮点数规则
        /// </summary>
        public const string NotNegativeDoublePattern = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$";

        /// <summary>
        /// 非正浮点数规则
        /// </summary>
        public const string NotPositiveDoublePattern = @"^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$";

        /// <summary>
        /// 字母规则
        /// </summary>
        /// <remarks>由26个英文字母组成的字符串</remarks>
        public const string LetterPattern = "^[A-Za-z]+$";

        /// <summary>
        /// 大写英文字母规则
        /// </summary>
        public const string UpperLetterPattern = "^[A-Z]+$";

        /// <summary>
        /// 小写英文字母规则
        /// </summary>
        public const string LowerLetterPattern = "^[a-z]+$";

        /// <summary>
        /// 数字和英文字母组成的规则
        /// </summary>
        public const string DigitalAndLetterPattern = "^[A-Za-z0-9]+$";

        /// <summary>
        /// 数字、英文字母或下划线组成的规则
        /// </summary>
        public const string DigitalOrLetterOrLinePattern = "^\\w+$";

        /// <summary>
        /// 汉字规则
        /// </summary>
        public const string ChinesePattern = @"^[\u4e00-\u9fa5]$";



        /// <summary>
        /// 密码规则
        /// </summary>
        /// <remarks>必须含有数字与字母，且不能包含中文与空格</remarks>
        public const string PasswordPattern = @"^(?![^\d]+$)(?![^a-zA-Z]+$)[^\u4e00-\u9fa5\s]+$";

        /// <summary>
        /// 电话规则
        /// </summary>
        /// <remarks>中国人手机号码</remarks>
        public const string PhoneNumberPattern = @"^((13[0-9])|(15[^4,\d])|(18[0,5-9]))\d{8}$";

        /// <summary>
        /// Email规则
        /// </summary>
        public const string EmailPattern = "^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$";

        /// <summary>
        /// URL规则
        /// </summary>
        public const string UrlPattern = "^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$";


        
        public bool IsMatchRegularPattern(string inputText, InputTextType formatType = default)
        {           
            return Regex.IsMatch(inputText, Default.GetValue(Enum.GetName(typeof(InputTextType), formatType) + "Pattern"));
        }

        public bool IsMatchRegularPattern(string inputText, string pattern)
        {
            return Regex.IsMatch(inputText, pattern);
        }

        public string GetValue(string propertyName) => this.GetType().GetField(propertyName).GetValue(default).ToString();
    }

    /// <summary>
    /// 输入文本格式
    /// </summary>
    public enum InputTextType
    {
        Digital,
        PositiveInt,
        NegativeInt,
        Int,
        NotNegativeInt,
        NotPositveInt,
        PositiveDouble,
        NegativeDouble,
        Double,
        NotNegativeDouble,
        NotPositiveDouble,
        Letter,
        UpperLetter,
        LowerLetter,
        DigitalAndLetter,
        DigitalOrLetterOrLine,
        Chinese,
        Password,
        PhoneNumber,
        Email,
        Url,
    }
}
