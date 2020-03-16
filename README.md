# 支付宝当面付
## 当面付扫码支付demo
### 使用流程
1. 于[支付宝开放平台](https://open.alipay.com/ "支付宝开放平台")中申请当面付功能,并设置相关参数
2. 于软件中填写相关参数(APPID、支付宝私匙、商户私匙)

![API相关设置](https://camo.githubusercontent.com/146cfca35025d0b9b9970335a09c587c0d789395/68747470733a2f2f63646e2e736175796f6f2e636f6d2f323032302f30322f32342f313538323533323631302e706e67)

3. 填写金额与详情,即可生成二维码.

![生成二维码](https://cdn.seeull.com/2020/03/15/1584252236.png)

4. 付款后订单号会填入Order框中,可用于主动查询付款结果.

![整体界面](https://cdn.seeull.com/2020/03/15/1584252253.png)

### 总流程

![总流程](https://cdn.seeull.com/2020/03/16/1584360149.png)

### 实现

[支付宝API调用实例——当面付Demo](https://www.seeull.com/archives/113.html "https://www.seeull.com/archives/113.html")


### 依赖:
1. [HandyControl](https://github.com/HandyOrg/HandyControl "HandyControl")
2. [Alipay SDK for .NET](https://github.com/alipay/alipay-sdk-net-all "Alipay SDK for .NET")