using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 添加笔记
    /// </summary>
    public class QuesNoteAdd : BasePage
    {
        int qid = Common.Request.QueryString["qid"].Int32 ?? 0;  //试题id
        int stnid = Common.Request.QueryString["stnid"].Int32 ?? 0;   //笔记id
        protected override void InitPageTemplate(HttpContext context)
        {
            //如果没有提交
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                EntitiesInfo.Student_Notes note=null;
                note = Business.Do<IStudent>().NotesSingle(stnid);
                if (note == null && qid > 0 && Extend.LoginState.Accounts.IsLogin)
                {
                    EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                    note = Business.Do<IStudent>().NotesSingle(qid, st.Ac_ID);
                }
                this.Document.SetValue("note", note);
                return;
            }

            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                if (!Extend.LoginState.Accounts.IsLogin) return;
                //提交的信息
                string tbNote = Common.Request.Form["tbNote"].String;
                EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                EntitiesInfo.Student_Notes note = null;
                note = Business.Do<IStudent>().NotesSingle(stnid);
                //如果笔记id取不到记录，用试题id取
                if (note == null && qid > 0)
                    note = Business.Do<IStudent>().NotesSingle(qid, st.Ac_ID);
                //如果提交信息为空，则为删除
                if (string.IsNullOrWhiteSpace(tbNote))
                {
                    if(note!=null)
                        Business.Do<IStudent>().NotesDelete(note.Stn_ID);
                    return;
                }
                EntitiesInfo.Student_Notes sn = note != null ? note : new EntitiesInfo.Student_Notes();
                sn.Stn_Context = tbNote;
                sn.Qus_ID = qid;
                sn.Ac_ID = st.Ac_ID;
                sn.Org_ID = this.Organ.Org_ID;
                try
                {
                    if (stnid > 0)
                        Business.Do<IStudent>().NotesSave(sn);
                    else
                        Business.Do<IStudent>().NotesAdd(sn);
                    this.Document.SetValue("isSuccess", true);
                }
                catch
                {
                    this.Document.SetValue("isSuccess", false);
                }
            }
        }
    }
}