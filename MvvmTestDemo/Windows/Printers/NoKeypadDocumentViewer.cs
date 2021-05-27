using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NoKeypadDocumentViewer
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-13 16:58:03
 */
namespace CookPopularControl.Controls.Windows.Printers
{
    public class NoKeypadDocumentViewer : DocumentViewer
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameworkElementAutomationPeer(this);
        }
    }
}
