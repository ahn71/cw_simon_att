﻿<%@ Page Title="Bonus Generation" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="bonus_generation.aspx.cs" Inherits="SigmaERP.payroll.bonus_generation" %>
<%@ Register assembly="ComplexScriptingWebControl" namespace="ComplexScriptingWebControl" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".meter > span").each(function () {
                $(this)
                    .data("origWidth", $(this).width())
                    .width(0)
                    .animate({
                        width: $(this).data("origWidth")
                    }, 1200);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ></asp:ScriptManager>   
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll/bonus_index.aspx">Bouns</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Bonus Generation</a></li>
                </ul>
            </div>
        </div>
    </div>
     <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

  
    <div class="main_box Mbox">
    	<div class="main_box_header PBoxheader">
            <h2>Bonus Generation</h2>
        </div>
   <div class="main_box_body Pbody">
        	<div class="main_box_content">
                  <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                          <asp:AsyncPostBackTrigger ControlID="btnGeneration" />
                      </Triggers>
                    <ContentTemplate>
                 <div class="input_division_info">
                    <table class="division_table">
                        <tr>
                             <td>
                                 Select Company<span class="requerd1">*</span>
                             </td>
                             <td>
                                 <asp:DropDownList ID="ddlCompanyList" runat="server" CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ClientIDMode="Static"> 
                                </asp:DropDownList>
                             </td>
                         </tr>
                            <tr>
                                <td>
                                    Select Month<span class="requerd1">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectBonusMonth" runat="server" CssClass="form-control select_width" ClientIDMode="Static"> 
                                </asp:DropDownList>
                                    
                                </td>
                            </tr>
                        <tr>
                                <td>
                                    
                                <td>
                                    <asp:CheckBox runat="server" ClientIDMode="Static" ID="ckbIsBonusPer" Text="Is Bonus % ?" />
                                    <asp:TextBox CssClass="form-control text_box_width" ID="txtPerOfBonus" runat="server">50</asp:TextBox>
                                    
                                </td>
                            </tr>
                         <tr>
                             <td>
                                 <asp:TextBox CssClass="form-control text_box_width" ID="txtGenerateMonth" Visible="false" runat="server"></asp:TextBox>
                             </td>
                         </tr>
                         <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            <asp:Image ID="imgLoading"  runat="server" ImageUrl="~/images/loading.gif" ClientIDMode="Static"  />
                                        </td>
                              <td>
                                           <asp:Label runat="server" ID="lblGenerationStatus">Generating..</asp:Label>
                                        </td>
                                    </tr>
                     </table>
                </div>            


                        <div class="payroll_generation_button">
                   
                    <asp:Button ID="btnGeneration" OnClick="btnGeneration_Click" OnClientClick="return imgShow();" runat="server" CssClass="Pbutton" Text="Generation" />
                            <%--<button  id="operation" class="Pbutton" >Generation</button>--%>
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="Pbutton" />
                    
                </div>
               
                      
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </div>
        </div>


    </div>

    <script type="text/javascript">   
        function imgShow()
        {
            $('#imgLoading').show();
            return;
        }
        function processing() {            
            $('#imgLoading').show();
            $.ajax({
                url: "/WebForm1.aspx",
                type: "get",
                data: {} ,
                success: function (response) {
                    var res = JSON.parse(response);
                    if (response.complete == 1) {
                        alert('alert');
                        $('#imgLoading').hide();
                        //process complete
                    } else {
                        $('#imgLoading').hide();
                        //process failed
                    }           
                },
                error: function(jqXHR, textStatus, errorThrown) {
                   console.log(textStatus, errorThrown);
                }


            });
        }
        $(document).ready(function () {
           
            $('#imgLoading').hide();
             //Progressbar initialization
             $("#progressbar").progressbar({ value: 0 });
             //Button click event
             $("#btnGenerate").click(function (e) {
               
                 e.preventDefault();
                
                 //Disabling button
                 //$("#operation").attr('disabled', 'disabled');
                 //Making sure that progress indicate 0
                 $("#progressbar").progressbar('value', 0);

                 
                 var companyId = $('#ddlCompanyList').val();
                 var smonth = $('#ddlSelectBonusMonth').val();
                 if (!InputValidationBasket()) return;
                 // return;
                 //Call PageMethod which triggers long running operation
                 PageMethods.Operation(smonth, companyId,function (result) {
                     if (result) {
                         //Updating progress
                         $("#progressbar").progressbar('value', result.progress)
                         //Setting the timer
                         setTimeout($.updateProgressbar, 500);
                     }
                 });
             });
         });


         function InputValidationBasket() {
             try {
                 if ($('#ddlSelectBonusMonth').val().trim().length <= 1) {
                     showMessage('Please select bonus generate month', 'error');
                     $('#ddlSelectBonusMonth').focus(); return false;
                 }
                 return true;
             }
             catch (exception) {
                 showMessage(exception, error)
             }
         }


    </script>


        
         

</asp:Content>
