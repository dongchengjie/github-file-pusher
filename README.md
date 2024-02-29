## gfp.exe

> Github文件推送器，使用http协议调用Restful API实现，无需安装Git

### 实例

```bash
# 将D:\xyz.yaml文件推送到用户username的example仓库下的subs目录下的demo.yaml（如果仓库不存在改文件，则创建新文件）
.\gfp.exe -r https://github.com/username/example -f subs/demo.yaml -l D:\xyz.yaml -t abcdefg

# 作用同上，担心本地无法正常访问Github，使用代理服务器127.0.0.1:1234
.\gfp.exe -r https://github.com/username/example -f subs/demo.yaml -l D:\xyz.yaml -t abcdefg --p -H 127.0.0.1 -P 1234
```

### 应用参数

| 参数             | 必填 | 默认值                              | 说明                                      |
| ---------------- | ---- | ----------------------------------- | ----------------------------------------- |
| -r, --repo       | 是   |                                     | 仓库地址                                  |
| -b, --branch     | 否   | main                                | 代码分支                                  |
| -f, --file       | 是   |                                     | 仓库文件（相对）路径，**文件可以不存在**  |
| -l, --local      | 是   |                                     | 本地文件路径（相对/绝对路径均可）         |
| -t, --token      | 是   |                                     | GitHub API Token（详见下方**创建token**） |
| -m, --message    | 否   | Added/Updated by Github File Pusher | 提交信息                                  |
| -c, --committer  | 否   | Github File Pusher                  | 提交者名称                                |
| -e, --email      | 否   | github@example.com                  | 提交者邮箱                                |
| -p, --proxy      | 否   | 否                                  | 使用代理                                  |
| -H, --proxy-host | 否   | 127.0.0.1                           | 代理服务器                                |
| -P, --proxy-port | 否   | 7890                                | 代理端口                                  |
| -T, --timeout    | 否   | 30000                               | 请求超时（ms）                            |
| --pause          | 否   |                                     | 否程序结束时pause                         |

### 创建token

1. 打开[token设置](https://github.com/settings/tokens)
2. 点击右上角**Generate new token** 按钮，选择**Generate new token (classic)**
3. 为token取名
4. 选择Expiration过期时长，**No expiration**表示永不过期
5. 为token创建相应的权限
   1. 如果是public仓库，勾选**public_repo                    Access public repositories**足矣
   2. 如果是私有仓库，勾选**repo                    Full control of private repositories**
6. 点击底部**Generate token**按钮创建token