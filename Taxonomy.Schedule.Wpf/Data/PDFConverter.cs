using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.IO.Packaging;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;
using System.Windows.Xps;

namespace Taxonomy.Schedule.Wpf.Data
{
    public static class PDFConverter
    {
        public static void ConvertToPdf(Window window, string filePath)
        {
            // Создание документа PDF
            Document document = new Document();

            try
            {
                // Инициализация писателя PDF с указанием пути к файлу
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                // Открытие документа для записи
                document.Open();

                // Создание элемента PDF для содержимого окна WPF
                using (MemoryStream stream = new MemoryStream())
                {
                    // Создание элемента FixedDocument из окна WPF
                    FixedDocument fixedDoc = CreateFixedDocument(window);

                    // Конвертация FixedDocument в XPS-документ
                    using (Package package = Package.Open(stream, FileMode.Create))
                    {
                        using (XpsDocument xpsDoc = new XpsDocument(package, CompressionOption.Maximum))
                        {
                            XpsDocumentWriter xpsWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                            xpsWriter.Write(fixedDoc.DocumentPaginator);
                        }
                    }

                    // Конвертация XPS-документа в PDF-документ
                    PdfReader reader = new PdfReader(stream.ToArray());
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.DirectContent.AddTemplate(page, 0, 0);
                    }
                }
            }
            finally
            {
                // Закрытие документа PDF
                document.Close();
            }
        }

        private static FixedDocument CreateFixedDocument(Window window)
        {
            // Создание FixedDocument
            FixedDocument fixedDoc = new FixedDocument();

            // Установка размера страницы и маржинов
            fixedDoc.DocumentPaginator.PageSize = new Size(window.Width, window.Height);
            Thickness pageMargin = (window.Content as FrameworkElement).Margin;

            // Создание страницы с контейнером для содержимого окна
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            // Установка маргинов страницы
            fixedPage.Margin = new Thickness(pageMargin.Left, pageMargin.Top, pageMargin.Right, pageMargin.Bottom);

            if (window.Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(window);
            }
            else if (window.Parent is Decorator parentDecorator)
            {
                parentDecorator.Child = null;
            }

            // Копирование содержимого окна в FixedPage
            UIElement content = window.Content as UIElement;
            
            fixedPage.Children.Add(content);
            fixedPage.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            fixedPage.Arrange(new Rect(new Point(), fixedPage.DesiredSize));

            // Добавление FixedPage в FixedDocument
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            return fixedDoc;
        }
    }
}
