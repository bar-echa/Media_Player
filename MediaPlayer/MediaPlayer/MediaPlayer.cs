
using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Windows.Forms;


namespace MediaPlayer
{
    public partial class MediaPlayer : Form
    {
        // COMPONENTS

        private AxWindowsMediaPlayer player = null!;
        private ListBox playlist = null!;

        private Label songTitle = null!;
        private Label statusLabel = null!;
        private Label timeLabel = null!;

        private TrackBar volumeBar = null!;
        private TrackBar progressBar = null!;

        private Button playButton = null!;
        private Button muteButton = null!;

        private System.Windows.Forms.Timer timer = null!;

        // MEDIA LIBRARY

        private List<string> mediaFiles = new List<string>();

        private int currentIndex = -1;
        private bool dragging = false;

        // COLORS

        private Color background =
            Color.FromArgb(18, 18, 28);

        private Color panelColor =
            Color.FromArgb(29, 27, 43);

        private Color purple =
            Color.FromArgb(139, 92, 246);

        private Color white = Color.White;

        // CONSTRUCTOR

        public MediaPlayer()
        {

            BuildUI();
        }

        // BUILD USER INTERFACE

        private void BuildUI()
        {
            Text = "NEXUS MEDIA PLAYER";

            Size = new Size(1350, 850);

            MinimumSize = new Size(1000, 650);

            StartPosition =
                FormStartPosition.CenterScreen;

            BackColor = background;

            Font = new Font("Segoe UI", 10);

            // SIDEBAR

            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(23, 21, 35),
                Padding = new Padding(15)
            };

            Controls.Add(sidebar);

            // LOGO

            Label logo = new Label
            {
                Text = "NEXUS",
                Font = new Font(
                    "Segoe UI",
                    23,
                    FontStyle.Bold
                ),
                ForeColor = Color.FromArgb(185, 145, 255),
                Location = new Point(15, 25),
                AutoSize = true
            };

            sidebar.Controls.Add(logo);

            // NAVIGATION TITLE

            Label navigation = new Label
            {
                Text = "WORKSPACE",
                ForeColor = Color.Gray,
                Location = new Point(20, 105),
                AutoSize = true
            };

            sidebar.Controls.Add(navigation);

            // SIDEBAR MENU

            AddSidebarButton(
                sidebar,
                " Dashboard",
                145
            );

            AddSidebarButton(
                sidebar,
                "Music Library",
                205
            );

            AddSidebarButton(
                sidebar,
                "My Playlist",
                265
            );

            AddSidebarButton(
                sidebar,
                "Video Player",
                325
            );

            // SIDEBAR FOOTER

            Label version = new Label
            {
                Text =
                    "NEXUS MEDIA PLAYER\n" +
            
                    "Built with C#",

                ForeColor = Color.Gray,
                Location = new Point(20, 700),
                AutoSize = true
            };

            sidebar.Controls.Add(version);

            // MAIN AREA

            Panel main = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = background,
                Padding = new Padding(25)
            };

            Controls.Add(main);

