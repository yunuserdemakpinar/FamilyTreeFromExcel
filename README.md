PROGRAMMIG LABORATORY

PROJECT 3

EKREM KIRDEMİR

Kocaeli University Faculty of Engineering, Computer Engineering 2. Grade kirdemir.ekrem@gmail.com

YUNUS ERDEM AKPINAR

Kocaeli University Faculty of Engineering, Computer Engineering 2. Grade  akpinaryunuserdem@gmail.com 

This program reads data from an Excel file and creates the relationships between family members and the graphs that show these relationships. The program creates a Form1 class and two methods named CreateFamilyTrees and CreateButtonsList. The CreateFamilyTrees method opens the Excel file, reads the data on its pages and creates graphical family trees using the PersonUI class. The Create- ButtonsList method creates buttons for GUI. The result of the program is ancestry tree graphs that show the relationships between family members.

# INTRODUCTION

The Family tree program is a tool that allows users to create and retrieve information about their family unit in the form of a graphical tree. The program is written in C# and uses Microsoft Visual Basic and Microsoft Office Interop Excel frameworks to read data from a Microsoft Excel spreadsheet and create a family tree.
The program is designed to be easy to use and user friendly. It has a simple and easy to use interface and allows users to quickly and easily retrieve information about family members. The program also includes a node module that allows the user to switch between different family tree panels.

# METHOD

The program starts by declaring several lists and variables. The roots list will store the root nodes of the family trees, the rows list will store the links between family members and the buttons list will store the buttons used in the GUI. The tablePath variable stores the path to the Excel table from which the program will read the data.

The program has a Form1 class that represents the main form of the GUI. The Form1 class has a Form1 Load method that is executed when the form is loaded. This function connects the other two methods: CreateFamilyTrees and CreateButtonsList.

The CreateFamilyTrees method reads data from an Excel table and creates graphical representations of family trees using the PersonUI class. The function starts by creating an instance of the ExcelApp.Application class and using it to open the Excel table specified by the tablePath variable. The function then iterates over the sheets in the table, creating a new family tree for each sheet.

The function reads data from the cells in each sheet and uses it to create PersonUI objects, which are graphical representations of the individuals in the family tree. The function then adds these objects to the appropriate panel in the GUI. The function also creates connections between family members by creating line objects and adding them to the lines list.
The CreateButtonsList function creates a list of buttons used in the GUI. buttons are created using the Button class and added to the buttons list.

Finally, the program has a Form1 constructor that is responsible for initializing the form and its components. The InitializeComponent function is used to set up the form and its components.

To understand how the code works, it is important to understand the structure of the Excel table from which the program reads the data. It is important to understand how the table is structured
is assumed to have four pages representing the individuals in the family tree. Each page contains data about the individuals in the family tree, organized in rows and columns. The columns represent different pieces of information about each individual, such as name, date of birth and blood type. Rows represent the different individuals in the family tree.

The CreateFamilyTrees function reads the data from the Excel table, iterating over the rows and columns of each page. The function reads the data in the cells for each row and uses them to create a PersonUI object. The function then adds the PersonUI object to the appropriate panel in the GUI. The method also creates connections between family members by creating line objects and adding them to the lines list.

The PersonUI class is a special class that represents a graphical representation of an individual in the family tree. The class has several properties such as name, surname and dateOfBirth, which represent the first name, last name and date of birth of the individual. The class also has a Draw function which is responsible for drawing the PersonUI object in the GUI.

The CreateButtonsList function creates a list of buttons used in the GUI. These Buttons are created using the Button class and added to the list of buttons.

### Person Class:

This class represents a Person class in the FamilyTree namespace in the C# programming language. This class represents a person in the family tree and contains information about the person's identity, relationships and personal details.

The Person class contains several special fields that store information about the person, including the person's first name, last name, date of birth, mother's and father's names, blood type, occupation, last name before marriage, and gender. The class also includes many publications that provide access to this information.

The Person class has a constructor method that takes many arguments and adjusts the values of the fields based on these arguments. It also has a method called UpdateInfo that allows the information of a person to be updated.

The Person class has a method called AddSpouse that allows a person to marry another person. If the person is already married, the method first checks if the current spouse is the same as the new spouse. If they are not the same, the method replaces the current one with the new one.

The Person class also has a method called AddChild that allows a person to have c children. This method takes the Person object as an argument and adds it to the list of c children of the current person.

The Person class also includes other methods such as Search, SearchInFamilyTree, Remove, and RemoveFromFamilyTree for various purposes, such as searching for a person in the family tree, removing a person from the family tree, and reviewing the information returned in the user interface. These methods search for a person in the family tree, remove a person from the family tree, and perform various other tasks.

# CONCLUSION

