using System;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：StoryboardCompletionListener
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 15:05:23
 */
namespace CookPopularControl.Tools.Listeners
{
    public class StoryboardCompletionListener
    {
        private readonly Storyboard _storyboard;
        private readonly Action<Storyboard> _continuation;

        public StoryboardCompletionListener(Storyboard storyboard, Action<Storyboard> continuation)
        {
            if (storyboard == null) throw new ArgumentNullException("storyboard");
            if (continuation == null) throw new ArgumentNullException("continuation");

            _storyboard = storyboard;
            _continuation = continuation;

            _storyboard.Completed += StoryboardOnCompleted;
        }

        private void StoryboardOnCompleted(object sender, EventArgs eventArgs)
        {
            _storyboard.Completed -= StoryboardOnCompleted;
            _continuation(_storyboard);
        }
    }
}
