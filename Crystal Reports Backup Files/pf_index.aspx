<%@ Page Title="Provident Fund" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="pf_index.aspx.cs" Inherits="SigmaERP.pf.pf_index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:none;">
                  
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="/payroll_default.aspx">Payroll</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Pactive">Provident Fund(PF)</a></li>
                           </ul>               
                  
                </div>
             </div>
        </div>
    
      <%--  <img alt="" class="main_/images" src="/images/hrd.png">--%>
     <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1200%;">
         <div class="col-lg-12" style="margin-top:10%">
             <div class="row">
                 <div class=" col-md-2">
                 </div>
                    <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_settings.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br /> PF Setting</a>
                     
                 </div>
                 <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pfentrypanel.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br /> PF Entry Panel </a>                     
                 </div>        
                 <div class=" col-md-2">
                      <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_ManualEntry.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />Manualy Entry</a>
                 </div>
                  <div class=" col-md-2">
                      <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_FDR.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />Investment</a>
                 </div>

             </div>             
                <div class="row">
                 <div class=" col-md-2">
                 </div>
                    
                      <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_interestentry.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />Investment Withdraw</a>
                     
                 </div>
                       <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_YearlyExpense.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br /> PF Expense</a>                     
                 </div> 
                     <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_interest_distribution.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />Profit Distribution</a>
                     
                 </div>                 
                    <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_report.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br /> PF Reprots</a>                     
                 </div>                 
             </div>   
              <div class="row">
                 <div class=" col-md-2">
                 </div>
                    
                      <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_withdraw.aspx"> <img class="image_width_for_module" src="/images/common/risefall.ico" /><br />PF Withdraw</a>
                     
                 </div>
                               
             </div>        
             </div>
</div>
</asp:Content>
