using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.pf
{
    public partial class pf_withdraw : System.Web.UI.Page
    {
        string CompanyId = "";
        string sqlcmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();

            }
        }
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__preRIndex__"] = "No";
                string getUserId = getCookies["__getUserId__"].ToString();

                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "pf_withdraw.aspx", ddlCompanyName, gvPFWithdrawList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                classes.commonTask.loadPFMemberListForWithdraw(ddlEmployeeList, ViewState["__CompanyId__"].ToString());                
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();


                loadPFWithDrawRecord();

            }
            catch { }

        }
        private void loadPFWithDrawRecord() 
        {
            DataTable dtWithdrawnMember = new DataTable();
            string PaybableType = (rblPayableType.SelectedValue == "0") ? "" : " pfw.PayableType=" + rblPayableType.SelectedValue + " and ";
            sqlDB.fillDataTable("select pfw.EmpId,pei.EmpName,SUBSTRING(pei.EmpCardNo,10,5) EmpCardNo, convert(varchar,WithdrawDate,105) WithdrawDate,EmpContribution,EmprContribution,Profit from PF_Withdraw pfw "+
                " inner join Personnel_EmployeeInfo pei on pfw.EmpId=pei.EmpId where "+PaybableType+" pei.CompanyId='"+ddlCompanyName.SelectedValue+"'", dtWithdrawnMember);
            gvPFWithdrawList.DataSource = dtWithdrawnMember;
            gvPFWithdrawList.DataBind();
        }
        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            //CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
            classes.commonTask.loadPFMemberListForWithdraw(ddlEmployeeList, ddlCompanyName.SelectedValue);
            loadPFWithDrawRecord();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            pfWithdraw();
        }
        private void pfWithdraw() 
        {
            //---------------get PF Year---------------------
            string PayableType;
            string[] WithdrawDate = txtWithdrawDate.Text.Trim().Split('-');
            DateTime zeroTime = new DateTime(1, 1, 1);
            string[] SelectedValue = ddlEmployeeList.SelectedValue.Split('/');
            DateTime pfStartDate = DateTime.Parse(SelectedValue[1]);
            DateTime pfWithdrawDate = DateTime.Parse(WithdrawDate[2] + "-" + WithdrawDate[1] + "-" + WithdrawDate[0]);
            TimeSpan pfYear = pfWithdrawDate.Subtract(pfStartDate);   
            float year = (zeroTime + pfYear).Year - 1;
            //--------------------End PF Year------------------------

            //---------------get PF Payable Type Info----------------

            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select PEmpPartStartyear,PEmpPartEndyear,PEmpEmprStartyear,PEmpEmprEndyear,PEmpEmprIrstStartyear,PEmpEmprIrstEndyear from PF_CalculationSetting where CompanyId='"+ddlCompanyName.SelectedValue+"'", dt);
            if (dt.Rows.Count > 0)
            {
                if (int.Parse(dt.Rows[0]["PEmpEmprIrstStartyear"].ToString()) < year) //Payable ( Employee contribution + Employeer contribution + Interest)
                    PayableType = "3";
                else if (int.Parse(dt.Rows[0]["PEmpEmprStartyear"].ToString()) < year)//Payable ( Employee contribution + Employeer contribution)
                    PayableType = "2";
                else//Payable ( Employee contribution)
                    PayableType = "1";


                try
                {

                    DataTable dtPFInfo = new DataTable();
                  sqlDB.fillDataTable(" with "+
                      "a as( select isnull( sum(EmpContribution),0) EmpContribution,isnull( sum(EmprContribution),0) EmprContribution,'" + SelectedValue[0] +
                      "' EmpID from PF_PFRecord   where EmpID='" + SelectedValue[0] + "')," +
                      " b as( select isnull( sum(Profit),0) Profit,'" + SelectedValue[0] + "' as EmpId from PF_Profit where EmpID='" + SelectedValue[0] + "') " +
                      "select EmpContribution,EmprContribution,Profit from a inner join b on a.EmpID=b.EmpID",dtPFInfo);

                  SqlCommand cmd = new SqlCommand("insert into PF_Withdraw values('" + SelectedValue[0] + "','" +
                        convertDateTime.getCertainCulture(txtWithdrawDate.Text).ToString() + "',"+dtPFInfo.Rows[0]["EmpContribution"].ToString()+
                        ","+dtPFInfo.Rows[0]["EmprContribution"].ToString()+","+dtPFInfo.Rows[0]["Profit"].ToString()+"," + PayableType + " ) ", sqlDB.connection);

                    if (int.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                    {
                        //---------Update Employee pf Status-----------------
                        SqlCommand cmd1 = new SqlCommand("update Personnel_EmpCurrentStatus set PfMember=0 where IsActive=1 and EmpId='" + SelectedValue[0] + "'", sqlDB.connection);
                        cmd1.ExecuteNonQuery();
                        classes.commonTask.loadPFMemberListForWithdraw(ddlEmployeeList, ddlCompanyName.SelectedValue);
                        //-------------------------------------------------     
                        rblPayableType.SelectedValue = "0";
                        loadPFWithDrawRecord();

                        lblMessage.InnerText = "success->" + btnSave.Text.Trim() + "n Successfully.";
                    }
                    else
                        lblMessage.InnerText = "error-> Unable to " + btnSave.Text.Trim() + " !";
                }
                catch (Exception ex)
                {
                    lblMessage.InnerText = "error->" + ex.Message;
                }
            }
            //----------------End PF Payable Type Info---------------


        }

        protected void rblPayableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPFWithDrawRecord();
        }

        protected void gvPFWithdrawList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("deleterow"))
            {
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                SQLOperation.forDeleteRecordByIdentifier("PF_Withdraw", "EmpId", gvPFWithdrawList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
               
                lblMessage.InnerText = "success->Successfully  Deleted";
                gvPFWithdrawList.Rows[rIndex].Visible = false;
                //---------Update Employee pf Status-----------------
                SqlCommand cmd1 = new SqlCommand("update Personnel_EmpCurrentStatus set PfMember=1 where IsActive=1 and EmpId='" + gvPFWithdrawList.DataKeys[rIndex].Values[0].ToString() + "'", sqlDB.connection);
                cmd1.ExecuteNonQuery();
                classes.commonTask.loadPFMemberListForWithdraw(ddlEmployeeList, ddlCompanyName.SelectedValue);
                //-------------------------------------------------
            }
        }

    }
}