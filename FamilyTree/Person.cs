using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;

namespace FamilyTree
{
    public class Person
    {
        private const int searchDelayTime = 500;

        private int id;
        private string name;
        private string surname;
        private int[] dateOfBirth;
        private string nameOfMother;
        private string nameOfFather;
        private string bloodGroup;
        private string job;
        private string surnameBeforeMarriage;
        private bool isMale;
        private PersonUI personUI;
        private List<Person> childeren;
        private Person spouse;
        private bool isSelected;

        public int Id { get => id; }
        public string Name { get => name; }
        public string Surname { get => surname; }
        public int[] DateOfBirth { get => dateOfBirth; }
        public string NameOfMother { get => nameOfMother; }
        public string NameOfFather { get => nameOfFather; }
        public string BloodGroup { get => bloodGroup; }
        public string Job { get => job; }
        public string SurnameBeforeMarriage { get => surnameBeforeMarriage; }
        public bool IsMale { get => isMale; }
        public PersonUI PersonUI { get => personUI; }
        public List<Person> Childeren { get => childeren; }
        public Person Spouse { get => spouse; set => spouse = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }

        public Person(int id, string name, string surname, int[] dateOfBirth, string nameOfMother, string nameOfFather, string bloodGroup, string job, string surnameBeforeMarriage, string isMale, string nameOfSpouse, PersonUI personUI, PersonUI spousePersonUI)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.dateOfBirth = dateOfBirth;
            this.nameOfMother = nameOfMother;
            this.nameOfFather = nameOfFather;
            this.bloodGroup = bloodGroup;
            this.job = job;
            this.surnameBeforeMarriage = surnameBeforeMarriage;
            this.personUI = personUI;
            isSelected = false;

            string isSpouseMale;
            if (isMale == "Erkek")
            {
                this.isMale = true;
                isSpouseMale = "Kadın";
            }
            else
            {
                this.isMale = false;
                isSpouseMale = "Erkek";
            }
            childeren = new List<Person>();
            if (nameOfSpouse != "")
            {
                spouse = new Person(-1, nameOfSpouse, "", new int[3] { -1, -1, -1 }, "", "", "", "", "", isSpouseMale, "", spousePersonUI, null);
                spouse.Spouse = this;
            }
        }