The Family Tree program is a tool that allows users to create and navigate graphical tree-like information about their family members. The program is written in C# and uses Microsoft Visual Basic and Microsoft Office Interop Excel frameworks to read data from a Microsoft Excel spreadsheet and create a family tree.

# BIBLIOGRAPHY
1. Juan, Angel (2006). ”Ch20 –Data Structures; ID06 - PRO- GRAMMING with JAVA (slide part of the book ’Big Java’, by CayS. Horstmann)” (PDF). p. 3. Archived from the original (PDF) on 2012-01-06. Retrieved 2011-07-10.
1. Black, Paul E. (2004-08-16). Pieterse, Vreda; Black, Paul E. (eds.). ”linked list”. Dictionary of Algorithms and Data Struc- tures. National Institute of Standards and Technology. Retrieved 2004-12-14.
1. Antonakos, James L.; Mansfield, Kenneth C. Jr. (1999). Practi- cal Data Structures Using C/C++. Prentice-Hall. pp. 165–190. ISBN 0-13-280843-9.
1. https://medium.com/kodcular/adan-z-ye-c-oop-2d766cf2d144
1. https://www.c-sharpcorner.com/UploadFile/84c85b/object- oriented-programming-using-C-Sharp-net/
1. Collins, William J. (2005) [2002]. Data Structures and the Java Collections Framework. New York: McGraw Hill. pp. 239–303. ISBN 0-07-282379-8.
1. https://learn.microsoft.com/en-us/dotnet/csharp/programming- guide/classes-and-structs/access-modifiers
1. https://www.pluralsight.com/courses/c-sharp-code-more-object -oriented?aid=7010a000002BWqGAAW&promo=&utm source non¯ branded&utm medium=digital paid search google&utm campaign=EMEA![](Aspose.Words.bf976d4e-5d9c-434a-a8a7-4587f2d1c872.001.png)![](Aspose.Words.bf976d4e-5d9c-434a-a8a7-4587f2d1c872.002.png) Dynamic&utm content=&gclid=Cj0KCQiA veebBhD ARIsAFaAvrHyp395fdtFdwBUKknQuSz0xxfOI0ILd xihO33PeQN3-n5pHLZzNRcaArXnEALw wcB
1. Green, Bert F. Jr. (1961). ”Computer Languages for Symbol Manipulation”. IRE Transactions on Human Factors in Elec- tronics (2): 3–8. doi:10.1109/THFE2.1961.4503292.
1. McCarthy, John (1960). ”Recursive Functions of Symbolic Ex- pressions and Their Computation by Machine, Part I”. Commu- nications of the ACM. 3 (4): 184. doi:10.1145/367177.367199. S2CID 1489409.
1. Parlante, Nick (2001). ”Linked list basics” (PDF). Stanford University. Retrieved 2009-09-21
1. Shanmugasundaram, Kulesh (2005-04-04). ”Linux Kernel Linked List Explained”. Retrieved 2009-09-21.
1. https://circuitstream.com/blog/learn-c-for-unity-lesson-6- inheritance-and-interfaces/
1. https://www.youtube.com/watch?v=2LA3BLqOw9g
1. https://en.wikipedia.org/wiki/List of terms relating to algorit hms and data structures
1. https://www.gencayyildiz.com/blog/cta- inheritancekalitimmiras/
1. Microsoft documentation on dynamic interfaces: https://docs.microsoft.com/en-us/dotnet/csharp/programming- guide/interfaces/dynamic-interfaces
1. ”C# 8.0 and .NET Core 3.0 - Modern Cross-Platform De- velopment - Fourth Edition” by Mark J. Price: This book includes a chapter on dynamic interfaces that provides a detailed overview of the topic, including examples of how to use dynamic interfaces in C#.
1. C# Corner tutorial on dynamic interfaces: https://www.c- sharpcorner.com/article/dynamic-interface-in-c-sharp/
1. C# Station tutorial on dynamic interfaces: https://www.csharp- station.com/Tutorial/CSharp/Lesson24
1. https://www.javatpoint.com/c-sharp-abstract
1. The Microsoft documentation on the Label control in C#: https://docs.microsoft.com/en- us/dotnet/api/system.windows.forms.label?view=netframework-

23. A tutorial on creating dynamic labels in C#: https://www.c- sharpcorner.com/article/creating-dynamic-labels-in-c-sharp/
23. A forum discussion on dynamically updating the text of a label in C#: https://www.dreamincode.net/forums/topic/246873- dynamically-update-label-text-c%23/

![](Aspose.Words.bf976d4e-5d9c-434a-a8a7-4587f2d1c872.003.png)

Fig. 1. psuedo-1

![](Aspose.Words.bf976d4e-5d9c-434a-a8a7-4587f2d1c872.004.png)

Fig. 2. psuedo-2

![](Aspose.Words.bf976d4e-5d9c-434a-a8a7-4587f2d1c872.005.png)

Fig. 3. psuedo-3
