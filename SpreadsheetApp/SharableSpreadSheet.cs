using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace SpreadsheetApp
{
    class SharableSpreadSheet
    {
        private String[,] spreadSheet;
        private Mutex[] rowsLock;
        private Mutex[] colsLock;
        private Mutex rwMutex; // everytime we want to lock a row or a column we first must recieve the rw mutex
        private Mutex m_counter; // used for the counter, when we get the rw we will be able to get m_counter
        private Mutex m_searchLimit;// get the mutex and the get the semaphore.
        private Mutex SL_Counter;// get to increase/decrease the searchers counter
        private Semaphore limitTheSearchUsers;
        private int rows;
        private int cols;
        private int searchLimit = -1;
        private int counter;// increased and decreased only if a user has the m_counter.
        private int searchers;// increase only when a searcher has the m_searchLimit.
        public SharableSpreadSheet(int nRows, int nCols, int nUsers = -1)
        {
            // nUsers used for setConcurrentSearchLimit, -1 mean no limit.
            // construct a nRows*nCols spreadsheet
            spreadSheet = new String[nRows, nCols];
            updateSpreadsheetFields(nRows, nCols);
            updateCounters();
            setConcurrentSearchLimit(nUsers);

        }
        // relevant only for the WindowsFormApplication
        public SharableSpreadSheet() { }
        private void updateCounters()
        {
            counter = 0;
            m_counter = new Mutex();
            m_searchLimit = new Mutex();
            searchers = 0;
            SL_Counter = new Mutex();
        }
        private void updateSpreadsheetFields(int nRows, int nCols)
        {
            rows = nRows;
            cols = nCols;
            rwMutex = new Mutex();
            rowsLock = new Mutex[rows];
            for (int i = 0; i < rows; i++)
                rowsLock[i] = new Mutex();
            colsLock = new Mutex[cols];
            for (int i = 0; i < cols; i++)
                colsLock[i] = new Mutex();
        }
        public String getCell(int row, int col)
        {
            // return the string at [row,col]

            if (Thread.CurrentThread.ManagedThreadId != 1)
                Console.WriteLine("User [{0}]: entered getCell method", Thread.CurrentThread.ManagedThreadId);
            String Content;


            rwMutex.WaitOne();



            colsLock[col].WaitOne(); //lock the column
            rowsLock[row].WaitOne();// lock the row


            rwMutex.ReleaseMutex();
            Content = spreadSheet[row, col]; // get the string from the spreadsheet
            colsLock[col].ReleaseMutex();
            rowsLock[row].ReleaseMutex();


            if (Thread.CurrentThread.ManagedThreadId != 1)
                Console.WriteLine("User [{0}]: [{1}] returned the string \"{2}\" from cell [{3},{4}].",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, Content, row, col);
            return Content;
        }
        public void setCell(int row, int col, String str)
        {
            // set the string at [row,col]

            if (Thread.CurrentThread.ManagedThreadId != 1)
            {
                Console.WriteLine("User [{0}]: entered setCell method", Thread.CurrentThread.ManagedThreadId);
            }

            rwMutex.WaitOne();



            colsLock[col].WaitOne(); //lock the column
            rowsLock[row].WaitOne();// lock the row




            rwMutex.ReleaseMutex();
            spreadSheet[row, col] = str;
            colsLock[col].ReleaseMutex();
            rowsLock[row].ReleaseMutex();


            if (Thread.CurrentThread.ManagedThreadId != 1)
                Console.WriteLine("User [{0}]: [{1}] string \"{2}\" inserted to cell [{3},{4}].",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str, row, col);
        }
        public Tuple<int, int> searchString(String str)
        {
            Console.WriteLine("User [{0}]: entered searchString method", Thread.CurrentThread.ManagedThreadId);
            int currentRow = -1, currentCol = -1, firstEntry = 0;//will throw an exception if still -1
            Tuple<int, int> stringLocation;
            if (searchLimit > 0)
            {
                m_searchLimit.WaitOne();
                limitTheSearchUsers.WaitOne();
                firstEntry = 1;

                SL_Counter.WaitOne();
                searchers++;

                SL_Counter.ReleaseMutex();
                m_searchLimit.ReleaseMutex();
            }
            for (int i = 0; i < rows; i++)
            {
                rwMutex.WaitOne();//get the rw lock

                rowsLock[i].WaitOne();//searching each row, suffice to lock the entire row rather then cells
                rwMutex.ReleaseMutex();
                for (int j = 0; j < cols; j++)
                {
                    if (spreadSheet[i, j] == null)
                        continue;
                    if (spreadSheet[i, j].Equals(str))
                    {
                        currentRow = i;
                        currentCol = j;
                        goto END_SEARCH;
                    }
                }
                rowsLock[i].ReleaseMutex();
            }
        // return first cell indexes that contains the string (search from first row to the last row)
        END_SEARCH:
            try
            {
                if (searchLimit > 0 && firstEntry == 1)
                {

                    SL_Counter.WaitOne();


                    firstEntry = 0;
                    limitTheSearchUsers.Release();
                    searchers--;
                    SL_Counter.ReleaseMutex();



                }
                if (currentRow == -1)
                    throw new Exception(String.Format("User [{0}]: Didn't find the string: " + str, Thread.CurrentThread.ManagedThreadId));
                stringLocation = new Tuple<int, int>(currentRow, currentCol);
                rowsLock[currentRow].ReleaseMutex();
                Console.WriteLine("User [{0}]: [{1}] found the first occurrence of string \"{2}\" from cell [{3},{4}].",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str, currentRow, currentCol);
                return stringLocation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }
        public void exchangeRows(int row1, int row2)
        {
            // exchange the content of row1 and row2
            Console.WriteLine("User [{0}]: entered exchangeRows method", Thread.CurrentThread.ManagedThreadId);

            rwMutex.WaitOne();
            String[] tmpContent = new String[cols];

            rowsLock[row1].WaitOne();
            rowsLock[row2].WaitOne();
            rwMutex.ReleaseMutex();
            // need to check if valid numbers
            //copy from row1 to tmp
            for (int i = 0; i < cols; i++)
                tmpContent[i] = spreadSheet[row1, i];
            //copy from row2 to row 1
            for (int i = 0; i < cols; i++)
                spreadSheet[row1, i] = spreadSheet[row2, i];
            //copy from tmp to row2
            for (int i = 0; i < cols; i++)
                spreadSheet[row2, i] = tmpContent[i];
            rowsLock[row1].ReleaseMutex();
            rowsLock[row2].ReleaseMutex();


            Console.WriteLine("User [{0}]: [{1}] exchanged rows {2} and {3}.",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, row1, row2);

        }
        public void exchangeCols(int col1, int col2)
        {
            // exchange the content of col1 and col2

            rwMutex.WaitOne();
            String[] tmpContent = new String[rows];

            colsLock[col1].WaitOne();
            colsLock[col2].WaitOne();
            rwMutex.ReleaseMutex();
            // need to check if valid numbers
            //copy from col1 to tmp
            for (int i = 0; i < rows; i++)
                tmpContent[i] = spreadSheet[i, col1];
            //copy from col2 to col 1
            for (int i = 0; i < rows; i++)
                spreadSheet[i, col1] = spreadSheet[i, col2];
            //copy from tmp to col2
            for (int i = 0; i < rows; i++)
                spreadSheet[i, col2] = tmpContent[i];
            colsLock[col1].ReleaseMutex();
            colsLock[col2].ReleaseMutex();
            Console.WriteLine("User [{0}]: [{1}] exchanged columns {2} and {3}.",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, col1, col2);
        }
        public int searchInRow(int row, String str)
        {
            // try using indexOf
            Console.WriteLine("User [{0}]: entered searchInRow method", Thread.CurrentThread.ManagedThreadId);

            int column = -1, firstEntry = 0;
            if (searchLimit > 0 && firstEntry == 0)
            {
                m_searchLimit.WaitOne();
                limitTheSearchUsers.WaitOne();
                firstEntry = 1;

                SL_Counter.WaitOne();
                searchers++;

                SL_Counter.ReleaseMutex();
                m_searchLimit.ReleaseMutex();
            }
            rwMutex.WaitOne();

            rowsLock[row].WaitOne();
            rwMutex.ReleaseMutex();

            // perform search in specific row
            for (int i = 0; i < cols; i++)
            {
                if (spreadSheet[row, i] == null)
                    continue;
                if (spreadSheet[row, i].Equals(str))
                {
                    column = i;
                    if (searchLimit > 0 && firstEntry == 1)
                    {
                        SL_Counter.WaitOne();


                        firstEntry = 0;
                        limitTheSearchUsers.Release();
                        searchers--;
                        SL_Counter.ReleaseMutex();


                    }
                    rowsLock[row].ReleaseMutex();

                    Console.WriteLine("User [{0}]: [{1}] searched the string \"{2}\" in row {3} and found it in column {4}.",
                        Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str, row, column);
                    return column;
                }
            }

            try
            {
                if (searchLimit > 0 && firstEntry == 1)
                {
                    SL_Counter.WaitOne();


                    firstEntry = 0;
                    limitTheSearchUsers.Release();
                    searchers--;
                    SL_Counter.ReleaseMutex();



                }
                rowsLock[row].ReleaseMutex();
                if (column == -1)
                    throw new Exception(String.Format("User [{0}]: Didn't find the str " + str + " in row " + row, Thread.CurrentThread.ManagedThreadId));
            }
            catch (Exception e)
            {


                Console.WriteLine(e.Message);
            }
            return -1;

        }
        public int searchInCol(int col, String str)
        {
            Console.WriteLine("User [{0}]: entered searchInCol method", Thread.CurrentThread.ManagedThreadId);

            int row = -1, firstEntry = 0;
            if (searchLimit > 0 && firstEntry == 0)
            {
                m_searchLimit.WaitOne();
                limitTheSearchUsers.WaitOne();
                firstEntry = 1;

                SL_Counter.WaitOne();
                searchers++;

                SL_Counter.ReleaseMutex();
                m_searchLimit.ReleaseMutex();
            }
            rwMutex.WaitOne();

            colsLock[col].WaitOne();
            rwMutex.ReleaseMutex();
            // perform search in specific column
            for (int i = 0; i < rows; i++)
            {
                if (spreadSheet[i, col] == null)
                    continue;
                if (spreadSheet[i, col].Equals(str))
                {
                    row = i;
                    if (searchLimit > 0 && firstEntry == 1)
                    {

                        SL_Counter.WaitOne();

                        firstEntry = 0;
                        limitTheSearchUsers.Release();
                        searchers--;
                        SL_Counter.ReleaseMutex();



                    }
                    colsLock[col].ReleaseMutex();
                    // Console.WriteLine("User [{0}]: released mutex on col {1}", Thread.CurrentThread.ManagedThreadId, col);

                    Console.WriteLine("User [{0}]: [{1}] searched the string \"{2}\" in column {3} and found it in row {4}.",
                        Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str, col, row);
                    return row;
                }
            }
            try
            {
                if (searchLimit > 0 && firstEntry == 1)
                {

                    SL_Counter.WaitOne();

                    firstEntry = 0;
                    limitTheSearchUsers.Release();
                    searchers--;
                    SL_Counter.ReleaseMutex();



                }
                colsLock[col].ReleaseMutex();
                // Console.WriteLine("User [{0}]: released mutex on col {1}", Thread.CurrentThread.ManagedThreadId, col);

                if (row == -1)
                    throw new Exception(String.Format("User [{0}]: Didn't find the str " + str + " in column " + col, Thread.CurrentThread.ManagedThreadId));
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return -1;

        }
        public Tuple<int, int> searchInRange(int col1, int col2, int row1, int row2, String str)
        {
            Console.WriteLine("User [{0}]: entered searchInRange method", Thread.CurrentThread.ManagedThreadId);

            int row = -1, col = -1, firstEntry = 0;
            if (searchLimit > 0 && firstEntry == 0)
            {
                m_searchLimit.WaitOne();
                limitTheSearchUsers.WaitOne();
                firstEntry = 1;

                SL_Counter.WaitOne();
                searchers++;

                SL_Counter.ReleaseMutex();
                m_searchLimit.ReleaseMutex();
            }

            // perform search within spesific range: [row1:row2,col1:col2] 
            //includes col1,col2,row1,row2
            //locking each row
            for (int i = row1; i < row2; i++)
            {


                rwMutex.WaitOne();

                rowsLock[i].WaitOne();
                rwMutex.ReleaseMutex();


                for (int j = col1; j < col2; j++)
                {
                    if (spreadSheet[i, j] == null)
                        continue;
                    if (spreadSheet[i, j].Equals(str))
                    {
                        row = i;
                        col = j;
                        if (searchLimit > 0 && firstEntry == 1)
                        {
                            SL_Counter.WaitOne();
                            firstEntry = 0;
                            limitTheSearchUsers.Release();
                            searchers--;
                            SL_Counter.ReleaseMutex();



                        }
                        rowsLock[row].ReleaseMutex();
                        Console.WriteLine("User [{0}]: [{1}] searched for the string \"{2}\" in rows {3}:{4} and columns {5}:{6} and found it in [{7}, {8}].",
                            Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str, row1, row2, col1, col2, row, col);
                        return new Tuple<int, int>(row, col);
                    }
                }
                rowsLock[i].ReleaseMutex();
            }
            try
            {
                if (searchLimit > 0 && firstEntry == 1)
                {
                    SL_Counter.WaitOne();

                    firstEntry = 0;
                    limitTheSearchUsers.Release();
                    searchers--;

                    SL_Counter.ReleaseMutex();

                }

                if (row == -1)
                    throw new Exception(String.Format("User [{0}]: Didn't find the str: " + str + " in the range [" + row1 + ":" + row2 + ", " + col1 + ":" + col2 + "]", Thread.CurrentThread.ManagedThreadId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;

        }
        public void addRow(int row1)
        {
            //add a row after row1
            Console.WriteLine("User [{0}]: entered addRow method", Thread.CurrentThread.ManagedThreadId);
            rwMutex.WaitOne();

            String[,] newSpreadSheet = new String[rows + 1, cols];

            while (counter > 0) ;//busy wait
            for (int i = 0; i <= row1; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newSpreadSheet[i, j] = spreadSheet[i, j];
                }
            }
            for (int i = row1 + 1; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    newSpreadSheet[i + 1, j] = spreadSheet[i, j];
                }
            }
            rows++;
            spreadSheet = newSpreadSheet;
            //increase the array of mutex by 1
            Mutex[] newrowsLock = new Mutex[rows];
            newrowsLock[rows - 1] = new Mutex();
            for (int i = 0; i < rows - 1; i++)
                newrowsLock[i] = rowsLock[i];
            rowsLock = newrowsLock;
            rwMutex.ReleaseMutex();
            Console.WriteLine("User [{0}]: [{1}] added a new row after row {2}.",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, row1);
        }
        /*lockingthe whole table since we want to add a new row and forthat we create a new spredsheet
         and copy all the cells from the old to the new*/
        public void addCol(int col1)
        {
            //add a column after col1
            Console.WriteLine("User [{0}]: entered addCol method", Thread.CurrentThread.ManagedThreadId);
            rwMutex.WaitOne();

            String[,] newSpreadSheet = new String[rows, cols + 1];
            while (counter > 0) ; //busy wait
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j <= col1; j++)
                {
                    newSpreadSheet[i, j] = spreadSheet[i, j];
                }
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = col1 + 1; j < cols; j++)
                {
                    newSpreadSheet[i, j + 1] = spreadSheet[i, j];
                }
            }
            cols++;
            spreadSheet = newSpreadSheet;
            // since we added a new column we need to add new locks
            Mutex[] newcolsLock = new Mutex[cols];
            newcolsLock[cols - 1] = new Mutex();
            for (int i = 0; i < cols - 1; i++)
                newcolsLock[i] = colsLock[i];
            colsLock = newcolsLock;
            rwMutex.ReleaseMutex();
            Console.WriteLine("User [{0}]: [{1}] added a new column after column{2}.",
                Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, col1);
        }

        public Tuple<int, int>[] findAll(String str, bool caseSensitive)
        {
            // perform search and return all relevant cells according to caseSensitive param
            Console.WriteLine("User [{0}]: entered findAll method", Thread.CurrentThread.ManagedThreadId);

            LinkedList<Tuple<int, int>> list = new LinkedList<Tuple<int, int>>();
            HashSet<int> setOfRowsThatHave_str = new HashSet<int>();
            int firstEntry = 0;


            if (caseSensitive)
            {

                for (int i = 0; i < rows; i++)
                {

                    rwMutex.WaitOne();

                    rowsLock[i].WaitOne();
                    rwMutex.ReleaseMutex();

                    for (int j = 0; j < cols; j++)
                    {
                        if (spreadSheet[i, j] == null)
                            continue;
                        if (spreadSheet[i, j].Equals(str))
                        {
                            list.AddLast(new Tuple<int, int>(i, j));
                            setOfRowsThatHave_str.Add(i);
                        }
                    }

                    rowsLock[i].ReleaseMutex();

                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {

                    rwMutex.WaitOne();

                    rowsLock[i].WaitOne();

                    rwMutex.ReleaseMutex();

                    for (int j = 0; j < cols; j++)
                    {
                        if (CaseInsensitiveContains(spreadSheet[i, j], str))
                        {
                            list.AddLast(new Tuple<int, int>(i, j));
                            setOfRowsThatHave_str.Add(i);

                        }
                    }

                    rowsLock[i].ReleaseMutex();


                }
            }



            if (list.Count > 0)
                Console.WriteLine("User [{0}]: [{1}] found all occurrences of string \"{2}\" at [{3}].",
                    Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str, string.Join(", ", list));
            else
            {
                Console.WriteLine("User [{0}]: [{1}] didn't find any occurrences of string \"{2}\"",
                   Thread.CurrentThread.ManagedThreadId, DateTime.Now.TimeOfDay, str);
            }
            return list.ToArray();
        }

        public void setAll(String oldStr, String newStr, bool caseSensitive)
        {
            // replace all oldStr cells with the newStr str according to caseSensitive param
            //if case sensitive search for the cell contains oldStr and replace all occurences with newStr


            if (caseSensitive)
            {
                for (int i = 0; i < rows; i++)
                {

                    rwMutex.WaitOne();

                    rowsLock[i].WaitOne();
                    rwMutex.ReleaseMutex();

                    for (int j = 0; j < cols; j++)
                    {
                        if (spreadSheet[i, j] == null)
                            continue;
                        if (spreadSheet[i, j].Equals(oldStr))
                        {
                            spreadSheet[i, j] = spreadSheet[i, j].Replace(oldStr, newStr);
                        }
                    }
                    rowsLock[i].ReleaseMutex();
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {

                    rwMutex.WaitOne();

                    rowsLock[i].WaitOne();
                    rwMutex.ReleaseMutex();

                    for (int j = 0; j < cols; j++)
                    {

                        if (CaseInsensitiveContains(spreadSheet[i, j], oldStr))//case not sensitive
                        {
                            spreadSheet[i, j] = Regex.Replace(spreadSheet[i, j], oldStr, newStr, RegexOptions.IgnoreCase); ;
                        }
                    }
                    rowsLock[i].ReleaseMutex();
                }
            }
       
        }
        public Tuple<int, int> getSize()
        {
            // return the size of the spreadsheet in nRows, nCols

            rwMutex.WaitOne();


            Tuple<int, int> size = new Tuple<int, int>(rows, cols);


            rwMutex.ReleaseMutex();

     
            return size;
        }
        public void setConcurrentSearchLimit(int nUsers)
        {
            // this function aims to limit the number of users that can perform the search operations concurrently.
            // The default is no limit. When the function is called, the max number of concurrent search operations is set to nUsers. 
            // In this case additional search operations will wait for existing search to finish.
            // This function is used just in the creation

            try
            {
                if (searchLimit == -1)
                {
                    rwMutex.WaitOne();
                    m_searchLimit.WaitOne();
                    while (counter > 0) ;
                    setTheSearchLimit(nUsers);
                    m_searchLimit.ReleaseMutex();
                    rwMutex.ReleaseMutex();
                }
                else
                {

                    m_searchLimit.WaitOne();

                    while (searchers > 0) ;
                    setTheSearchLimit(nUsers);

                    m_searchLimit.ReleaseMutex();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (searchLimit == -1)
                {
                    rwMutex.ReleaseMutex();

                    m_searchLimit.ReleaseMutex();
                }
                else
                {
                    m_searchLimit.ReleaseMutex();


                }
            }
        }
        private void setTheSearchLimit(int nUsers)
        {

            if (nUsers > 0 && limitTheSearchUsers == null)// the beginning
            {
                limitTheSearchUsers = new Semaphore(0, nUsers);
                limitTheSearchUsers.Release(nUsers);
                
                searchLimit = nUsers;


            }

            else if ((nUsers != -1 && nUsers != 0 && nUsers != searchLimit) && limitTheSearchUsers != null)//more users
            {
                limitTheSearchUsers = new Semaphore(0, nUsers);
                limitTheSearchUsers.Release(nUsers);
                

            }
            else if (nUsers == -1)
            {
                limitTheSearchUsers = null;
                searchLimit = nUsers;
            }
            else if (nUsers == 0)
                throw new Exception("nUsers can't be zero!");
        }
        public void save(String fileName)
        {
            // save the spreadsheet to a file fileName.
            using (Stream stream = File.Open(fileName + ".sprdsht", FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, spreadSheet);
            }
            // you can decide the format you save the data. There are several options.
        }
        public void load(String fileName)
        {
            // load the spreadsheet from fileName, must contain .sprdsht
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                spreadSheet = (String[,])bformatter.Deserialize(stream);
                rows = spreadSheet.GetLength(0);
                cols = spreadSheet.GetLength(1);
                updateSpreadsheetFields(rows, cols);
                updateCounters();

            }
            // replace the data and size of the current spreadsheet with the loaded data
        }
        
        private bool CaseInsensitiveContains(string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (text == null)
                return false;
            return text.Equals(value, stringComparison);
        }

    }
}