            // HEADER

            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 95
            };

            main.Controls.Add(header);

            Label heading = new Label
            {
                Text = "Welcome to NEXUS",

                Font = new Font(
                    "Segoe UI",
                    22,
                    FontStyle.Bold
                ),

                ForeColor = white,
                Location = new Point(190, 5),
                AutoSize = true
            };

            header.Controls.Add(heading);

            Label subtitle = new Label
            {
                Text =
                    "Your personal multimedia experience",

                ForeColor = Color.Silver,
                Location = new Point(250, 60),
                AutoSize = true
            };

            header.Controls.Add(subtitle);

            // ADD MEDIA BUTTON

            Button addButton = CreateButton(
                "+ Add Media",
                purple,
                145,
                42
            );

            addButton.Dock = DockStyle.Right;

            addButton.Width = 145;

            addButton.Click += AddMedia_Click;

            header.Controls.Add(addButton);

            // CONTENT AREA

            Panel content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 10)
            };

            main.Controls.Add(content);

            // PLAYLIST PANEL

            Panel libraryPanel = new Panel
            {
                Dock = DockStyle.Right,
                Width = 300,
                BackColor = panelColor,
                Padding = new Padding(15)
            };

            content.Controls.Add(libraryPanel);

            Label libraryTitle = new Label
            {
                Text = "YOUR PLAYLIST",

                ForeColor = white,

                Font = new Font(
                    "Segoe UI",
                    13,
                    FontStyle.Bold
                ),

                Location = new Point(15, 15),
                AutoSize = true
            };

            libraryPanel.Controls.Add(libraryTitle);

            // PLAYLIST

            playlist = new ListBox
            {
                Location = new Point(12, 55),

                Width = 270,
                Height = 480,

                BackColor = panelColor,
                ForeColor = white,

                BorderStyle = BorderStyle.None,

                Font = new Font("Segoe UI", 10),

                IntegralHeight = false
            };

            playlist.SelectedIndexChanged +=
                Playlist_SelectedIndexChanged;

            libraryPanel.Controls.Add(playlist);

            // REMOVE BUTTON

            Button removeButton = CreateButton(
                "Remove Selected",
                Color.FromArgb(190, 55, 75),
                270,
                42
            );

            removeButton.Location = new Point(15, 550);

            removeButton.Click += RemoveMedia_Click;

            libraryPanel.Controls.Add(removeButton);

            // PLAYER PANEL

            Panel playerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = panelColor,
                Padding = new Padding(15)
            };

            content.Controls.Add(playerPanel);

            // NOW PLAYING LABEL

            Label nowPlaying = new Label
            {
                Text = "NOW PLAYING",

                ForeColor = Color.FromArgb(185, 145, 255),

                Font = new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold
                ),

                Location = new Point(20, 15),
                AutoSize = true
            };

            playerPanel.Controls.Add(nowPlaying);

            // WINDOWS MEDIA PLAYER

            player = new AxWindowsMediaPlayer();

            ((System.ComponentModel.ISupportInitialize)player)
                .BeginInit();

            player.Enabled = true;

            player.Location = new Point(250, 50);

            player.Size = new Size(650, 370);

            //player.uiMode = "full";

            //player.stretchToFit = true;

            playerPanel.Controls.Add(player);

            ((System.ComponentModel.ISupportInitialize)player)
                .EndInit();

            // SONG TITLE

            songTitle = new Label
            {
                Text = "No Media Selected",

                ForeColor = white,

                Font = new Font(
                    "Segoe UI",
                    15,
                    FontStyle.Bold
                ),

                Location = new Point(250, 440),

                AutoSize = true
            };

            playerPanel.Controls.Add(songTitle);

            // STATUS

            statusLabel = new Label
            {
                Text = "Ready to play",

                ForeColor = Color.LightGray,

                Location = new Point(250, 475),

                AutoSize = true
            };

            playerPanel.Controls.Add(statusLabel);

            // PROGRESS BAR

            progressBar = new TrackBar
            {
                Location = new Point(250, 510),

                Width = 620,

                Minimum = 0,
                Maximum = 1000,

                Value = 0,

                TickStyle = TickStyle.None
            };

            progressBar.MouseDown +=
                (s, e) => dragging = true;

            progressBar.MouseUp += (s, e) =>
            {
                dragging = false;
                SeekMedia();
            };

            progressBar.Scroll += (s, e) =>
            {
                if (dragging)
                    SeekMedia();
            };

            playerPanel.Controls.Add(progressBar);

            // TIME DISPLAY

            timeLabel = new Label
            {
                Text = "00:00 / 00:00",

                ForeColor = Color.Silver,

                Location = new Point(250, 550),

                AutoSize = true
            };

            playerPanel.Controls.Add(timeLabel);

            // PREVIOUS BUTTON

            Button previousButton = CreateButton(
                "Previous",
                Color.FromArgb(65, 60, 85),
                115,
                45
            );

            previousButton.Location =
                new Point(20, 590);

            previousButton.Click += Previous_Click;

            playerPanel.Controls.Add(previousButton);

            // PLAY BUTTON

            playButton = CreateButton(
                "Play",
                purple,
                115,
                45
            );

            playButton.Location =
                new Point(150, 590);

            playButton.Click += Play_Click;

            playerPanel.Controls.Add(playButton);

            // PAUSE BUTTON

            Button pauseButton = CreateButton(
                "Pause",
                Color.FromArgb(65, 60, 85),
                115,
                45
            );

            pauseButton.Location =
                new Point(280, 590);

            pauseButton.Click += Pause_Click;

            playerPanel.Controls.Add(pauseButton);

            // NEXT BUTTON

            Button nextButton = CreateButton(
                "Next",
                Color.FromArgb(255, 60, 85),
                115,
                45
            );

            nextButton.Location =
                new Point(410, 590);

            nextButton.Click += Next_Click;

            playerPanel.Controls.Add(nextButton);

            // STOP BUTTON

            Button stopButton = CreateButton(
                "Stop",
                Color.FromArgb(180, 55, 75),
                100,
                45
            );

            stopButton.Location =
                new Point(540, 590);

            stopButton.Click += Stop_Click;

            playerPanel.Controls.Add(stopButton);

            // MUTE BUTTON

            muteButton = CreateButton(
                "Mute",
                Color.FromArgb(65, 60, 85),
                110,
                40
            );

            muteButton.Location =
                new Point(250, 655);

            muteButton.Click += Mute_Click;

            playerPanel.Controls.Add(muteButton);

            // VOLUME CONTROL

            volumeBar = new TrackBar
            {
                Location = new Point(355, 650),

                Width = 250,

                Minimum = 0,
                Maximum = 100,

                Value = 50,

                TickStyle = TickStyle.None
            };

            volumeBar.Scroll += Volume_Scroll;

            playerPanel.Controls.Add(volumeBar);

            player.settings.volume = 50;

            // TIMER

            timer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };

            timer.Tick += Timer_Tick;

            timer.Start();
        }

        // SIDEBAR BUTTON DESIGN

        private void AddSidebarButton(
            Panel panel,
            string text,
            int y)
        {
            Button button = new Button
            {
                Text = text,

                Location = new Point(10, y),

                Size = new Size(185, 45),

                FlatStyle = FlatStyle.Flat,

                BackColor = Color.FromArgb(23, 21, 35),

                ForeColor = Color.White,

                TextAlign = ContentAlignment.MiddleLeft,

                Font = new Font("Segoe UI", 10)
            };

            button.FlatAppearance.BorderSize = 0;

            panel.Controls.Add(button);

            button.Click += (s, e) =>
            {
                if (text.Contains("Playlist"))
                {
                    playlist.Focus();
                }
                else if (
                    text.Contains("Library") ||
                    text.Contains("Video"))
                {
                    AddMedia_Click(s, e);
                }
                else
                {
                    statusLabel.Text = "NEXUS Dashboard";
                }
            };
        }

        // BUTTON DESIGN

        private Button CreateButton(
            string text,
            Color color,
            int width,
            int height)
        {
            return new Button
            {
                Text = text,

                Size = new Size(width, height),

                BackColor = color,

                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat,

                Font = new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                ),

                Cursor = Cursors.Hand
            };
        }



        private void AddMedia_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Add Media to NEXUS";

                dialog.Filter =
                    "Media Files|*.mp3;*.mp4;*.wav;*.avi;*.wmv;*.wma;*.mkv;*.mov|All Files|*.*";

                dialog.Multiselect = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    int added = 0;

                    foreach (string file in dialog.FileNames)
                    {
                        if (!mediaFiles.Contains(file))
                        {
                            mediaFiles.Add(file);

                            playlist.Items.Add(
                                Path.GetFileName(file)
                            );

                            added++;
                        }
                    }

                    statusLabel.Text =
                        added + " media file(s) added";

                    // Automatically play first media

                    if (currentIndex == -1 &&
                        mediaFiles.Count > 0)
                    {
                        PlayMedia(0);
                    }
                }
            }
        }

        // PLAY MEDIA BY INDEX

        private void PlayMedia(int index)
        {
            if (index < 0 || index >= mediaFiles.Count)
                return;

            try
            {
                currentIndex = index;

                player.URL = mediaFiles[index];

                songTitle.Text =
                    Path.GetFileName(mediaFiles[index]);

                playlist.SelectedIndex = index;

                statusLabel.Text = "Loading media...";

                playButton.Text = "? Play";

                progressBar.Value = 0;

                timeLabel.Text = "00:00 / 00:00";
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Unable to play media";

                MessageBox.Show(
                    "Unable to open media:\n" + ex.Message,
                    "NEXUS Media Player",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // PLAYLIST SELECTION

        private void Playlist_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            if (playlist.SelectedIndex >= 0)
            {
                int selected = playlist.SelectedIndex;

                if (selected != currentIndex)
                {
                    PlayMedia(selected);
                }
            }
        }

        // PLAY BUTTON

        private void Play_Click(object? sender, EventArgs e)
        {
            if (currentIndex == -1)
            {
                statusLabel.Text = "Please add media first";

                MessageBox.Show(
                    "Please add a music or video file first.",
                    "NEXUS Media Player",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            player.Ctlcontrols.play();

            statusLabel.Text = "Playing";

            playButton.Text = "? Playing";
        }

        // PAUSE BUTTON

        private void Pause_Click(object? sender, EventArgs e)
        {
            if (currentIndex == -1)
                return;

            player.Ctlcontrols.pause();

            statusLabel.Text = "Paused";

            playButton.Text = "? Play";
        }

        // STOP BUTTON

        private void Stop_Click(object? sender, EventArgs e)
        {
            if (currentIndex == -1)
                return;

            player.Ctlcontrols.stop();

            statusLabel.Text = "Stopped";

            playButton.Text = "? Play";

            progressBar.Value = 0;

            timeLabel.Text = "00:00 / 00:00";
        }

        // NEXT MEDIA

        private void Next_Click(object? sender, EventArgs e)
        {
            if (currentIndex < mediaFiles.Count - 1)
            {
                PlayMedia(currentIndex + 1);
            }
            else if (mediaFiles.Count > 0)
            {
                PlayMedia(0);
            }
        }

        // PREVIOUS MEDIA

        private void Previous_Click(object? sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                PlayMedia(currentIndex - 1);
            }
            else if (mediaFiles.Count > 0)
            {
                PlayMedia(mediaFiles.Count - 1);
            }
        }

        // REMOVE SELECTED MEDIA

        private void RemoveMedia_Click(object? sender, EventArgs e)
        {
            int index = playlist.SelectedIndex;

            if (index < 0)
            {
                statusLabel.Text = "Select a media file first";
                return;
            }

            if (index == currentIndex)
            {
                player.Ctlcontrols.stop();

                currentIndex = -1;

                songTitle.Text = "No Media Selected";

                statusLabel.Text = "Media removed";

                playButton.Text = "? Play";

                progressBar.Value = 0;

                timeLabel.Text = "00:00 / 00:00";
            }

            mediaFiles.RemoveAt(index);

            playlist.Items.RemoveAt(index);

            if (currentIndex > index)
            {
                currentIndex--;
            }

            // Play first remaining media if necessary

            if (currentIndex == -1 &&
                mediaFiles.Count > 0)
            {
                PlayMedia(0);
            }
        }

        // MUTE AND UNMUTE

        private void Mute_Click(object? sender, EventArgs e)
        {
            player.settings.mute =
                !player.settings.mute;

            if (player.settings.mute)
            {
                muteButton.Text = "?? Unmute";
            }
            else
            {
                muteButton.Text = "?? Mute";
            }
        }

        // VOLUME CONTROL

        private void Volume_Scroll(object? sender, EventArgs e)
        {
            player.settings.volume = volumeBar.Value;

            if (volumeBar.Value == 0)
            {
                statusLabel.Text = "Volume: 0%";
            }
            else
            {
                statusLabel.Text =
                    "Volume: " + volumeBar.Value + "%";
            }
        }

        // =====================================
        // PART 3 - PLAYBACK PROGRESS
        // =====================================

        // UPDATE PLAYBACK INFORMATION

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (currentIndex < 0)
                return;

            try
            {
                double duration =
                    player.currentMedia.duration;

                double position =
                    player.Ctlcontrols.currentPosition;

                if (duration > 0)
                {
                    // UPDATE PROGRESS BAR

                    if (!dragging)
                    {
                        int progress = (int)(
                            position / duration * 1000
                        );

                        progress = Math.Max(
                            0,
                            Math.Min(1000, progress)
                        );

                        progressBar.Value = progress;
                    }

                    // CURRENT TIME

                    TimeSpan current =
                        TimeSpan.FromSeconds(position);

                    // TOTAL DURATION

                    TimeSpan total =
                        TimeSpan.FromSeconds(duration);

                    // DISPLAY TIME

                    timeLabel.Text =
                        FormatTime(current) +
                        " / " +
                        FormatTime(total);

                    // PLAYBACK STATUS

                    if (player.playState ==
                        WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        statusLabel.Text = "Playing";
                    }
                    else if (player.playState ==
                        WMPLib.WMPPlayState.wmppsPaused)
                    {
                        statusLabel.Text = "Paused";
                    }
                }
            }
            catch
            {
                // Media information may not be ready yet.
            }
        }

        // FORMAT TIME

        private string FormatTime(TimeSpan time)
        {
            if (time.TotalHours >= 1)
            {
                return time.ToString(@"hh\:mm\:ss");
            }

            return time.ToString(@"mm\:ss");
        }

        // SEEK MEDIA

        private void SeekMedia()
        {
            try
            {
                if (currentIndex < 0)
                    return;

                double duration =
                    player.currentMedia.duration;

                if (duration > 0)
                {
                    double newPosition =
                        duration *
                        progressBar.Value / 1000.0;

                    player.Ctlcontrols.currentPosition =
                        newPosition;
                }
            }
            catch
            {
                statusLabel.Text =
                    "Unable to seek this media";
            }
        }

        // HANDLE PLAYBACK ERRORS

        private void Player_ErrorEvent(
            object sender,
             EventArgs eventArgs)
        {
            statusLabel.Text = "Media playback error";

            MessageBox.Show(
                "The selected media could not be played.",
                "NEXUS Media Player",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        // APPLICATION CLOSING

        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            try
            {
                // STOP TIMER

                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }

                // STOP MEDIA PLAYER

                if (player != null)
                {
                    player.Ctlcontrols.stop();
                    player.close();
                }
            }
            catch
            {
                // Ignore shutdown errors.
            }

            base.OnFormClosing(e);
        }
    }
}
