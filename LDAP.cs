using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDAP_demo
{
	public partial class LDAP : Form
	{
		public DirectorySearcher dirSearch= null;
		public string domain = "ETH";
			
		public LDAP()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			label3.Text = GetSystemDomain();
		}

		private string GetSystemDomain()
		{
			try
			{
				return Domain.GetComputerDomain().ToString().ToLower();
			}
			catch (Exception e)
			{
				e.Message.ToString();
				return string.Empty;
			}
		}

		private DirectorySearcher GetDirectorySearcher(string username, string passowrd)
		{
			if (dirSearch == null)
			{
				try
				{
					dirSearch = new DirectorySearcher(new DirectoryEntry("LDAP://" + domain, username, passowrd));
				}
				catch (DirectoryServicesCOMException e)
				{
					MessageBox.Show("Connection Creditial is Wrong!!!, please Check.", "Erro Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Message.ToString();
				}
				return dirSearch;
			}
			else
			{
				return dirSearch;
			}
		}

		private SearchResult SearchUserByUserName(DirectorySearcher ds, string username)
		{
			ds.Filter = "(&((&(objectCategory=Person)(objectClass=User)))(samaccountname=" + username + "))";
			ds.SearchScope = SearchScope.Subtree;
			ds.ServerTimeLimit = TimeSpan.FromSeconds(90);
			SearchResult userObject = ds.FindOne();
			if (userObject != null)
				return userObject;
			else
				return null;
		}

		private void GetUserInformation(string username, string passowrd)
		{
			Cursor.Current = Cursors.WaitCursor;
			SearchResult rs = null;
			rs = SearchUserByUserName(GetDirectorySearcher(username, passowrd),usernameTxt.Text);

			if (rs != null)
				showInformations(rs);	
			else
				MessageBox.Show("User not found!!!", "Search Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		public void showInformations(SearchResult rs)
		{
			label1.Text = rs.GetDirectoryEntry().Properties["samaccountname"].Value.ToString();
			label2.Text = rs.GetDirectoryEntry().Properties["mail"].Value.ToString();
			label9.Text = rs.GetDirectoryEntry().Properties["title"].Value.ToString();
			label11.Text = rs.GetDirectoryEntry().Properties["company"].Value.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (usernameTxt.Text.Trim().Length != 0 && passwordTxt.Text.Trim().Length != 0)
				GetUserInformation(usernameTxt.Text.Trim(), passwordTxt.Text.Trim());
			else
				MessageBox.Show("Please enter all required inputs.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

	
	}
}
