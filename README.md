# entries_manager
Simple entries manager using a classic client-server architecture.
The user can enter a simple diary entry containing title and text message.
This project demonstrates my skills in desktop development (WPF/.NET), backend development (Express in Node.js)
and database development (Microsoft SQL Server/T-SQL).
This project also uses dependency injection in the desktop application using the Ninject library (NuGet package).
Refer to [the screenshot guide](#screenshot-guide) for more information.

## Used technologies
![.NET](https://img.shields.io/badge/.NET-violet?style=for-the-badge&logo=.NET) ![WPF](https://img.shields.io/badge/WPF-violet?style=for-the-badge&logo=.NET) ![C#](https://img.shields.io/badge/C%23-green?style=for-the-badge)
![Node.js](https://img.shields.io/badge/node.js-lightblue?style=for-the-badge&logo=node.js) ![Express](https://img.shields.io/badge/express-%2317191a?style=for-the-badge&logo=express) ![JavaScript](https://img.shields.io/badge/javascript-yellow?style=for-the-badge&logo=javascript) 
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-red?style=for-the-badge) ![T-SQL](https://img.shields.io/badge/T--SQL-%23eb99f7?style=for-the-badge)

* **Frontend:** WPF (C#) <br/>
* **Backend:** Express in Node.js <br/>
* **Database:** Microsoft SQL Server <br/>

## <a name="screenshot-guide"></a>Screenshot guide
Prerequisites: 
* Create a Microsoft SQL Server (either standalone or Docker)
* Create a new database called EntriesManager
* Execute the database [script](Database/sql-script-1.sql)
* Start the [backend](Backend) application (configure all necessary information [here](Backend/config))
* Start the [desktop](WPF_Frontend) application (configure all necessary information [here](WPF_Frontend/WPF_Frontend/Config))
 
**Step 1**: Register if you have no account.
</br>
![step1](img/step1.png)
</br>
**Step 2**: Log in.
</br>
![step2](img/step2.png)
</br>
**Step 3**: You can see the home screen now.
</br>
![step3](img/step3.png)
</br>
**Step 4**: The profile icon can be used to modify the profile.
</br>
![step4](img/step4.png)
</br>
**Step 5**: The user has to fill in the title and the text message in order to add a new entry.
</br>
![step5](img/step5.png)
</br>
**Step 6**: The entry is now ready to be added.
</br>
![step6](img/step6.png)
</br>
**Step 7**: The user can see the entry on the home screen now.
</br>
![step7](img/step7.png)
</br>
**Step 8**: The user can also update the entry.
</br>
![step8](img/step8.png)
</br>
**Step 9**: The whole text of the entry can be displayed using the "View" option (due to the lack of space).
</br>
![step9](img/step9.png)