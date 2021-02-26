using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using ServiceInterfaces;
using Extend;
namespace SiteShow.Manage.Student
{
    public partial class CouponRecharge : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            //this.Form.Attributes.Add("target", "_blank");
            if (st == null) return;
            if (!IsPostBack)
            {
                //当前学员的卡券数量
                int stid = st == null ? -1 : st.Ac_ID;
                ltCoupon.Text = st.Ac_Coupon.ToString();
            }
        }
        /// <summary>
        /// 充值码充值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCode_Click(object sender, EventArgs e)
        {
            string moneyCode = tbCode.Text.Trim();
            //没有传入充值码
            if (string.IsNullOrWhiteSpace(moneyCode)) return;
            try
            {
                EntitiesInfo.RechargeCode code = Business.Do<IRecharge>().CouponCheckCode(moneyCode);
                if (code != null)
                {
                    EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                    code.Ac_ID = st.Ac_ID;
                    code.Ac_AccName = st.Ac_AccName;
                    Business.Do<IRecharge>().CouponUseCode(code);
                    Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                    //当前学员的卡券
                    int stid = st == null ? -1 : st.Ac_ID;
                    ltCoupon.Text = st.Ac_Coupon.ToString();
                    this.Alert("充值成功！成功充值" + code.Rc_Price + "元。");
                }
            }
            catch (Exception ex)
            {
                this.cv3.ErrorMessage = ex.Message;
                this.cv3.IsValid = false;
            }
        }     
    }
}