<%@ Page Title="Salary" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="salary_index.aspx.cs" Inherits="SigmaERP.payroll.salary_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar" style="border-bottom: none;">

                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Salary</a></li>
                </ul>

            </div>
        </div>
    </div>

    <%--  <img alt="" class="main_/images" src="/images/hrd.png">--%>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1400%;background-size:cover;">
        <div class="container" style="margin-top: 10%">
            <div class="row">

                <div runat="server" id="divSalaryEntry" class="col-md-3">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/payroll_entry_panel.aspx">
                        <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />
                        Salary Set Entry Panel</a>

                </div>
                <div runat="server" id="divAllowanceCalculation" class="col-md-3">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/allowance_calculation_settings.aspx">
                        <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />
                        Allowance Calculation </a>

                </div>
                <div runat="server" id="divSalaryGenerate" class="col-md-3" title="Regular Salary Generate">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/payroll_generation1.aspx">
                        <img class="image_width_for_module" src="/images/common/generate.ico" /><br />
                        Salary Generate</a>
                </div>
                <div runat="server" id="divSeperationGenerate" class="col-md-3" title="Resigned Salary Generate">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/separation_generation_rss.aspx">
                        <img class="image_width_for_module" src="/images/common/generate.ico" /><br />
                        Seperation Generate</a>
                </div>



                
                <div class=" col-md-3" title="Monthly Salary Sheet">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/salary_sheet_Report.aspx">
                        <img class="image_width_for_module" src="/images/common/advanceentry.ico" /><br />
                        Salary Sheet</a>
                </div>
                <div runat="server" id="divSalarySummary" class=" col-md-3" title="Monthly Salary Summary">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/summary_of_salary.aspx">
                        <img class="image_width_for_module" src="/images/common/businesstype.ico" /><br />
                        Salary Summary</a>

                </div>
                <div runat="server" id="divOvertimeSheet" class=" col-md-3" title="Overtime Payment Sheet">

                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/ot_payment_sheet.aspx">
                        <img class="image_width_for_module" src="/images/common/businesstype.ico" /><br />
                        Only Overtime Sheet</a>
                </div>
                <div runat="server" id="divSeperationSheet" class=" col-md-3" title="Resigned Salary Sheet">

                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/separation_pmt_sheet.aspx">
                        <img class="image_width_for_module" src="/images/common/advanceentry.ico" /><br />
                        Seperation Sheet</a>
                </div>

                <div runat="server" id="divPromotionEntry" class=" col-md-3" title="Employee Promotion">
                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/promotion.aspx">
                        <img class="image_width_for_module" src="../images/common/qualification.ico" /><br />
                        Promotion Entry Panel</a>

                </div>
                <div runat="server" id="divPromotionEntryComp" class=" col-md-3" title="Employee Promotion">
                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/promotionc.aspx">
                        <img class="image_width_for_module" src="../images/common/qualification.ico" /><br />
                        Promotion Entry Panel</a>
                </div>
                <div runat="server" id="divPromotionReport" class=" col-md-3" title="Employees Promotion Report">
                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/promotion_sheet.aspx">
                        <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                        Promotion List Report</a>

                </div>
                <div runat="server" id="divPromotionReportComp" class=" col-md-3" title="Employees Promotion Report">
                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/promotion_sheetc.aspx">
                        <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                        Promotion List Report</a>

                </div>
                <div runat="server" id="divIncrement" class=" col-md-3" title="Salary Increment">

                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/salary_increment.aspx">
                        <img class="image_width_for_module" src="../images/common/religion.ico" /><br />
                        Increment Entry Panel</a>
                </div>
                <div runat="server" id="divIncrementComp" class=" col-md-3" title="Salary Increment">

                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/salary_incrementc.aspx">
                        <img class="image_width_for_module" src="../images/common/religion.ico" /><br />
                        Increment Entry Panel</a>
                </div>
                <div runat="server" id="divAutoIncrementComp" class=" col-md-3" title="Salary Increment">

                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/AutoIncrementPanel.aspx">
                        <img class="image_width_for_module" src="../images/common/religion.ico" /><br />
                        Auto Increment Panel</a>
                </div>
                <div runat="server" id="divIncrementReport" class=" col-md-3" title="Salary Increment Report">

                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/increment_sheet.aspx">
                        <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                        Increment List Report</a>
                </div>
                <div runat="server" id="divIncrementReportComp" class=" col-md-3" title="Salary Increment Report">

                    <a class="ds_Settings_Basic_Text Pbox" href="/personnel/increment_sheetc.aspx">
                        <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                        Increment List Report</a>
                </div>

                <div runat="server" id="divPaySlip" class=" col-md-3" title="Monthly Salary Payslip">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/pay_slip.aspx">
                        <img class="image_width_for_module" src="/images/common/advanceentry.ico" /><br />
                        Pay Slip</a>
                </div>
                <div runat="server" id="divEarnLeavePaymentGeneration" class=" col-md-3" title="Earn Leave Payment Generation">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/earnleave_payment_generation.aspx">
                        <img class="image_width_for_module" src="/images/common/generate.ico" /><br />
                        Earn Leave Generation</a>
                </div>
                <div runat="server" id="divEarnLeavePaymentGenerationComp" class=" col-md-3" title="Earn Leave Payment Generation">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/earnleave_payment_generationc.aspx">
                        <img class="image_width_for_module" src="/images/common/generate.ico" /><br />
                        Earn Leave Generation</a>
                </div>
                <div runat="server" id="divEarnLeavePaymentSheet" class="col-md-3" title="Earn Leave Payment Sheet">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/earnleave_payment_sheet.aspx">
                        <img class="image_width_for_module" src="/images/common/advanceentry.ico" /><br />
                        Earn Leave Payment Sheet</a>
                </div>
                <div runat="server" id="divEarnLeavePaymentSheetComp" class="col-md-3" title="Earn Leave Payment Sheet">
                    <a class="ds_Settings_Basic_Text Pbox" href="/payroll/earnleave_payment_sheetc.aspx">
                        <img class="image_width_for_module" src="/images/common/advanceentry.ico" /><br />
                        Earn Leave Payment Sheet</a>
                </div>
                <%--<div class=" col-md-2" title="Final Bill Payment Sheet">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/final_bill_payment_sheet.aspx"><img class="image_width_for_module" src="/images/common/qualification.ico" /><br />Seperation Final Bill Sheet</a> 
                    
                 </div>
                 <div class=" col-md-2" title="Monthly Salary Flow">
                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/monthly_salary_flow.aspx"><img class="image_width_for_module" src="/images/common/salaryflow.ico" /><br />Monthly Salary Flow</a> 
                 
                 </div>
                 <div class=" col-md-2" title="Current Salary Structure">

                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/CurrentSalaryStructure.aspx"><img class="image_width_for_module" src="/images/common/businesstype.ico" /><br />Emp Salary Structure</a>
                 </div>--%>

                <div class=" col-md-2"></div>
            </div>
            <%--    <div class="row">

                 <div class=" col-md-2"></div>

                 <div class=" col-md-2" title="Punishment&Other's Pay">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/Punishment_OthersPay.aspx"><img class="image_width_for_module" src="../images/common/qualification.ico" /><br />Punishment&Other's Pay</a> 
                    
                 </div>
                
                 <div class=" col-md-2"></div>
             </div>--%>
        </div>
    </div>
    <script type="text/javascript">

        $(document).keyup(function (e) {
            if (e.keyCode == 79) {
                goToNewTabandWindow('/payroll/payroll_generation_rss.aspx');
            }
            else if (e.keyCode == 80) {
                goToNewTabandWindow('/payroll/salary_sheet_Report_2nd.aspx');
            }
        });
        function goToNewTabandWindow(url) {
            window.open(url);
        }

    </script>
</asp:Content>
