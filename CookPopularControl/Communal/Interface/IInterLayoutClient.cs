using CookPopularControl.Controls.Dragables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IInterLayoutClient
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 08:55:14
 */
namespace CookPopularControl.Communal.Interface
{
    /// <summary>
    /// Implementors should provide a mechanism to provide a new host control which contains a new <see cref="TabablzControl"/>.
    /// </summary>
    public interface IInterLayoutClient
    {
        /// <summary>
        /// Provide a new host control and tab into which will be placed into a newly created layout branch.
        /// </summary>
        /// <param name="partition">Provides the partition where the drag operation was initiated.</param>
        /// <param name="source">The source control where a dragging operation was initiated.</param>
        /// <returns></returns>
        INewTabHost<UIElement> GetNewHost(object partition, DragableTabControl source);
    }
}
