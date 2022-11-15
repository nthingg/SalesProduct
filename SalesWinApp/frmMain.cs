
using DataAccesses.DataAccess;
using DataAccesses.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmMain : Form
    {       

        public frmMain()
        {
            InitializeComponent();
        }
        private frmMembers memberManagement;
        private frmProducts productManagement;
        private frmOrders orderManagement;
        private Member member;
        private IMemberRepository memberRepository;
        public IMemberRepository MemberRepository { set => memberRepository = value; }
        public Member Member { set => member = value; }
        private void memberManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllChildForm();
            memberManagement = new frmMembers
            {
                Member = member,
                MemberRepository = memberRepository
            };
            memberManagement.MdiParent = this;
            memberManagement.Show();
        }

        private void productManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllChildForm();
            productManagement = new frmProducts();
            productManagement.MdiParent = this;
            productManagement.Show();
        }

        private void orderManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeAllChildForm();
            orderManagement = new frmOrders
            {
                Member = member,
            };
            orderManagement.MdiParent = this;
            orderManagement.Show();

        }
        private void closeAllChildForm()
        {
            if (memberManagement != null) memberManagement.Close();
            if (productManagement != null) productManagement.Close();
            if (orderManagement != null) orderManagement.Close();   
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!member.IsAdmin)
            {
                productManagementToolStripMenuItem.Enabled = false;
            }
            memberManagementToolStripMenuItem_Click(null, null);
        }
    }
}
