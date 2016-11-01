using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentsPDM_dll;
namespace ConsoleTestAplication
{
    class Program
    {
        static void Main(string[] args)
         {
            //    PDM pdm = new PDM();

            //    foreach (var item in pdm.SearchDoc("3535"))
            //    {
            //        Console.WriteLine(item.Path);
            //        pdm.DownLoadFile(item);
            //        Console.WriteLine(pdm.CloneDowladFileTo(@"D:\temp\", item));
            //       // Console.WriteLine(pdm.Get(item));
            //    }


            List<string> stringArray = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                stringArray.Add(900 + rnd.Next(0, 4) + "-" + rnd.Next(0, 100) + "-"+rnd.Next(0, 100));
            }

            //            stringArray.Add("900-11.00");
            //stringArray.Add("аббревиатура");
            //            stringArray.Add("абитуриент");
            //            stringArray.Add("абонемент");
            //            stringArray.Add("аборт");
            //            stringArray.Add("абракадабра");
            //            stringArray.Add("абсент");
            //            stringArray.Add("авария");
            //            stringArray.Add("авгур");
            //            stringArray.Add("автокефалия");
            //          stringArray.Add(  "агальма");
            //stringArray.Add("агломерат");
            //            stringArray.Add("аграф");
            //            stringArray.Add("адепт");
            //            stringArray.Add("адмирал");
            //            stringArray.Add("ажиотаж");
            //            stringArray.Add("азарт");
            //            stringArray.Add("акант");
            //            stringArray.Add("аквилон");
            //            stringArray.Add("аккламация");
            //            stringArray.Add("акколада");
            //            stringArray.Add("аккордеон");
            //            stringArray.Add("акрибия");
            //            stringArray.Add("акробатика");
            //            stringArray.Add("акростих");
            //            stringArray.Add("аксиология");
            //            stringArray.Add("акупунктура");
            //            stringArray.Add("акут");
            //            stringArray.Add("акушер");
            //            stringArray.Add("алеаторика");
            //            stringArray.Add("алембик");
            //            stringArray.Add("алиби");

            while (true)
            {
                Console.WriteLine("Введите сегмент имени");
                string str = Console.ReadLine();
                Console.WriteLine("Результат запроса");
                foreach (var item in stringArray.Where(s => s.Contains(str)))
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("Количесвто элементов " + stringArray.Where(s => s.Contains(str)).Count());
            }
            Console.ReadKey();
        }
    }
}
