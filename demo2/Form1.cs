using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demo2
{
    public partial class From1 : Form
    {
        public From1()
        {
            InitializeComponent();
        }

        private void musicPlayer_Enter(object sender, EventArgs e)
        {

        }

        private bool flag = true;
        private bool pauseFlag = false;
        private int selectIndex = -1;
        private string path = @"D:";
        /// <summary>
        /// 播放/暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                play();
            }
            else
            {
                pause();
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        private void play()
        {
            if (filesList.Count == 0)
            {
                return;
            }
            if (picIndex == 2)
            {
                Random ran = new Random();
                selectIndex = ran.Next(0, filesList.Count);
            }
            if (!pauseFlag)
            {
                musicPlayer.URL = filesList[selectIndex];
                checkedListBox1.SelectedIndex = selectIndex;
                ReadSongWord rsw = new ReadSongWord();
                Dictionary<string, Object>  obj = rsw.ReadMp3(filesList[selectIndex]);
                label1.Text = obj["title"].ToString();
                label2.Text = obj["artist"].ToString();
                label3.Text = obj["album"].ToString();
                pictureBox1.Image = (Image)obj["image"];
                formatWord(filesList[selectIndex]);
            }
            musicPlayer.Ctlcontrols.play();
            pictureBox8.Image = Properties.Resources.pause;
            flag = false;
        }
        /// <summary>
        /// 暂停
        /// </summary>
        private void pause()
        {
            musicPlayer.Ctlcontrols.pause();
            flag = true;
            pauseFlag = true;
            pictureBox8.Image = Properties.Resources.play;
        }

        //歌词时间
        List<double> times = new List<double>();
        //歌词
        List<string> words = new List<string>();
        /// <summary>
        /// 从歌词文件中读取并整理歌词
        /// </summary>
        /// <param name="path"></param>
        private void formatWord(string path)
        {
            times.Clear();
            words.Clear();
            string path1 = path+ ".lrc";
            string path2 = path.Replace("mp3", "lrc");
            if (File.Exists(path1))
            {
                path = path1;
            }else if(File.Exists(path2)){
                path = path2;
            } else {
                label5.Text = "暂无歌词";
                return;
            }
            string[] lines = File.ReadAllLines(path);
            for(int i = 0; i < lines.Length; i++)
            {
                string[] temps = lines[i].Split(new char[2] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                if (temps.Length == 2)
                {
                    string[] timeStrs = temps[0].Split(new Char[] { ':' });
                    double t1 = double.Parse(timeStrs[0]) * 60;
                    double t2 = double.Parse(timeStrs[1]);
                    times.Add(t1 + t2);
                    words.Add(temps[1]);
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            musicPlayer.settings.autoStart = false;
            //musicPlayer.URL = @"D:\music\Taylor Swift 泰勒 斯威夫特\2006.6.19 Tim McGraw\01 - Tim McGraw (Album Version).mp3";
        }


        List<string> filesList = new List<string>();
        /// <summary>
        /// 导入歌曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = path;
            ofd.Filter = "MP3|*.mp3|音乐文件|*.wav|所有文件|*.*";
            ofd.Title = "请选择文件";
            ofd.Multiselect = true;
            ofd.ShowDialog();
            string[] files = ofd.FileNames;
            if (files.Length > 0)
            {
                path = Path.GetDirectoryName(ofd.FileName);
                for (int i = 0; i < files.Length; i++)
                {
                    filesList.Add(files[i]);

                    checkedListBox1.Items.Add(Path.GetFileName(files[i]));
                }
                selectIndex = 0;

            }
        }

        /// <summary>
        /// 上一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            int item = checkedListBox1.SelectedIndex;
            item--;
            if(item == -1)
            {
                item = filesList.Count-1;
            }
            checkedListBox1.SelectedIndex = item;
            selectIndex = item;
            play();
            pauseFlag = false;

        }
        /// <summary>
        ///下一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            nextMusic();

        }

        /// <summary>
        /// 下一曲
        /// </summary>
        void nextMusic()
        {
            if (filesList.Count == 0)
            {
                return;
            }
            int item = checkedListBox1.SelectedIndex;
            item++;
            if (item == filesList.Count)
            {
                item = 0;
            }
            checkedListBox1.SelectedIndex = item;
            selectIndex = item;
            play();
            pauseFlag = false;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            int checkCount = checkedListBox1.CheckedItems.Count;
            for (int i = 0; i < checkCount; i++)
            {
                    for (int j = 0; j < checkedListBox1.Items.Count; j++)
                {
                    if (checkedListBox1.GetItemChecked(j))
                    {
                        filesList.RemoveAt(j);
                        checkedListBox1.Items.RemoveAt(j);
                    }
                }
            }
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                musicPlayer.URL = filesList[0];
                checkedListBox1.SelectedIndex = 0;
            }
            
        }
        /// <summary>
        /// 列表双击播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_DoubleClick_1(object sender, EventArgs e)
        {
            try
            {
                //Console.WriteLine(listBox1.SelectedIndex);
                if (checkedListBox1.Items.Count > 0 && checkedListBox1.SelectedIndex != -1)
                {
                    selectIndex = checkedListBox1.SelectedIndex;
                    play();
                    pauseFlag = false;
                }
            }
            catch
            {

            }
        }


        /// <summary>
        /// 歌曲进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //musicPlayer.currentMedia.duration.ToString() 总秒
            //musicPlayer.Ctlcontrols.currentPosition.ToString(); 当前秒
            if(musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {

                label4.Text =  musicPlayer.currentMedia.durationString + "/" + musicPlayer.Ctlcontrols.currentPositionString;
                double t1 = Double.Parse(musicPlayer.currentMedia.duration.ToString());
                double t2 = Double.Parse(musicPlayer.Ctlcontrols.currentPosition.ToString());
                double percent = t2 / t1;
                int w = pictureBox2.Width;
                pictureBox5.Width = Convert.ToInt32(w * percent);
                pictureBox3.Location = new Point(Convert.ToInt32((w-6) * percent),pictureBox3.Location.Y);
                if (t1 - t2 <= 1)
                {
                    nextMusic();
                }
            }
        }
        /// <summary>
        /// 点击进度条实际
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {

            //pictureBox3.Location = new Point(X, pictureBox3.Location.Y);
            controllMusic(e);
        }
        /// <summary>
        /// 点击控制歌曲进度
        /// </summary>
        /// <param name="e"></param>
        private void controllMusic(MouseEventArgs e)
        {
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                Point p = e.Location;
                double x = p.X;
                double w = pictureBox2.Width;
                double a = x / w * musicPlayer.currentMedia.duration;
                musicPlayer.Ctlcontrols.currentPosition = x / w * musicPlayer.currentMedia.duration;

            }
        }
        /// <summary>
        /// 进度条头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox4_MouseClick(object sender, MouseEventArgs e)
        {
            controllMusic(e);
        }
        /// <summary>
        /// 已播放进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox5_MouseClick(object sender, MouseEventArgs e)
        {
            controllMusic(e);
        }

        private int picIndex = 1;
        /// <summary>
        /// 播放顺序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox6_MouseClick(object sender, MouseEventArgs e)
        {
            picIndex++;
            if (picIndex > 2)
            {
                picIndex = 1;
            }
            if (picIndex == 1)
            {
                pictureBox6.Image = Properties.Resources.turn; ;
            } else
            {
                pictureBox6.Image = Properties.Resources.random;
            }
        }
        /// <summary>
        /// 上一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox7_MouseClick(object sender, MouseEventArgs e)
        {
            if (filesList.Count == 0)
            {
                return;
            }
            int item = checkedListBox1.SelectedIndex;
            item--;
            if (item == -1)
            {
                item = filesList.Count - 1;
            }
            checkedListBox1.SelectedIndex = item;
            selectIndex = item;
            play();
            pauseFlag = false;
        }
        /// <summary>
        /// 下一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            nextMusic();
        }
        /// <summary>
        /// 暂停/播放图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                play();
            }
            else
            {
                pause();
            }
        }
        /// <summary>
        /// 展示歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                for (int i = 0; i < times.Count-1; i++)
                {
                    double position = musicPlayer.Ctlcontrols.currentPosition;
                    if (position > times[i] && position < times[i + 1])
                    {
                        label5.Text = words[i];
                    }
                }

            }
        }
        /// <summary>
        /// 静音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (musicPlayer.settings.mute)
            {
                musicPlayer.settings.mute = false;
                pictureBox10.BackgroundImage = Properties.Resources.musicOpen;
            }
            else
            {
                musicPlayer.settings.mute = true;
                pictureBox10.BackgroundImage = Properties.Resources.musicClose;
            }
        }
        private void controlVolume(MouseEventArgs e)
        {
            Point p = e.Location;
            double x = p.X;
            double w = pictureBox11.Width;
            double percent = x / w;
            musicPlayer.settings.volume = Convert.ToInt32(percent * 100);
            pictureBox12.Width = Convert.ToInt32(w * percent);
            //pictureBox12.Width = Convert.ToInt32(x / w * pictureBox12.Width);
        }
        /// <summary>
        /// 音量减
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox12_MouseClick(object sender, MouseEventArgs e)
        {
            controlVolume(e);
        }
        /// <summary>
        /// 音量加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox11_MouseClick(object sender, MouseEventArgs e)
        {
            controlVolume(e);
        }
    }

   

}
