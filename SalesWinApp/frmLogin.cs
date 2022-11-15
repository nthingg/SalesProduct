
using DataAccesses.DataAccess;
using DataAccesses.Repository;

namespace SalesWinApp
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private IMemberRepository memberRepository = new MemberRepository();
        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String email = txtEmail.Text.Trim();
            String password = txtPassword.Text.Trim();
            Member? member = checkLogin(email, password);
            if (member != null)
            {
                frmMain mainForm = new frmMain
                {
                    Member = member,
                    MemberRepository = memberRepository                  
                };
                mainForm.FormClosed += new FormClosedEventHandler(ManagementFormClosed);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect email or password",
                    "Member Management - Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
        }
        private void ManagementFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private Member? checkLogin(String email, String password)
        {
            var member = memberRepository.GetAllMembers().Where(mem => mem.Email == email && mem.Password == password).FirstOrDefault();
            return member;
        }
    }
}