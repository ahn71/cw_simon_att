using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class leave_default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!IsPostBack)
                {
                    HttpCookie getCookies = Request.Cookies["userInfo"];
                    if (getCookies == null || getCookies.Value == "")
                    {
                        Response.Redirect("~/ControlPanel/Login.aspx");

                    }
                    if (getCookies["__IsCompliance__"].ToString().Equals("True"))
                    {
                        divHoliday.Visible = false;
                        divHolidayComp.Visible = true;
                        //divEarnLeaveGeneration.Visible = false;
                        //divEarnLeaveGenerationComp.Visible = true;

                        divConfiguration.Visible = false;
                        divApplication.Visible = false;
                        //divShortLeave.Visible = false;
                        divLeaveApproval.Visible = false;
                        //divShortLvApproval.Visible = false;
                        divLeaveList.Visible = false;
                        divSummaryReport.Visible = false;
                        //divBalanceReport.Visible = false;
                    }
                    else
                    {
                        divHoliday.Visible = true;
                        divHolidayComp.Visible = false;
                        //divEarnLeaveGeneration.Visible = true;
                        //divEarnLeaveGenerationComp.Visible = false;
                    }
                }




            }
            catch (Exception ex)
            {
                Response.Redirect("~/ControlPanel/Login.aspx");
            }
        }
    }
}