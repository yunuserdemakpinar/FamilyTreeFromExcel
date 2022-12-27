using Microsoft.VisualBasic;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace FamilyTree
{
    public partial class Form1 : Form
    {
        List<Person> roots = new List<Person>();
        List<List<List<int>>> lines = new List<List<List<int>>>();
        List<Button> buttons = new List<Button>();
        string tablePath = Directory.GetCurrentDirectory() + @"\..\..\..\..\Family.xlsx";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] passedInArgs = Environment.GetCommandLineArgs();
            if (passedInArgs.Length > 1)
                tablePath = passedInArgs[1];

            CreateFamilyTrees();
            CreateButtonsList();
        }

        private void CreateFamilyTrees()
        {
            Panel[] panels = new Panel[4];
            panels[0] = panel1;
            panels[1] = panel2;
            panels[2] = panel3;
            panels[3] = panel4;
            ExcelApp.Application excelApp = new ExcelApp.Application();
            ExcelApp.Workbook excelBook = excelApp.Workbooks.Open(tablePath);
            for (int i = 1; i <= 4; i++)
            {
                ExcelApp._Worksheet excelSheet = excelBook.Sheets[i];
                ExcelApp.Range excelRange = excelSheet.UsedRange;
                int rowCount = excelRange.Rows.Count;
                int columnCount = excelRange.Columns.Count;

                for (int j = 2; j <= rowCount; j++)
                {
                    int id = int.Parse(excelRange.Cells[j, 1].Value2.ToString());
                    string name = excelRange.Cells[j, 2].Value2.ToString();
                    string surname = excelRange.Cells[j, 3].Value2.ToString();
                    string dateOfBirthStr = excelRange.Cells[j, 4].Value.ToString().Split(" ")[0];
                    string[] dateOfBirths = new string[3];
                    if (dateOfBirthStr.Contains("."))
                        dateOfBirths = dateOfBirthStr.Split(".");
                    else
                        dateOfBirths = dateOfBirthStr.Split("/");
                    int[] dateOfBirth = new int[3];
                    for (int k = 0; k < 3; k++)
                        dateOfBirth[k] = int.Parse(dateOfBirths[k]);
                    string nameOfMother = excelRange.Cells[j, 6].Value2.ToString();
                    string nameOfFather = excelRange.Cells[j, 7].Value2.ToString();
                    string bloodGroup = excelRange.Cells[j, 8].Value2.ToString();
                    string job = "";
                    if (excelRange.Cells[j, 9].Value2 != null)
                        job = excelRange.Cells[j, 9].Value2.ToString();
                    string nameBeforeMariage = "";
                    if (excelRange.Cells[j, 11].Value2 != null)
                        nameBeforeMariage = excelRange.Cells[j, 11].Value2.ToString();
                    string isMale = excelRange.Cells[j, 12].Value2.ToString();
                    string spouseName = "";
                    if (excelRange.Cells[j, 5].Value2 != null)
                        spouseName = excelRange.Cells[j, 5].Value2.ToString();

                    PersonUI tempPersonUI = new PersonUI(name, surname, dateOfBirth, bloodGroup, job, isMale);
                    panels[i - 1].Controls.Add(tempPersonUI);
                    PersonUI tempSpousePersonUI;

                    if (spouseName != "")
                    {
                        string isSpouseMale = "";
                        if (isMale == "Kadın")
                            isSpouseMale = "Erkek";
                        tempSpousePersonUI = new PersonUI(spouseName, "", new int[3] { -1, -1, -1 }, "?", "?", isSpouseMale);
                        panels[i - 1].Controls.Add(tempSpousePersonUI);
                    }
                    else
                        tempSpousePersonUI = null;

                    if (j == 2)
                        roots.Add(new Person(id, name, surname, dateOfBirth, nameOfMother, nameOfFather, bloodGroup, job, nameBeforeMariage, isMale, spouseName, tempPersonUI, tempSpousePersonUI));
                    else
                    {
                        if (excelRange.Cells[j, 10].Value2.ToString() == "Evli")
                        {
                            if (!roots[i - 1].AddSpouse(new Person(id, name, surname, dateOfBirth, nameOfMother, nameOfFather, bloodGroup, job, nameBeforeMariage, isMale, spouseName, tempPersonUI, tempSpousePersonUI)))
                                roots[i - 1].AddChild(new Person(id, name, surname, dateOfBirth, nameOfMother, nameOfFather, bloodGroup, job, nameBeforeMariage, isMale, spouseName, tempPersonUI, tempSpousePersonUI));
                        }
                        else
                            roots[i - 1].AddChild(new Person(id, name, surname, dateOfBirth, nameOfMother, nameOfFather, bloodGroup, job, nameBeforeMariage, isMale, spouseName, tempPersonUI, tempSpousePersonUI));
                    }
                }

                lines.Add(roots[i - 1].DrawFamilyTreeAndGetLines());
            }
            excelApp.Quit();
            
            roots[0].SyncSpouses(new List<Person>() { roots[1], roots[2], roots[3] });
            roots[1].SyncSpouses(new List<Person>() { roots[0], roots[2], roots[3] });
            roots[2].SyncSpouses(new List<Person>() { roots[0], roots[1], roots[3] });
            roots[3].SyncSpouses(new List<Person>() { roots[0], roots[1], roots[2] });
            
            tabPage1.Text = roots[0].Surname;
            tabPage2.Text = roots[1].Surname;
            tabPage3.Text = roots[2].Surname;
            tabPage4.Text = roots[3].Surname;
        }

        private void CreateButtonsList()
        {
            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            buttons.Add(button7);
            buttons.Add(button8);
            buttons.Add(button9);
        }

        private void CloseAllButtons()
        {
            foreach (Button btn in buttons)
                btn.Enabled = false;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab != tabControl1.TabPages[tabControl1.SelectedIndex])
                    tab.Enabled = false;
            }
        }

        private void OpenAllButtons()
        {
            richTextBox1.Text = "";
            foreach (Button btn in buttons)
                btn.Enabled = true;
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab != tabControl1.TabPages[tabControl1.SelectedIndex])
                    tab.Enabled = true;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (List<int> line in lines[0])
            {
                g.DrawLine(new Pen(Color.Black, 2), new Point(line[0], line[1]), new Point(line[2], line[3]));
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (List<int> line in lines[1])
            {
                g.DrawLine(new Pen(Color.Black, 2), new Point(line[0], line[1]), new Point(line[2], line[3]));
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (List<int> line in lines[2])
            {
                g.DrawLine(new Pen(Color.Black, 2), new Point(line[0], line[1]), new Point(line[2], line[3]));
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (List<int> line in lines[3])
            {
                g.DrawLine(new Pen(Color.Black, 2), new Point(line[0], line[1]), new Point(line[2], line[3]));
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!e.TabPage.Enabled)
                e.Cancel = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            List<Person> wantedPersons = new List<Person>();
            richTextBox1.Text = "Çocuğu olmayan insanlar:\n";
            await roots[tabControl1.SelectedIndex].FindChildlessPersons(wantedPersons, richTextBox1);
            endSearchBtn.Enabled = true;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            List<string> wantedPersons = new List<string>();
            richTextBox1.Text = "Üvey çocuklar:\n";
            await roots[tabControl1.SelectedIndex].FindStepPersons(wantedPersons, richTextBox1);
            if (wantedPersons.Count > 0)
                endSearchBtn.Enabled = true;
            else
            {
                MessageBox.Show("Soy ağacında üvey çocuk yok!", "Hata");
                EndSearch();
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            string wantedBloodGroup = Interaction.InputBox("Aramak istediğiniz kan grubunu giriniz!\nGeçerli girdiler:\n    A\n    B\n    AB\n    0", "Kan Grubu Girşi", "", 100, 100);
            if (wantedBloodGroup == "A" || wantedBloodGroup == "B" || wantedBloodGroup == "AB" || wantedBloodGroup == "0")
            {
                List<string> wantedPersons = new List<string>();
                richTextBox1.Text = "Kan grubu \"" + wantedBloodGroup + "\" olan insanlar:\n";
                await roots[tabControl1.SelectedIndex].FindBloodGroupInFamilyTree(wantedBloodGroup, wantedPersons, richTextBox1);
                if (wantedPersons.Count > 0)
                {
                    endSearchBtn.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Soy ağacında aradığnız kan grubundan kimse yok!", "Hata");
                    EndSearch();
                }
            }
            else
            {
                MessageBox.Show("Kan grubu girişi doğru yapılmadığından işlem iptal edilmiştir!", "Hata");
                OpenAllButtons();
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            List<string> wantedPersons = new List<string>();
            richTextBox1.Text = "Ata mesleğini devam ettiren insanlar:\n";
            await roots[tabControl1.SelectedIndex].FindSameJobs(wantedPersons, richTextBox1);
            if (wantedPersons.Count > 0)
            {
                endSearchBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Soy ağacında ata mesleğini devam ettiren kimse yok!", "Arama sonucu");
                EndSearch();
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            List<string> wantedPersons = new List<string>();
            richTextBox1.Text = "Adaş olan isimler:\n";
            await roots[tabControl1.SelectedIndex].FindSameNames(wantedPersons, richTextBox1);
            if (wantedPersons.Count > 0)
            {
                endSearchBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Soy ağacında adaş kimse yok!", "Arama sonucu");
                EndSearch();
            }
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            string name1 = Interaction.InputBox("Yakınlığını çıkarmak istediğiniz 1. kişinin ismini soyağacında göründüğü şekilde giriniz!", "İsim ve Soyisim Girişi", "", 100, 100);
            string name2 = Interaction.InputBox("Yakınlığını çıkarmak istediğiniz 2. kişinin ismini soyağacında göründüğü şekilde giriniz!", "İsim ve Soyisim Girişi", "", 100, 100);
            richTextBox1.Text = name1 + " ve " + name2 + " soyağacında aranıyor.";
            List<Person> persons1 = new List<Person>();
            List<Person> persons2 = new List<Person>();
            await roots[tabControl1.SelectedIndex].FindTwoPeopleAndClosestRoot(persons1, persons2, name1, name2);
            if (persons1.Count + persons2.Count > 1)
            {
                if (persons2.Count > 0)
                {
                    if (persons1[0].GetAge() >= persons2[0].GetAge())
                    {
                        richTextBox1.Text = name2 + " " + name1 + " in";
                        for (int i = 1; i < persons1.Count; i++)
                        {
                            if (persons1[i].IsMale)
                                richTextBox1.Text += " babasının ";
                            else
                                richTextBox1.Text += " annesinin ";
                        }
                        for (int i = persons2.Count - 1; i > 0; i--)
                        {
                            if (persons2[i].IsMale)
                                richTextBox1.Text += " oğlunun ";
                            else
                                richTextBox1.Text += " kızının ";
                        }
                        if (persons2[0].IsMale)
                            richTextBox1.Text += " oğlu.";
                        else
                            richTextBox1.Text += " kızı.";
                    }
                    else
                    {
                        richTextBox1.Text = name1 + " " + name2 + " in";
                        for (int i = 1; i < persons2.Count; i++)
                        {
                            if (persons2[i].IsMale)
                                richTextBox1.Text += " babasının ";
                            else
                                richTextBox1.Text += " annesinin ";
                        }
                        if (persons1[persons1.Count - 1].IsMale)
                            richTextBox1.Text += " babasının ";
                        else
                            richTextBox1.Text += " annesinin ";
                        for (int i = persons1.Count - 2; i > 0; i--)
                        {
                            if (persons1[i].IsMale)
                                richTextBox1.Text += " oğlunun ";
                            else
                                richTextBox1.Text += " kızının ";
                        }
                        if (persons1[0].IsMale)
                            richTextBox1.Text += " oğlu.";
                        else
                            richTextBox1.Text += " kızı.";
                    }
                }
                else
                {
                    richTextBox1.Text = name2 + " " + name1 + " in";
                    for (int i = 1; i < persons1.Count - 1; i++)
                    {
                        if (persons1[i].IsMale)
                            richTextBox1.Text += " babasının ";
                        else
                            richTextBox1.Text += " annesinin ";
                    }
                    if (persons1[persons1.Count - 1].IsMale)
                        richTextBox1.Text += " babası.";
                    else
                        richTextBox1.Text += " annesi.";
                }
                endSearchBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Soy ağacında yazdığınız isimlerde kimse yok!", "Hata");
                EndSearch();
            }
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            string wantedName = Interaction.InputBox("Soyağacını çıkarmak istediğiniz kişinin adını soyağacında göründüğü şekilde giriniz!", "İsim ve Soyisim Girişi", "", 100, 100);
            richTextBox1.Text = wantedName + " soyağacında aranıyor.";
            Person wantedPerson = await roots[tabControl1.SelectedIndex].FindName(wantedName);
            if (wantedPerson != null)
            {
                richTextBox1.Text = wantedName + " bulundu ve soyağacı çizildi!";
                Form2 form2 = new Form2(this, wantedPerson);
                form2.ShowDialog();
                EndSearch();
            }
            else
            {
                MessageBox.Show("Soy ağacında yazdığınız isimde kimse yok!", "Hata");
                EndSearch();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            int genCount = roots[tabControl1.SelectedIndex].FindGenerationCount(new List<int>());
            richTextBox1.Text = "Soy ağacının nesil sayısı: " + genCount;
            endSearchBtn.Enabled = true;
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            CloseAllButtons();
            string wantedName = Interaction.InputBox("Nesil sayısını öğrenmek istediğiniz kişinin ismini ve soyismini soyağacında yazdığı şekilde giriniz!", "İsim ve Soyisim Girişi", "", 100, 100);
            richTextBox1.Text = wantedName + " soyağacında aranıyor.";
            int genCount = await roots[tabControl1.SelectedIndex].FindNameAndGenerationCount(wantedName);
            if (genCount - 1 >= 0)
            {
                richTextBox1.Text = wantedName + " isimli kişiden sonraki nesil sayısı: " + (genCount - 1);
                endSearchBtn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Soy ağacında yazdığınız isimde kimse yok!", "Hata");
                EndSearch();
            }
        }

        private void endSearchBtn_Click(object sender, EventArgs e)
        {
            EndSearch();
        }

        public void EndSearch()
        {
            richTextBox1.Text = "";
            roots[tabControl1.SelectedIndex].DeselectAllPerson();
            OpenAllButtons();
            endSearchBtn.Enabled = false;
        }
    }
}