using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slider1
{
    public partial class Form1 : Form
    {
        string imagePath = @"./Images/";                    //путь папки с картиночками
        string filePath = @"./";                            //путь папки с exe
        string[] fileName;                                  //имя файла для чтения
        string[] image;                                     //имя  картинки для чтения

        List<string> lines = new List<string>();
        IniFile MyIni = new IniFile(@"./setting.ini");

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;           //Окно на весь экран

            var watcher = new FileSystemWatcher(filePath);          //путь файлвотчера
            watcher.EnableRaisingEvents = true;                     //ивенты файлвотчера
            watcher.SynchronizingObject = this;                     //синхронизация с обьектом
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; //фильтры оповещений

            watcher.Changed += OnChanged;                   //проверки на действия
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            watcher.Filter = "*.txt";                      //фильтр имени

            InitializeComponent();
            SetSize();
            LoadParams();
            CheckFile();


            ToolStripMenuItem changeText = new ToolStripMenuItem("Настройка шрифтов");
            ToolStripMenuItem quitItem = new ToolStripMenuItem("Выход");
            ToolStripMenuItem bgcolor = new ToolStripMenuItem("Цвет фона");
            ToolStripMenuItem bgimage = new ToolStripMenuItem("Картинка на фон");

            //добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { changeText, bgcolor, bgimage, quitItem });

            //ассоциируем контекстное меню с текстовым полем
            pictureBox1.ContextMenuStrip = contextMenuStrip1;
            groupBox1.ContextMenuStrip = contextMenuStrip1;

            //устанавливаем обработчики событий для меню
            changeText.Click += changeText_Click;
            quitItem.Click += quitItem_Click;
            bgcolor.Click += bgcolor_Click;
            bgimage.Click += bgimageToolStripMenuItem_Click;
        }


        private int ImageNumber = 1;

        private void LoadNextImages()                       //грузим фотки по таймеру
        {
            image = Directory.GetFiles(imagePath, "*.jpg");

            timer1.Start();
            ImageNumber++;
            if (ImageNumber == image.Length)
            {
                ImageNumber = 1;
            }
            for (int i = 0; i <= ImageNumber; i++)
            {
                pictureBox1.ImageLocation = image[i];
            }
        }

        private void CheckFile()                            //проверка файла
        {
            fileName = Directory.GetFiles(filePath, "*.txt").OrderBy(f => new FileInfo(f).CreationTime).ToArray();
            if (fileName.Length != 0)    //если файл сущесвует
            {
                 ReadFile();
            }
            else                                            //если файл отсутствует
            {
                lines.Clear();                              //чистим строки и текст
                label1.Text = "";
                groupBox1.Visible = false;                  //скрываем инфу
            }
        }

        private void ReadFile()                             //чтение файла
        {
            label1.Text = null;
            string[] line = File.ReadAllLines(fileName.Last());    //читаем строки
            int count = File.ReadAllLines(fileName.Last()).Length; //кол-во строк

            foreach (string s in line)                      //строки в список
            {
                lines.Add(s);
            }

            for (int i = 0; i < count; i++)                   //вывод в текст
            {
                label1.Text += lines[i] + "\n";
            }

            if(label1.Text=="")
                 groupBox1.Visible = false;                   //показываем инфу
            if(label1.Text!="")
                 groupBox1.Visible = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)           //проверка  изменения  
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            lines.Clear();
            ReadFile();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)          //проверка создания
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
            {
                return;
            }
            CheckFile();

        }

        private void OnDeleted(object sender, FileSystemEventArgs e)          //проверка удаления
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                return;
            }
            fileName = null;
            CheckFile();
        }

        private void OnRenamed(object sender, RenamedEventArgs e)          //проверка удаления
        {
            fileName = null;
            CheckFile();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadNextImages();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void SetSize()
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length <= 1)
            {
                pictureBox1.Width = Screen.PrimaryScreen.Bounds.Width;
                pictureBox1.Height = Screen.PrimaryScreen.Bounds.Height;
            }
            else
            {
                pictureBox1.Width = Screen.AllScreens[1].Bounds.Width;
                pictureBox1.Height = Screen.AllScreens[1].Bounds.Height;
            }
        }

        private void настройкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            label1.Font = fontDialog1.Font;
        }

        private void pictureBox1_MouseMove(object sender, EventArgs e)
        {
            if (MousePosition.Y <= menuStrip1.Height && !menuStrip1.Visible)
                menuStrip1.Visible = true;
            if (MousePosition.Y > menuStrip1.Height && menuStrip1.Visible)
                menuStrip1.Visible = false;
        }

        private void changeText_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            label1.Font = fontDialog1.Font;
            if (DialogResult != DialogResult.Cancel)
            {
                MyIni.Write("font", fontDialog1.Font.FontFamily.Name);
                MyIni.Write("fontSize", fontDialog1.Font.Size.ToString());
            }
        }

        private void bgcolor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            groupBox1.BackColor = colorDialog1.Color;
        }

        public Image ImageOpen(string filename)
        {
            return Image.FromFile(filename);
        }
        private void bgimageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
            groupBox1.BackgroundImage = ImageOpen(openFileDialog1.FileName);
        }

        private void LoadParams()
        {
            if (MyIni.KeyExists("fontSize"))
            {
                float x = float.Parse(MyIni.Read("fontSize"));

                label1.Font = new Font(MyIni.Read("font"), x);
            }
            else
                label1.Font = new Font("Consolas", 12);
        }
        //выход из приложения(пожилого)
        private void quitItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
 
}