        public void UpdateInfo(int id, string name, string surname, int[] dateOfBirth, string nameOfMother, string nameOfFather, string bloodGroup, string job, string surnameBeforeMarriage)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.dateOfBirth = dateOfBirth;
            this.nameOfMother = nameOfMother;
            this.nameOfFather = nameOfFather;
            this.bloodGroup = bloodGroup;
            this.job = job;
            this.surnameBeforeMarriage = surnameBeforeMarriage;
            personUI.UpdateInfo(name, surname, dateOfBirth, bloodGroup, job, isMale);
        }

        public bool AddSpouse(Person spouse)
        {
            if (this.spouse != null)
            {
                if (spouse.Spouse.Name.Replace(name, "*").Contains("*") && this.spouse.Name.Replace(spouse.Name, "*").Contains("*"))
                {
                    this.spouse.PersonUI.Dispose();
                    spouse.Spouse.PersonUI.Dispose();
                    this.spouse = spouse;
                    spouse.Spouse = this;
                    return true;
                }
            }
            foreach (Person person in childeren)
            {
                if (person.AddSpouse(spouse))
                    return true;
            }

            return false;
        }

        public bool AddChild(Person child)
        {
            foreach (Person person in childeren)
            {
                if (person.AddChild(child))
                    return true;
            }
            if (spouse != null)
            {
                if (name == child.NameOfFather || name == child.NameOfMother)
                {
                    childeren.Add(child);
                    spouse.Childeren.Add(child);
                    return true;
                }
                if (spouse.Name == child.NameOfFather || spouse.Name == child.NameOfMother)
                {
                    childeren.Add(child);
                    spouse.childeren.Add(child);
                    return true;
                }
            }

            return false;
        }

        public int GetAge()
        {
            DateTime today = DateTime.Now;

            if (dateOfBirth[1] < today.Month)
                return today.Year - dateOfBirth[2];
            else if (dateOfBirth[1] > today.Month)
                return today.Year - dateOfBirth[2] - 1;
            else if (dateOfBirth[0] < today.Day)
                return today.Year - dateOfBirth[2];
            else
                return today.Year - dateOfBirth[2] - 1;
        }

        public void SyncSpouses(List<Person> roots)
        {
            if (spouse != null)
            {
                if (spouse.bloodGroup == "")
                {
                    foreach (Person root in roots)
                    {
                        if (root.FindAndSyncSpouse(this))
                            break;
                    }
                }
                if (bloodGroup == "")
                {
                    foreach (Person root in roots)
                    {
                        if (root.FindAndSyncSpouse(spouse))
                            break;
                    }
                }
                foreach (Person person in childeren)
                    person.SyncSpouses(roots);
            }
        }

        public bool FindAndSyncSpouse(Person syncPerson)
        {
            if (spouse != null)
            {
                if (bloodGroup != "" && syncPerson.spouse.Name.Contains(name) && (spouse.Name.Contains(syncPerson.Name) || syncPerson.Name.Contains(spouse.Name)))
                {
                    syncPerson.spouse.UpdateInfo(id, name, surname, dateOfBirth, nameOfMother, nameOfFather, bloodGroup, job, surnameBeforeMarriage);
                    return true;
                }
                if (spouse.BloodGroup != "" && syncPerson.spouse.Name.Contains(spouse.Name) && (name.Contains(syncPerson.Name) || syncPerson.Name.Contains(name)))
                {
                    syncPerson.spouse.UpdateInfo(spouse.Id, spouse.name, spouse.surname, spouse.dateOfBirth, spouse.nameOfMother, spouse.nameOfFather, spouse.bloodGroup, spouse.job, spouse.surnameBeforeMarriage);
                    return true;
                }
                foreach (Person person in childeren)
                    if (person.FindAndSyncSpouse(syncPerson))
                        return true;
            }
            return false;
        }

        public List<List<int>> DrawFamilyTreeAndGetLines(Form2 form2 = null)
        {
            List<List<int>> drawableLines = new List<List<int>>();
            DrawFamilyTree(drawableLines, form2:form2);
            return drawableLines;
        }

        public int DrawFamilyTree(List<List<int>> drawableLines, int lineIndex = 0, int printOffset = 0, int prevLine = 0, Form2 form2 = null)
        {
            List<int> lineLens = new List<int>();
            FindLineLen(lineLens);
            prevLine = PrintFamilyTreeToForm(lineLens, lineLens.Max() * 150 + (lineLens.Max() - 1) * 25, lineIndex, printOffset, drawableLines, prevLine, form2);
            int offset = 0;
            foreach (Person person in Childeren)
            {
                if (Childeren.Count == 1 && person.Spouse == null)
                    offset += person.DrawFamilyTree(drawableLines, lineIndex + 1, offset + printOffset + 87, prevLine, form2);
                else
                    offset += person.DrawFamilyTree(drawableLines, lineIndex + 1, offset + printOffset, prevLine, form2);
            }
            return lineLens.Max() * 150 + (lineLens.Max() - 1) * 25 + 25;
        }
        
        public void FindLineLen(List<int> lines, int lineIndex = 0)
        {
            if (lines.Count < lineIndex + 1)
                lines.Add(0);
            if (spouse != null)
                lines[lineIndex] += 2;
            else
                lines[lineIndex]++;
            foreach (Person person in Childeren)
                person.FindLineLen(lines, lineIndex + 1);
        }

        public int PrintFamilyTreeToForm(List<int> lines, int size, int lineIndex, int printOffset, List<List<int>> drawableLines, int prevLine, Form2 form2 = null)
        {
            int returnInt = 0;

            PersonUI temp;

            if (form2 == null)
                temp = personUI;
            else
                temp = form2.CreatePersonUI(Name, Surname, DateOfBirth, BloodGroup, Job, IsMale);

            temp.Location = new Point((size - (lines[0] * 150 + (lines[0] - 1) * 25)) / 2 + printOffset, 25 + lineIndex * 100);
            if (spouse != null)
            {
                PersonUI temp2;

                if (form2 == null)
                    temp2 = spouse.PersonUI;
                else
                    temp2 = form2.CreatePersonUI(spouse.name, spouse.surname, spouse.dateOfBirth, spouse.bloodGroup, spouse.job, spouse.isMale);

                temp2.Location = new Point((size - (lines[0] * 150 + (lines[0] - 1) * 25)) / 2 +  175 + printOffset, 25 + lineIndex * 100);
                drawableLines.Add(new List<int>() { temp.Location.X + 75, temp.Location.Y + 37, temp2.Location.X + 75, temp2.Location.Y + 37 });
                if (childeren.Count > 0)
                {
                    drawableLines.Add(new List<int>() { (temp.Location.X + temp2.Location.X + 150) / 2, temp.Location.Y + 37, (temp.Location.X + temp2.Location.X + 150) / 2, temp.Location.Y + 87 });
                    returnInt = (temp.Location.X + temp2.Location.X + 150) / 2;
                }
            }

            if (lineIndex != 0)
            {
                drawableLines.Add(new List<int>() { temp.Location.X + 75, temp.Location.Y + 37, temp.Location.X + 75, temp.Location.Y - 13});
                drawableLines.Add(new List<int>() { temp.Location.X + 75, temp.Location.Y - 13, prevLine, temp.Location.Y - 13 });
            }

            return returnInt;
        }

        public void SelectPerson()
        {
            personUI.BackColor = Color.FromArgb(255, 255, 255, 0);
            isSelected = true;
        }

        public void DeselectAllPerson()
        {
            isSelected = false;
            if (isMale)
            {
                if (bloodGroup != "")
                    personUI.BackColor = Color.FromArgb(255, 0, 230, 255);
                else
                    personUI.BackColor = Color.FromArgb(255, 0, 180, 200);
            }
            else
            {
                if (bloodGroup != "")
                    personUI.BackColor = Color.FromArgb(255, 255, 150, 205);
                else
                    personUI.BackColor = Color.FromArgb(255, 200, 100, 150);
            }
            if (spouse != null)
            {
                spouse.IsSelected = false;
                if (spouse.IsMale)
                {
                    if (spouse.BloodGroup != "")
                        spouse.PersonUI.BackColor = Color.FromArgb(255, 0, 230, 255);
                    else
                        spouse.PersonUI.BackColor = Color.FromArgb(255, 0, 180, 200);
                }
                else
                {
                    if (spouse.BloodGroup != "")
                        spouse.PersonUI.BackColor = Color.FromArgb(255, 255, 150, 205);
                    else
                        spouse.PersonUI.BackColor = Color.FromArgb(255, 200, 100, 150);
                }
            }
            foreach (Person person in childeren)
                person.DeselectAllPerson();
        }

        public void SearchPerson()
        {
            personUI.BackColor = Color.FromArgb(255, 255, 50, 50);
        }

        public void DesearchPerson()
        {
            if (!isSelected)
            {
                if (isMale)
                {
                    if (bloodGroup != "")
                        personUI.BackColor = Color.FromArgb(255, 0, 230, 255);
                    else
                        personUI.BackColor = Color.FromArgb(255, 0, 180, 200);
                }
                else
                {
                    if (bloodGroup != "")
                        personUI.BackColor = Color.FromArgb(255, 255, 150, 205);
                    else
                        personUI.BackColor = Color.FromArgb(255, 200, 100, 150);
                }
            }
            else
                SelectPerson();
        }

        public async Task FindBloodGroupInFamilyTree(string neededBloodGroup, List<string> wantedPersons, RichTextBox richTextBox)
        {
            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if (bloodGroup != "" && bloodGroup.Substring(0, bloodGroup.Length - 3) == neededBloodGroup)
            {
                wantedPersons.Add(name + " " + surname);
                richTextBox.Text += "    " + name + " " + surname + "\n";
                SelectPerson();
            }
            if (spouse != null)
            {
                spouse.SearchPerson();
                await Task.Delay(searchDelayTime);
                spouse.DesearchPerson();

                if (spouse.BloodGroup != "" && spouse.BloodGroup.Substring(0, spouse.BloodGroup.Length - 3) == neededBloodGroup)
                {
                    wantedPersons.Add(spouse.Name + " " + spouse.Surname);
                    richTextBox.Text += "    " + spouse.Name + " " + spouse.Surname + "\n";
                    spouse.SelectPerson();
                }
                foreach (Person person in childeren)
                    await person.FindBloodGroupInFamilyTree(neededBloodGroup, wantedPersons, richTextBox);
            }

        }

        public async Task FindSameJobs(List<string> wantedPersons, RichTextBox richTextBox, List<string> jobs = null, List<Person> persons = null)
        {
            if (jobs == null)
            {
                persons = new List<Person>();
                jobs = new List<string>();
            }

            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if (job != "" && jobs.Contains(job))
            {
                int jobIndex = jobs.IndexOf(job);
                if (!persons[jobIndex].IsSelected)
                {
                    wantedPersons.Add(persons[jobIndex].Name + " " + persons[jobIndex].Surname + " - " + job);
                    richTextBox.Text += "    " + persons[jobIndex].Name + " " + persons[jobIndex].Surname + " - " + job + " (İlk Yapan)\n";
                    persons[jobIndex].SelectPerson();
                }
                wantedPersons.Add(name + " " + surname + " - " + job);
                richTextBox.Text += "    " + name + " " + surname + " - " + job + "\n";
                SelectPerson();
            }
            else if (job != "")
            {
                persons.Add(this);
                jobs.Add(job);
            }
            if (spouse != null)
            {
                spouse.SearchPerson();
                await Task.Delay(searchDelayTime);
                spouse.DesearchPerson();

                if (spouse.Job != "" && jobs.Contains(spouse.Job))
                {
                    int jobIndex = jobs.IndexOf(spouse.Job);
                    if (!persons[jobIndex].IsSelected)
                    {
                        wantedPersons.Add(persons[jobIndex].Name + " " + persons[jobIndex].Surname + " - " + spouse.Job);
                        richTextBox.Text += "    " + persons[jobIndex].Name + " " + persons[jobIndex].Surname + " - " + spouse.Job + " (İlk Yapan)\n";
                        persons[jobIndex].SelectPerson();
                    }
                    wantedPersons.Add(spouse.Name + " " + spouse.Surname + " - " + spouse.Job);
                    richTextBox.Text += "    " + spouse.Name + " " + spouse.Surname + " - " + spouse.Job + "\n";
                    spouse.SelectPerson();
                }
                else if (spouse.Job != "")
                {
                    persons.Add(spouse);
                    jobs.Add(spouse.Job);
                }
                foreach (Person person in childeren)
                    await person.FindSameJobs(wantedPersons, richTextBox, new List<string>(jobs), new List<Person>(persons));
            }
        }

        public async Task FindSameNames(List<string> wantedPersons, RichTextBox richTextBox, List<string> names = null, List<Person> persons = null)
        {
            if (names == null)
            {
                persons = new List<Person>();
                names = new List<string>();
            }

            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if (names.Contains(name))
            {
                int nameIndex = names.IndexOf(name);
                if (!persons[nameIndex].IsSelected)
                {
                    wantedPersons.Add(name);
                    richTextBox.Text += "    " + name + "\n";
                    persons[nameIndex].SelectPerson();
                }
                SelectPerson();
            }
            else
            {
                persons.Add(this);
                names.Add(name);
            }
            if (spouse != null)
            {
                spouse.SearchPerson();
                await Task.Delay(searchDelayTime);
                spouse.DesearchPerson();

                if (names.Contains(spouse.Name))
                {
                    int nameIndex = names.IndexOf((spouse.Name));
                    if (!persons[nameIndex].IsSelected)
                    {
                        wantedPersons.Add(spouse.Name);
                        richTextBox.Text += "    " + spouse.Name + "\n";
                        persons[nameIndex].SelectPerson();
                    }
                    spouse.SelectPerson();
                }
                else
                {
                    persons.Add(spouse);
                    names.Add(spouse.Name);
                }
                foreach (Person person in childeren)
                    await person.FindSameNames(wantedPersons, richTextBox, names, persons);
            }
        }

        public int FindGenerationCount(List<int> genCounts, int genCount = 0)
        {
            genCounts.Add(genCount);
            foreach (Person person in childeren)
                person.FindGenerationCount(genCounts, genCount + 1);
            return genCounts.Max() + 1;
        }

        public async Task<int> FindNameAndGenerationCount(string wantedName)
        {
            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if ((name + surname).Replace(" ", "") == wantedName.Replace(" ", ""))
            {
                SelectPerson();
                return FindGenerationCount(new List<int>());
            }
            if (spouse != null)
            {
                spouse.SearchPerson();
                await Task.Delay(searchDelayTime);
                spouse.DesearchPerson();

                if ((spouse.Name + spouse.Surname).Replace(" ", "") == wantedName.Replace(" ", ""))
                {
                    spouse.SelectPerson();
                    return FindGenerationCount(new List<int>());
                }
            }
            foreach (Person person in childeren)
            {
                int genCount = await person.FindNameAndGenerationCount(wantedName);
                if (genCount != -1)
                    return genCount;
            }     

            return -1;
        }

        public async Task<Person> FindName(string wantedName)
        {
            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if ((name + surname).Replace(" ", "") == wantedName.Replace(" ", ""))
            {
                SelectPerson();
                return this;
            }
            if (spouse != null)
            {
                spouse.SearchPerson();
                await Task.Delay(searchDelayTime);
                spouse.DesearchPerson();

                if ((spouse.Name + spouse.Surname).Replace(" ", "") == wantedName.Replace(" ", ""))
                {
                    spouse.SelectPerson();
                    return spouse;
                }
            }
            foreach (Person person in childeren)
            {
                Person temp = await person.FindName(wantedName);
                if (temp != null)
                    return temp;
            }

            return null;
        }

        public async Task FindChildlessPersons(List<Person> wantedPersons, RichTextBox richTextBox)
        {
            foreach (Person person in childeren)
                await person.FindChildlessPersons(wantedPersons, richTextBox);

            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if (childeren.Count == 0)
            {
                SelectPerson();

                richTextBox.Text = "Çocuğu olmayan insanlar:\n";
                if (wantedPersons.Count != 0)
                {
                    bool isInserted = false;
                    for (int i = 0; i < wantedPersons.Count; i++)
                    {
                        if (wantedPersons[i].GetAge() > GetAge() && !isInserted)
                        {
                            wantedPersons.Insert(i, this);
                            isInserted = true;
                        }
                        richTextBox.Text += "    " + wantedPersons[i].name + " " + wantedPersons[i].surname + " - " + wantedPersons[i].GetAge() + "\n";
                    }
                    if (!isInserted)
                    {
                        wantedPersons.Add(this);
                        richTextBox.Text += "    " + name + " " + surname + " - " + GetAge() + "\n";
                    }
                }
                else
                {
                    wantedPersons.Add(this);
                    richTextBox.Text += "    " + wantedPersons[0].name + " " + wantedPersons[0].surname + " - " + wantedPersons[0].GetAge() + "\n";
                }

                if (spouse != null)
                {
                    spouse.SearchPerson();
                    await Task.Delay(searchDelayTime);
                    spouse.DesearchPerson();

                    spouse.SelectPerson();

                    richTextBox.Text = "Çocuğu olmayan insanlar:\n";
                    bool isInserted = false;
                    for (int i = 0; i < wantedPersons.Count; i++)
                    {
                        if (wantedPersons[i].GetAge() > spouse.GetAge() && !isInserted)
                        {
                            wantedPersons.Insert(i, spouse);
                            isInserted = true;
                        }
                        richTextBox.Text += "    " + wantedPersons[i].name + " " + wantedPersons[i].surname + " - " + wantedPersons[i].GetAge() + "\n";
                    }
                    if (!isInserted)
                    {
                        wantedPersons.Add(spouse);
                        richTextBox.Text += "    " + spouse.Name + " " + spouse.Surname + " - " + spouse.GetAge() + "\n";
                    }
                }
            }
        }

        public async Task FindStepPersons(List<string> wantedPersons, RichTextBox richTextBox)
        {
            foreach (Person person in childeren)
            {
                person.SearchPerson();
                await Task.Delay(searchDelayTime);
                person.DesearchPerson();

                if ((person.nameOfMother != name && person.nameOfFather != name) || (!spouse.name.Contains(person.nameOfMother) && !spouse.name.Contains(person.nameOfFather)))
                {
                    richTextBox.Text = "Üvey çocuklar:\n";
                    if (wantedPersons.Count != 0)
                    {
                        bool isInserted = false;
                        for (int k = 0; k < wantedPersons.Count; k++)
                        {
                            if (wantedPersons[k][0] > person.Name[0] && !isInserted)
                            {
                                wantedPersons.Insert(k, person.Name);
                                isInserted = true;
                            }
                            richTextBox.Text += "    " + wantedPersons[k] + "\n";
                        }
                        if (!isInserted)
                        {
                            wantedPersons.Add(person.Name);
                            richTextBox.Text += "    " + person.Name + "\n";
                        }
                    }
                    else
                    {
                        wantedPersons.Add(person.Name);
                        richTextBox.Text += "    " + person.Name + "\n";
                    }

                    person.SelectPerson();
                }
            }

            foreach (Person person in childeren)
                await person.FindStepPersons(wantedPersons, richTextBox);
        }

        public async Task FindStepSiblings(List<string> wantedPersons, RichTextBox richTextBox)
        {
            for (int i = 0; i < childeren.Count - 1; i++)
            {
                childeren[i].SearchPerson();
                for (int j = i + 1; j < childeren.Count; j++)
                {
                    childeren[j].SearchPerson();
                    await Task.Delay(searchDelayTime);
                    childeren[j].DesearchPerson();

                    if (childeren[i].nameOfFather != childeren[j].NameOfFather || childeren[i].nameOfMother != childeren[j].nameOfMother)
                    {
                        richTextBox.Text = "Üvey kardeşler:\n";
                        if (wantedPersons.Count != 0)
                        {
                            bool isInserted = false;
                            for (int k = 0; k < wantedPersons.Count; k++)
                            {
                                if (wantedPersons[k][0] > childeren[i].Name[0] && !isInserted)
                                {
                                    wantedPersons.Insert(k, childeren[i].Name + " - " + childeren[j].Name);
                                    isInserted = true;
                                }
                                richTextBox.Text += "    " + wantedPersons[k] + "\n";
                            }
                            if (!isInserted)
                            {
                                wantedPersons.Add(childeren[i].Name + " - " + childeren[j].Name);
                                richTextBox.Text += "    " + childeren[i].Name + " - " + childeren[j].Name + "\n";
                            }
                        }
                        else
                        {
                            wantedPersons.Add(childeren[i].Name + " - " + childeren[j].Name);
                            richTextBox.Text += "    " + childeren[i].Name + " - " + childeren[j].Name + "\n";
                        }

                        if (!childeren[i].isSelected)
                            childeren[i].SelectPerson();
                        childeren[j].SelectPerson();
                    }
                }
                childeren[i].DesearchPerson();
            }

            foreach (Person person in childeren)
                await person.FindStepSiblings(wantedPersons, richTextBox);
        }

        public async Task<int> FindTwoPeopleAndClosestRoot(List<Person> persons1, List<Person> persons2, string name1, string name2, int findedPerson = 0)
        {
            int fPerson = 0;
            foreach (Person person in childeren)
            {
                if (fPerson < 2)
                    fPerson += await person.FindTwoPeopleAndClosestRoot(persons1, persons2, name1, name2, fPerson > findedPerson ? fPerson : findedPerson);
                if (fPerson == 2)
                {
                    persons1.Add(this);
                    return 3;
                }
                else if (fPerson > 2)
                    return 3;
            }

            SearchPerson();
            await Task.Delay(searchDelayTime);
            DesearchPerson();

            if ((name + surname).Replace(" ", "") == name1.Replace(" ", "") || (name + surname).Replace(" ", "") == name2.Replace(" ", ""))
            {
                SelectPerson();
                if (findedPerson == 0)
                    persons1.Add(this);
                else
                    persons2.Add(this);
                if (fPerson == 0)
                    return 1;
                else
                    return 3;
            }

            if (spouse != null)
            {
                spouse.SearchPerson();
                await Task.Delay(searchDelayTime);
                spouse.DesearchPerson();

                if ((spouse.Name + spouse.Surname).Replace(" ", "") == name1.Replace(" ", "") || (spouse.Name + spouse.Surname).Replace(" ", "") == name2.Replace(" ", ""))
                {
                    spouse.SelectPerson();
                    if (findedPerson == 0)
                        persons1.Add(Spouse);
                    else
                        persons2.Add(Spouse);
                    if (fPerson == 0)
                        return 1;
                    else
                        return 3;
                }
            }

            if (fPerson == 1)
            {
                if (findedPerson == 0)
                    persons1.Add(this);
                else
                    persons2.Add(this);
            }

            return fPerson;
        }
    }
}