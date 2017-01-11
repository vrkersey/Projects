//Victor Kersey, Nick Steiner
//11-03-2016
//CS 3500
//PS6

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> cells; //to hold cell names and their contents
        private Dictionary<string, object> valueDictionary; //to hold cell names and their values
        private DependencyGraph dg; //to keep track of the dependencies
        private bool changed; //switch that is used to track if a change was made but not saved

        /// <summary>
        /// Property that indicates if changes were made since the last save
        /// </summary>
        public override bool Changed
        {
            get
            {
                return changed;
            }

            protected set
            {
                changed = value;
            }
        }

        /// <summary>
        /// three-arguement contructor, passes all three arguements to abstract constructor
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) 
            : base(isValid, normalize, version)
        {
            Changed = false;
            valueDictionary = new Dictionary<string, object>();
            cells = new Dictionary<string, object>();
            dg = new DependencyGraph();
        }

        /// <summary>
        /// zero-arguement contructor, passes three 'default' arguements to abstract constructor
        /// </summary>
        public Spreadsheet()
            : this(s => true, s => s, "default") { }

        /// <summary>
        /// four-arguement constructor, passes three arguements to abstract constructor. The first arguement is the file to open a spreadsheet
        /// </summary>
        public Spreadsheet(string fileName, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : this(isValid, normalize, version)
        {
            if (GetSavedVersion(fileName) != version)
                throw new SpreadsheetReadWriteException("Saved Version does not equal the version provided");
            OpenXMLFile(fileName);
        }

        /// <summary>
        /// Returns the contents of a cell. 
        /// If the cell has no contents "" is returned
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <returns>The contents of a given cell</returns>
        public override object GetCellContents(string name)
        {
            if (name == null)
                throw new InvalidNameException();

            string normalizedName = Normalize.Invoke(name);

            if (name == null || !couldBeAVariable(normalizedName))
                throw new InvalidNameException();

            object v; //used to retrieve the contents of the cells dictionary
            if (!cells.TryGetValue(normalizedName, out v)) //cell has no value
                return ""; //if you set this to 0.0 it makes way more sense and it will make lookup not have to throw an exception
            return v;
        }

        /// <summary>
        /// used to find all cells who have contents
        /// </summary>
        /// <returns>Contents of given cell</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return new HashSet<string>(cells.Keys);
        }

        /// <summary>
        /// New Driver method for the overloaded protected SetContentsOfCell methods
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="content">Cell's contents</param>
        /// <returns>a set of all cells affected by the given cell</returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (name == null)
                throw new InvalidNameException();

            Changed = true; //the worksheet has been modified if anything is changed

            string normalizedName = Normalize.Invoke(name); //normalize it before passing it to the protected helper methods
            double n;
            HashSet<string> set; //to hold the ISet returned by the protected helper methods

            if (Double.TryParse(content, out n)) //content is a Double
                set = new HashSet<string>(SetCellContents(normalizedName, n));
            else if (content.Count() > 1 && content[0] == '=') //content is a formula
            {
                Formula f = new Formula(content.Substring(1), Normalize, IsValid);
                set = new HashSet<string>(SetCellContents(normalizedName, f));
            }
            else //content is a string
                set = new HashSet<string>(SetCellContents(normalizedName, content));

            foreach (string el in set) //reset the values of all cells that rely on current cell
            {
                object contents;

                if (cells.TryGetValue(el, out contents) && object.ReferenceEquals(contents.GetType(), typeof(Formula)))
                {
                    Formula f = (Formula)contents;
                    valueDictionary[el] = f.Evaluate(lookup);
                }
            }
            return set;
        }

        /// <summary>
        /// Used to set the contents of a given cell to a formula
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="formula">Cell's contents</param>
        /// <returns>a set of all cells affected by the given cell</returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (name == null || !couldBeAVariable(name))
                throw new InvalidNameException();

            if (formula == null)
                throw new ArgumentNullException();

            if (cells.ContainsKey(name)) //check to see if the cell used to have some other data in it
                removeDependencies(name);

            foreach (string el in formula.GetVariables()) //adding the dependencies
                dg.AddDependency(el, name);

            cells[name] = formula;
            valueDictionary[name] = formula.Evaluate(lookup);

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Used to set the contents of a given cell to a formula
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="formula">Cell's contents</param>
        /// <returns>a set of all cells affected by the given cell</returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (name == null || !couldBeAVariable(name))
                throw new InvalidNameException();

            if (text == null)
                throw new ArgumentNullException();

            if (cells.ContainsKey(name))//check to see if the cell used to have some other data in it
                removeDependencies(name);

            if (text != "") //we don't want blank cells in the dictionary
                cells[name] = text;
            valueDictionary[name] = new FormulaError("Can not evaluate a string");

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Used to set the contents of a given cell to a formula
        /// </summary>
        /// <param name="name">Cell's name</param>
        /// <param name="formula">Cell's contents</param>
        /// <returns>a set of all cells affected by the given cell</returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null || !couldBeAVariable(name))
                throw new InvalidNameException();

            if (cells.ContainsKey(name))//check to see if the cell used to have some other data in it
                removeDependencies(name);

            cells[name] = number;
            valueDictionary[name] = number;

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        //Helper method to remove the dependency of all variables that may have been in the cell
        private void removeDependencies(string name)
        {
            HashSet<string> set = new HashSet<string>(dg.GetDependees(name));
            foreach (string el in set)
                dg.RemoveDependency(el, name);
            cells.Remove(name);
            valueDictionary.Remove(name);
        }

        /// <summary>
        /// way of getting the direct dependents of a given cell. Helper method for getCellsToRecalculate
        /// </summary>
        /// <param name="name">Cell name</param>
        /// <returns>Direct Dependents</returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
                throw new ArgumentNullException();
            if (!couldBeAVariable(name))
                throw new InvalidNameException();

            HashSet<string> dependents = new HashSet<string>(); //to hold all the direct dependents
            if (dg.HasDependents(name))
                foreach (string el in dg.GetDependents(name))
                {
                    dependents.Add(el);
                }

            return dependents;
        }

        // Helper method for detecting if a variable is a variable.
        // uses isValid in determining if a string could be a variable
        private bool couldBeAVariable(string s)
        {
            int n;
            if (!char.IsLetter(s[0])) //if the first letter is not a letter
                return false;
            bool intSwitch = false; //used to test if a letter comes after a number

            for (int i = 1; i < s.Count(); i++)
            {
                if (Int32.TryParse(s[i].ToString(), out n))
                {
                    intSwitch = true;
                }
                else if (intSwitch)
                {
                    return false;
                }
            }
            return IsValid.Invoke(s); //passed my tests now check isValid
        }

        /// <summary>
        /// returns the version of the spreadsheet
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            string version = null;
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    version = reader["version"];
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("unable to obtain Version information");
            }
            return version;
        }
        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// </summary>
        /// <param name="filename"></param>
        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("  ");
            try
            {
                // Create an XmlWriter inside this block, and automatically Dispose() it at the end.
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    foreach (string el in cells.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", el);
                        writer.WriteElementString("contents", CellInfo(el));
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                Changed = false; //after the file has saved the file is no longer changed
            }catch
            {
                throw new SpreadsheetReadWriteException("The File didn't save correctly");
            }
        }

        //Helper method for getting the contents of a cell back as a saveable string
        //mainly used to add a '=' in front of a formula
        private string CellInfo(string el)
        {
            StringBuilder sb = new StringBuilder();
            object contents = GetCellContents(el);
            if (object.ReferenceEquals(contents.GetType(), typeof(Formula))) 
            {
                sb.Append("=");
                sb.Append(contents.ToString());
            }
            else
            {
                sb.Append(contents.ToString());
            }
            return sb.ToString();
        }

        // Opens a spreadsheet from an XML File, The file must be formatted exactly the same as save()
        private void OpenXMLFile(string fileName)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    string name = null;
                    string contents = null;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    this.Version = reader["version"];
                                    break;
                                case "cell":
                                    //cell found
                                    break;
                                case "name":
                                    reader.Read();
                                    name = reader.Value;
                                    break;
                                case "contents":
                                    reader.Read();
                                    contents = reader.Value;
                                    if (name != null)
                                    {
                                        SetContentsOfCell(name, contents);
                                        name = null;
                                    }
                                    break;

                            }

                        }
                    }
                }
            }
            catch
            {
                throw new SpreadsheetReadWriteException("The File didn't open correctly");
            }
        }

        /// <summary>
        /// returns the value of a given cell
        /// </summary>
        /// <param name="name">cell Name</param>
        /// <returns>double, string, FormulaError</returns>
        public override object GetCellValue(string name)
        {
            if (name == null)
                throw new InvalidNameException();
            string normalizedName = Normalize.Invoke(name);
            if (!couldBeAVariable(normalizedName))
                throw new InvalidNameException();
            object vContents;
            object cContents;
            
            if (valueDictionary.TryGetValue(normalizedName, out vContents) && cells.TryGetValue(normalizedName,out cContents)) //to see if the value is already in the valueDictionary
            {
                if (vContents.GetType() == typeof(FormulaError) && cContents.GetType() == typeof(string))
                {
                    return cContents;
                }
                return vContents;
            }

            return "";
        }
        /// <summary>
        /// lookup method used in evaluate. 
        /// </summary>
        /// <param name="name">cell name</param>
        /// <returns>double</returns>
        private double lookup(string name)
        {
            double n; //used to hold doubles that we get out of valueDictionary
            object contents = GetCellContents(name); //used to hold objects that we get out of cells dictionary
            object vContents = GetCellValue(name); //used to hold objects that we get out of value dictionary

            if (object.ReferenceEquals(vContents.GetType(), typeof(double))) //to see if the value is already in the valueDictionary
                return (double)vContents;
            else if (object.ReferenceEquals(vContents.GetType(), typeof(FormulaError)))
                throw new Exception();

            if (Double.TryParse(contents.ToString(),out n)) //see if the contents are a double
            {
                return (double)contents;
            }
            else if (object.ReferenceEquals(contents.GetType(), typeof(string))) //see if the contents are a string
            {
                throw new Exception();  
            }
            else if (Object.ReferenceEquals(contents.GetType(), typeof(Formula))) //see if the contents are a formula
            {
                Formula f = (Formula)contents;
                foreach (string el in f.GetVariables())
                    contents = f.Evaluate(lookup); //evaluate the contents
                if (double.TryParse(contents.ToString(), out n)) //if the evaluated value is a double add it to valueDictionary
                {
                    valueDictionary[name] = n;
                }
                else if (object.ReferenceEquals(contents.GetType(), typeof(FormulaError)))
                {
                    throw new Exception();
                }
            }
            return n;  
        }

    }
}
