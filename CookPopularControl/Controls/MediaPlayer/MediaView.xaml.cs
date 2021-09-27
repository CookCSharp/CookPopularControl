using CookPopularControl.Communal.Attached;
using CookPopularControl.Communal.ViewModel;
using CookPopularControl.Tools.Extensions;
using CookPopularControl.Tools.Helpers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace CookPopularControl.Controls.MediaPlayer
{
    /// <summary>
    /// UserMediaView.xaml 的交互逻辑
    /// </summary>
    public partial class MediaView : UserControl
    {
        MediaViewModel mediaViewModel = new MediaViewModel();
        DispatcherTimer playTimer; //播放进度时钟
        DispatcherTimer controlViewTimer; //控制界面显示时钟
        DispatcherTimer mouseMoveCheckTimer; //监测鼠标移动时钟
        private Point controlViewMousePoint = new Point();
        private Point controlViewMovePoint = new Point();

        public MediaView()
        {
            InitializeComponent();

            this.DataContext = mediaViewModel;
            this.Loaded += MediaView_Loaded;
        }

        private void MediaView_Loaded(object sender, RoutedEventArgs e)
        {
            playTimer = IntervalMultiSeconds(playTimer, 1, RefreshSlider);
            controlViewTimer = IntervalMultiSeconds(controlViewTimer, 3, ControlGirdCollapsed);
            mouseMoveCheckTimer = IntervalMultiSeconds(mouseMoveCheckTimer, 0.1, CheckMouseMove);

            mouseMoveCheckTimer.Start();
            mediaElement.MouseLeftButtonDown += MediaElement_MouseLeftButtonDown;
        }

        private void CheckMouseMove()
        {
            if (ParentGrid.IsMouseOver)
            {
                if (controlViewMousePoint == controlViewMovePoint)
                    controlViewTimer.Start();
                else
                    controlViewMousePoint = controlViewMovePoint;
            }
        }

        #region 控制界面显示与否

        private void ParentGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            controlViewTimer.Start();
        }

        private void ParentGrid_MouseMove(object sender, MouseEventArgs e)
        {
            controlViewMovePoint = e.GetPosition(controlGrid);
            mediaViewModel.IsControlOpen = true;
            mouseMoveCheckTimer.Start();
        }

        private void ControlGirdCollapsed()
        {
            controlViewTimer.Stop();
            mouseMoveCheckTimer.Stop();
            mediaViewModel.IsControlOpen = false;
        }

        #endregion

        #region 更改播放媒体位置

        private void playSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            playTimer.Stop();
        }

        private void playSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playTimer.Start();
            mediaElement.Position = TimeSpan.FromSeconds(playSlider.Value);
        }

        private void playSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            playTimer.Stop();
        }

        private void playSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            playTimer.Start();
            mediaElement.Position = TimeSpan.FromSeconds(playSlider.Value);
        }

        #endregion

        #region 播放、暂停、重放

        private void PlayPause_Control(object sender, RoutedEventArgs e)
        {
            if (!play_pause.IsChecked!.Value)
                Pause();
            else
                Play();
        }

        //播放
        private void Play()
        {
            mediaElement.Play();
            playTimer.Start();
        }

        //暂停
        private void Pause()
        {
            mediaElement.Pause();
            playTimer.Stop();
        }

        //重放
        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
            Play();
            play_pause.IsChecked = true;
        }

        #endregion

        #region 音量控制


        private void volume_MouseEnter(object sender, MouseEventArgs e)
        {
            mediaViewModel.IsVolumeOpen = true;
        }

        private void volume_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(volume).Y >= volume.Height || e.GetPosition(volume).X <= 0 || e.GetPosition(volume).X >= volume.Width)
                mediaViewModel.IsVolumeOpen = false;
        }

        private void VolumePopup_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(volume_grid).Y <= 0 || e.GetPosition(volume_grid).X <= 0 || e.GetPosition(volume_grid).X >= volume_grid.Width)
                mediaViewModel.IsVolumeOpen = false;
        }

        private double mutedVolumeValue = 0;
        //静音模式
        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            if (!volume.IsChecked!.Value)
            {
                mediaElement.IsMuted = false;
                volumeSlider.Value = mutedVolumeValue;
            }
            else
            {
                mediaElement.IsMuted = true;
                mutedVolumeValue = volumeSlider.Value;
                volumeSlider.Value = 0;
            }
        }

        //音量值
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volume_value.Text = ((int)(volumeSlider.Value * 100)).ToString();

            if (volumeSlider.Value > 0)
                mediaElement.IsMuted = false;
            else
                mediaElement.IsMuted = true;

            if (volumeSlider.Value > 0.5 && volumeSlider.Value <= 1.0)
            {
                FrameworkElementBaseAttached.SetIconGeometry(volume, ResourceHelper.GetResource<Geometry>("VolumeHighGeometry"));
            }
            else if (volumeSlider.Value > 0 && volumeSlider.Value <= 0.5)
            {
                FrameworkElementBaseAttached.SetIconGeometry(volume, ResourceHelper.GetResource<Geometry>("VolumeMediumGeometry"));
            }
            else
            {
                FrameworkElementBaseAttached.SetIconGeometry(volume, ResourceHelper.GetResource<Geometry>("VolumeOffGeometry"));
            }
        }

        #endregion

        #region 倍速控制

        private void playSpeed_MouseEnter(object sender, MouseEventArgs e)
        {
            mediaViewModel.IsSpeedOpen = true;
        }

        private void playSpeed_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(playSpeed).Y >= playSpeed.Height || e.GetPosition(playSpeed).X <=0  || e.GetPosition(playSpeed).X >= playSpeed.Width)
                mediaViewModel.IsSpeedOpen = false;
        }

        private void SpeedPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(speed_grid).Y <= 0 || e.GetPosition(speed_grid).X <= 0 || e.GetPosition(speed_grid).X >= speed_grid.Width)
                mediaViewModel.IsSpeedOpen = false;
        }

        Double[] speed = new double[7] { 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0 };
        //播放速度
        private void speedSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                mediaElement.SpeedRatio = speed[speedSlider.SelectedIndex];
            }
        }

        #endregion

        #region 其它控制

        //更新SliderValue
        private void RefreshSlider()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                playSlider.Value = mediaElement.Position.TotalSeconds;
            }));
        }

        //媒体加载完
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            playSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            volumeSlider.Value = mediaElement.Volume;
            playTimer.Start();
        }

        //媒体播放结束
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            var mediaPlayer = userMedia.TemplatedParent as MediaPlayer;
            mediaElement.Position = TimeSpan.FromSeconds(0);
            playSlider.Value = 0;
            mediaElement.LoadedBehavior = MediaState.Manual;
            if (!mediaPlayer.IsCyclePlay)
            {
                Pause();
                play_pause.IsChecked = false;
            }
        }

        //媒体播放错误
        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageDialog.Show("播放错误：" + e.ErrorException.Message);
        }

        //全屏、退出全屏
        private void FullScreent_Click(object sender, RoutedEventArgs e)
        {
            if (fullScreent.IsChecked == true)
            {
                fullScreent.ToolTip = "退出全屏";
                Window.GetWindow(this).WindowState = WindowState.Maximized;
            }
            else
            {
                fullScreent.ToolTip = "全屏";
                Window.GetWindow(this).WindowState = WindowState.Normal;
            }
        }

        //双击全屏
        private void MediaElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                fullScreent.IsChecked = true;
                FullScreent_Click(fullScreent, null);
            }
        }

        //上一个
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            var mediaPlayer = userMedia.TemplatedParent as MediaPlayer;
            int index = mediaPlayer.ItemSource.ToList().IndexOf(mediaPlayer.CurrentUri);
            index -= 1;
            if (mediaPlayer.IsCyclePlay)
            {
                if (index < 0)
                    index = mediaPlayer.ItemSource.Count() - 1;
                mediaPlayer.CurrentUri = mediaPlayer.ItemSource.ElementAt(index);           
            }
            else
            {
                if (index >= 0)
                {
                    mediaPlayer.CurrentUri = mediaPlayer.ItemSource.ElementAt(index);
                }
            }

            Play();
            play_pause.IsChecked = true;
        }

        //下一个
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            var mediaPlayer = userMedia.TemplatedParent as MediaPlayer;
            int index = mediaPlayer.ItemSource.ToList().IndexOf(mediaPlayer.CurrentUri);
            index += 1;
            if (mediaPlayer.IsCyclePlay)
            {
                if (index >= mediaPlayer.ItemSource.Count())
                    index = 0;
                 mediaPlayer.CurrentUri = mediaPlayer.ItemSource.ElementAt(index);
            }
            else
            {
                if (index < mediaPlayer.ItemSource.Count())
                {
                    mediaPlayer.CurrentUri = mediaPlayer.ItemSource.ElementAt(index);
                }
            }

            Play();
            play_pause.IsChecked = true;
        }

        #endregion

        /// <summary>
        /// ControlGrid显示与隐藏
        /// </summary>
        /// <param name="isControlPop">是否打开</param>
        /// <param name="times">动画时间</param>
        private void CollapsedControl(bool isControlPop, double times = 3D)
        {
            //ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
            //Duration duration = new Duration(TimeSpan.FromSeconds(3));
            //DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame(Visibility.Collapsed);
            //objectAnimationUsingKeyFrames.Duration = duration;
            //objectAnimationUsingKeyFrames.KeyFrames.Add(discreteObjectKeyFrame);
            //AnimationClock clock = objectAnimationUsingKeyFrames.CreateClock();
            //ControlGrid.ApplyAnimationClock(VisibilityProperty, clock);

            BooleanAnimationUsingKeyFrames booleanAnimationUsingKeyFrames = new BooleanAnimationUsingKeyFrames();
            Duration duration = new Duration(TimeSpan.FromSeconds(times));
            DiscreteBooleanKeyFrame discreteBooleanKeyFrame = new DiscreteBooleanKeyFrame(isControlPop);
            booleanAnimationUsingKeyFrames.Duration = duration;
            booleanAnimationUsingKeyFrames.KeyFrames.Add(discreteBooleanKeyFrame);
            AnimationClock clock = booleanAnimationUsingKeyFrames.CreateClock();
            controlGrid.ApplyAnimationClock(System.Windows.Controls.Grid.VisibilityProperty, clock);
        }

        public DispatcherTimer IntervalMultiSeconds(DispatcherTimer timer, double second, Action action)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(second);
            timer.Tick += delegate { action(); };
            return timer;
        }
    }
}
