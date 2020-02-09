using System;
using System.Diagnostics;

namespace NissenCode
{
    public enum Level {
        Info = 0,
        Error = 1,
        Warning = 2
    }
    public class Messenger
    {
        public string message { get; }
        public Level level { get; }
        public DateTime time { get; }
        public string caller { get; internal set; }

        private ConsoleColor[] colors = new ConsoleColor[] {
            Console.ForegroundColor,
            ConsoleColor.Red,
            ConsoleColor.Yellow
        };


        public Messenger(string message, Level level = Level.Info)
        {
            time = DateTime.Now;
            this.message = message;
            this.level = level;
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            var clasName = methodInfo.ReflectedType.Name;

            this.caller = clasName;
        }
        public void log()
        {
            log(ToString());
        }
        internal void log(string msg) {
            Console.ForegroundColor = colors[(int)level];
            switch (level)
            {
                case Level.Info:
                    Console.WriteLine(msg);
                    break;
                case Level.Error:
                    Console.Error.WriteLine(msg);
                    break;
                case Level.Warning:
                    Console.WriteLine(msg);
                    break;
            }
            Console.ForegroundColor = colors[(int) Level.Info];
        }
        public override string ToString()
        {
            return time.ToString("[ yyyy-MM-dd HH:mm:ss ]") + "[ " + level.ToString() + " ][ "+ caller + " ] " + message;
        }
    }

    public class Messenger<T> : Messenger
    {
        public T value { get; }
        public void log(bool withValue = false)
        {
            log(ToString()+(withValue? "( " + value + " )" : ""));
        }
        public Messenger(T value, string message, Level level = Level.Info) : base(message, level)
        {
            this.value = value;
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            var clasName = methodInfo.ReflectedType.Name;

            this.caller = clasName;
        }
    }
}
