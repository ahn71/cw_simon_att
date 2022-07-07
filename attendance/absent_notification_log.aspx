<%@ Page Title="" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="absent_notification_log.aspx.cs" Inherits="SigmaERP.attendance.absent_notification_log" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script src="../scripts/jquery-1.8.2.js"></script>
        <script type="text/javascript">

            var oldgridcolor;
            function SetMouseOver(element) {
                oldgridcolor = element.style.backgroundColor;
                element.style.backgroundColor = '#ffeb95';
                element.style.cursor = 'pointer';
                // element.style.textDecoration = 'underline';
            }
            function SetMouseOut(element) {
                element.style.backgroundColor = oldgridcolor;
                // element.style.textDecoration = 'none';

            }

     </script>
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Absent Notification List</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <div style="padding:0;margin-top:25px;max-width:100%;">
    <div class="row Rrow">
                <div id="divElementContainer" runat="server" class="list_main_content_box_header MBoxheader" style="width: 100%;">
                     
              
                </div>
     
    <asp:UpdatePanel runat="server" ID="up2">
                    <Triggers>                       
                    </Triggers>
                    <ContentTemplate>
                <div style="width: 100%; margin:0px auto ">
                    
                        <table>
                            <tr>
                                <td></td>
                                <td><asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblEmpType_SelectedIndexChanged">
                                </asp:RadioButtonList></td>
                                <td>From Date</td>
                                <td><asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender
                                    ID="TextBoxDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                </asp:CalendarExtender></td>
                                <td>To Date</td>
                                <td><asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender
                                    ID="CalendarExtender1" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtToDate">
                                </asp:CalendarExtender></td>
                                <td>
                                    <asp:Button ID="btnSearch" CssClass="Mbutton" runat="server" Text="Search" OnClick="btnSearch_Click"   />
                                </td>
                            </tr>
                        </table>                      
                         <asp:GridView HeaderStyle-BackColor="#2B5E4E" HeaderStyle-Height="28px" ID="gvAbsentList" runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px"  Width="100%" >
                          <Columns>                                                     
                             <asp:BoundField DataField="EmpCardNo" HeaderText="Card No"/>                              
                             <asp:BoundField DataField="EmpName" HeaderText="Name"/>
                             <asp:BoundField DataField="DptName" HeaderText="Department" />
                             <asp:BoundField DataField="DsgName" HeaderText="DsgName"/>                                                 
                             <asp:BoundField DataField="Date" HeaderText="Date"/>                                                 
                          </Columns>                         
                     </asp:GridView>                                      
                </div>      
    </ContentTemplate>
</asp:UpdatePanel>
             </div>
            </div>
</asp:Content>
