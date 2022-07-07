﻿using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class ShiftManagement : System.Web.UI.Page
    {
        string SqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        { 

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = 100;
            ProgressBar1.BackColor = System.Drawing.Color.Blue;
            ProgressBar1.ForeColor = Color.White;
            ProgressBar1.Height = new Unit(20);
            ProgressBar1.Width = new Unit(500);
            lblErrorMessage.Text = "";
            if (!IsPostBack)
            {
                lblErrorMessage.Text = "";
                setPrivilege(sender,e);
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
              
               // HttpCookie getCookies = Request.Cookies["userInfo"];
              //  ViewState["__CompanyId__"] = ddlCompanyList.SelectedValue;
                divRecordMessage.Visible = true;
                
                divRecordMessage.InnerText = "Shift Manage Panel";

                
            }
           
        }
        private void setPrivilege(object sender, EventArgs e)
        {
            try
            {                
                ViewState["__WriteAction__"] = "1";                
                ViewState["__UpdateAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__DptId__"] = getCookies["__DptId__"].ToString();

                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "ShiftManagement.aspx", ddlCompanyList, gvEmpList, btnSubmit);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                          
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);

                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                {
                    ddlDepartmentList.SelectedValue = ViewState["__DptId__"].ToString();
                    ddlDepartmentList.Enabled = false;
                    ddlDepartmentList_SelectedIndexChanged(sender, e);
                }
                    

            }
            catch { }

        }
        DataTable dtSTD;
        private void loadSTD()  // STD=Shift Transfer Date
        {
            try
            {
                dtSTD=new DataTable ();
                sqlDB.fillDataTable("Select distinct FORMAT(ShiftTransferDate,'dd-MM-yyyy') as STD From v_EmployeeDetails where EmpStatus in ('1','8') and IsActive='1' and ActiveSalary='True' and SftId='318'  order by STD  desc",dtSTD);

            }
            catch { }
        }
        private bool CheckDateRangeBasket(DateTime TFromDate,DateTime TToDate, string GID)
        {
            try
            {
                GID = (GID == "0") ? "" : " AND GId = " + ddlGroupList.SelectedValue;
                DataTable dt = new DataTable();              
                sqlDB.fillDataTable("Select Distinct DptName+' | '+SftName+' ('+Convert(Varchar,TFromDate)+'  '+Convert(varchar,TToDate)+') ' as Info From v_ShiftTransferInfoDetails where DptId ='" + ddlDepartmentList.SelectedValue + "' AND SDate >='" + TFromDate.ToString("yyyy-MM-dd") + "' AND SDate <='" + TToDate.ToString("yyyy-MM-dd") + "' AND SftId=" + ddlNewShift.SelectedValue + "  "+GID, dt);

                if (dt!=null && dt.Rows.Count > 0) 
                {
                    
                    lblMessage.InnerText = "warning-> Already this roster created for " + dt.Rows[0]["Info"].ToString();
                    return false;               
                }
                else return true;

            }
            catch { return false; }
        }

        DataTable dtShiftInfoDetails = new DataTable();
        private void LoadAllEmployeeList(bool ForJustDepartment)
        {
            try
            {
                dtShiftInfoDetails = new DataTable();
                if (!ForJustDepartment)  // if for just department is true then execut else block.else execute if block 
                {
                    DateTime TFromDate = DateTime.Parse(commonTask.ddMMyyyyTo_yyyyMMdd(txtFromDate.Text.Trim())); 
                    DateTime TToDate = DateTime.Parse(commonTask.ddMMyyyyTo_yyyyMMdd(txtToDate.Text.Trim()));
                    if (!CheckDateRangeBasket(TFromDate, TToDate,ddlGroupList.SelectedValue)) return;

                     SqlCmd = " Select Distinct EmpId,EmpCardNo+' ('+ EmpProximityNo+')' as EmpCardNo,EmpName,DsgName,CAST('' as varchar(30)) as SftName,EmpType,Format(GETDATE()+1,'yyyy-MM-dd') as EntryDate,CAST(0 AS bit) AS Status,CAST(0 AS bit) AS Assigned," +
                        "CAST(NULL as int) as FId,CAST('' as varchar(30)) as Notes,SftId From v_Personnel_EmpCurrentStatus where IsActive=1 and DptId ='" + ddlDepartmentList.SelectedItem.Value + "' " +
                        " AND GId=" + ddlGroupList.SelectedValue + " AND  EmpDutyType='Roster' AND EmpStatus in (1,8) AND   EmpId not in (Select Distinct EmpId From v_ShiftTransferInfoDetails " +
                        " where DptId ='" + ddlDepartmentList.SelectedItem.Value + "'AND Format(EntryDate,'yyyy-MM-dd')='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND TFromDate='" + TFromDate.ToString("yyyy-MM-dd") + "' AND TToDate='" + TToDate.ToString("yyyy-MM-dd") + "' AND GId=" + ddlGroupList.SelectedValue + " )   " +
                        " union "+
                        " Select Distinct  EmpId,EmpCardNo+' ('+ EmpProximityNo+')' as EmpCardNo,EmpName,DsgName,SftName,EmpType,Format(GETDATE()+1,'yyyy-MM-dd') as EntryDate,CAST(0 AS bit) AS Status,CAST(1 AS bit) AS Assigned," +
                        " CAST(NULL as int) as FId,CAST('' as varchar(30)) as Notes,SftId From v_ShiftTransferInfoDetails "+
                        " where DptId ='" + ddlDepartmentList.SelectedItem.Value + "'AND Format(EntryDate,'yyyy-MM-dd')='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND TFromDate='" + TFromDate.ToString("yyyy-MM-dd") + "' AND TToDate='" + TToDate.ToString("yyyy-MM-dd") + "'" +
                        " and SftId <> " + ddlNewShift.SelectedItem.Value + " AND GId=" + ddlGroupList.SelectedValue + " " +
                        "order by EmpCardNo";
                    


                    //sqlDB.fillDataTable("Select Distinct EmpId,EmpCardNo,EmpName,DsgName,SftName,EmpType,Format(GETDATE()+1,'yyyy-MM-dd') as EntryDate,CAST(0 AS bit) AS Status,CAST(NULL as int) as FId,CAST('' as varchar(30)) as Notes,SftId From v_Personnel_EmpCurrentStatus where DptId ='" + ddlDepartmentList.SelectedItem.Value + "'   AND GId=" + ddlGroupList.SelectedValue + " AND  EmpDutyType='Roster' AND  " +
                    //" EmpId not in (Select Distinct EmpId From v_ShiftTransferInfoDetails where DptId ='" + ddlDepartmentList.SelectedItem.Value + "'AND Format(EntryDate,'yyyy-MM-dd')='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND TFromDate='" + TFromDate.ToString("yyyy-MM-dd") + "' AND TToDate='" + TToDate.ToString("yyyy-MM-dd") + "' AND SftId=" + ddlNewShift.SelectedItem.Value + " AND GId=" + ddlGroupList.SelectedValue + " )  order by EmpCardNo", dtShiftInfoDetails);


                }
                else
                {
                    SqlCmd = " Select Distinct EmpId,EmpCardNo+' ('+ EmpProximityNo+')' as EmpCardNo,EmpName,DsgName,CAST('' as varchar(30)) as SftName,EmpType,CAST(0 AS bit) AS Status,CAST(0 AS bit) AS Assigned,CAST(NULL as int) as FId,CAST('' as varchar(30)) as Notes,SftId From v_Personnel_EmpCurrentStatus where IsActive=1 and DptId ='" + ddlDepartmentList.SelectedItem.Value + "' AND CompanyId='" + ddlCompanyList.SelectedValue + "' AND  GId=" + ddlGroupList.SelectedValue + "" +
                     " AND EmpDutyType='Roster' And EmpStatus in (1,8) order by EmpCardNo";
                }
                sqlDB.fillDataTable(SqlCmd, dtShiftInfoDetails);

                gvEmpList.DataSource = dtShiftInfoDetails;
                gvEmpList.DataBind();
                if (dtShiftInfoDetails.Rows.Count == 0)
                {
                    divRecordMessage.Visible = true;
                    divRecordMessage.InnerText = "Now  Shift is Empty!";
                    lblSelectedRow.Text = ""; lblTotalRow.Text = "";
                }
                else {
                    divRecordMessage.Visible = false;
                    if (!ViewState["__ReadAction__"].ToString().Equals("0"))
                    gvEmpList.Visible = true; 
                    lblTotalRow.Text = "Total Employee = " + dtShiftInfoDetails.Rows.Count.ToString();
                    DataTable dtSelected = dtShiftInfoDetails.Select("status='true'", null).CopyToDataTable();
                    lblSelectedRow.Text = "Selected Employee = " + dtSelected.Rows.Count.ToString();
                
                }

              //  ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            }
            catch
            { }
        }
        private void shiftTransfer()
        {
            try
            {
                CheckBox chk = new CheckBox();
                DataTable dtSTId = new DataTable();
                int STId = 0;
                string fDate = commonTask.ddMMyyyyTo_yyyyMMdd(txtFromDate.Text.Trim());
                string tDate = commonTask.ddMMyyyyTo_yyyyMMdd(txtToDate.Text.Trim());
                string SftID = ddlNewShift.SelectedValue;
                string DptID = ddlDepartmentList.SelectedValue;
                string CompanyID = ddlCompanyList.SelectedValue;
                string GID = ddlGroupList.SelectedValue;
                if (!CheckAllreadyAssigned(fDate,tDate)) return;                
                STId =Roster.saveShiftTransfer(fDate, tDate, SftID, DptID, CompanyID, GID);
                    if (STId == 0) return;                
                int v;
                for (int i = 0; i < gvEmpList.Rows.Count; i++)
                {

                    v = i + 1;
                    ProgressBar1.Value = (100 * v / gvEmpList.Rows.Count);
                    System.Threading.Thread.Sleep(50);
                    chk = (CheckBox)gvEmpList.Rows[i].Cells[2].FindControl("chkChosen");
                    DropDownList ddl = (DropDownList)gvEmpList.Rows[i].Cells[2].FindControl("ddlFloorList");
                    TextBox txtNotes = (TextBox)gvEmpList.Rows[i].Cells[3].FindControl("txtRemaks");
                    if (chk.Checked)Roster.saveShiftTransferDetails(fDate,tDate,STId.ToString(), gvEmpList.DataKeys[i].Values[0].ToString(),DptID,CompanyID,GID, ddl.SelectedValue, txtNotes.Text);

                }
                aRosterMissingLog.HRef = "/roster-missing-log.aspx?STID=" + STId.ToString();
                divRecordMessage.Visible = true;
                divRecordMessage.InnerText = "Successfully shift assigned in " + ddlNewShift.SelectedItem.Text + "";
                gvEmpList.Visible = false;
                AllClear();

            }
            catch { }

        }
        private void shiftTransfer_old() 
        {
            try
            {
                CheckBox chk = new CheckBox();                
                DataTable dtSTId = new DataTable();
                int STId = 0;
                string fDate = commonTask.ddMMyyyyTo_yyyyMMdd(txtFromDate.Text.Trim());
                string tDate = commonTask.ddMMyyyyTo_yyyyMMdd(txtToDate.Text.Trim());
                if (!chkUpdate.Checked)   // for new shift entry 
                {
                    if (!CheckAllreadyAssigned(fDate, tDate)) return;
                    STId=saveShiftTransfer();
                    if (STId==0) return;
                   
                }
                else  // for update current shift
                {
                   
                    sqlDB.fillDataTable("select Max(STId) as STId from ShiftTransferInfo where CompanyId='"+ddlCompanyList.SelectedValue+"' AND DptId='"+ddlDepartmentList.SelectedValue+"' AND SftId="+ddlNewShift.SelectedValue+" AND TFromDate='"+commonTask.ddMMyyyyTo_yyyyMMdd(txtFromDate.Text.Trim())+"' AND TToDate='"+ commonTask.ddMMyyyyTo_yyyyMMdd(txtToDate.Text.Trim())+"'", dtSTId);                    
                    if (dtSTId.Rows[0]["STId"].ToString() == "")
                    {
                        lblErrorMessage.Text = "error->Please entered a new shift then modify."; return;
                    }
                    STId = int.Parse(dtSTId.Rows[0]["STId"].ToString());
                }                
                int v;
                for (int i = 0; i < gvEmpList.Rows.Count; i++)
                {
                  
                    v=i+1;
                    ProgressBar1.Value = (100*v/gvEmpList.Rows.Count);
                    System.Threading.Thread.Sleep(50);
                    chk = (CheckBox)gvEmpList.Rows[i].Cells[2].FindControl("chkChosen");
                    DropDownList ddl = (DropDownList)gvEmpList.Rows[i].Cells[2].FindControl("ddlFloorList");
                    TextBox txtNotes=(TextBox)gvEmpList.Rows[i].Cells[3].FindControl("txtRemaks");

                      
                    if (chk.Checked) saveShiftTransferDetails(STId.ToString(), gvEmpList.DataKeys[i].Values[0].ToString(),ddl.SelectedValue,txtNotes.Text);
                           
                }
                aRosterMissingLog.HRef= "/roster-missing-log.aspx?STID="+STId.ToString();
                divRecordMessage.Visible = true;
                divRecordMessage.InnerText = "Successfully shift assigned in " + ddlNewShift.SelectedItem.Text + "";
                gvEmpList.Visible = false;                 
                AllClear();
                
            }
            catch { }
          
        }             

        private bool CheckAllreadyAssigned(string fDate,string tDate)
        {
            try
            {
                
                DataTable dtSTInfo=new DataTable ();                

                string GID = (ddlGroupList.SelectedValue == "0") ? "" : " AND GId='" + ddlGroupList.SelectedValue + "'";
                sqlDB.fillDataTable("select Max(Format(SDate,'dd-MM-yyyy')) as SDate from v_ShiftTransferInfoDetails where DptId='" + ddlDepartmentList.SelectedValue + "' AND SftId =" + ddlNewShift.SelectedValue + " AND SDate >='" + fDate + "' AND SDate <='" + tDate + "'  "+ GID, dtSTInfo);
                if (dtSTInfo.Rows.Count > 0 && dtSTInfo.Rows[0]["SDate"].ToString().Trim()!="")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);﻿
                    lblErrorMessage.Text = "Already this shift are assigned for " + dtSTInfo.Rows[0]["SDate"].ToString()+System.Environment.NewLine+".Now you can modify,for modify must be checkd shift modify option.";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        void AllClear()
        {
            try
            {
              //  ddlDepartmentList.SelectedIndex = 0;               
                ddlNewShift.SelectedIndex = 0;
                lblTotalRow.Text = "";
                lblSelectedRow.Text = "";
                ProgressBar1.Value = 0;
                Response.Redirect("../personnel/ShiftManagement.aspx");
            }
            catch { }
        }
        private int saveShiftTransfer()
        {
            try
            {
                string  fDate =commonTask.ddMMyyyyTo_yyyyMMdd(txtFromDate.Text.Trim());
                string  tDate = commonTask.ddMMyyyyTo_yyyyMMdd(txtToDate.Text.Trim());

                SqlCmd = @"
INSERT INTO [dbo].[ShiftTransferInfo]
           ([SftId]
           ,[TFromDate]
           ,[TToDate]          
           ,[EntryDate]
           ,[DptId]
           ,[CompanyId]
           ,[GId])
     VALUES
           ("+ ddlNewShift.SelectedValue + ",'"+ fDate + "','"+tDate+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+ddlDepartmentList.SelectedValue+"','"+ddlCompanyList.SelectedValue+"','"+ddlGroupList.SelectedValue+"');"+
                      "SELECT SCOPE_IDENTITY()";
            return CRUD.ExecuteReturnID(SqlCmd, sqlDB.connection);
                //string[] getColumns = { "SftId", "TFromDate", "TToDate","DptId","CompanyId","GID"};
                //string[] getValues = {ddlNewShift.SelectedValue,fDate,
                //                     tDate,ddlDepartmentList.SelectedItem.Value.ToString(),ddlCompanyList.SelectedValue,ddlGroupList.SelectedItem.Value.ToString()};
                //if (SQLOperation.forSaveValue("ShiftTransferInfo", getColumns, getValues, sqlDB.connection) == true) return true;
                //else return false;
                
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void saveShiftTransferDetails(string STId,string EmpId,string FId,string Notes)
        {
            try
            {

               
                DateTime FromDate=DateTime.Parse(commonTask.ddMMyyyyTo_yyyyMMdd(txtFromDate.Text.Trim()));
                DateTime ToDate = DateTime.Parse(commonTask.ddMMyyyyTo_yyyyMMdd(txtToDate.Text.Trim()));

                string [] GetDsgID_EmpTypeId_GId=DsgId_EmpTypeId(EmpId).Split('|');
                
                while(FromDate <= ToDate)
                {
                    try
                    {
                        //--> delete the existing data----
                        CRUD.Execute("delete from ShiftTransferInfoDetails where EmpId='" + EmpId + "' AND SDate='" + FromDate.ToString("yyyy-MM-dd") + "' AND STId=" + STId + "", sqlDB.connection);
                        //--< delete the existing data-----
                        string[] getColumns = { "STId", "SDate", "EmpId", "DptId", "DsgId", "EmpTypeId", "CompanyId", "IsWeekend", "GId", "FId", "Notes" };
                        string[] getValues = { STId,FromDate.ToString("yyyy-MM-dd"),EmpId,
                                         ddlDepartmentList.SelectedValue,GetDsgID_EmpTypeId_GId[0],GetDsgID_EmpTypeId_GId[1],ddlCompanyList.SelectedValue,CheckIsOffDay(EmpId,FromDate.ToString("yyyy-MM-dd")).ToString(),
                                         GetDsgID_EmpTypeId_GId[2],FId,Notes};
                        SQLOperation.forSaveValue("ShiftTransferInfoDetails", getColumns, getValues, sqlDB.connection);
                    }
                    catch (Exception ex) { InsertToMissingLog(STId,EmpId,FromDate,ex.Message); }                                    
                    FromDate=FromDate.AddDays(1);
                   
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }
        }
        
        private void InsertToMissingLog(string STID, string EmpID, DateTime Date,string Error)
        {
            try {
                SqlCmd = @"INSERT INTO [dbo].[ShiftTransferDetailsMissingLog]
           ([EmpID]           
           ,[Date]
           ,[InsertTime]
           ,[Error]
           ,[STID])
     VALUES
           ('"+ EmpID + "','"+Date.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+Error.Replace("'","")+"',"+ STID + ")";
                CRUD.Execute(SqlCmd,sqlDB.connection);

            } catch(Exception ex ) { }
        }
        private string DsgId_EmpTypeId(string EmpId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select DsgId,EmpTypeId,GID from Personnel_EmpCurrentStatus where SN = (Select Max (SN) from Personnel_EmpCurrentStatus where EmpId='"+EmpId+"' AND EmpStatus in ('1','8'))",dt);
                return dt.Rows[0]["DsgId"].ToString()+'|'+dt.Rows[0]["EmpTypeId"].ToString()+'|'+dt.Rows[0]["GId"].ToString();
            }
            catch{return null;}
        }

        private byte CheckIsOffDay(string EmpId, string HWDate)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select SL From tblHolydayWeekentEmployeeWiseDetails where EmpId='" + EmpId + "' AND HWDate='" + HWDate + "'", dt);
                if (dt.Rows.Count > 0) return 1;
                else return 0;
            }
            catch { return 0; }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (!InputValidationBasket()) return;      // for required validation
            if (!CheckAnyEmployeeIsSelected()) return; // for checking any employee is selected ?
            shiftTransfer();
            chkUpdate.Checked = false;
        }

        private bool CheckAnyEmployeeIsSelected()
        {
            try
            {
                CheckBox chkb = gvEmpList.HeaderRow.FindControl("chkHeaderChosen") as CheckBox;
                if (chkb.Checked) return true;
                else
                {
                    foreach(GridViewRow gr in gvEmpList.Rows)
                    {
                        if ((gr.FindControl("chkChosen") as CheckBox).Checked) return true;
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                lblErrorMessage.Text = "Any employee is not selected ! ";
                return false;
            }
            catch { return false; }
        }


        protected void gvEmpList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
               // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtFloorlist = new DataTable();
                    DropDownList ddlFloorList = (e.Row.FindControl("ddlFloorList") as DropDownList);
                    sqlDB.fillDataTable("select FId,FName from HRD_Floor where IsActive='True' order by FId", dtFloorlist );
                    ddlFloorList.DataValueField = "FId";
                    ddlFloorList.DataTextField = "FName";
                    ddlFloorList.DataSource = dtFloorlist;
                    ddlFloorList.DataBind();

                    string getFID = (dtShiftInfoDetails.Rows[e.Row.RowIndex]["FId"].ToString() == "") ? "0" : dtShiftInfoDetails.Rows[e.Row.RowIndex]["FId"].ToString();
                    ddlFloorList.SelectedValue = getFID;
                    ddlFloorList.Items.Insert(0, new ListItem("", "0"));

                    if(DateTime.Now.ToString("yyyy-MM-dd").Equals(gvEmpList.DataKeys[e.Row.RowIndex].Values[1].ToString()))

                    e.Row.BackColor = Color.LightSkyBlue;
                    else e.Row.BackColor = Color.WhiteSmoke;
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
                 
               
            }
            catch { }

            try {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    bool Assigned = bool.Parse(gvEmpList.DataKeys[e.Row.RowIndex].Values[2].ToString());

                    foreach (TableCell cell in e.Row.Cells)
                    {
                        if (Assigned)
                        {
                            cell.BackColor = ColorTranslator.FromHtml("#BFB1D0");
                        }
                      
                    }
                }
            }
            catch { }
        }

        protected void rblChoseType_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
          //  LoadAllEmployeeList();
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvEmpList.DataSource = null;
                gvEmpList.DataBind();
                classes.commonTask.LoadShiftForSMOperation(ddlNewShift, ddlCompanyList.SelectedValue,ddlDepartmentList.SelectedValue);

                commonTask.loadGroupByDepartment(ddlGroupList, ddlDepartmentList.SelectedValue);

                ddlGroupList_SelectedIndexChanged(sender,e);



            }
            catch { }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
               // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
               classes.commonTask.LoadGrouping(ddlGroupList, ddlCompanyList.SelectedValue);
               classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);
               classes.commonTask.LoadShiftForSMOperation(ddlNewShift, ddlCompanyList.SelectedValue);
            }
            catch { }
        }

        protected void chkChosen_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);       
                CheckBox chk;
                int rowAmount = 0;
                foreach(GridViewRow gvr in gvEmpList.Rows)
                {
                    chk = (CheckBox)gvr.Cells[2].FindControl("chkChosen");
                    if (chk.Checked) rowAmount++;
                }
                lblSelectedRow.Text = "Selected Employee = "+rowAmount.ToString();

              
            }
            catch { }
        }

        protected void chkHeaderChosen_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
               // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                CheckBox chk;
                CheckBox chkHeader =(CheckBox)gvEmpList.HeaderRow.FindControl("chkHeaderChosen");
                if (chkHeader.Checked)
                {
                    foreach(GridViewRow gvr in gvEmpList.Rows)
                    {
                        chk = (CheckBox)gvr.Cells[2].FindControl("chkChosen");
                        chk.Checked = true;
                    }
                    lblSelectedRow.Text = "Selected Employee = " +gvEmpList.Rows.Count;
                }
                else
                {
                    foreach (GridViewRow gvr in gvEmpList.Rows)
                    {
                        chk = (CheckBox)gvr.Cells[2].FindControl("chkChosen");
                        chk.Checked = false;
                    }
                    lblSelectedRow.Text = "Selected Employee = 0";
                }
            }
            catch { }
        }
        private bool InputValidationBasket()
        {
            try
            {
                if (txtFromDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "change me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select valid from date";
                    ddlNewShift.SelectedIndex = 0;
                    txtFromDate.Focus(); return false;
                }
                if (txtToDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select valid to date";
                    ddlNewShift.SelectedIndex = 0;
                    txtToDate.Focus();                  
                    return false;
                }
                if (ddlNewShift.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select New Shift!";                  
                    ddlNewShift.Focus(); return false;
                }
                return true;
            }
            catch { return false; }
        }
        protected void ddlNewShift_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (!InputValidationBasket()) return;
            LoadAllEmployeeList(false);
            System.Threading.Thread.Sleep(1000);
        }

        protected void chkUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdate.Checked == false && ViewState["__WriteAction__"].ToString().Equals("0"))
            {
                btnSubmit.CssClass = "";
                btnSubmit.Enabled = false;
            }
            else 
            {
                btnSubmit.CssClass = "css_btn";
                btnSubmit.Enabled = true;
            }
        }

        protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAllEmployeeList(true);
                System.Threading.Thread.Sleep(1000);
            }

            catch { }
        }
    }
}