# 支付宝当面付
## 当面付扫码支付demo
### 使用流程
1. 于[支付宝开放平台](https://open.alipay.com/ "支付宝开放平台")中申请当面付,并获取相关API
2. 填写相关信息(APPID、支付宝私匙、商户私匙)

![API相关设置](https://cdn.sauyoo.com/2020/02/24/1582532610.png)

3. 填写金额与详情,即可生成二维码.

![生成二维码](https://cdn.sauyoo.com/2020/02/24/1582532641.png)

4. 付款后订单号会填入Order框中,可用于主动查询付款结果.

![整体界面](https://cdn.sauyoo.com/2020/02/24/1582532560.png)

### 依赖:
1. [HandyControl](https://github.com/HandyOrg/HandyControl "HandyControl")
2. [Alipay SDK for .NET](https://github.com/alipay/alipay-sdk-net-all "Alipay SDK for .NET")