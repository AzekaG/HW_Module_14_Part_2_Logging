using NLog;
using NLog.Conditions;
using NLog.Targets;
using System.Numerics;

namespace Task_2
{
    /*Создайте приложение для генерации фейковых пользователей. У
каждого пользователя должна быть такая информация:
 Имя
 Фамилия
 Контактный телефон
 Email
 Адрес
Генерация фейковых пользователей должна быть оформлена в
виде класса. Фактически нужно создать класс для генерации
фейкового пользователя.Напишите код для тестирования фейковых пользователей
В программе настройте логирование с использованием Serialog.*/
    internal class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();



        class User
        {
            string[] Names = { "Харитон ", "Шерлок ", "Борис ", "Гарри ", "Фёдор ", "Григорий ","", "Инга ", "Анна", "Люся ", "Ника ", "Ярослава ", "Жасмин ", "Ульяна ", "Эвелина ", "Анастасия " , "не имя" ,"тоже не имя" };
            string[] Surnames = { "Цушко ", "Ситникова ", "Шамрыло ", "Шуфрич ", "Ерёменко ","", "Андрухович ", "Якушев ", "Шестаков ", "Не Фамилия", "", "проверка", "Погомий " };
            string[] Adresses = { "Кривой Рог", "Винница", "Николаев", "Полтава", "Кременчуг","", "Тернополь", "Павлоград", "Бердянск", "Севастополь", "Одесса" };


            public string Name { get; set; }
            public string Surname { get; set; }
            public string Number { get; set; }
            public string Email { get; set; }
            public string Adress { get; set; }
            public User(Random random) { GenerateNewUser(random); }
           

            public void GenerateNewUser(Random random)
            {
                Name = Names[random.Next(0, Names.Length)];
                Surname = Surnames[random.Next(0, Surnames.Length)];
                PhoneRand(random);
                MailRand(random);
                Adress = Adresses[random.Next(0, Adresses.Length)];
            }
            void MailRand(Random random)
            {
                string[] arr = { "gmail.com", "i.ua", "ZohoNow.Com", "mailbox.com", "fastmail.com", "Outlook.com", "Yahoo.com" };
                Email = (Name + random.Next(int.Parse(Number.Substring(5)) % 100 / 2).ToString() + "@" + arr[random.Next(arr.Length - 1)]).Replace(' ', '_');

            }
            void PhoneRand(Random random)
            {

                Number = "+380" + (random.Next(999, 9999)).ToString() + (random.Next(9999, 99999)).ToString();
            }
            public void OutputInfo()
            {
                Console.WriteLine("Name : " + Name);
                Console.WriteLine("Surname : " + Surname);
                Console.WriteLine("Phone : " + Number);
                Console.WriteLine("Email : " + Email);
                Console.WriteLine("Adress : " + Adress);
                Console.WriteLine();
            }
            public override string ToString()
            {
                return ("\nName : " + Name)+
                ("\nSurname : " + Surname)+
                ("\nPhone : " + Number)+
                ("\nEmail : " + Email)+
                ("\nAdress : " + Adress);
            }

        }
        class UserBase
        {
           public List<User> users = new List<User>();
            public void GenerateNewUsers(int amount , Random random)
            {
                for (int i = 0; i < amount; i++)
                {
                    User user = new User(random);
                    users.Add(user);
                }

            }
            public void OutputInfo()
            {
                foreach (User user in users) 
                {
                    user.OutputInfo();
                    Console.WriteLine();
                }
            }

        }
        static void LogTextLetter(UserBase obj)
        {

            int count = 0;
            foreach (var el in obj.users)
            {
                if (String.IsNullOrEmpty(el.Number)) { count++; }
                if (String.IsNullOrEmpty(el.Name)) { count++; }
                if (String.IsNullOrEmpty(el.Adress)) { count++; }
                if (String.IsNullOrEmpty(el.Email) || !el.Email.Contains("@")) { count++; }
                if (String.IsNullOrEmpty(el.Surname)) { count++; }
               switch(count)
                {

                        case 0: Logger.Info(el.ToString()); break;
                        case 1: Logger.Debug(el.ToString()); break;
                        case 2: Logger.Error(el.ToString()); break;
                        case 3: Logger.Warn(el.ToString()); break;
                        case 4: Logger.Fatal(el.ToString()); break;
                        case 5: Logger.Fatal(el.ToString()); break;
                }count = 0;
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










            Random random = new Random();
            UserBase users = new UserBase();

            Console.WriteLine("в генераторе имен специально допущены ошибки (такие как пустые строки или *нет имени* и т.д.) для того , " +
                "чтобы дать логгеру возможность показать разные степени ворнинги. ");
            int choice;
             
            Console.WriteLine("Enter amount of Peoples for generation : ");
            int amount = int.Parse(Console.ReadLine());
            do
            {
                
                Console.WriteLine("Choose an action : ");
                Console.WriteLine("1-Show generated humans");
                Console.WriteLine("2-Logging a process");
                Console.WriteLine("0-Exit");
                choice = int.Parse(Console.ReadLine());
                users.GenerateNewUsers(amount, random);
                switch (choice) 
                {
                    case 1:
                        {
                            users.OutputInfo();

                        }
                        break;
                    case 2:
                        {

                            LogTextLetter(users);
                        }
                        break;
                    default: { } break;

                
                
                
                
                }



            } while (choice!=0);










           
            users.OutputInfo();
        }
    }
}