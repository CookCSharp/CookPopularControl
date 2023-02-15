/*
 * Description：HookFactory 
 * Author： Chance.Zheng
 * Create Time: 2022-11-03 17:46:39
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CookPopularCSharpToolkit.Communal
{
    public class HookFactory : IDisposable
    {
        private readonly SyncFactory syncFactory = new SyncFactory();

        //public ApplicationWatcher GetApplicationWatcher()
        //{
        //    return new ApplicationWatcher(syncFactory);
        //}

        public KeyboardWatcher GetKeyboardWatcher()
        {
            return new KeyboardWatcher(syncFactory);
        }

        public MouseWatcher GetMouseWatcher()
        {
            return new MouseWatcher(syncFactory);
        }

        public ClipboardWatcher GetClipboardWatcher()
        {
            return new ClipboardWatcher(syncFactory);
        }

        //public PrintWatcher GetPrintWatcher()
        //{
        //    return new PrintWatcher(syncFactory);
        //}

        public void Dispose()
        {
            syncFactory.Dispose();
        }
    }
}
