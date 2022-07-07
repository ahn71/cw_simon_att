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
    public partial class pf_interestentry : System.Web.UI.Page
    {
        string CompanyId = "";
        string sqlcmd = "";
        DataTable dt;
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "pf_interestentry.aspx", ddlCompanyName, gvPFSettings, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                commonTask.loadPFInvestmentType(ddlType);
                
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                

            }


            catch { }

        }
        private void loadInterest()
        {
            try
            {
                CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;

                sqlcmd = "select PF_FDR_Interest.FdrID,CompanyID,PF_FDR.FdrNo,PF_FDR_Interest.InterestAmount,Charge,NetInterest,convert(varchar(10), WithdrawDate,105) as WithdrawDate,TaxChargePer,TaxCharge  from PF_FDR_Interest inner join PF_FDR on PF_FDR_Interest.FdrID=PF_FDR.FdrID where PF_FDR.CompanyID='" + CompanyId + "' and PF_FDR.Type=" + ddlType.SelectedValue + "";

                 dt = new DataTable();
                sqlDB.fillDataTable(sqlcmd, dt);
                gvPFSettings.DataSource = dt;
                gvPFSettings.DataBind();

            }
            catch { }
        }
       
        private void loadFDRInfo() 
        {
            sqlcmd = "select FdrAmount,InterestRate,InterestAmount,convert(varchar(10), FromDate,105) FromDate,convert(varchar(10), ToDate,105)  ToDate,Period from PF_FDR where FdrID="+ddlFDRList.SelectedValue+"";
            dt = new DataTable();
            sqlDB.fillDataTable(sqlcmd, dt);
            txtFDRAmount.Text = dt.Rows[0]["FdrAmount"].ToString();
            txtInterestRate.Text = dt.Rows[0]["InterestRate"].ToString();
            txtInterestAmount.Text = dt.Rows[0]["InterestAmount"].ToString();
            txtFromDate.Text = dt.Rows[0]["FromDate"].ToString();
            txtToDate.Text = dt.Rows[0]["ToDate"].ToString();
            txtPeriod.Text = dt.Rows[0]["Period"].ToString();            
            
        }

        protected void ddlFDRList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadFDRInfo();
        }

        protected void txtBankCharge_TextChanged(object sender, EventArgs e)
        {           
            if (txtInterestAmount.Text.Trim() != "" && txtBankCharge.Text.Trim() != "" && txtTaxCharge.Text.Trim() != "")
            {
                txtNetInterestAmount.Text = (int.Parse(txtInterestAmount.Text.Trim()) - int.Parse(txtBankCharge.Text.Trim()) - int.Parse(txtTaxCharge.Text.Trim())).ToString();
            }
        }
        private void SaveFDRInterest()
        {
            try
            {
                SQLOperation.forDeleteRecordByIdentifier("PF_FDR_Interest", "FdrID", ddlFDRList.SelectedValue, sqlDB.connection);
                SqlCommand cmd = new SqlCommand("insert into PF_FDR_Interest values('" + ddlFDRList.SelectedValue+ "','" +txtInterestAmount.Text.Trim() + "','" +
                    txtBankCharge.Text.Trim() + "','" + txtNetInterestAmount.Text.Trim() + "','" + convertDateTime.getCertainCulture(txtWithdrawDate.Text).ToString() +
                    "',"+txtTaxChargePer.Text.Trim()+","+txtTaxCharge.Text.Trim()+") ", sqlDB.connection);               
                if (int.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                {
                    loadInterest();
                    if(btnSave.Text=="Save")
                    lblMessage.InnerText = "success-> Successfully Saved.";
                    else
                        lblMessage.InnerText = "success-> Successfully Updated.";
                    allClear();
                }
                else
                    lblMessage.InnerText = "error-> Unable to Save !";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error>" + ex.Message;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveFDRInterest();
        }

        protected void gvPFSettings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Alter"))
            {
                string a = ViewState["__preRIndex__"].ToString();
                if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvPFSettings.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");


                gvPFSettings.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                ViewState["__preRIndex__"] = rIndex;

                
               

                sqlcmd = "select FdrAmount,InterestRate,PF_FDR_Interest.InterestAmount,convert(varchar(10), FromDate,105) FromDate,convert(varchar(10), ToDate,105)"+
                    "  ToDate,Period,convert(varchar(10), WithdrawDate,105) WithdrawDate,NetInterest,Charge,TaxChargePer,TaxCharge  from PF_FDR_Interest inner  join PF_FDR on PF_FDR_Interest.FdrID=PF_FDR.FdrID where PF_FDR_Interest.FdrID=" + gvPFSettings.DataKeys[rIndex].Values[1].ToString() + "";
                dt = new DataTable();
                sqlDB.fillDataTable(sqlcmd, dt);
                ddlCompanyName.SelectedValue = gvPFSettings.DataKeys[rIndex].Values[0].ToString();
                ddlFDRList.SelectedValue = gvPFSettings.DataKeys[rIndex].Values[1].ToString();
                txtFDRAmount.Text = dt.Rows[0]["FdrAmount"].ToString();
                txtInterestRate.Text = dt.Rows[0]["InterestRate"].ToString();
                txtFromDate.Text = dt.Rows[0]["FromDate"].ToString();
                txtToDate.Text = dt.Rows[0]["ToDate"].ToString();
                txtPeriod.Text = dt.Rows[0]["Period"].ToString();
                txtInterestAmount.Text = dt.Rows[0]["InterestAmount"].ToString();
                txtTaxChargePer.Text = dt.Rows[0]["TaxChargePer"].ToString();
                txtTaxCharge.Text = dt.Rows[0]["TaxCharge"].ToString();
                txtBankCharge.Text = dt.Rows[0]["Charge"].ToString();
                txtWithdrawDate.Text = dt.Rows[0]["WithdrawDate"].ToString();
                txtNetInterestAmount.Text = dt.Rows[0]["NetInterest"].ToString();
            
                btnSave.Text = "Update";
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Pbutton";
                }
            }
            else if (e.CommandName.Equals("deleterow"))
            {
                SQLOperation.forDeleteRecordByIdentifier("PF_FDR_Interest", "FdrID", gvPFSettings.DataKeys[rIndex].Values[1].ToString(), sqlDB.connection);
                allClear();
                lblMessage.InnerText = "success->Successfully  Deleted";
                gvPFSettings.Rows[rIndex].Visible = false;
            }
        }
        private void allClear()
        {

           
            txtFDRAmount.Text = "";
            // txtRateOfInterest.Text = "";
            txtInterestRate.Text = "";
            txtInterestAmount.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtPeriod.Text = "";
            ddlFDRList.SelectedValue = "0";
            txtWithdrawDate.Text = "";
            txtNetInterestAmount.Text = "";
           
            
            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Pbutton";
            }
            btnSave.Text = "Save";
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.loadFDRList(ddlFDRList, ddlCompanyName.SelectedValue,ddlType.SelectedValue);
            loadInterest();
        }

        protected void txtTaxChargePer_TextChanged(object sender, EventArgs e)
        {
            if (txtInterestAmount.Text.Trim() != "" && txtTaxChargePer.Text.Trim() != "")
                txtTaxCharge.Text = ((int.Parse(txtInterestAmount.Text.Trim()) * int.Parse(txtTaxChargePer.Text.Trim())) / 100).ToString();
            if (txtInterestAmount.Text.Trim() != "" && txtBankCharge.Text.Trim() != "" && txtTaxCharge.Text.Trim() != "")
            {
                txtNetInterestAmount.Text = (int.Parse(txtInterestAmount.Text.Trim()) - int.Parse(txtBankCharge.Text.Trim())-int.Parse(txtTaxCharge.Text.Trim())).ToString();
            }
        }
    
    }
}