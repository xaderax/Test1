using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace Test
{
    public partial class Form1 : Form
    {
        //static string temp = "ItemLink(\"  \",\"../aip/gen/gen0/1-gen0-1.pdf\",\"GEN 0.1 Предисловие\");";
        
        static string path = "c:\\AIP";
        public static string url = "http://www.caiga.ru/common/AirInter/validaip/html/menurus.htm";
        static string aip1path = "http://www.caiga.ru/common/AirInter/validaip";
        public static string filename = "test.txt";
        WebClient myWebClient = new WebClient();
            public static string[] lines = File.ReadAllLines(filename, Encoding.Default);
        static public DirectoryInfo dir = new DirectoryInfo(path);
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string temp in lines)
            {

                if (temp.StartsWith("ItemBegin"))
                {
                    richTextBox1.AppendText("Обнаружен новый элемент дерева" + Environment.NewLine);
                    richTextBox1.AppendText("Parsing string:\"" + temp + Environment.NewLine);
                    MatchCollection results = Regex.Matches(temp, "\\(.*\"..(?<URL>.*)\".*\"(?<Name>.*)\".*\\)");
                    Match r = results[0];
                    //richTextBox1.AppendText(path);
                    richTextBox1.AppendText("URL:\"" + r.Groups["URL"].Value + "\". Name:\" " + r.Groups["Name"].Value + "\"" + Environment.NewLine);

                    path = path + "\\" + r.Groups["Name"].Value.Replace("/", "!");
                    //richTextBox1.AppendText(path);
                    Directory.CreateDirectory(path);
                    richTextBox1.AppendText("Создаем каталог \"" + r.Groups["Name"].Value + "\" в \"" + Directory.GetParent(path).FullName + "\"" + Environment.NewLine);
                    richTextBox1.AppendText("Текущий каталог:\"" + path + "\"" + Environment.NewLine + Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                }
                else if (temp.StartsWith("ItemLink"))
                {
                    richTextBox1.AppendText("Обнаружен новый документ" + Environment.NewLine);
                    richTextBox1.AppendText("Parsing string:\"" + temp + Environment.NewLine);
                    MatchCollection results = Regex.Matches(temp, "\\(.*\"..(?<URL>.*)\".*\"(?<Name>.*)\".*\\)");
                    Match r = results[0];
                    richTextBox1.AppendText("URL:\"" + r.Groups["URL"].Value + "\". Name:\" " + r.Groups["Name"].Value + "\"" + Environment.NewLine);
                    richTextBox1.AppendText("Сохраняем файл:\"" + r.Groups["Name"].Value.Replace("/", "!") + ".pdf" + "\" в каталог :\"" + path + "\"" + Environment.NewLine);
                    myWebClient.DownloadFile(aip1path + r.Groups["URL"].Value, path + "\\" + r.Groups["Name"].Value.Replace("/", "!") + ".pdf");
                    richTextBox1.AppendText("Готово!" + Environment.NewLine + Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                }
                else if (temp.StartsWith("ItemEnd"))
                {
                    richTextBox1.AppendText(" Обнаружен конец ветви. Возврат к предыдущему каталогу" + Environment.NewLine);
                    path = Directory.GetParent(path).FullName;
                    //path = Directory.GetParent(path).FullName;
                    richTextBox1.AppendText("Текущий каталог:\"" + path + "\"" + Environment.NewLine + Environment.NewLine);
                    richTextBox1.ScrollToCaret();
                }
            }
        }
    }
} 
