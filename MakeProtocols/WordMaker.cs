//using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
//using System.Windows;

namespace MakeProtocols
{
    public class WordMaker
    {
        ObservableCollection<Automat> Automats = new ObservableCollection<Automat>();
        ProtocolDocument protocolDocument = new ProtocolDocument();

        const double pointsForConvert = 28.328611898;

        string directoryPath = Environment.CurrentDirectory + "\\";
        string protocolName = "";

        public WordMaker(ObservableCollection<Automat> automats, ProtocolDocument _protocolDocument)
        {
            //Заполнение списка автоматов из конструктора
            for (int i = 0; i < automats.Count; i++)
                Automats.Add(automats[i].Clone());

            //Создание Общих сведений о протоколе из конструктора
            protocolDocument = _protocolDocument;

            //Создание директории для протоколов одного объекта (одного МСС)
            if (protocolDocument != null)
            {
                if (!string.IsNullOrEmpty(protocolDocument.ObjectProt))
                {
                    directoryPath += protocolDocument.ObjectProt + "\\";
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                }
            }

            //Полный путь будущего файла протокола
            Regex regex = new Regex(@"(?<=\d\d\.)\d\d(?=[Ээ])");
            protocolName = "№" + regex.Match(protocolDocument.Shifr).Value + " Протокол наладки модуля универсального " + protocolDocument.Shifr + ".docx";
        }

