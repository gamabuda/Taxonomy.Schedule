using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Taxonomy.Schedule.Wpf.Data;
using win = System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;

namespace Taxonomy.Schedule.Wpf.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReqstDocumentWindow.xaml
    /// </summary>
    public partial class ReqstDocumentWindow : Window
    {
        public Chat Chat { get; set; }
        public ReqstDocumentWindow(Chat chat)
        {
            InitializeComponent();

            Chat = chat;
            this.DataContext = Chat;

            DateRun.Text = DateTime.Now.ToShortDateString();
        }

        public ReqstDocumentWindow(Chat chat, DateTime dateTime1, DateTime dateTime2)
        {
            InitializeComponent();

            Chat = chat;
            this.DataContext = Chat;

            DateRun.Text = DateTime.Now.ToShortDateString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(dp1.SelectedDate == null || dp2.SelectedDate == null)
            {
                dp1.BorderBrush = System.Windows.Media.Brushes.Red;
                dp2.BorderBrush = System.Windows.Media.Brushes.Red;
                return;
            }
            win.FolderBrowserDialog dialog = new FolderBrowserDialog();

            win.DialogResult result = dialog.ShowDialog();

            if (result == win.DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                // Выбранный путь к папке
                selectedPath += @"\\doucement";

                // Создание нового документа
                Document document = new Document();

                // Создание объекта для записи в файл
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(selectedPath, FileMode.Create));

                // Открытие документа для записи
                document.Open();

                BaseFont baseFont = BaseFont.CreateFont("c:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont);
                // Добавление данных в документ
                var date1 = dp1.SelectedDate;

                var title = new iTextSharp.text.Paragraph(new Phrase("ПРИКАЗ (распорежение)", font));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);
                document.Add(new iTextSharp.text.Paragraph(new Phrase($"Предоставить {Chat.Request.Title} наемному сторуднику {Chat.Employee.Surname} {Chat.Employee.Name} должностью {Chat.Employee.Post.Name} за период работы с {date1.Value.ToShortDateString()} по {dp1.SelectedDate.Value.ToShortDateString()}", font)));
                
                var sub3 = new iTextSharp.text.Paragraph(new Phrase($"Руководитель: {Chat.Employee1.Surname} {Chat.Employee1.Name}", font));
                sub3.Alignment = Element.ALIGN_RIGHT;
                document.Add(sub3);

                var sub1 = new iTextSharp.text.Paragraph(new Phrase($"Подпись: _________________________", font));
                sub1.Alignment = Element.ALIGN_RIGHT;
                document.Add(sub1);

                var sub2 = new iTextSharp.text.Paragraph(new Phrase($"С приказом(распорежением) работник ознакомлен: _________________________", font));
                sub2.Alignment = Element.ALIGN_RIGHT;
                document.Add(sub2);
                document.Add(new iTextSharp.text.Paragraph(new Phrase(DateRun.Text = DateTime.Now.ToShortDateString(), font)));
                // Закрытие документа
                document.Close();

                // Открытие созданного PDF-файла
                System.Diagnostics.Process.Start(selectedPath);
            }

            this.Close();
        }
    }
}
