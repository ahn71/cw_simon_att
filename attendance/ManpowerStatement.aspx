﻿<%@ Page Title="Manpower Statement" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="ManpowerStatement.aspx.cs" Inherits="SigmaERP.attendance.ManpowerStatement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .division_table_leave1 {
            width: 100%;
        }
        .division_table_leave1 tr {
                height: 35px;
        }        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Manpower Statement</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="ddlCompany" />          
        </Triggers>
        <ContentTemplate>
            <div class="main_box Mbox">
         <div class="main_box_header MBoxheader">                    
                    <h2>Manpower Statement</h2>
                </div>               
                <div class="employee_box_body">
                    <div class="employee_box_content">

                <div class="bonus_generation" style="width:61%; margin:0px auto;">
<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>             
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">
                        <tr id="trCompanyName" runat="server" visible="true">
                            <td>Company
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>Shift 
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShift" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                </asp:DropDownList>
                            </td>                          
                        </tr>
                        <tr>
                            <td>Employee Type
                            </td>
                            <td>:</td>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal">
                                </asp:RadioButtonList>
                            </td>
                            <td>Date
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" AutoComplete="off" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender
                                    ID="TextBoxDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtDate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                      <div id="workerlist" runat="server" class="id_card" style="background-color:white; width:61%;">
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" style="height:270px !important" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC" >
                                <table style="margin-top:60px;" class="employee_table">                    
                              <tr>
                                    <td >
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                    </td>
                               </tr>
                        </table>
                    </div>
                     <div class="id_card_right EilistR">
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  style="height:270px !important"  ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                </div>
                        <div class="job_card_button_area">                            
                            <asp:Button ID="btnPreview" CssClass="Mbutton" runat="server" ValidationGroup="save" OnClientClick="return InputValidation();" Text="Preview" OnClick="btnPreview_Click" />
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="Mbutton" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">  
         $(document).ready(function () {
             $("#ddlShiftList").select2();
         });
         function load() {
             $("#ddlShiftList").select2();
         }
         function goToNewTabandWindow(url) {
             window.open(url);
             load();
         }

        
         function InputBoxNew()
         {
           
             $('#txtCardNo').val('');
         }
         function InputValidation()
         {
             if (validateText('txtDate', 1, 100, "Please Select Attendance Date ") == false) return false;
             return true;             
         }
    </script>
</asp:Content>