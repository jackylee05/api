skills:
  - name: API Integration (.NET Core)
    description: 基于 .NET Core 实现第三方系统接口对接（HTTPS + JSON）
    abilities:
      - 熟练使用 ASP.NET Core Web API 构建接口服务
      - 支持 POST 请求处理，UTF-8 JSON 编码解析
      - 实现接口路由规范（如 /VmiV2/SeresESignCallBack）
      - 处理大小写敏感参数绑定

  - name: Security & Signature
    description: 接口签名与安全校验（HmacSHA256）
    abilities:
      - 使用 HmacSHA256 生成签名
      - 按规则拼接签名字符串（RequestTime + InterfaceName + VmiCode + 密钥）
      - 防止接口篡改与重放攻击
      - 实现签名校验中间件

  - name: Callback API Handling
    description: 回调接口处理能力
    abilities:
      - 处理复杂 JSON 嵌套结构（signerList / nextSignerList）
      - 支持签署流程状态回调（flowStatus / signStatus）
      - 解析签署文件列表（downloadUrl / fileKey）
      - 处理签署链接（长链 + 短链）

  - name: Data Modeling
    description: 数据建模与DTO设计
    abilities:
      - 定义强类型 DTO（CallBackProcessVO / Signer / File）
      - 支持嵌套对象映射
      - 使用 System.Text.Json 或 Newtonsoft.Json 进行序列化
      - 处理时间格式（yyyy-MM-dd HH:mm:ss）

  - name: HTTP & Environment Config
    description: 多环境接口配置
    abilities:
      - 管理测试/生产环境地址
      - 使用 appsettings.json 管理配置
      - 支持 HTTPS 调用
      - 实现 HttpClient 工厂模式

  - name: Logging & Error Handling
    description: 日志与异常处理
    abilities:
      - 记录接口请求/响应日志
      - 处理异常返回统一结构（Code / Message / Data）
      - 支持接口失败重试机制
      - 使用中间件统一异常处理

  - name: Response Standardization
    description: 标准化返回结构
    abilities:
      - 统一返回格式（Code / Message / Data）
      - 成功/失败状态码规范
      - 接口幂等处理

  - name: Enterprise Integration
    description: 企业系统对接能力
    abilities:
      - 对接ESB系统
      - 处理第三方回调流程
      - 支持电子签章业务流程
      - 兼容外部系统接口规范