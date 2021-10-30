using CookPopularControl.Controls.Dragables;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TabEmptiedResponse
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:36:49
 */
namespace CookPopularControl.Communal.Data
{
    public enum TabEmptiedResponse
    {
        /// <summary>
        /// Allow the Window to be closed automatically.
        /// </summary>
        CloseWindowOrLayoutBranch,
        /// <summary>
        /// The window will not be closed by the <see cref="DragableTabControl"/>, probably meaning the implementor will close the window manually
        /// </summary>
        DoNothing
    }
}
