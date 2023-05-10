namespace HW_Module_14_Part_2_Logging
{
    using NLog;
    using NLog.Conditions;
    using NLog.Targets;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Json;
    using System.Xml.Linq;
    using static System.Net.Mime.MediaTypeNames;




    /*Создайте приложение для поиска информации в файле по
текстовому шаблону. Варианты поддерживаемых шаблонов:
 Отобразить все предложения, содержащие хотя бы одну
маленькую, английскую букву
 Отобразить все предложения, содержащие хотя бы одну
цифру
 Отобразить все предложения, содержащие хотя бы одну
большую, английскою букву
В программе настройте логирование с использованием NLog.*/


    internal class Program
    {

        
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        


        class FinderInfo
        {
            public void FindLittleLetter(string text) 
            {
                string[] temp = text.Split(new char[] { '.' });
                var request = from el in temp
                              from letters in el
                              where letters > 'a' && letters < 'z'
                              group el by el;

                foreach (var words in request)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(words.Key+'.');
                    Console.ResetColor();
                }

            }

           
            public void FindNumberInText(string text)
            {
                string[] temp = text.Split(new char[] { '.' });
                var request = from el in temp
                              from letters in el
                              where letters > '0' && letters < '9'
                              group el by el;

                foreach (var words in request)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(words.Key+'.');
                    Console.ResetColor();
                }

            }

            public void FindBigLetter(string text)
            {
                string[] temp = text.Split(new char[] { '.' });
                var request = from el in temp
                              from letters in el
                              where letters > 'A' && letters < 'Z'
                              group el by el;

                foreach (var words in request)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(words.Key+'.');
                    Console.ResetColor();
                }

            }

        }

        static void LogTextLetter(string obj)
        {
            string[] temp = obj.Split(new char[] { '.' });

            foreach (var el in temp)
            {
                foreach (var letters in el)
                {
                    if (letters > 'A' && letters < 'Z')
                        Logger.Info(letters+ '.');
                    else if (letters > 'a' && letters < 'z')
                        Logger.Warn(letters+ '.');
                    Thread.Sleep(10);
                }
            }
        }
        static void LogTextNumbers(string obj)
        {
            string[] temp = obj.Split(new char[] { '.' });
            foreach (var el in temp)
            {
                bool flag = true;
                foreach (var letters in el)
                {
                    if (Char.IsNumber(letters))
                    {
                        Logger.Info(el); flag = false; break;
                    }
               }
                if (flag) Logger.Error(el);
                Thread.Sleep(10);
            }
        }

        static void Main(string[] args)
    {
            #region IniLogger
            var config = new NLog.Config.LoggingConfiguration();
            LogManager.Configuration = new NLog.Config.LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("Console Target")
            {

                Layout = @"${counter}|[${date:format=yyyy-MM-dd HH\:mm\:ss}] [${logger}/${uppercase: ${level}}] [THREAD: ${threadid}] >> ${message}"

            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            NLog.LogManager.Configuration = config;

            #endregion IniLogger


            #region NLog Colors

            var Trace = new ConsoleRowHighlightingRule();
            Trace.Condition = ConditionParser.ParseExpression("level == LogLevel.Trace");
            Trace.ForegroundColor = ConsoleOutputColor.Yellow;
            consoleTarget.RowHighlightingRules.Add(Trace);
            var Debug = new ConsoleRowHighlightingRule();
            Debug.Condition = ConditionParser.ParseExpression("level == LogLevel.Debug");
            Debug.ForegroundColor = ConsoleOutputColor.DarkCyan;
            consoleTarget.RowHighlightingRules.Add(Debug);
            var Info = new ConsoleRowHighlightingRule();
            Info.Condition = ConditionParser.ParseExpression("level == LogLevel.Info");
            Info.ForegroundColor = ConsoleOutputColor.Green;
            consoleTarget.RowHighlightingRules.Add(Info);
            var Warn = new ConsoleRowHighlightingRule();
            Warn.Condition = ConditionParser.ParseExpression("level == LogLevel.Warn");
            Warn.ForegroundColor = ConsoleOutputColor.DarkYellow;
            consoleTarget.RowHighlightingRules.Add(Warn);
            var Error = new ConsoleRowHighlightingRule();
            Error.Condition = ConditionParser.ParseExpression("level == LogLevel.Error");
            Error.ForegroundColor = ConsoleOutputColor.DarkRed;
            consoleTarget.RowHighlightingRules.Add(Error);
            var Fatal = new ConsoleRowHighlightingRule();
            Fatal.Condition = ConditionParser.ParseExpression("level == LogLevel.Fatal");
            Fatal.ForegroundColor = ConsoleOutputColor.Black;
            Fatal.BackgroundColor = ConsoleOutputColor.DarkRed;
            consoleTarget.RowHighlightingRules.Add(Fatal);

            #endregion NLog Colors
            string text = " We use both first and third-party cookies to personalise web content," +
                   " analyse visits to our websites and tailor advertisements. ONLY BIG LETTERS . " +
                   "Some of these cookies are necessary for the website to function 22222," +
                   " whilst others require your consent. More detail can be found in" +
               " our cookie policy and you can tailor your choices in the preference centre. Little Sentence with number 123.";
        
        FileStream stream = new FileStream("Text.json" , FileMode.Create);
        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(string));
        jsonFormatter.WriteObject(stream, text);
            stream.Close();

            Console.WriteLine("\n\nOrigin text\n\n"+ text+"\n\n_____________________________________________");
            FinderInfo finderInfo = new FinderInfo();
            Console.WriteLine("\n\nNow we find only sentenses witch contains BIG LETTER\n");
            finderInfo.FindBigLetter(text);
            Console.WriteLine("\n\nNow we find only sentenses witch contains little letter\n");
            finderInfo.FindLittleLetter(text);
            Console.WriteLine("\n\nNow we find only sentenses witch contains NUMBERS\n");
            finderInfo.FindNumberInText(text);

            //LogTextLetter(text);
            //LogTextNumbers(text);


        }
    }
}