using CommandLine;

namespace gfp.Src.Framework.CommandLine
{
    public class Options
    {
        [Option('r', "repo", Required = true, HelpText = "仓库地址")]
        public string Repo { get; set; }
        [Option('b', "branch", Required = false, Default = "main", HelpText = "代码分支")]
        public string Branch { get; set; }
        [Option('f', "file", Required = true, HelpText = "仓库文件路径")]
        public string File { get; set; }
        [Option('l', "local", Required = true, HelpText = "本地文件路径")]
        public string Local { get; set; }
        [Option('t', "token", Required = true, HelpText = "GitHub API Toekn")]
        public string Token { get; set; }
        [Option('m', "message", Required = false, Default = "Added/Updated by Github File Pusher", HelpText = "提交信息")]
        public string Message { get; set; }
        [Option('c', "committer", Required = false, Default = "Github File Pusher", HelpText = "提交者名称")]
        public string Committer { get; set; }
        [Option('e', "email", Required = false, Default = "github@example.com", HelpText = "提交者邮箱")]
        public string CommitterEmail { get; set; }
        [Option('p', "proxy", Required = false, Default = false, HelpText = "使用代理")]
        public bool Proxy { get; set; }
        [Option('H', "proxy-host", Required = false, Default = "127.0.0.1", HelpText = "代理服务器")]
        public string ProxyHost { get; set; }
        [Option('P', "proxy-port", Required = false, Default = 7890, HelpText = "代理端口")]
        public int ProxyPort { get; set; }
        [Option('T', "timeout", Required = false, Default = 30 * 1000, HelpText = "请求超时（毫秒）")]
        public int Timoyout { get; set; }
        [Option("pause", Required = false, Default = false, HelpText = "程序结束时pause")]
        public bool Pause { get; set; }
    }
}
