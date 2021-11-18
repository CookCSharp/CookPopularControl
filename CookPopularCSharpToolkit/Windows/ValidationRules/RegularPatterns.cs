using System;
using System.Text.RegularExpressions;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RegularPatterns
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-22 14:03:48
 */
namespace CookPopularCSharpToolkit.Windows
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
        public const string NotNegativeIntPattern = @"^[0-9]\d*$";

        /// <summary>
        /// 非正整数规则
        /// </summary>
        public const string NotPositveIntPattern = @"^-[0-9]\d*$";

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
        /// 只包含字母和数字规则
        /// </summary>
        public const string DigitalAndLetterPattern = @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]+$";

        /// <summary>
        /// 只包含大小写字母和数字规则
        /// </summary>
        public const string DigitalAndUpperLowerLetterPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[a-z])[a-zA-Z\d]+$";

        /// <summary>
        /// 数字或英文字母组成的规则
        /// </summary>
        public const string DigitalOrLetterPattern = "^[A-Za-z0-9]+$";

        /// <summary>
        /// 数字、英文字母或下划线组成的规则
        /// </summary>
        public const string DigitalOrLetterOrLinePattern = "^\\w+$";

        /// <summary>
        /// 汉字规则
        /// </summary>
        public const string ChinesePattern = @"^[\u4e00-\u9fa5]+$";



        /// <summary>
        /// 密码1规则
        /// </summary>
        /// <remarks>必须含有数字与字母，且不能包含中文与空格</remarks>
        public const string Password1Pattern = @"^(?![^\d]+$)(?![^a-zA-Z]+$)[^\u4e00-\u9fa5\s]+$";

        /// <summary>
        /// 密码2规则
        /// </summary>
        /// <remarks>必须含有字母与数字,其它任意字符亦可</remarks>
        public const string Password2Pattern = "^(?=.*\\d)(?=.*[a-zA-Z]).{0,}$";

        /// <summary>
        /// 密码3规则
        /// </summary>
        /// <remarks>必须含有大小字母与数字,其它任意字符亦可</remarks>
        public const string Password3Pattern = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{0,}$";

        /// <summary>
        /// 手机号码规则
        /// </summary>
        /// <remarks>国内手机号码</remarks>
        public const string PhoneNumberPattern = @"^(13[0-9]|14[5|7]|15[0|1|2|3|4|5|6|7|8|9]|18[0|1|2|3|4|5|6|7|8|9])\d{8}$";

        /// <summary>
        /// 座机号码
        /// </summary>
        /// <remarks>国内</remarks>
        public const string LandLineNumberPattern = @"\d{3}-\d{8}|\d{4}-\d{7}";

        /// <summary>
        /// 身份证号(15位、18位数字)规则
        /// </summary>
        public const string IDCardPattern = @"^\d{15}|\d{18}$";

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
    /// 指定输入文本规则
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
        DigitalAndUpperLowerLetter,
        DigitalOrLetter,
        DigitalOrLetterOrLine,
        Chinese,
        Password1,
        Password2,
        Password3,
        PhoneNumber,
        LandLineNumber,
        IDCard,
        Email,
        Url,
        Other,//供大家自定义
    }
}
