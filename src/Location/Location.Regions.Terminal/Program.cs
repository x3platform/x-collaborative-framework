namespace X3Platform.Location.Regions.Terminal
{
    #region Using Libraries
    using System;
    using System.IO;
    using System.Reflection;
    using X3Platform.CommandLine;
    using X3Platform.CommandLine.Text;
    using X3Platform.Configuration;
    using X3Platform.Location.Regions.Model;
    using X3Platform.Util;

    #endregion

    static class Program
    {
        private static readonly AssemblyName program = Assembly.GetExecutingAssembly().GetName();

        /// <summary>ͷ����Ϣ</summary>
        private static readonly HeadingInfo headingInfo = new HeadingInfo(program.Name,
            string.Format("{0}.{1}", program.Version.Major, program.Version.Minor));

        private sealed class Options
        {
            #region Standard Option Attribute

            [Option("import", HelpText = "���롣")]
            public bool Import { get; set; }

            [Option('s', "silent", HelpText = "����ģʽ������ִ������Զ��رա�")]
            public bool Silent { get; set; }

            /// <summary>�÷�</summary>
            /// <returns></returns>
            [HelpOption(HelpText = "��ʾ������Ϣ��")]
            public string GetUsage()
            {
                var help = new HelpText(Program.headingInfo);

                help.AdditionalNewLineAfterOption = true;

                help.Copyright = new CopyrightInfo(KernelConfigurationView.Instance.WebmasterEmail, 2010, DateTime.Now.Year);

                help.AddPreOptionsLine("������������й���");
                help.AddPreOptionsLine("�������ݶ�ȡ���������Ĺ���.\r\n");

                help.AddPreOptionsLine("Usage:");
                help.AddPreOptionsLine(string.Format("  {0} --import", program.Name));

                help.AddPreOptionsLine(string.Format("  {0} --help", program.Name));
                help.AddOptions(this);

                return help;
            }
            #endregion
        }

        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();

                if (args.Length == 0)
                {
                    Console.WriteLine(options.GetUsage());
                    Environment.Exit(0);
                }

                var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);

                if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(1)))
                {
                    Command(options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "}");
                Console.WriteLine(ex);
            }

            Environment.Exit(0);
        }

        private static void Command(Options options)
        {
            // ��ʼʱ��
            TimeSpan beginTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            if (options.Import)
            {
                // ���ɻ���Ӧ��SQL�ű�
                Import(options);
            }

            // ����ʱ��
            TimeSpan endTimeSpan = new TimeSpan(DateTime.Now.Ticks);

            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.Yellow);
            Console.WriteLine("\r\nִ�н���������ʱ{0}�롣", beginTimeSpan.Subtract(endTimeSpan).Duration().TotalSeconds);
            CommandLineHelper.SetTextColor(CommandLineHelper.Foreground.White);

            if (!options.Silent)
            {
                Console.WriteLine("Pass any key to continue");
                Console.Read();
            }
        }

        #region ��̬����:Import(Options options)
        /// <summary>ִ��</summary>
        /// <param name="options"></param>
        static void Import(Options options)
        {
            try
            {
                string[] lines = File.ReadAllLines("data.txt");

                foreach (string line in lines)
                {
                    string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    RegionInfo region = new RegionInfo() { Id = values[0].Trim(), Name = values[1].Trim() };

                    Console.WriteLine("code {0} => name {1}", values[0], values[1]);

                    if (region.ParentId == "0")
                    {
                        region.Path = region.Name;
                    }
                    else
                    {
                        RegionInfo parent = RegionContext.Instance.RegionService.FindOne(region.ParentId);

                        region.Path = parent.Path + "\\" + region.Name;
                    }

                    RegionContext.Instance.RegionService.Save(region);
                }
                Console.WriteLine("\r\nfinished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                // file.Generate(ex.ToString());
            }
        }
        #endregion
    }
}