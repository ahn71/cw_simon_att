using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class attendance_default : System.Web.UI.Page
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
                        divMonthSetup.Visible = false;
                        divMonthSetupComp.Visible = true;

                        divAttProcessing.Visible = false;
                        divManuallyCount.Visible = false;
                        divAttendanceList.Visible = false;
                        divAttSummary.Visible = false;
                        divInOutReport.Visible = false;
                        divManualReport.Visible = false;
                        divManpowerStatement.Visible = false;
                        divManpowerWiseAttendance.Visible = false;
                        divOvertimeReport.Visible = false;
                       // divAbsentNotification.Visible = false;
                        divOutduty.Visible = false;
                        divOutdutyApproval.Visible = false;
                        divOutdutyReport.Visible = false;
                    }
                    else
                    {
                        divMonthSetup.Visible = true;
                        divMonthSetupComp.Visible = false;
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