///Updated by Neal Phelps u0669056 on 2/25/2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    /// <summary>
    /// Class that implements the AbstractSpreadsheet class and creates spreadsheet
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //Initialie instance variables
        DependencyGraph dp;
        Dictionary<String, Cell> spreadsheet;
        Regex isValid;
        /// <summary>
        /// 
        ///Gives the given changed bool a get and a protected set and is initialized
        /// </summary>
        public override bool Changed
        {
            get;

            protected set;
        }


        /// <summary>
        /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
        /// spreadsheet consists of an infinite number of named cells.
        /// 
        /// A string is a cell name if and only if it consists of one or more letters, 
        /// followed by a non-zero digit, followed by zero or more digits.  Cell names
        /// are not case sensitive.
        /// 
        /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that 
        /// "A15" and "a15" name the same cell.)  On the other hand, "Z", "X07", and 
        /// "hello" are not cell names."
        /// 
        /// A spreadsheet contains a cell corresponding to every possible cell name.  
        /// In addition to a name, each cell has a contents and a value.  The distinction is
        /// important, and it is important that you understand the distinction and use
        /// the right term when writing code, writing comments, and asking questions.
        /// 
        /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
        /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
        /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
        /// 
        /// In an empty spreadsheet, the contents of every cell is the empty string.
        ///  
        /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
        /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
        /// in the grid.)
        /// 
        /// If a cell's contents is a string, its value is that string.
        /// 
        /// If a cell's contents is a double, its value is that double.
        /// 
        /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
        /// The value of a Formula, of course, can depend on the values of variables.  The value 
        /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
        /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
        /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
        /// is a double, as specified in Formula.Evaluate.
        /// 
        /// Spreadsheets are never allowed to contain a combination of Formulas that establish
        /// a circular dependency.  A circular dependency exists when a cell depends on itself.
        /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
        /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
        /// dependency.
        ///Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            spreadsheet = new Dictionary<string, Cell>();
            dp = new DependencyGraph();
            isValid = new Regex(".*");
            Changed = false;
        }
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        public Spreadsheet(Regex isValid)
        {
            spreadsheet = new Dictionary<string, Cell>();
            dp = new DependencyGraph();
            this.isValid = isValid;
            Changed = false;
        }
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  If there's a problem reading source, throws an IOException
        /// If the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  If there is an invalid cell name, or a 
        /// duplicate cell name, or an invalid formula in the source, throws a SpreadsheetReadException.
        /// If there's a Formula that causes a circular dependency, throws a SpreadsheetReadException. 
        public Spreadsheet(TextReader source)
        {

            spreadsheet = new Dictionary<string, Cell>();
            dp = new DependencyGraph();
            Changed = false;

            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "Spreadsheet.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;

            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                isValid = new Regex(reader.GetAttribute("IsValid"));
                                break;

                            case "cell":
                                String name = reader.GetAttribute("name");
                                String content = reader.GetAttribute("contents");
                                try
                                {
                                    SetContentsOfCell(name, content);
                                }
                                catch (InvalidNameException)
                                {
                                    throw new SpreadsheetReadException("");
                                }
                                catch (FormulaFormatException)
                                {
                                    throw new SpreadsheetReadException("");
                                }
                                catch (FormulaEvaluationException)
                                {
                                    throw new SpreadsheetReadException("");
                                }
                                catch (CircularException)
                                {
                                    throw new SpreadsheetReadException("");
                                }
                                catch (KeyNotFoundException)
                                {
                                    throw new SpreadsheetReadException("");
                                }


                                break;

                        }
                    }
                }
            }

        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (isInvalid(name))
            {
                throw new InvalidNameException();
            }
            Cell cell;
            if (spreadsheet.TryGetValue(name, out cell))
            {
                return cell.GetContents();
            }
            return "";

        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> list = new HashSet<string>();
            foreach (KeyValuePair<string, Cell> cell in spreadsheet)
            {
                if (!cell.Value.IsCellEmpty())
                {
                    list.Add(cell.Key);
                }
            }
            return list;
        }

        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            Cell cell = new Cell();
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (isInvalid(name))
            {
                throw new InvalidNameException();
            }
            if (spreadsheet.TryGetValue(name, out cell))
            {
                cell.SetContents(formula);
            }
            else
            {
                Cell newCell = new Cell();
                newCell.SetContents(formula);
                spreadsheet.Add(name, newCell);
            }
            HashSet<string> set = new HashSet<string>();
            foreach (String s in formula.GetVariables())
            {
                if (!isInvalid(s))
                {
                    set.Add(s.ToUpper());
                }
            }
            dp.ReplaceDependees(name, set);
            //replace dependencies
            return new HashSet<String>(GetCellsToRecalculate(name)); ;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            Cell cell = new Cell();
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (isInvalid(name))
            {
                throw new InvalidNameException();
            }
            if (spreadsheet.TryGetValue(name, out cell))
            {
                cell.SetContents(text);
            }
            else
            {
                Cell newCell = new Cell();
                newCell.SetContents(text);
                spreadsheet.Add(name, newCell);
                //spreadsheet[name].SetContents(number);
            }
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            Cell cell = new Cell();
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (isInvalid(name))
            {
                throw new InvalidNameException();
            }
            if (spreadsheet.TryGetValue(name, out cell))
            {
                cell.SetContents(number);
            }
            else
            {
                Cell newCell = new Cell();
                newCell.SetContents(number);
                spreadsheet.Add(name, newCell);
                //spreadsheet[name].SetContents(number);
            }
            return new HashSet<String>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            name = name.ToUpper();
            if (isInvalid(name.ToUpper()))
            {
                throw new InvalidNameException();
            }
            return dp.GetDependents(name);
        }

        /// <summary>
        /// Tests to see if Spreadsheet cell name is an invalid syntax
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isInvalid(String name)
        {
            bool hitDigit = false;
            char[] cArr = name.ToCharArray();
            for (int i = 0; i < cArr.Length; i++)
            {
                if (!Char.IsLetterOrDigit(cArr[i]))
                {
                    return true;
                }
                if (!Char.IsLetter(cArr[0]))
                {
                    return true;
                }
                if (Char.IsLetter(cArr[i]) && hitDigit == true)
                {
                    return true;
                }
                if (Char.IsNumber(cArr[i]))
                {
                    if (cArr[i].Equals('0') && Char.IsLetter(cArr[i - 1]))
                    {
                        return true;
                    }
                    hitDigit = true;
                }

            }
            if (hitDigit == false)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            Changed = false;
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", isValid.ToString());
                double d = 0;
                foreach (String name in GetNamesOfAllNonemptyCells())
                {
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", name);
                    if (GetCellContents(name).GetType() == typeof(String))
                    {
                        writer.WriteAttributeString("contents", (String) spreadsheet[name].GetContents());
                    }
                    else if (Double.TryParse(GetCellContents(name).ToString(), out d))
                    {
                        writer.WriteAttributeString("contents", d.ToString());
                    }
                    else
                    {
                        writer.WriteAttributeString("contents", "=" + GetCellContents(name).ToString());
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
            dest.Close();

        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (isInvalid(name))
            {
                throw new InvalidNameException();
            }
            Cell cell;
            if (spreadsheet.TryGetValue(name, out cell))
            {
                return cell.GetValue();
            }
            return "";

        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            HashSet<String> set = new HashSet<string>();
            double d = 0;
            if(name == null)
            {
                throw new InvalidNameException();
            }
            //check if string, double or formula
            if (!isInvalid(name))
            {
                if (Double.TryParse(content, out d))
                {
                    set = (HashSet<string>)SetCellContents(name, d);
                    Changed = true;
                    CalculateCells(set);
                }
                else if (content.StartsWith("="))
                {
                    Formula f = new Formula(content.Substring(1), s => s.ToUpper(), s => isValid.IsMatch(s.ToUpper()) && !isInvalid(s));
                    Changed = true;
                    set = (HashSet<string>)SetCellContents(name, f);
                    CalculateCells(set);

                }
                else
                {
                    Changed = true;
                    set = (HashSet<string>)SetCellContents(name, content);
                    CalculateCells(set);
                }
            }
            else
            {
                throw new InvalidNameException();
            }
            return set;
        }
        /// <summary>
        /// Takes in the cells that need to be recalculated and reevaluted them if any of them as a formula
        /// as its contents and resets all neccessary values.
        /// </summary>
        /// <param name="set"></param>
        private void CalculateCells(HashSet<string> set)
        {
            foreach (String s in set)
            {
                //Change
                if (spreadsheet[s].GetContents().GetType().Equals(typeof(Formula)))
                {
                    Formula f = (Formula)spreadsheet[s].GetContents();
                    try
                    {
                        spreadsheet[s].SetValue(f.Evaluate(GetDoubleValue));
                    }
                    catch (FormulaEvaluationException)
                    {
                        spreadsheet[s].SetValue(new FormulaError());
                    }
                }
            }
        }
        /// <summary>
        /// Is the validation callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(" *** Validation Error: {0}", e.Message);
        }
        /// <summary>
        /// Get value of given cell's name
        /// </summary>
        /// <returns></returns>
        private double GetDoubleValue(String name)
        {
            double d = 0;
            try
            {
                name = spreadsheet[name].GetValue().ToString();

            }
            catch (KeyNotFoundException)
            {
                throw new FormulaEvaluationException("Key or name doesnt exist");
            }

            if (Double.TryParse(name, out d))
            {
                return d;
            }
            else
            {
                throw new FormulaEvaluationException("Key or name doesnt exist");
            }

        }
    }
}

