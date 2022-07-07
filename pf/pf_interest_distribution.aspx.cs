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
    public partial class pf_interest_distribution : System.Web.UI.Page
    {
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "pf_interest_distribution.aspx", ddlCompanyName, gvPFSettings, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                //loadInterest();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadFDRListForProfitDistribution(ddlFDRList, ddlCompanyName.SelectedValue);
                classes.commonTask.loadPFExpenseYear(ddlYear,ddlCompanyName.SelectedValue);

            }


            catch { }

        }
        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.loadFDRListForProfitDistribution(ddlFDRList, ddlCompanyName.SelectedValue);
            classes.commonTask.loadPFExpenseYear(ddlYear, ddlCompanyName.SelectedValue);
           // loadInterest();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (rblDistribution.SelectedValue == "Profit")
                ProfitDistribute();
            else
                ExpenseDistribute();
        }
        private void ProfitDistribute()
        {
            try
            {
                //------------------------------- Get FDR Information ----------------------------------------------------
                DataTable dtFDR = new DataTable();
                sqlDB.fillDataTable("select NetInterest,WithdrawDate from PF_FDR_Interest where  FdrID="+ddlFDRList.SelectedValue+"", dtFDR);
                string MaturedDate = dtFDR.Rows[0]["WithdrawDate"].ToString();
                string ProfitMonth = dtFDR.Rows[0]["WithdrawDate"].ToString();// DateTime.Parse(MaturedDate).AddMonths(1).ToString("MM-yyyy");

                //-------------------- Get PF Members --------------------------------------------------------------------
                DataTable dtPfMember = new DataTable();

                sqlDB.fillDataTable("  with pr as (select EmpID,EmpContribution,EmprContribution,0 as profit,CONVERT(VARCHAR(7), Month, 126) Month from PF_PFRecord where Month<'" + MaturedDate +
                   "' and EmpID in(select EmpId from Personnel_EmpCurrentStatus where IsActive=1 and PfMember=1))," +
                   " pp as (select EmpID,0 as EmpContribution,0 as EmprContribution, profit,CONVERT(VARCHAR(7), Month, 126) Month from PF_Profit where Month<'" + MaturedDate +
                   "' and EmpID in(select EmpId from Personnel_EmpCurrentStatus where IsActive=1 and PfMember=1)), " +
                   " pr1 as (select * from pr union select * from  pp)," +
                   " pr2 as(select EmpiD,Month,(sum(EmpContribution)+sum(EmprContribution)+sum(profit) ) * (DATEDIFF(DAY,Month+'-01','" + MaturedDate + "')/30*30) Amount from pr1 group by Empid,Month) " +
                   " select EmpID,sum(Amount) Amount from pr2 group by Empid", dtPfMember);

                //sqlDB.fillDataTable(" with a as ( "+
                //    " select pcs.EmpID,(pr.EmpContribution+pr.EmprContribution+sum( isnull(pp.Profit,0))) * (DATEDIFF(DAY,pr.Month,'"+MaturedDate+"')/30*30) TK "+
                //    " from Personnel_EmpCurrentStatus pcs inner join PF_PFRecord pr on pcs.EmpId=pr.EmpID and pcs.IsActive=1 and PfMember=1 left join PF_Profit pp on pr.EmpID=pp.EmpId and pr.Month=pp.Month "+
                //    " where pr.Month<'" + MaturedDate + "' and pcs.CompanyId='"+ddlCompanyName.SelectedValue+"' " +
                //    " group by pcs.EmpID, pr.Month,pr.EmpContribution,pr.EmprContribution ) "+
                //    " select a.EmpID, sum(a.TK) Amount from a  group by a.EmpID", dtPfMember);
                //--------------------------------------------------------------------------------------------------------

                float TotalAmount = float.Parse( dtPfMember.Compute("sum(Amount)", "").ToString());
                float TotalProfit = float.Parse(dtFDR.Rows[0]["NetInterest"].ToString());
                float Profit = 0;
                int count = 0;
                //-------------------------- Delete Existing Record for this FDR------------------------------------------
                SqlCommand cmdDel = new SqlCommand("delete from PF_Profit where FdrID='" + ddlFDRList.SelectedValue + "'", sqlDB.connection);
                cmdDel.ExecuteNonQuery();
                //---------------------------------------------------------------------------------------------------------
                for (int i = 0; i < dtPfMember.Rows.Count; i++)
                {
                    Profit = TotalProfit / TotalAmount * float.Parse(dtPfMember.Rows[i]["Amount"].ToString());
                    SqlCommand cmd = new SqlCommand("insert into PF_Profit values('" + ddlFDRList.SelectedValue + "','" + dtPfMember.Rows[i]["EmpId"].ToString() + "','" +
                    ProfitMonth + "','" + Profit + "') ", sqlDB.connection);
                    if (int.Parse(cmd.ExecuteNonQuery().ToString())== 1)
                        count++;
                }

                lblMessage.InnerText = "success-> Successfully  processed for " + count + " emplyee.";

            }
            catch { lblMessage.InnerText="error-> Unable to process !"; }
        }
        private void ExpenseDistribute()
        {
            try
            {
                //------------------------------- Get FDR Information ----------------------------------------------------
                string[] value = ddlYear.SelectedValue.Split('/');
                string ExpenseDate = value[0];
                float TotalExpense = float.Parse(value[1]);


                //-------------------- Get PF Members --------------------------------------------------------------------
                DataTable dtPfMember = new DataTable();
                sqlDB.fillDataTable("  with pr as (select EmpID,EmpContribution,EmprContribution,0 as profit,CONVERT(VARCHAR(7), Month, 126) Month from PF_PFRecord where Month<='"+ExpenseDate+
                    "' and EmpID in(select EmpId from Personnel_EmpCurrentStatus where IsActive=1 and PfMember=1)),"+
                    " pp as (select EmpID,0 as EmpContribution,0 as EmprContribution, profit,CONVERT(VARCHAR(7), Month, 126) Month from PF_Profit where Month<='"+ExpenseDate+
                    "' and EmpID in(select EmpId from Personnel_EmpCurrentStatus where IsActive=1 and PfMember=1)), "+
                    " pr1 as (select * from pr union select * from  pp),"+
                    " pr2 as(select EmpiD,Month,(sum(EmpContribution)+sum(EmprContribution)+sum(profit) ) * (DATEDIFF(DAY,Month+'-01','"+ExpenseDate+"')/30*30) Amount from pr1 group by Empid,Month) "+
                    " select EmpID,sum(Amount) Amount from pr2 group by Empid", dtPfMember);
                //--------------------------------------------------------------------------------------------------------

                float TotalAmount = float.Parse(dtPfMember.Compute("sum(Amount)", "").ToString());
                
                float Expense = 0;
                int count = 0;
                //-------------------------- Delete Existing Record for this FDR------------------------------------------
                SqlCommand cmdDel = new SqlCommand("delete from PF_Expense where Month='" + ExpenseDate + "'", sqlDB.connection);
                cmdDel.ExecuteNonQuery();
                //---------------------------------------------------------------------------------------------------------
                for (int i = 0; i < dtPfMember.Rows.Count; i++)
                {
                    Expense = TotalExpense / TotalAmount * float.Parse(dtPfMember.Rows[i]["Amount"].ToString());
                    SqlCommand cmd = new SqlCommand("insert into PF_Expense values('" + dtPfMember.Rows[i]["EmpId"].ToString() + "','" +ExpenseDate + "','" 
                        + Expense + "') ", sqlDB.connection);
                    if (int.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                        count++;
                }

                lblMessage.InnerText = "success-> Successfully  processed for " + count + " emplyee.";

            }
            catch { lblMessage.InnerText = "error-> Unable to process !"; }
        }
        protected void rblDistribution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblDistribution.SelectedValue == "Profit")
            {
                divYear.Visible = false;
                divFdrNo.Visible = true;
            }
            else 
            {
                divYear.Visible = true;
                divFdrNo.Visible = false;
            }
        }
    }
}