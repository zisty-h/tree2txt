using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tree2txt
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MainClass main = new MainClass();

            if (args.Length == 0 || args.Length == 1)
            {
                main.Help();
            }
            else
            {
                main.Main(args[0], args[1]);
            }
        }
    }

    class MainClass
    {
        public void Help()
        {
            Console.WriteLine("~.exe {Dir-Path} {Output text file's path}");
        }
        public void Main(string directoryPath, string outputFilePath)
        {
            try
            {
                // 出力ファイルが存在しない場合は新規作成する
                if (!File.Exists(outputFilePath))
                {
                    File.Create(outputFilePath).Close(); // ファイルを作成してすぐに閉じる
                }

                // ファイルに書き込む
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    writer.WriteLine($"Directory Tree of {directoryPath}");
                    writer.WriteLine("==============================");

                    // ディレクトリのツリーを再帰的に取得して書き込む
                    WriteDirectoryTree(writer, directoryPath, "");
                }

                Console.WriteLine($"Directory tree saved to {outputFilePath}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        static void WriteDirectoryTree(StreamWriter writer, string directoryPath, string indent)
        {
            try
            {
                // サブディレクトリとファイルを取得
                string[] subDirectories = Directory.GetDirectories(directoryPath);
                string[] files = Directory.GetFiles(directoryPath);

                // サブディレクトリを書き込む
                foreach (string subDir in subDirectories)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(subDir);
                    writer.WriteLine($"{indent}+-- {dirInfo.Name}");
                    WriteDirectoryTree(writer, subDir, indent + "    ");
                }

                // ファイルを書き込む
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    writer.WriteLine($"{indent}    |-- {fileInfo.Name}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Error: You do not have access privileges to this dir.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
