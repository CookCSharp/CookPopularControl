using CookPopularControl.Controls.Dragables;
using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LocationReportException
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 08:52:50
 */
namespace CookPopularControl.Communal.Data
{
    /// <summary>
    /// 提供<see cref="LocationReport"/>关于<see cref="Exception"/>
    /// </summary>
    public class LocationReportException : Exception
    {
        public LocationReportException()
        {
        }

        public LocationReportException(string message) : base(message)
        {
        }

        public LocationReportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
