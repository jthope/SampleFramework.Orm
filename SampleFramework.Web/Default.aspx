﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SampleFramework.Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h1>Categories</h1>
		<asp:DataGrid ID="uxCategoriesGrid" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" AllowCustomPaging="True" OnPageIndexChanged="uxCategoriesGrid_PageIndexChanged" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="5">
			<AlternatingItemStyle BackColor="White" />
			<Columns>
				<asp:BoundColumn DataField="Id" HeaderText="Category Id"></asp:BoundColumn>
				<asp:BoundColumn DataField="ParentCategory" HeaderText="Parent Category"></asp:BoundColumn>
				<asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
				<asp:BoundColumn DataField="CreatedBy" HeaderText="Created By"></asp:BoundColumn>
				<asp:BoundColumn DataField="CreatedOn" HeaderText="Created On"></asp:BoundColumn>
				<asp:BoundColumn DataField="ModifiedBy" HeaderText="Modified By"></asp:BoundColumn>
				<asp:BoundColumn DataField="ModifiedOn" HeaderText="Modified On"></asp:BoundColumn>
			</Columns>
			<FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
			<HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
			<ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
			<PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />
			<SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
		</asp:DataGrid>
		<asp:Button ID="uxEditRecord" runat="server" Text="Edit Record 7 (to display audit trail functionality)" OnClick="uxEditRecord_Click" />
		<h1>Audit Trail</h1>
		<asp:DataGrid ID="uxAuditTrailGrid" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True" AllowCustomPaging="True" OnPageIndexChanged="uxAuditTrailGrid_PageIndexChanged" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="10">
			<AlternatingItemStyle BackColor="White" />
			<Columns>
				<asp:BoundColumn DataField="TableAffected" HeaderText="Table Affected"></asp:BoundColumn>
				<asp:BoundColumn DataField="RowIdValue" HeaderText="Row Id"></asp:BoundColumn>
				<asp:BoundColumn DataField="FieldChanged" HeaderText="Field Changed"></asp:BoundColumn>
				<asp:BoundColumn DataField="OldValue" HeaderText="Old Value"></asp:BoundColumn>
				<asp:BoundColumn DataField="NewValue" HeaderText="New Value"></asp:BoundColumn>
				<asp:BoundColumn DataField="ModifiedBy" HeaderText="Modified By"></asp:BoundColumn>
				<asp:BoundColumn DataField="ModifiedDate" HeaderText="Modified Date"></asp:BoundColumn>
			</Columns>
			<FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
			<HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
			<ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
			<PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />
			<SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
		</asp:DataGrid>
	</div>
	</form>
</body>
</html>
