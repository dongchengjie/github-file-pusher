using CommandLine;
using CommandLine.Text;
using gfp.Src.Framework.CommandLine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace gfp.Src
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Parser parser = new Parser(with => with.HelpWriter = null);
            ParserResult<Options> result = parser.ParseArguments<Options>(args);
            result.WithParsed(options => FilePusher.Push(options)).WithNotParsed(errs => DisplayHelp(result, errs));
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            if (errs.IsVersion())
            {
                Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version);
            }
            else
            {
                Console.WriteLine(HelpText.AutoBuild(result, settings =>
                {
                    settings.AddDashesToOption = true;                                          // 显示破折号
                    settings.AddEnumValuesToHelpText = true;                                    // 描述枚举值的取值范围
                    settings.AdditionalNewLineAfterOption = false;                              // 描述末尾空新行
                    settings.AutoHelp = true;                                                   // 自动生成--help
                    settings.AutoVersion = true;                                                // 自动生成--version
                    settings.Copyright = string.Empty;                                          // 版权信息
                    settings.Heading = "A CLI program for updating Github files over http.";    // 顶部信息
                    settings.MaximumDisplayWidth = int.MaxValue;                                // 单行展示字数限制
                    return HelpText.DefaultParsingErrorsHandler(result, settings);
                }, e => e));
            }
        }
    }
}
