using System.Collections.Generic;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MediaViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-27 19:24:22
 */
namespace CookPopularControl.Communal.ViewModel
{
    internal class MediaViewModel : ViewModelBase
    {
        private bool _isVolumeOpen = false;
        private bool _isSpeedOpen = false;
        private bool _isControlOpen = false;
        private List<string> _playSpeeds = new List<string>() { "0.5X", "0.75X", "1.0X", "1.25X", "1.5X", "1.75X", "2.0X" };


        /// <summary>                                            
        /// VolumePopup是否打开                                  
        /// </summary>                                           
        public bool IsVolumeOpen
        {
            get { return _isVolumeOpen; }
            set { SetProperty(ref _isVolumeOpen, value); }
        }

        /// <summary>
        /// SpeedPopup是否打开
        /// </summary>
        public bool IsSpeedOpen
        {
            get { return _isSpeedOpen; }
            set { SetProperty(ref _isSpeedOpen, value); }
        }

        /// <summary>
        /// ControlPopup是否打开
        /// </summary>
        public bool IsControlOpen
        {
            get { return _isControlOpen; }
            set { SetProperty(ref _isControlOpen, value); }
        }

        /// <summary>
        /// 播放速度列表
        /// </summary>
        public List<string> PlaySpeeds
        {
            get { return _playSpeeds; }
            set { _playSpeeds = value; }
        }
    }
}
