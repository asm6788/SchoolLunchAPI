using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SchoolLunchAPI;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
           var 결과 =   SchoolLunch.급식불러오기(2016, 10, "F100000401", 관할지역.광주광역시, 학교종류.중학교);
      
            for (int i = 1; i < 결과.Count; i++)
            {
                Console.WriteLine(결과[i].날짜 + "일의 급식" + 결과[i].급식메뉴);
            }
            Console.Read();
        }
    }
}
