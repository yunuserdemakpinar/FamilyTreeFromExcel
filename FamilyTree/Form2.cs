namespace FamilyTree
{
    public partial class Form2 : Form
    {
        Person root;
        Form1 mainForm;
        List<List<int>> lines = new List<List<int>>();

        public Form2(Form1 mainForm, Person root)
        {
            this.mainForm = mainForm;
            this.root = root;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Text = root.Name + " " + root.Surname;
            lines = root.DrawFamilyTreeAndGetLines(this);
        }

        public PersonUI CreatePersonUI(string name, string surname, int[] datOfBirth, string bloodGroup, string job, bool isMale)
        {
            PersonUI personUI = new PersonUI(name, surname, datOfBirth, bloodGroup, job, isMale ? "Erkek" : "Kadın");
            panel1.Controls.Add(personUI);
            return personUI;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (List<int> line in lines)
            {
                g.DrawLine(new Pen(Color.Black, 2), new Point(line[0], line[1]), new Point(line[2], line[3]));
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.EndSearch();
        }
    }
}
