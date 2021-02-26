using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SiteShow.Manage.Utility
{
    public partial class toolsBar : System.Web.UI.UserControl
    {
        #region ��ť�¼�
        //�¼�
        public event EventHandler Add;
        public event EventHandler Modify;
        public event EventHandler Delete;
        public event EventHandler View;
        public event EventHandler Verify;
        public event EventHandler Recover;
        public event EventHandler Answer;
        public event EventHandler Output;
        //public event chanageState Chanage;

        //��ť�¼�
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (Add != null)
                this.Add(sender, e);            
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            if (Modify != null)
                this.Modify(sender, e);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (Delete != null)
                this.Delete(sender, e);
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            if (View != null)
                this.View(sender, e);
        }
        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (Verify != null)
                this.Verify(sender, e);
        }
        protected void btnRecover_Click(object sender, EventArgs e)
        {
            if (Recover != null)
                this.Recover(sender, e);
        }
        protected void btnAnswer_Click(object sender, EventArgs e)
        {
            if (Answer != null)
                this.Answer(sender, e);
        }
        protected void btnOutput_Click(object sender, EventArgs e)
        {
            if (Output != null)
                this.Output(sender, e);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                this.ChanageEvent(null, null);
            //�������·����Ϊ��
            if (_openWinPath != "")
            {
                //������Ӱ�ť����
                if (_AddButtonOpen)
                {
                    this.btnAdd.Attributes.Add("onclick", "return OnAdd(window.name);");
                }
            }
            if (_gvName != "")
            {
                this.btnModify.Attributes.Add("onclick", "return OnEdit();");
                this.btnDelete.Attributes.Add("onclick", "return OnDelete();");
                this.btnAnswer.Attributes.Add("onclick", "return OnView();");
                this.btnVerify.Attributes.Add("onclick", "return OnVerify();");
                this.btnRecover.Attributes.Add("onclick", "return OnRecover();");
                this.btnAnswer.Attributes.Add("onclick", "return OnAnswer();");
                this.btnInput.Attributes.Add("onclick", "return OnInput();");
                this.btnOutput.Attributes.Add("onclick", "return OnOutput();");
                this.btnPrint.Attributes.Add("onclick", "return OnPrint();");
            }
            
        }
        /// <summary>
        /// ����״̬ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChanageEvent(object sender, EventArgs e)
        {
            this.btnAdd.Visible = this._AddButtonVisible;
            this.btnModify.Visible = this._ModifyButtonVisible;
            this.btnDelete.Visible = this._DelButtonVisible;
            this.btnView.Visible = this._ViewButtonVisible;
            this.btnVerify.Visible = this._VerifyButtonVisible;
            this.btnRecover.Visible = this._RecoverButtonVisible;
            this.btnAnswer.Visible=this._AnsButtonVisible;
            this.btnInput.Visible = this._InputButtonVisible;
            this.btnOutput.Visible = this._OutputButtonVisible;
            this.btnPrint.Visible = this._PrintButtonVisible;
            //
            this.btnAdd.Enabled = this._AddButtonEnable;
            this.btnModify.Enabled = this._ModifyButtonEnable;
            this.btnDelete.Enabled = this._DelButtonEnable;
            this.btnView.Enabled = this._ViewButtonEnable;
            this.btnVerify.Enabled = this._VerifyButtonEnable;
            this.btnRecover.Enabled = this._RecoverButtonEnable;
            this.btnAnswer.Enabled = this._AnsButtonEnable;
            this.btnOutput.Enabled = this._OutputButtonEnable;
        }
        #region ��ťЧ�����Ƿ���ʾ����ã�
        //������ť�Ƿ���ʾ
        private bool _AddButtonVisible = true;
        public bool AddButtonVisible
        {
            get
            {
                return this._AddButtonVisible;
            }
            set
            {
                this._AddButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _AddButtonEnable = true;
        public bool AddButtonEnable
        {
            get
            {
                return this._AddButtonEnable;
            }
            set
            {
                this._AddButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _AddButtonOpen = true;
        /// <summary>
        /// ��Ӱ�ť�Ƿ�����������
        /// </summary>
        public bool AddButtonOpen
        {
            get
            {
                return this._AddButtonOpen;
            }
            set
            {
                this._AddButtonOpen = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //�޸İ�ť
        private bool _ModifyButtonVisible = false;
        public bool ModifyButtonVisible
        {
            get
            {
                return this._ModifyButtonVisible;
            }
            set
            {
                this._ModifyButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _ModifyButtonEnable = true;
        public bool ModifyButtonEnable
        {
            get
            {
                return this._ModifyButtonEnable;
            }
            set
            {
                this._ModifyButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //ɾ����ť�Ƿ���ʾ
        private bool _DelButtonVisible = true;
        public bool DelButtonVisible
        {
            get
            {
                return this._DelButtonVisible;
            }
            set
            {
                this._DelButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _DelButtonEnable = true;
        public bool DelButtonEnable
        {
            get
            {
                return this._DelButtonEnable;
            }
            set
            {
                this._DelButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //�鿴��ť�Ƿ���ʾ
        private bool _ViewButtonVisible = false;
        public bool ViewButtonVisible
        {
            get
            {
                return this._ViewButtonVisible;
            }
            set
            {
                this._ViewButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _ViewButtonEnable = true;
        public bool ViewButtonEnable
        {
            get
            {
                return this._ViewButtonEnable;
            }
            set
            {
                this._ViewButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //��֤��ť�Ƿ���ʾ
        private bool _VerifyButtonVisible = false;
        public bool VerifyButtonVisible
        {
            get
            {
                return this._VerifyButtonVisible;
            }
            set
            {
                this._VerifyButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _VerifyButtonEnable = true;
        public bool VerifyButtonEnable
        {
            get
            {
                return this._VerifyButtonEnable;
            }
            set
            {
                this._VerifyButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //��ԭ��ť�Ƿ���ʾ
        private bool _RecoverButtonVisible = false;
        public bool RecoverButtonVisible
        {
            get
            {
                return this._RecoverButtonVisible;
            }
            set
            {
                this._RecoverButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _RecoverButtonEnable = true;
        public bool RecoverButtonEnable
        {
            get
            {
                return this._RecoverButtonEnable;
            }
            set
            {
                this._RecoverButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //�ظ���ť�Ƿ���ʾ
        private bool _AnsButtonVisible = false;
        public bool AnsButtonVisible
        {
            get
            {
                return this._AnsButtonVisible;
            }
            set
            {
                this._AnsButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _AnsButtonEnable = true;
        public bool AnsButtonEnable
        {
            get
            {
                return this._AnsButtonEnable;
            }
            set
            {
                this._AnsButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //���밴ť�Ƿ���ʾ
        private bool _InputButtonVisible = false;
        public bool InputButtonVisible
        {
            get
            {
                return this._InputButtonVisible;
            }
            set
            {
                this._InputButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //������ť�Ƿ���ʾ
        private bool _OutputButtonVisible = false;
        public bool OutputButtonVisible
        {
            get
            {
                return this._OutputButtonVisible;
            }
            set
            {
                this._OutputButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        //��ӡ��ť�Ƿ���ʾ
        private bool _PrintButtonVisible = false;
        public bool PrintButtonVisible
        {
            get
            {
                return this._PrintButtonVisible;
            }
            set
            {
                this._PrintButtonVisible = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        private bool _OutputButtonEnable = true;
        public bool OutputButtonEnable
        {
            get
            {
                return this._OutputButtonEnable;
            }
            set
            {
                this._OutputButtonEnable = value;
                this.ChanageEvent(this, new EventArgs());
            }
        }
        #endregion

        #region ��������
        private int _openWinWidth = 0;
        public int WinWidth
        {
            get
            {
                return _openWinWidth;
            }
            set
            {
                _openWinWidth = value;
            }
        }
        private int _openWinHeight = 0;
        public int WinHeight
        {
            get
            {
                return _openWinHeight;
            }
            set
            {
                _openWinHeight = value;
            }
        }
        private string _openWinPath = "";
        /// <summary>
        /// �������ļ�·��
        /// </summary>
        public string WinPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_openWinPath)) return _openWinPath;
                HttpContext context = System.Web.HttpContext.Current;
                string path = context.Request.Url.PathAndQuery;
                //�����ҳ�治����������ֱ�ӷ���
                if (path.IndexOf("?") < 0) return _openWinPath;
                //�����ҳ�������������ҳ��Ĳ���������ҳ��
                //��ҳ��Ĳ���
                string mPara = path.Substring(path.LastIndexOf("?")+1);
                if (_openWinPath.IndexOf("?") < 0)
                    _openWinPath = _openWinPath + "?" + mPara;
                else
                    _openWinPath = _openWinPath + "&" + mPara;
                return _openWinPath;
            }
            set
            {
                _openWinPath = value;
            }
        }
        private string _inputPath = "";
        /// <summary>
        /// ���봰�ڵ�·��
        /// </summary>
        public string InputPath
        {
            get
            {
                HttpContext context = System.Web.HttpContext.Current;
                string path = context.Request.Url.PathAndQuery;
                //�����ҳ�治����������ֱ�ӷ���
                if (path.IndexOf("?") < 0) return _inputPath;
                //�����ҳ�������������ҳ��Ĳ���������ҳ��
                //��ҳ��Ĳ���
                string mPara = path.Substring(path.LastIndexOf("?") + 1);
                if (_inputPath.IndexOf("?") < 0)
                    _inputPath = _inputPath + "?" + mPara;
                else
                    _inputPath = _inputPath + "&" + mPara;
                return _inputPath;
            }
            set
            {
                _inputPath = value;
            }
        }
        private bool _isWinOpen = true;
        /// <summary>
        /// �Ƿ񵯳��Ӵ��ڣ����Ϊflash,��תת���Ӵ��ڣ������ǵ���
        /// </summary>
        public bool IsWinOpen
        {
            get
            {
                return _isWinOpen;
            }
            set
            {
                _isWinOpen = value;
            }
        }
        #endregion

        #region ��������
        private bool _isBatchReco = true;
        /// <summary>
        /// �Ƿ�����������ԭ
        /// </summary>
        public bool IsBatchReco
        {
            get
            {
                return _isBatchReco;
            }
            set
            {
                _isBatchReco = value;
            }
        }
        #endregion

        #region ��ʾ��Ϣ
        private string _showMsg= "";
        /// <summary>
        /// �������ļ�·��
        /// </summary>
        public string DelShowMsg
        {
            get
            {
                return _showMsg;
            }
            set
            {
                _showMsg = value;
            }
        }

        #endregion

        private string _gvName = "";
        /// <summary>
        /// Ҫ���Ƶ�GridView��id
        /// </summary>
        public string GvName
        {
            get
            {
                return _gvName;
            }
            set
            {
                _gvName = value;
            }
        }

        //public System.Web.UI.WebControls.Button AddButton
        //{
        //    get
        //    {
        //        return this.btnAdd;
        //    }
        //    set
        //    {
        //        this.btnAdd = value;
        //    }
        //}
    }
}