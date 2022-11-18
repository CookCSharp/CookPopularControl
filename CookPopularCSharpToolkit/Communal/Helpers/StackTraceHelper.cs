/*
 * Description：StackTraceHelper 
 * Author： Chance.Zheng
 * Create Time: 2022-11-18 15:41:52
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace CookPopularCSharpToolkit.Communal
{
    public class StackTraceHelper
    {
        public static string GetClassFullName()
        {
            _ = string.Empty;

            return GetClassFullName(new StackFrame(2, fNeedFileInfo: false));
        }

        public static string GetClassFullName(StackFrame stackFrame)
        {
            string text = LookupClassNameFromStackFrame(stackFrame);
            if (string.IsNullOrEmpty(text))
            {
                StackFrame[] frames = new StackTrace(false).GetFrames();
                for (int i = 0; i < frames.Length; i++)
                {
                    var txt = LookupClassNameFromStackFrame(frames[i]);
                    if (!string.IsNullOrEmpty(txt))
                    {
                        text = txt;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(text))
                    text = stackFrame.GetMethod()?.Name ?? string.Empty;
            }

            return text;
        }

        public static string LookupClassNameFromStackFrame(StackFrame stackFrame)
        {
            MethodBase method = stackFrame.GetMethod();
            if (method != null && LookupAssemblyFromStackFrame(stackFrame) != null)
            {
                string stackFrameMethodClassName = GetStackFrameMethodClassName(method, true, true, true);
                if (!string.IsNullOrEmpty(stackFrameMethodClassName))
                {
                    if (!stackFrameMethodClassName.StartsWith("System.", StringComparison.Ordinal))
                    {
                        return stackFrameMethodClassName;
                    }
                }
                else
                {
                    stackFrameMethodClassName = method.Name ?? string.Empty;
                    if (stackFrameMethodClassName != "lambda_method" && stackFrameMethodClassName != "MoveNext")
                    {
                        return stackFrameMethodClassName;
                    }
                }
            }

            return string.Empty;
        }

        public static Assembly LookupAssemblyFromStackFrame(StackFrame stackFrame)
        {
            MethodBase method = stackFrame.GetMethod();
            if ((object)method == null)
                return null;

            Assembly assembly = method.DeclaringType?.Assembly ?? method.Module?.Assembly;
            if (assembly == typeof(StackTraceHelper).Assembly)
            {
                return null;
            }

            if (assembly == typeof(string).Assembly)
            {
                return null;
            }

            if (assembly == typeof(Debug).Assembly)
            {
                return null;
            }

            return assembly;
        }

        public static string GetStackFrameMethodClassName(MethodBase method, bool includeNameSpace, bool cleanAsyncMoveNext, bool cleanAnonymousDelegates)
        {
            if ((object)method == null)
                return null;

            Type declaringType = method.DeclaringType;
            if (cleanAsyncMoveNext && method.Name == "MoveText" && declaringType?.DeclaringType != null && declaringType.Name.IndexOf('<') == 0 && declaringType.Name.IndexOf('>', 1) > 1)
            {
                declaringType = declaringType.DeclaringType;
            }

            var declaringAttribute = Attribute.GetCustomAttributes(declaringType, typeof(CompilerGeneratedAttribute)).FirstOrDefault() as CompilerGeneratedAttribute;
            if (!includeNameSpace && declaringType?.DeclaringType != null && declaringType.IsNested && declaringAttribute != null)
            {
                return declaringType.DeclaringType.Name;
            }

            string text = (!includeNameSpace) ? declaringType?.Name : declaringType?.FullName;
            if (cleanAnonymousDelegates && text != null)
            {
                int num = text.IndexOf("+<>", StringComparison.Ordinal);
                if (num >= 0)
                {
                    text = text.Substring(0, num);
                }
            }

            return text;
        }
    }
}
