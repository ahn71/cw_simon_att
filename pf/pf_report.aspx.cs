using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.pf
{
    public partial class pf_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {

                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();


            }
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CShortName__"] = getCookies["__CShortName__"].ToString();


                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "vat_tax_report.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
              
               
           
                //-----------------------------------------------------


            }
            catch { }
        }


        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (!rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    txtEmpCardNo.Enabled = true;
                    pnl1.Enabled = false;

                    pnl1.Visible = false;
                    txtEmpCardNo.Focus();
                    if (rblReportType.SelectedValue == "MonthlySheet")
                    {
                        tdMonth.InnerText = "From Month";
                        txtToMonth.Enabled = true;
                    }

                }
                else
                {
                    txtEmpCardNo.Enabled = false;
                    pnl1.Enabled = true;                 

                    pnl1.Visible = true;
                    if (rblReportType.SelectedValue == "MonthlySheet")
                    {
                        tdMonth.InnerText = "Month";
                        txtToMonth.Enabled = false;
                    }
                       

                }
                


            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
          
            if (rblGenerateType.SelectedItem.Text.Equals("All") && lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning->Please select any Department"; lstSelected.Focus(); return; }
            if (!rblGenerateType.SelectedItem.Text.Equals("All") && txtEmpCardNo.Text.Trim().Length < 4) { lblMessage.InnerText = "warning->Please type valid Card No!(Minimum last 4 digit.)"; txtEmpCardNo.Focus(); return; }
            if (rblReportType.SelectedValue == "MonthlySheet")
                MonthlyPFSheet();
            else if (rblReportType.SelectedValue == "BalanceSheet")
                PFBalanceSheet();
            else if (rblReportType.SelectedValue == "BalanceSummary")
                PFBalanceSummary();

        }
        private void MonthlyPFSheet()
        {
            try
            {
                string CompanyList = "";
                string DepartmentList = "";
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                string[] Month = txtFromMonth.Text.Trim().Split('-');
                string getSQLCMD;
                DataTable dt = new DataTable();
                string daterange = "";
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {

                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "select  pfr.EmpId,substring(EmpCardNo,10,6) EmpCardNo,EmpName,DsgName,GId,DptId,pcs.DptName,pcs.GName, convert(varchar(10),pcs.PfDate,105) PfDate, EmpContribution EmpContributionAmount,EmprContribution EmprContributionAmount,CompanyId,CompanyName,Address from PF_PFRecord pfr inner join v_Personnel_EmpCurrentStatus pcs on pfr.EmpID=pcs.EmpId  where pfr.Month='" + Month[2] + "-" + Month[1] + "-01' and pcs.IsActive=1 and pcs.CompanyId='" + CompanyList + "' and pcs.DptId " + DepartmentList + "   order by convert(int,DptId),convert(int,GId),CustomOrdering";




                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }
                    daterange = Month[1] + " - " + Month[2];
                }
                else
                {
                    string[] ToMonth = txtToMonth.Text.Trim().Split('-');
                   // getSQLCMD = ";with openingbalance as (Select es.EmpId,(ISNULL(sum(ms.ProvidentFund),0)+es.PfOpeningBalance) as Payable from Payroll_MonthlySalarySheet ms  right outer join Personnel_EmployeeInfo es on ms.EmpId=es.EmpId and YearMonth>=(Select PfcountDate From HRD_CompanyInfo where CompanyId='" + CompanyList + "') and YearMonth<'" + "" + "' where PfMember=1  and es.CompanyId='" + ddlCompanyName.SelectedValue + "' group by es.EmpId ,PfOpeningBalance) SELECT format(YearMonth,'MMM-yyyy') MonthName,ProvidentFund EmpContributionAmount,ProvidentFund EmprContributionAmount, EmpName,Substring(EmpCardNo,10,6) as EmpCardNo, DptId, DptName, DsgName,CONVERT(VARCHAR(10), PfDate, 105) as PfDate, CompanyName,  CompanyId, GName, GId, Address,openingbalance.Payable as OpeningBalance,(openingbalance.Payable+(ProvidentFund*2)) as ClosingBalance   FROM    v_MonthlySalarySheet left join openingbalance on v_MonthlySalarySheet.EmpId=openingbalance.EmpId    where  CompanyId  in(" + CompanyList + ")  and YearMonth='" + "" + "' and  ProvidentFund>0 and  IsActive=1 and EmpCardNo like'%" + txtEmpCardNo.Text + "' ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";

                    getSQLCMD = "select format( Month,'MM-yyyy')  YearMonth, pfr.EmpId,substring(EmpCardNo,10,6) EmpCardNo,EmpName,DsgName,GId,DptId,pcs.DptName,pcs.GName, convert(varchar(10),pcs.PfDate,105) PfDate ,convert(varchar(10),pcs.EmpJoiningDate,105) EmpJoiningDate, EmpContribution ProvidentFund ,CompanyId,CompanyName,Address from PF_PFRecord pfr inner join v_Personnel_EmpCurrentStatus pcs on pfr.EmpID=pcs.EmpId  where pfr.Month>='" + Month[2] + "-" + Month[1] + "-01' and pfr.Month<='" + ToMonth[2] + "-" + ToMonth[1] + "-01' and pcs.IsActive=1 and pcs.CompanyId='" + CompanyList + "' and pcs.EmpCardNo like'%" + txtEmpCardNo.Text + "' order by Month";
                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }

                    daterange = Month[1] + " - " + Month[2] + " To " + ToMonth[1] + " - " + ToMonth[2];
                }

                Session["__MonthlyPFSheet__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=MonthlyPFSheet- " +daterange.Replace('-','/')+ "-"+rblGenerateType.SelectedValue+"');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

      
        private void PFBalanceSheet()
        {
            try
            {
                string CompanyList = "";
                string DepartmentList = "";
                string []FMonth=txtFromMonth.Text.Trim().Split('-');
                string []TMonth=txtToMonth.Text.Trim().Split('-');
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();                
                string getSQLCMD;
                DataTable dt = new DataTable();
                string DateRange = "";
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {

                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "with pe as(select EmpId,sum(Expense) Expense from PF_Expense  where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] + 
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpId),pp as(select EmpId,sum(Profit) Profit from PF_Profit  where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] + 
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where DptID "+DepartmentList+" and IsActive=1) group by EmpId), "+
                        " pr as (select EmpID,sum(EmpContribution) EmpContribution,sum(EmprContribution) EmprContribution from PF_PFRecord where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] + 
                        "'  and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus where DptID "+DepartmentList+" and IsActive=1) group by EmpId) ,"+
                        " ob as(select pr.EmpId,pr.EmpContribution+pr.EmprContribution+isnull(pp.Profit,0)-ISNULL(pe.Expense,0) OpeningBalance from pr left join "+
                        " pp on pr.EmpId=pp.EmpId left join pe on pr.EmpID=pe.EmpId), pp1 as( select EmpId,sum(Profit) Profit from PF_Profit  where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] + "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpId),  pr1 as(select EmpID,sum(EmpContribution) EmpContribution,sum(EmprContribution) EmprContribution from PF_PFRecord where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0]+
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] + "' and  EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpID ) ,  pe1 as(select EmpID,sum(Expense) Expense from PF_Expense where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] + "' and  EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpID )  select CustomOrdering, pr1.EmpID,SUBSTRING(pcs.EmpCardNo,10,10) as EmpCardNo,EmpName,DptName,DsgName,DptId,CompanyId,CompanyName,Address,"+
                        " convert(varchar,PFdate,105) PFDate,isnull(ob.OpeningBalance,0) OpeningBalance, pr1.EmpContribution,pr1.EmprContribution,isnull(pp1.Profit,0) Profit,ISNULL(pe1.Expense,0) Expense "+
                        " from pr1 inner join v_Personnel_EmpCurrentStatus  pcs on pr1.EmpID=pcs.EmpId left join pp1 on pr1.EmpID=pp1.EmpId left join ob on pr1.EmpID=ob.OpeningBalance left join pe1 on "+
                        " pr1.EmpID=pe1.EmpId where DptID "+DepartmentList+" and IsActive=1 order by convert(int,DptId),CustomOrdering";
                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }
                    DateRange = "General Ledger From " + txtFromMonth.Text.Trim() + " To " + txtToMonth.Text.Trim();

                }
                else
                {
                    getSQLCMD = "with pe as(select EmpId,sum(Expense) Expense from PF_Expense  where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where EmpCardNo Like'%"+txtEmpCardNo.Text.Trim()+
                        "' and IsActive=1) group by EmpId),pp as(select EmpId,sum(Profit) Profit from PF_Profit  where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where EmpCardNo Like'%" + txtEmpCardNo.Text.Trim() + 
                        "' and IsActive=1) group by EmpId), pr as (select EmpID,sum(EmpContribution) EmpContribution,sum(EmprContribution) EmprContribution "+
                        " from PF_PFRecord where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +"'  and EmpId in (select distinct EmpId "+
                        " from Personnel_EmpCurrentStatus where EmpCardNo Like'%" + txtEmpCardNo.Text.Trim() + "' and IsActive=1) group by EmpId) ," +
                        " ob as(select pr.EmpId,pr.EmpContribution+pr.EmprContribution+isnull(pp.Profit,0)-ISNULL(pe.Expense,0) OpeningBalance from pr left join " +
                        " pp on pr.EmpId=pp.EmpId left join pe on pr.EmpID=pe.EmpId), pp1 as( select EmpId,sum(Profit) Profit from PF_Profit  where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] + "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus "+
                        "  where EmpCardNo Like'%" + txtEmpCardNo.Text.Trim() + "' and IsActive=1) group by EmpId),  pr1 as(select EmpID,sum(EmpContribution) EmpContribution,sum(EmprContribution) EmprContribution from PF_PFRecord where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] + "' and  EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus "+
                        "  where EmpCardNo Like'%" + txtEmpCardNo.Text.Trim() + "' and IsActive=1) group by EmpID ) ,  pe1 as(select EmpID,sum(Expense) Expense from PF_Expense where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] + "' and  EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus "+
                        " where EmpCardNo Like'%" + txtEmpCardNo.Text.Trim() + "' and IsActive=1) group by EmpID )  select CustomOrdering, pr1.EmpID,SUBSTRING(pcs.EmpCardNo,10,10) as EmpCardNo, "+
                        " EmpName,DptName,DsgName,DptId,CompanyId,CompanyName,Address," +
                        " convert(varchar,PFdate,105) PFDate,isnull(ob.OpeningBalance,0) OpeningBalance, pr1.EmpContribution,pr1.EmprContribution,isnull(pp1.Profit,0) Profit,ISNULL(pe1.Expense,0) Expense " +
                        " from pr1 inner join v_Personnel_EmpCurrentStatus  pcs on pr1.EmpID=pcs.EmpId left join pp1 on pr1.EmpID=pp1.EmpId left join ob on pr1.EmpID=ob.OpeningBalance left join pe1 on " +
                        " pr1.EmpID=pe1.EmpId where EmpCardNo Like'%" + txtEmpCardNo.Text.Trim() + "' and IsActive=1 ";

                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }
                    DateRange = "Individual Ledger From " + txtFromMonth.Text.Trim() + " To " + txtToMonth.Text.Trim();


                }
                Session["__PFBalanceSheet__"] = dt;
                
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PfBalanceSheet-"+DateRange.Replace('-','/')+"');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void PFBalanceSummary()
        {
            try
            {
                string CompanyList = "";
                string DepartmentList = "";
                string[] FMonth = txtFromMonth.Text.Trim().Split('-');
                string[] TMonth = txtToMonth.Text.Trim().Split('-');
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                string getSQLCMD;
                DataTable dt = new DataTable();
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "with pe as(select EmpId,sum(Expense) Expense from PF_Expense  where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpId),pp as(select EmpId,sum(Profit) Profit from PF_Profit  where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpId),  pr as (select EmpID,sum(EmpContribution) EmpContribution,sum(EmprContribution) EmprContribution "+
                        " from PF_PFRecord where Month<'" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +"'  and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus "+
                        " where DptID "+DepartmentList+" and IsActive=1) group by EmpId) , ob as(select pr.EmpId,pr.EmpContribution+pr.EmprContribution+isnull(pp.Profit,0)-ISNULL(pe.Expense,0) OpeningBalance "+
                        " from pr left join  pp on pr.EmpId=pp.EmpId left join pe on pr.EmpID=pe.EmpId), pp1 as( select EmpId,sum(Profit) Profit "+
                        " from PF_Profit  where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +"' and  Month<='" +TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] +
                        "' and EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus  where DptID "+DepartmentList+" and IsActive=1) group by EmpId), "+
                        " pr1 as(select EmpID,sum(EmpContribution) EmpContribution,sum(EmprContribution) EmprContribution from PF_PFRecord where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] +"' and  EmpId in (select distinct EmpId from Personnel_EmpCurrentStatus where DptID "+DepartmentList+
                        " and IsActive=1) group by EmpID ) ,  pe1 as(select EmpID,sum(Expense) Expense from PF_Expense where Month>='" + FMonth[2] + "-" + FMonth[1] + "-" + FMonth[0] +
                        "' and  Month<='" + TMonth[2] + "-" + TMonth[1] + "-" + TMonth[0] +"' and  EmpId in (select distinct EmpId "+
                        " from Personnel_EmpCurrentStatus where DptID "+DepartmentList+" and IsActive=1) group by EmpID )  " +
                        " select count(distinct pcs.EmpId) EmpID,  DptId, DptName,CompanyId,CompanyName,Address,sum( isnull(ob.OpeningBalance,0)) OpeningBalance "+
                        ", sum( pr1.EmpContribution ) EmpContribution ,sum(pr1.EmprContribution) EmprContribution,sum( isnull(pp1.Profit,0)) Profit ,sum( isnull(pe1.Expense,0)) Expense "+
                        "  from pr1 inner join v_Personnel_EmpCurrentStatus  pcs on pr1.EmpID=pcs.EmpId left join pp1 on pr1.EmpID=pp1.EmpId left join ob on pr1.EmpID=ob.OpeningBalance "+
                        " left join pe1 on  pr1.EmpID=pe1.EmpId  where DptID "+DepartmentList+" and IsActive=1 group by DptId, DptName,CompanyId,CompanyName,Address order by convert(int,DptId)";
                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }      
                Session["__PFBalanceSummary__"] = dt;
                string DateRange = txtFromMonth.Text.Trim() + " To " + txtToMonth.Text.Trim();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PfBalanceSummary-" + DateRange.Replace('-', '/') + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
       

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);
               
              
            }
            catch { }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);

        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

        protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReportType.SelectedValue == "MonthlySheet" && rblGenerateType.SelectedValue == "0")
            {
                tdMonth.InnerText = "Month";
                txtToMonth.Enabled = false;
            }
            else
            {
                tdMonth.InnerText = "From Month";
                txtToMonth.Enabled = true;
            }
        }
    }
}