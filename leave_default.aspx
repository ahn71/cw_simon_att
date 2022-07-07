<%@ Page Title="Leave" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="leave_default.aspx.cs" Inherits="SigmaERP.leave_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar" style="border-bottom: none;">
                <div style="margin-top: 5px">
                    <ul>
                        <li><a href="/default.aspx">Dashboard</a></li>
                        <li><a class="seperator" href="#">/</a></li>
                        <li><a href="#" class="ds_negevation_inactive Lactive">Leave</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1000%;">
        <div class="col-lg-12" style="margin-top: 10%">
            <div class="row">
                <div class=" col-md-2"></div>
                <div runat="server" id="divConfiguration" class="col-md-2" title="Leave Configuration">
                    <a class="ds_leave_Basic_Text" href="/leave/LeaveConfig.aspx">
                        <img class="image_width_for_module" src="images/common/grade.ico" /><br />
                        Configuration </a>
                </div>
                <div runat="server" id="divHoliday" class=" col-md-2" title="All Holiday Setup">
                    <a class="ds_leave_Basic_Text" href="/leave/holyday.aspx">
                        <img class="image_width_for_module" src="images/common/qualification.ico" /><br />
                        Holiday Setup </a>
                </div>
                <div runat="server" id="divHolidayComp" class=" col-md-2" title="All Holiday Setup">
                    <a class="ds_leave_Basic_Text" href="/leave/holyday1.aspx">
                        <img class="image_width_for_module" src="images/common/qualification.ico" /><br />
                        Holiday Setup </a>
                </div>
                <div runat="server" id="divApplication" class=" col-md-2" title="Leave Application">
                    <a class="ds_leave_Basic_Text" href="/leave/aplication.aspx">
                        <img class="image_width_for_module" src="images/common/application.ico" /><br />
                        Application</a>
                </div>
                <div runat="server" id="divLeaveApproval" class=" col-md-2" title="Pending Leave Approved">
                    <a class="ds_leave_Basic_Text" href="/leave/for_approve_leave_list.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Leave Approval</a>
                </div>

                <%-- <div runat="server" id="divShortLeave" class=" col-md-2" title="Earn Leave Report">
                     <a class="ds_leave_Basic_Text" href="/leave/short_leave.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Short Leave </a>                
                 </div> --%>
                <div class=" col-md-2"></div>
            </div>
            <div class="row">
                <div class=" col-md-2"></div>

                <%--  <div runat="server" id="divShortLvApproval" class=" col-md-2" title="Pending Leave Approved">
                      <a class="ds_leave_Basic_Text" href="/leave/for_approve_shortleave_list.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Short Lv Approval</a>
                 </div>--%>
                <div runat="server" id="divLeaveList" class=" col-md-2 " title="All Leave List">
                    <a class="ds_leave_Basic_Text" href="/leave/all_leave_list.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        All Leave List</a>
                </div>
                <div runat="server" id="divSummaryReport" class=" col-md-2 " title="Yearly Purpose Report">
                    <a class="ds_leave_Basic_Text" href="/leave/yearly_leaveStatus_report.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Summary Report</a>
                </div>
                <%-- <div runat="server" id="divBalanceReport" class=" col-md-2 " title="Leave Balance Report">
                    <a class="ds_leave_Basic_Text" href="/leave/leave_balance_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Balance Report</a> 
                 </div>   --%>
                <div class=" col-md-2"></div>
            </div>
            <div class="row">
                <div class=" col-md-2"></div>
                

                <%--      <div class=" col-md-2" title="Official Purpose Report">
                     <a class="ds_leave_Basic_Text" href="/leave/company_purpose_leave_report.aspx"><img class="image_width_for_module" src="images/common/add document.ico" /><br />Office Purpose</a>                    
                 </div>--%>

                <%--<div runat="server" id="divEarnLeaveGeneration"  class=" col-md-2" title="Earn Leave Generate">
                      <a class="ds_leave_Basic_Text" href="/leave/earnleave_generation.aspx"><img class="image_width_for_module" src="images/common/generate.ico" /><br />EL Generate</a>                     
                 </div>
                   <div runat="server" id="divEarnLeaveGenerationComp" class=" col-md-2" title="Earn Leave Generate">
                      <a class="ds_leave_Basic_Text" href="/leave/earnleave_generationc.aspx"><img class="image_width_for_module" src="images/common/generate.ico" /><br />EL Generate</a>                     
                 </div>--%>


                <%--<div class=" col-md-2 " title="Maternity Application">
                      <a class="ds_leave_Basic_Text" href="/leave/MaternityLeaveApplication.aspx"><img class="image_width_for_module" src="images/common/application.ico" /><br />Ma. Application</a> 
                 </div>
                   <div class=" col-md-2 " title="Maternity Voucher">
                     <a class="ds_leave_Basic_Text" href="/leave/maternity.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Ma. Voucher</a> 
                 </div>--%>
                <div class=" col-md-2"></div>
            </div>
        </div>
    </div>

</asp:Content>
