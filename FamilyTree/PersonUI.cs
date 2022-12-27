namespace FamilyTree
{
    public partial class PersonUI : Panel
    {
        private Label nameSurnameLbl;
        private Label dateOfBirthLbl;
        private Label bloodGroupLbl;
        private Label jobLbl;

        public PersonUI(string name, string surName, int[] dateOfBirth, string bloodGroup, string job, string isMale)
        {
            InitializeComponent();

            if (bloodGroup == "")
            {
                bloodGroup = "?";
                job = "?";
            }

            if (isMale == "Erkek")
            {
                if (bloodGroup != "?")
                    BackColor = Color.FromArgb(255, 0, 230, 255);
                else
                    BackColor = Color.FromArgb(255, 0, 180, 200);
            }
            else
            {
                if (bloodGroup != "?")
                    BackColor = Color.FromArgb(255, 255, 150, 205);
                else
                    BackColor = Color.FromArgb(255, 200, 100, 150);
            }

            Size = new Size(150, 75);

            nameSurnameLbl = new Label();
            Controls.Add(nameSurnameLbl);
            nameSurnameLbl.AutoSize = false;
            nameSurnameLbl.TextAlign = ContentAlignment.MiddleCenter;
            nameSurnameLbl.Width = 150;
            nameSurnameLbl.Height = 20;
            nameSurnameLbl.Font = new Font("Arial", 12, FontStyle.Bold);
            nameSurnameLbl.Text = name + " " + surName;
            nameSurnameLbl.Location = new Point(Width / 2 - nameSurnameLbl.Width / 2, 10);

            dateOfBirthLbl = new Label();
            Controls.Add(dateOfBirthLbl);
            dateOfBirthLbl.AutoSize = false;
            dateOfBirthLbl.TextAlign = ContentAlignment.MiddleCenter;
            dateOfBirthLbl.Width = 75;
            dateOfBirthLbl.Height = 15;
            dateOfBirthLbl.Font = new Font("Arial", 9, FontStyle.Italic);
            if (dateOfBirth[0] > 0)
                dateOfBirthLbl.Text = String.Format("{0:D2}/{1:D2}/{2:D4}", dateOfBirth[0], dateOfBirth[1], dateOfBirth[2]);
            else
                dateOfBirthLbl.Text = "-";
            dateOfBirthLbl.Location = new Point(Width / 2 - dateOfBirthLbl.Width / 2, 30);

            bloodGroupLbl = new Label();
            Controls.Add(bloodGroupLbl);
            bloodGroupLbl.AutoSize = false;
            bloodGroupLbl.TextAlign = ContentAlignment.MiddleRight;
            bloodGroupLbl.Width = 50;
            bloodGroupLbl.Height = 20;
            bloodGroupLbl.Font = new Font("Arial", 10, FontStyle.Italic);
            bloodGroupLbl.Text = bloodGroup;
            bloodGroupLbl.Location = new Point(Width - bloodGroupLbl.Width - 5, Height - bloodGroupLbl.Height - 5);

            jobLbl = new Label();
            Controls.Add(jobLbl);
            jobLbl.AutoSize = false;
            jobLbl.TextAlign = ContentAlignment.MiddleLeft;
            jobLbl.Width = 100;
            jobLbl.Height = 20;
            jobLbl.Font = new Font("Arial", 10, FontStyle.Italic);
            jobLbl.Text = job;
            jobLbl.Location = new Point(5, Height - bloodGroupLbl.Height - 5);
        }

        public void UpdateInfo(string name, string surName, int[] dateOfBirth, string bloodGroup, string job, bool isMale)
        {
            nameSurnameLbl.Text = name + " " + surName;
            dateOfBirthLbl.Text = String.Format("{0:D2}/{1:D2}/{2:D4}", dateOfBirth[0], dateOfBirth[1], dateOfBirth[2]);
            bloodGroupLbl.Text = bloodGroup;
            jobLbl.Text = job;

            if (isMale)
                BackColor = Color.FromArgb(255, 0, 230, 255);
            else
                BackColor = Color.FromArgb(255, 255, 150, 205);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
