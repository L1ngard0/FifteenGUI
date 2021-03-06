﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FifteenGUI
{
    public partial class Fifteen : Form
    {
        GameLibrary.Game game;
        Random Rand;  
        DateTime timeDate;
        TimeSpan sec;
        int count;
        
        public Fifteen()
        {
            InitializeComponent();
            game = new GameLibrary.Game(4);
            Rand = new Random();
            timer1.Interval = 500;
        }

        private void Fifteen_Load(object sender, EventArgs e)
        {
            const int n = 4;
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                {
                    Button button = new Button();
                    button.Dock = DockStyle.Fill;
                    button.Text = "*";
                    button.Font = new Font("Calibri", 30);
                    button.Tag = i + j * n;
                    button.Name = "button" + (n * j + i).ToString();
                    button.Click += Button_Click;
                    tableLayoutPanel1.Controls.Add(button, i, j);
                }
            this.ActiveControl = null;
            GameStart();
        }

        private void GameStart()
        {
            game.Start();
            for (int i = 0; i < Rand.Next(500); i++)
            {
                game.ShiftRandom();
            }
            RefreshButtonField();
            count = 0;
            CountStatus.Text = count.ToString();//счетчиков ходов
            timeDate = DateTime.Now;
            timer1.Start();//таймер времени в игре
        }

        private void startMenu_Click(object sender, EventArgs e)
        {
            GameStart();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            int position = Convert.ToInt32(((Button)sender).Tag);
            if (game.Shift(position))//перемещение кнопки в новую позицию
            {
                RefreshButtonField();
                count++;
                CountStatus.Text = count.ToString();
            }
            if (game.Check())//Завершение игры 
            {
                timer1.Stop();
                Win win = new Win(count, sec);//вывод окна с количеством ходов и временем игры
                win.ShowDialog();
                GameStart();
            }
            this.ActiveControl = null;
        }

        private Button GetButton(int index)
        {
            string buttons = "button" + index.ToString();
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                Button button = c as Button;
                if (button != null && button.Name == buttons)
                {
                    return button;
                }
            }
            return null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sec = DateTime.Now.Subtract(timeDate);
            TimeStatus.Text = sec.ToString(@"hh\:mm\:ss");
            
        }

        private void RefreshButtonField()
        {
            for (int position = 0; position < 16; position++)
            {
                GetButton(position).Text = game.GetNumber(position).ToString(); 
                GetButton(position).Visible = game.GetNumber(position) > 0; 
            }
        }

        private void отменаХодаToolStripMenuItem_Click(object sender, EventArgs e)//отмена хода 
        { 
            Restore();
        }

        private void Restore()
        {
            if (count > 0)
            {
                game.GameRestore();
                count--;
                CountStatus.Text = count.ToString();
                RefreshButtonField();
            }
        }
    }
}
