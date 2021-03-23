<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="tes1.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Transaksi</h3>
   
    <div id="master">
        <label>Kode Transaksi</label>
        <asp:TextBox ID="kodetransaksi" runat="server"  ></asp:TextBox>
        <br />
        <label>Tanggal Transaksi</label>
        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged"  >
        </asp:Calendar>
        <asp:TextBox ID="tanggaltransaksi" runat="server"></asp:TextBox>
        <br />
        <label>Nama Outlet</label>
        <asp:DropDownList id="namaoutlet" runat="server">
        </asp:DropDownList>
    </div>

    <br />
    <div id="detail">
        <label>Detail Grid</label>
        <div>
            <asp:TextBox ID="NamaBarang" runat="server"></asp:TextBox>
            <asp:TextBox ID="Qty" runat="server"></asp:TextBox>
            <asp:TextBox ID="Harga" runat="server"></asp:TextBox>
            <asp:TextBox ID="Total" runat="server"></asp:TextBox>
        </div>
        <asp:GridView ID="detailgrid" runat="server"  AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound"  >
        </asp:GridView>
        <asp:Button ID="addGrid" runat="server" Text="AddGrid" OnClick="addButton_Click" />
        <br />
    </div>

    <br />
    <div >
        <asp:Button ID="SubmitButton" runat="server"  Text="Submit" />
    </div>
</asp:Content>
