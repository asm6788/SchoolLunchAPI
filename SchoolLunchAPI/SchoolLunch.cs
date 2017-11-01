using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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
    public class 학교정보
    {
        // string 지역;
        public 학교종류 초중고;
        public string 학교이름;
        public string 학교코드;
        // string 학생수및성비;
        public string 학교주소;
        //   string 교원수및성비;
        //  string 전화;
        // string 팩스;
        // string 설립구분;
        // string 설립유형;
        public string 홈페이지;

        public 학교정보(string 홈페이지, string 학교주소, string 학교코드, string 학교이름, 학교종류 초중고)
        {
            this.홈페이지 = 홈페이지;
            this.학교주소 = 학교주소;
            this.학교코드 = 학교코드;
            this.학교이름 = 학교이름;
            this.초중고 = 초중고;
        }
    }
    public class SchoolLunch
    {
        static bool isContainHangul(string s)
        {

            char[] charArr = s.ToCharArray();
            foreach (char c in charArr)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    return true;
                }

            }
            return false;
        }
        static int CheckDigit(string input)
        {
            bool first = false;
            bool twice = false;
            if (Regex.IsMatch(input.Substring(input.Length - 1), @"^\d+$"))
            {
                first = true;

            }
            if (input.Length >= 2)
            {
                if (Regex.IsMatch(input.Substring(input.Length - 2), @"^\d+$"))
                {
                    twice = true;

                }
            }
            if (first && twice)
            {
                return 2;
            }
            else if (first)
            {
                return 1;
            }
            return 0;

        }
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
            List<급식> 결과 = new List<급식>();
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
                            어디교육청 = "stu.gbe.kr";
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
                        if (CheckDigit(배열[i - 1]) != 0)
                        {
                            if (배열[i - 1].Remove(0, 배열[i - 1].Length - CheckDigit(배열[i - 1])) != "")
                            {
                                날짜 = Convert.ToInt32(배열[i - 1].Remove(0, 배열[i - 1].Length - CheckDigit(배열[i - 1])));
                                배열[i - 1] = 배열[i - 1].Remove(배열[i - 1].Length - CheckDigit(배열[i - 1]), CheckDigit(배열[i - 1]));
                            }
                            내용.Add(new 급식(날짜, 배열[i - 1]));
                        }
                        else
                        {
                            내용.Add(new 급식(날짜, 배열[i - 1]));
                        }
                    }
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
                                    temp2 = temp2 + "\r\n" + 내용[i].급식메뉴.Remove(내용[i].급식메뉴.Length - 1, 1);
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
                            결과.Add(new 급식(temp.날짜, temp2.Replace(":", "")));
                            temp2 = "";

                            temp = 내용[i];
                        }
                    }
                    결과.Add(new 급식(temp.날짜, temp2));
                    결과 = 결과.Where(s => !string.IsNullOrWhiteSpace(s.급식메뉴)).Distinct().ToList();
                    결과.RemoveAll(x => x.날짜 < 1);
                    결과.RemoveAll(x => x.날짜 > 31);
                   
                }
            return 결과;
        }
        public static List<학교정보> 학교정보파싱(string 검색학교)
        {
            HttpWebRequest wReq;
            Stream PostDataStream;
            Stream respPostStream;
            StreamReader readerPost;
            HttpWebResponse wResp;
            StringBuilder postParams = new StringBuilder();
            List<학교정보> 학교정보들 = new List<학교정보>();
            string 학교이름 = "";
            string 학교코드 = "";
            string 학교주소 = "";
            string 학교홈페이지 = "";
            학교종류 초중고 = 학교종류.None;
            //SEARCH_GS_HANGMOK_CD=&SEARCH_GS_HANGMOK_NM=&SEARCH_SCHUL_NM=%BF%F9%B0%E8%C1%DF&SEARCH_GS_BURYU_CD=&SEARCH_KEYWORD=%BF%F9%B0%E8%C1%DF
            //보낼 데이터 추
            postParams.Append("SEARCH_GS_HANGMOK_CD=");
            postParams.Append("&SEARCH_GS_HANGMOK_NM=");
            postParams.Append("&SEARCH_SCHUL_NM=" + HttpUtility.UrlEncode(검색학교, Encoding.GetEncoding("euc-kr")));
            postParams.Append("&SEARCH_GS_BURYU_CD=");
            postParams.Append("&SEARCH_KEYWORD=" + HttpUtility.UrlEncode(검색학교, Encoding.GetEncoding("euc-kr")));

            //Encoding 정의 및 보낼 데이터 정보를 Byte배열로 변환(String -> Byte[])
            Encoding encoding = Encoding.UTF8;
            byte[] result = encoding.GetBytes(postParams.ToString());
            //<p class="School_Division">
            //보낼 곳과 데이터 보낼 방식 정의
            wReq = (HttpWebRequest)WebRequest.Create("http://www.schoolinfo.go.kr/ei/ss/Pneiss_f01_l0.do");
            wReq.Method = "POST";
            wReq.ContentType = "application/x-www-form-urlencoded";
            wReq.ContentLength = result.Length;

            string temp;
            //데이터 전송
            PostDataStream = wReq.GetRequestStream();
            PostDataStream.Write(result, 0, result.Length);
            PostDataStream.Close();
            wResp = (HttpWebResponse)wReq.GetResponse();
            respPostStream = wResp.GetResponseStream();
            readerPost = new StreamReader(respPostStream, Encoding.Default);
            String resultPost = readerPost.ReadToEnd();
            //     Console.WriteLine(resultPost);
            while (true)
            {
                resultPost = resultPost.Remove(0, resultPost.IndexOf("School_Name")).Remove(0, 76);
                temp = resultPost;
                학교이름 = resultPost = resultPost.Remove(resultPost.IndexOf("<"), resultPost.Length - resultPost.IndexOf("<"));
                if (!isContainHangul(학교이름))
                {
                    break;
                }
                resultPost = temp;
                resultPost = resultPost.Remove(0, resultPost.IndexOf("School_Division"));
                resultPost = resultPost.Remove(0, 45);
                resultPost = resultPost.Remove(0, resultPost.IndexOf("mapD_Class"));
                resultPost = resultPost.Remove(0, 16);
                temp = resultPost;
                resultPost = resultPost.Remove(resultPost.IndexOf("</span>"), resultPost.Length - resultPost.IndexOf("</span>"));
                if (resultPost == "초")
                {
                    초중고 = 학교종류.초등학교;
                }
                else if (resultPost == "중")
                {
                    초중고 = 학교종류.중학교;
                }
                else if (resultPost == "고")
                {
                    초중고 = 학교종류.고등학교;
                }
                resultPost = temp;
                resultPost = resultPost.Remove(0, resultPost.IndexOf("searchSchul")).Remove(0, 12);
                temp = resultPost;
                resultPost = resultPost.Remove(resultPost.IndexOf(")"), resultPost.Length - resultPost.IndexOf(")")).Replace("'", "");
                학교코드 = resultPost;
                resultPost = temp;
                resultPost = resultPost.Remove(0, resultPost.IndexOf("학교주소")).Remove(0, 11);
                temp = resultPost;
                resultPost = resultPost.Remove(resultPost.IndexOf("</li>"), resultPost.Length - resultPost.IndexOf("</li>"));
                학교주소 = resultPost;
                resultPost = temp;
                //  Console.WriteLine(resultPost);
                resultPost = resultPost.Remove(0, resultPost.IndexOf("홈페이지")).Remove(0, 38);
                temp = resultPost;
                resultPost = resultPost.Remove(resultPost.IndexOf("target"), resultPost.Length - resultPost.IndexOf("target"));
                resultPost = resultPost.Remove(resultPost.Length - 2, 1);
                학교홈페이지 = resultPost;
                resultPost = temp;
                학교정보들.Add(new 학교정보(학교홈페이지, 학교주소, 학교코드, 학교이름, 초중고));
            }
            return 학교정보들;
        }
    }
}
