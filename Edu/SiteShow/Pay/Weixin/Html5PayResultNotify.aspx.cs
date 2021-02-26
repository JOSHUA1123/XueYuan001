﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Common;
using ServiceInterfaces;

namespace WxPayAPI
{
    /// <summary>
    /// 微信Html5支付的回调处理
    /// </summary>
    public partial class Html5PayResultNotify : System.Web.UI.Page
    {
        protected int total_fee = 0;  //充值的钱数
        //int piid = Common.Request.QueryString["piid"].Int32 ?? 0;
        string serial = Common.Request.QueryString["serial"].String; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(serial))
            {
                //交易流水与支付接口
                EntitiesInfo.MoneyAccount moneyAccount = Business.Do<IAccounts>().MoneySingle(serial);
                EntitiesInfo.PayInterface payInterface = Business.Do<IPayInterface>().PaySingle(moneyAccount.Pai_ID);
                total_fee = (int)(moneyAccount.Ma_Money * 100);
                //商户id与支付密钥
                string appid = payInterface.Pai_ParterID;  //绑定支付的APPID（必须配置）
                Common.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
                string mchid = config["MCHID"].Value.String;    //商户id
                string paykey = config["Paykey"].Value.String;  //支付密钥  
                //
                WxPayData data = new WxPayData();
                data.SetValue("out_trade_no", serial);  //商户订单号
                WxPayData result = WxPayApi.OrderQuery(data, appid, mchid, paykey);//提交订单查询请求给API，接收返回数据

                Log.Info("OrderQuery", "订单查询结果GET, result : " + result.ToXml());
                string state = result.GetValue("trade_state").ToString();
                if (state == "SUCCESS")
                {
                    //付款方与收款方（商户id)
                    moneyAccount.Ma_Buyer = result.GetValue("attach").ToString();
                    moneyAccount.Ma_Seller = result.GetValue("mch_id").ToString();
                    Business.Do<IAccounts>().MoneyConfirm(moneyAccount);
                    lbError.Visible = false;
                    lbSucess.Visible = true;
                }
                else
                {
                    lbError.Visible = true;
                    lbSucess.Visible = false;
                }
            }
            else
            {
                //接收来自微信支付中心的异步请求
                ResultNotify resultNotify = new ResultNotify(this);
                //resultNotify.ProcessNotify();
                //获取结果
                WxPayData notifyData = resultNotify.GetNotifyData();
                string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                Log.Info(this.GetType().ToString(), "商户流水号 : " + out_trade_no);
                if (!string.IsNullOrWhiteSpace(out_trade_no))
                {
                    EntitiesInfo.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(out_trade_no);
                    if (maccount != null)
                    {
                        //付款方与收款方（商户id)
                        maccount.Ma_Buyer = notifyData.GetValue("attach").ToString();
                        maccount.Ma_Seller = notifyData.GetValue("mch_id").ToString();
                        Business.Do<IAccounts>().MoneyConfirm(maccount);
                    }
                }
            }
        }       
    }
}