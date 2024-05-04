
## About The Project

During my studies in Software and Information Systems Engineering, we learned application developemnt using C# with WPF and Windows Form.
We've Learned many aspects of Object Oriented Programming including Multithreaded programming.
This project mimics Google's Excel spreadsheet where multiple users can edit the table. utilizing locks such as Mutex and Semaphore we implemented an efficient way of locking 
the cell chosen by the user without hindering the performance. Each user is represented by a different thread, and when a thread has access to a certain cell the mutex and semaphore locks 
take action to see if the lock is with the current user or not.

### Built With
C# Windows Form Applications
Multithread Programming

## Workflow

After running the application you will be presented with the startup screen where you choose how to create your table:
![image](https://github.com/DannyKazakov/SpreadsheetApp/assets/113122323/1e831ccc-3842-43e2-957c-bf8c2ac4ccfa)
* The load button gives you an option to load an already created spreadsheet via this app or press create after providing the necessary details.

![image](https://github.com/DannyKazakov/SpreadsheetApp/assets/113122323/00157d8f-1460-49e5-9720-0fe3b42fc299)
![image](https://github.com/DannyKazakov/SpreadsheetApp/assets/113122323/ffc53a39-a239-41ce-9e0b-462880205c91)

You have options to add a row or a column to the table, which happens only if thread has available locks to add a row or a column and the ability to search for a string or setAll cells.
![image](https://github.com/DannyKazakov/SpreadsheetApp/assets/113122323/e18465e3-0d95-4a56-821f-4717e005938f)


To implement different users interacting with the spreadsheet, we created a threadpool of 5 people trying to perform different actions to see that they are done and not interrupting the actions of others
## Contact

### Daniel Kazakov
* https://www.linkedin.com/in/danielkazakov/
* Dannizakov@gmail.com
<p align="right">(<a href="#readme-top">back to top</a>)</p>



