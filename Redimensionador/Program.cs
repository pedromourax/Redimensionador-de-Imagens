using System;
using System.Drawing;
using System.IO;
using System.Threading;


namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args) 
        {
            Console.WriteLine("Iniciando o redimensionador...");

            //Receber a altura que o usuário deseja
            //Console.WriteLine("Qual tamanho deseja redimensionar? em px");
            //int tamanho = Convert.ToInt32(Console.ReadLine());

            //Iniciando o redimensionador por meio do thread
            Thread thread = new Thread(Redimensionador);
            thread.Start();

            //Encerrar o processo
            Console.Read();
        }

        public static void Redimensionador()
        {
            string dir_entrada = "Entrada de Arquivos";
            string dir_saida = "Saida de Arquivos";
            string dir_redimensionado = "Arquivos Redimensionados";
            #region Criação de Diretórios
            if (!Directory.Exists(dir_entrada))
            {
                Directory.CreateDirectory(dir_entrada);
            }

            if (!Directory.Exists(dir_saida))
            {
                Directory.CreateDirectory(dir_saida);
            }

            if (!Directory.Exists(dir_redimensionado))
            {
                Directory.CreateDirectory(dir_redimensionado);
            }
            #endregion


            FileStream fileStream;
            FileInfo fileInfo;
            int tamanho = 200;


            while (true)
            {
                var arquivos = Directory.EnumerateFiles(dir_entrada);

                foreach ( var item in arquivos ) 
                {
                    fileStream = new FileStream(item, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(item);

                    string nonequal = DateTime.Now.Millisecond.ToString();

                    string diretorio1 = Environment.CurrentDirectory + @"\" + dir_redimensionado + @"\" + nonequal + fileInfo.Name;

                    //redimensionar
                    Redimensionamento(Image.FromStream(fileStream), tamanho, diretorio1);
                    //fechar o arquivo
                    fileStream.Close();

                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + dir_saida + @"\" + fileInfo.Name;

                    fileInfo.MoveTo(caminhoFinalizado);
                    //Console.WriteLine(item);
                    
                }

                Thread.Sleep(new TimeSpan(0,0,3));
            }

            static void Redimensionamento(Image imagem, int altura, string diretorio)
            {
                double ratio = (double)altura / imagem.Height;
                int newWidth = (int)(imagem.Width * ratio);
                int newHeight = (int)(imagem.Height * ratio);

                Bitmap novaImagem = new Bitmap(newWidth, newHeight);

                using(Graphics g = Graphics.FromImage(novaImagem))
                {
                    g.DrawImage(imagem, 0, 0, newWidth, newHeight);
                }

                novaImagem.Save(diretorio);
                novaImagem.Dispose();

            }
        }

    }
    
}