        //Создание файла Word и общие настройки
        public void CreateWordFile()
        {
            Document doc = new Document();
            Section section = doc.AddSection();

            try
            {
                section.PageSetup.Margins.All = Convert.ToSingle(1 * pointsForConvert); //Перевод в сантиметры
                section.PageSetup.Margins.Left = Convert.ToSingle(2 * pointsForConvert);

                //Верхний колонтитул с номером страницы
                HeaderFooter headersFooter = section.HeadersFooters.Header;
                Paragraph paragraphHeader = headersFooter.AddParagraph();
                paragraphHeader.AppendField("page number", FieldType.FieldPage);
                paragraphHeader.Format.HorizontalAlignment = HorizontalAlignment.Center;

                //Форматирование колонтитула со страницей
                ParagraphStyle style = new ParagraphStyle(doc);
                style.CharacterFormat.Bold = false;
                style.CharacterFormat.FontName = "Times New Roman";
                style.CharacterFormat.FontSize = 10;
                style.CharacterFormat.TextColor = Color.Black;
                doc.Styles.Add(style);
                paragraphHeader.ApplyStyle(style);

                //Создание параграфа с таблицей общей информации организации и объекта
                string[] data = {
                    @"Лаборатория: ООО  «ЭНКОН» 28 - ЭТЛ / 765. 454000, Челябинск, Свердловский проспект, д.2, оф.325А",
                    @"Решение о регистрации электролаборатории рег. №28-ЭТЛ\765 от «25» октября 2021 г.",
                    @"Заказчик: ООО «АмурМинералс»",
                    @"Объект: "+ protocolDocument.ObjectProt,
                    @"Дата: " + protocolDocument.DateProt
                    };

                Table orgTable = section.AddTable(true);
                orgTable.ResetCells(5, 1);
                for (int i = 0; i < data.Length; i++)
                {
                    TableRow tr = orgTable.Rows[i];
                    Paragraph p = tr.Cells[0].AddParagraph();
                    tr.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    TextRange txtRange = p.AppendText(data[i]);
                    txtRange.CharacterFormat.FontName = "Times New Roman";
                    txtRange.CharacterFormat.FontSize = 11;
                }

                //Параграф с шапкой протокола
                section.AddParagraph();
                section.AddParagraph();
                Paragraph paragraph = section.AddParagraph();
                paragraph.AppendText("ПРОТОКОЛ №" + protocolDocument.NumbProt);
                style = new ParagraphStyle(doc);
                style.CharacterFormat.Bold = true;
                style.CharacterFormat.FontName = "Times New Roman";
                style.CharacterFormat.FontSize = 14;
                style.CharacterFormat.TextColor = Color.Black;
                style.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                doc.Styles.Add(style);
                paragraph.ApplyStyle(style);

                paragraph = section.AddParagraph();
                paragraph.AppendText("Наладки модулей унифицированных");
                style = new ParagraphStyle(doc);
                style.CharacterFormat.Bold = true;
                style.CharacterFormat.FontName = "Times New Roman";
                style.CharacterFormat.FontSize = 12;
                style.CharacterFormat.TextColor = Color.Black;
                style.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                doc.Styles.Add(style);
                paragraph.ApplyStyle(style);

                paragraph = section.AddParagraph();
                paragraph.AppendText(protocolDocument.Modules);
                paragraph.ApplyStyle(style);

                section.AddParagraph();
                section.AddParagraph();

                //Создание автоматического списка с общими данными протокола
                ParagraphStyle regularStyleNumb_1 = new ParagraphStyle(doc);
                regularStyleNumb_1.CharacterFormat.Bold = false;
                regularStyleNumb_1.CharacterFormat.FontName = "Times New Roman";
                regularStyleNumb_1.CharacterFormat.FontSize = 12;
                regularStyleNumb_1.CharacterFormat.TextColor = Color.Black;
                regularStyleNumb_1.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                regularStyleNumb_1.ParagraphFormat.FirstLineIndent = Convert.ToSingle(0.63 * pointsForConvert);
                doc.Styles.Add(regularStyleNumb_1);

                data = new string[] {
                    "1.  Протокол касается только объекта, подвергнутого измерениям.",
                    "2.  Протокол оформлен в соответствии с требованиями ГОСТ Р 50571.16-2007.",
                    "3.  Проектная документация: Шифр 3760-4.1.12-ЭМ АО Механобр Инжиниринг.",
                    "4.  Должно соответствовать требованиям ПУЭ 1.8, инструкции по эксплуатации.",
                    "5.  Климатические условия проведения испытаний: температура +20 °С",
                    "6.  Цель проведения испытания: приемо-сдаточные.",
                    "7.  Общие данные:",
                    "    Принципиальные схемы АО «Электронмаш». " + protocolDocument.Shifr,
                    "    Номинальное напряжение 0,4 кВ",
                    "8.  Испытание оборудования",
                    "    8.1. Паспортные данные оборудования",
                    "    8.1.1. Данные автоматических выключателей",

                };
                for (int i = 0; i < data.Length; i++)
                {
                    paragraph = section.AddParagraph();
                    paragraph.AppendText(data[i]);
                    paragraph.ApplyStyle(regularStyleNumb_1);
                }
                //Таблица данных автоматических выключателей
                ParagraphStyle regularStyleTable_1 = new ParagraphStyle(doc);
                regularStyleTable_1.Name = "regularStyleTable_1";
                regularStyleTable_1.CharacterFormat.Bold = false;
                regularStyleTable_1.CharacterFormat.FontName = "Times New Roman";
                regularStyleTable_1.CharacterFormat.FontSize = 11;
                regularStyleTable_1.CharacterFormat.TextColor = Color.Black;
                regularStyleTable_1.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                regularStyleTable_1.ParagraphFormat.FirstLineIndent = Convert.ToSingle(0 * pointsForConvert);
                doc.Styles.Add(regularStyleTable_1);

                Table commonAutomatsTable = section.AddTable(true);
                commonAutomatsTable.ResetCells(Automats.Count + 1, 6);

                string[] Header = { @"Фидер/монтаж. символ", "Тип", "Зав. № автомата", "Ном. Ток, А", "Ном. напряж., кВ", "Тип расцепителя" };
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = commonAutomatsTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }

                for (int r = 0; r < Automats.Count; r++)
                {
                    TableRow tr = commonAutomatsTable.Rows[r + 1];

                    Paragraph p = tr.Cells[0].AddParagraph();
                    tr.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Automats[r].NameAutomat);
                    p.ApplyStyle(regularStyleTable_1);

                    p = tr.Cells[1].AddParagraph();
                    tr.Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    txtRange = p.AppendText(Automats[r].Type);
                    p.ApplyStyle(regularStyleTable_1);

                    p = tr.Cells[2].AddParagraph();
                    tr.Cells[2].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    txtRange = p.AppendText(Automats[r].VendorNumb);
                    p.ApplyStyle(regularStyleTable_1);

                    p = tr.Cells[3].AddParagraph();
                    tr.Cells[3].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    txtRange = p.AppendText(Automats[r].NominalCurrent);
                    p.ApplyStyle(regularStyleTable_1);

                    p = tr.Cells[4].AddParagraph();
                    tr.Cells[4].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    txtRange = p.AppendText(Automats[r].NominalVoltage);
                    p.ApplyStyle(regularStyleTable_1);

                    p = tr.Cells[5].AddParagraph();
                    tr.Cells[5].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    txtRange = p.AppendText(Automats[r].TypeBreaker);
                    p.ApplyStyle(regularStyleTable_1);

                }

                //Параграфы с общими данными и шифром карты уставок

                //Таблица с уставками для автоматов

                //Таблица Результаты испытаний 

                //Текст про проверку изоляции и проверку реле

                //Параграф про проверку автоматических выключателей

                //Таблица проверки автоматов

                //Текст Проверка действия и состояния схемы

                // таблица Проверка изоляции

                //Текст про испытание повышенным напряжением 

                //Текст применение испытательного оборудования

                //таблица испытательного оборудования

                //Текст заключение и кто выполнил проверку
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при создании файла Word в классе WordMaker\n\n" + ex.ToString(), "Ошибка!");
            }

            doc.SaveToFile(directoryPath + protocolName);

            doc.Dispose();

            System.Windows.MessageBox.Show("Создание файла успешно завершено!", "Выполнено!");
        }


        //Начало 
    }
}
