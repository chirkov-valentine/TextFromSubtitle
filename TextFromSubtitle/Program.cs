using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextFromSubtitle
{
    class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {
            /*string pattern = @"(?:^\d.+$)|(?:\d+$)|(?:[01]\d|2[0123]):(?:[012345]\d):(?:[0123456789\,]+)|(?:\s{1}[-->]+\s{1})|(?:^\[Music\])|(?:^\[музыка\])|([\r\n]+)";
            string substitution = @" ";
            string bufferString = string.Empty;
            const int LINES_COUNT = 100;
            int lineCounter = 0;
            StringBuilder sb = new StringBuilder(bufferString);
            RegexOptions options = RegexOptions.Multiline;*/
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Subtitle files (*.srt) | *.srt;";
            if (fd.ShowDialog() == DialogResult.Cancel)
                return;
            string path = fd.FileName;
            string pathOut = Path.ChangeExtension(path, ".txt");

            if (File.Exists(pathOut))
            {
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                File.Delete(pathOut);
            }
            
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FileStream fsOut = File.Create(pathOut))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            using (StreamWriter sw = new StreamWriter(fsOut))
            {

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // 2 line
                    sr.ReadLine();
                    // 3 line
                    string input = sr.ReadLine();
                    if (!isMusicLine(input))
                    {
                        sw.Write(input);
                        sw.Write(" ");
                    }
                    // 4 line
                    sr.ReadLine();
                }
            }

        }

        static bool isMusicLine(string input)
        {
            RegexOptions options = RegexOptions.Singleline;
            string pattern = @"(?:^\[Music\])|(?:^\[музыка\])";
            Regex regex = new Regex(pattern, options);
            return regex.IsMatch(input);
                
        }





    }
}
