<%@ Page Title="Attendance" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="attendance_default.aspx.cs" Inherits="SigmaERP.attendance_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:none;">
                     <div style="margin-top: 5px">
                           <ul>
                               <li><a href="/default.aspx">Dashboard</a></li>
                               <li> <a class="seperator" href="/hrd_default.aspx">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Mactive">Attendance</a></li>
                           </ul>               
                     </div>
                </div>
             </div>
        </div>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1000%;">
    <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-2"></div>

                 <div runat="server" id="divMonthSetup" class="col-md-2" title="Attendance Month Setup" >
                     <a class="ds_attendance_Basic_Text" href="/attendance/monthly_setup.aspx"> <img class="image_width_for_module" src="images/common/grade.ico" /><br /> Month Setup </a>
                     
                 </div>
                 <div  runat="server" id="divMonthSetupComp"  class="col-md-2" title="Attendance Month Setup" >
                     <a class="ds_attendance_Basic_Text" href="/attendance/monthly_setup1.aspx"> <img class="image_width_for_module" src="images/common/grade.ico" /><br /> Month Setup </a>
                     
                 </div>
                      <div runat="server" id="divAttProcessing" class=" col-md-2" title="Machine Data Import">
                      <a class="ds_attendance_Basic_Text" href="/attendance/import_data_ahg.aspx"><img class="image_width_for_module" src="images/common/generate.ico" /><br /> Att. Processing</a>
                 </div> 
                 <%--<div class="col-md-2" title="Daily Logout Setup" >
                     <a class="ds_attendance_Basic_Text" href="/attendance/monthly_logout_setup.aspx"> <img class="image_width_for_module" src="images/common/grade.ico" /><br /> Logout Setup</a>
                     
                 </div>
                 <div class=" col-md-2" title="Attendance Late Deduction">
                        <a class="ds_attendance_Basic_Text" href="/attendance/late_deduction.aspx"><img class="image_width_for_module" src="images/common/qualification.ico" /><br />Late Deduction</a>
                 </div>
                   <div class=" col-md-2" title="Attendance Late Deduction">
                        <a class="ds_attendance_Basic_Text" href="/attendance/employee-wise_hw_setup.aspx"><img class="image_width_for_module" src="images/common/qualification.ico" /><br />Emp Weekend</a>
                 </div>--%>
                   <div runat="server" id="divManuallyCount" class=" col-md-2" title="Attendance Manually Count ">
                      <a class="ds_attendance_Basic_Text" href="/attendance/attendance.aspx"><img class="image_width_for_module" src="images/common/application.ico" /><br />Manually Count</a>
                 </div>
                 <div runat="server" id="divAttendanceList" class=" col-md-2" title="All Attendance List">
                      <a class="ds_attendance_Basic_Text" href="/attendance/attendance_list.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Attendance List</a>                    
                 </div>                           
                 <div class=" col-md-2"></div>
             </div>

               <div class="row">
                 <div class=" col-md-2"></div>

                 <div runat="server" id="divAttSummary"  class=" col-md-2" title="Daily Attendance Summary">
                     <a class="ds_attendance_Basic_Text" href="/attendance/attendance_summary.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Att Summary </a>   
                 </div>
                  <div runat="server" id="divInOutReport" class=" col-md-2 " title="Daily In-Out Report">
                     <a class="ds_attendance_Basic_Text" href="/attendance/daily_movement.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />In-Out Report</a>
                 </div>  
                 <div  runat="server" id="divManualReport" class=" col-md-2 " title="Todays Attendance Stutus">
                    <a class="ds_attendance_Basic_Text" href="/attendance/daily_manualAttendance_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Manual Report</a> 
                 </div>
                     
                    <%--<div class=" col-md-2 " title="Daily Early Late Out Report">
                    <a class="ds_attendance_Basic_Text" href="/attendance/early_out_late_out.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Early In-Out</a> 
                 </div>--%>

                 <div runat="server" id="divMonthlyStatus"  class=" col-md-2" title="Monthly Attendance Status">
                     <a class="ds_attendance_Basic_Text" href="/attendance/monthly_in_out_report.aspx"><img class="image_width_for_module" src="images/common/add document.ico" /><br />Monthly Status</a> 
                   
                 </div>       
                  
                 <div class=" col-md-2"></div>
             </div>
    
               <div class="row">

                 <div class=" col-md-2"></div>                            
                  <div runat="server" id="divManpowerWiseAttendance" class=" col-md-2 " title="Manpower Wise Attendance Report">
                     <a class="ds_attendance_Basic_Text" href="/attendance/attendance_summary_manpower.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Manpower Wise Attendance</a>
                 </div> 
                 <div runat="server" id="divOvertimeReport" class=" col-md-2 " title="Overtime Report">
                     <a class="ds_attendance_Basic_Text" href="/attendance/overtime_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Overtime Report</a>
                 </div>  
                 <div runat="server" id="divOutduty" class=" col-md-2 " title="Manpower Wise Attendance Report">
                     <a class="ds_attendance_Basic_Text" href="/attendance/out_duty.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Outduty</a>
                 </div>  
                 <div runat="server" id="divOutdutyApproval" class=" col-md-2 " title="Overtime Report">
                     <a class="ds_attendance_Basic_Text" href="/attendance/out_duty_approval.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Outduty Approval</a>
                 </div> 
               
                <%-- <div class=" col-md-2 " title="Job Card">
                      <a class="ds_attendance_Basic_Text" href="/attendance/job_card_with_summary.aspx"><img class="image_width_for_module" src="images/common/application.ico" /><br />Job Card</a> 
                 </div> --%>                 
                 <div class=" col-md-2"></div>
             </div>

               <div class="row">
                 <div class=" col-md-2"></div>             
                 <div runat="server" id="divOutdutyReport" class=" col-md-2 " title="Overtime Report">
                     <a class="ds_attendance_Basic_Text" href="/attendance/outduty_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Outduty Report</a>
                 </div>      
                   <%--<div runat="server" id="divAbsentNotification" class=" col-md-2 " title="Absent Notification">
                     <a class="ds_attendance_Basic_Text" href="/attendance/absent_notification_log.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Absent Notification</a>
                 </div> --%>
                   <div runat="server" id="divManpowerStatement" class=" col-md-2 " title="Manpower Statement">
                     <a class="ds_attendance_Basic_Text" href="/attendance/ManpowerStatement.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Manpower Statement</a>
                 </div> 
                 <div class=" col-md-2"></div>
             </div>
    
       
    

</div></div>
</asp:Content>
