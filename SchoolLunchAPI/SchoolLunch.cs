using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolLunchAPI
{
    public class 급식
    {
        public int 날짜 = 0;
        public string 급식메뉴 = "";


        public 급식(int 날짜, string 급식메뉴)
        {
            this.날짜 = 날짜;
            this.급식메뉴 = 급식메뉴;
        }
        public 급식()
        {

        }
    }

    public class SchoolLunch
    {
         static bool CheckNumber(string letter)
        {
            bool IsCheck = true;

            Regex numRegex = new Regex(@"[0-9]");
            Boolean ismatch = numRegex.IsMatch(letter);

            if (!ismatch)
            {
                IsCheck = false;
            }

            return IsCheck;
        }

        public static List<급식> 급식불러오기(int Years, int Month, string ID, 관할지역 지역, 학교종류 종류)
        {
            string ResultOfstring = "0";
            using (WebClient client = new WebClient())
            {
                string 어디교육청 = "";
                switch (지역)
                {
                    case 관할지역.서울특별시:
                        어디교육청 = "stu.sen.go.kr";
                        break;
                    case 관할지역.인천광역시:
                        어디교육청 = "stu.ice.go.kr";
                        break;
                    case 관할지역.부산광역시:
                        어디교육청 = "stu.pen.go.kr";
                        break;
                    case 관할지역.광주광역시:
                        어디교육청 = "stu.gen.go.kr";
                        break;
                    case 관할지역.대전광역시:
                        어디교육청 = "stu.dje.go.kr";
                        break;
                    case 관할지역.대구광역시:
                        어디교육청 = "stu.dge.go.kr";
                        break;
                    case 관할지역.세종특별자치시:
                        어디교육청 = "stu.sje.go.kr";
                        break;
                    case 관할지역.울산광역시:
                        어디교육청 = "stu.use.go.kr";
                        break;
                    case 관할지역.경기도:
                        어디교육청 = "stu.goe.go.kr";
                        break;
                    case 관할지역.강원도:
                        어디교육청 = "stu.kwe.go.kr";
                        break;
                    case 관할지역.충청북도:
                        어디교육청 = "stu.cbe.go.kr";
                        break;
                    case 관할지역.충청남도:
                        어디교육청 = "stu.cne.go.kr";
                        break;
                    case 관할지역.경상북도:
                        어디교육청 = "stu.gbe.go.kr";
                        break;
                    case 관할지역.경상남도:
                        어디교육청 = "stu.gne.go.kr";
                        break;
                    case 관할지역.전라북도:
                        어디교육청 = "stu.jbe.go.kr";
                        break;
                    case 관할지역.전라남도:
                        어디교육청 = "stu.jne.go.kr";
                        break;
                    case 관할지역.제주도:
                        어디교육청 = "stu.jje.go.kr";
                        break;
                }
                string[] 배열 = null;
                client.Encoding = Encoding.UTF8;
                if (Month.ToString().Length == 1)
                {
                    ResultOfstring = "0" + Month;
                }
                else
                {
                    ResultOfstring = Month.ToString();
                }
                string htmlCode = client.DownloadString("http://" + 어디교육청 + "/sts_sci_md00_001.do?schulCode=" + ID + "&schulCrseScCode=" + Convert.ToInt32(종류) + "&schulKndScCode=0" + Convert.ToInt32(종류) + "&ay=" + Years + "&mm=" + ResultOfstring + "&");
                htmlCode = htmlCode.Remove(0, htmlCode.IndexOf("tbody"));
                //  Console.WriteLine(htmlCode);
                htmlCode = htmlCode.Remove(htmlCode.IndexOf("/tbody"));
                htmlCode = htmlCode.Replace("\t", "");
                htmlCode = htmlCode.Replace("\r\n", "");
                htmlCode = htmlCode.Replace("<td><div>", ":");
                // htmlCode = htmlCode.Replace("<br />", "");
                htmlCode = htmlCode.Replace("</div></td>", "");
                htmlCode = htmlCode.Replace(@"<td class=""last""><div>", "");
                htmlCode = htmlCode.Replace("t", "");
                htmlCode = htmlCode.Replace("ody", "");
                int 날짜 = 0;
                List<급식> 내용 = new List<급식>();
                배열 = htmlCode.Split("<br />".ToCharArray()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                for (int i = 1; i < 배열.Length; i++)
                {
                    if (배열[i - 1].LastIndexOf(":") != -1)
                    {
                        if (배열[i - 1].Remove(0, 배열[i - 1].LastIndexOf(":") + 1) != "")
                        {
                            날짜 = Convert.ToInt32(배열[i - 1].Remove(0, 배열[i - 1].LastIndexOf(":") + 1));
                            배열[i - 1] = 배열[i - 1].Remove(배열[i - 1].LastIndexOf(":"));
                        }
                        내용.Add(new 급식(날짜, 배열[i - 1]));
                    }
                    else
                    {
                        내용.Add(new 급식(날짜, 배열[i - 1]));
                    }
                }
                List<급식> 결과 = new List<급식>();
                급식 temp = new 급식();
                string temp2 = "";
                for (int i = 0; i < 내용.Count; i++)
                {
                    if (i == 0)
                    {
                        temp = 내용[i];
                    }
                    if (내용[i].날짜 == temp.날짜)
                    {
                        temp2 = temp2 + "\r\n" + 내용[i].급식메뉴;
                    }
                    else
                    {
                        if (내용[i].급식메뉴.Length > 0 && temp2.Length - 1 > 0 && temp2.Length - 2 > 0)
                        {
                            if (내용[i].급식메뉴[0] != ':')
                            {
                                temp2 = temp2 + "\r\n" + 내용[i].급식메뉴;
                            }
                            if (CheckNumber(temp2[temp2.Length - 1].ToString()) && CheckNumber(temp2[temp2.Length - 2].ToString()))
                            {
                                temp2 = temp2.Remove(temp2.Length - 2, 2);
                            }
                            else if (CheckNumber(temp2[temp2.Length - 1].ToString()))
                            {
                                temp2 = temp2.Remove(temp2.Length - 1, 1);
                            }
                        }
                        결과.Add(new 급식(temp.날짜, temp2));
                        temp2 = "";

                        temp = 내용[i];
                    }
                }
                결과.Add(new 급식(temp.날짜, temp2));
                
                return 결과;
            }
        }
    }
}
