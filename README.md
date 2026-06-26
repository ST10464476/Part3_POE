# Part3_POE
# Task & Security Chatbot

## 📋 Project Name
**Task & Security Chatbot** - A comprehensive WPF desktop application that combines task management with cybersecurity education.

---

## 📖 Brief Description
The **Task & Security Chatbot** is an intelligent WPF desktop application that serves two primary purposes:

1. **Task Management Assistant** - Helps users create, track, complete, and delete tasks with reminders.
2. **Cybersecurity Education** - Provides information about cybersecurity topics and offers interactive quizzes to test knowledge.

The application features a user-friendly chat interface where users can interact with the bot naturally, making it accessible for users of all technical levels.

---

## 🚀 How to Open and Run the Project

### Step 1: Prerequisites
- Visual Studio 2019 or later (Community, Professional, or Enterprise)
- .NET Framework 4.7.2 or later
- SQL Server LocalDB (included with Visual Studio)

### Step 2: Clone or Download
```bash
git clone [your-repository-url]
```
Or download the ZIP file and extract it to your desired location.

### Step 3: Open the Project
1. Navigate to the project folder
2. Double-click the solution file: `POE_Part3.sln`
3. Visual Studio will open with the project loaded

### Step 4: Restore NuGet Packages
- Right-click on the solution in Solution Explorer
- Select **Restore NuGet Packages**
- Wait for the packages to download and install

### Step 5: Build and Run
1. Press **F5** or click the **Start** button in Visual Studio
2. Alternatively, go to **Debug** → **Start Debugging**
3. The application window will open with the chatbot interface

---

## 💻 Software Required

| Software | Version | Purpose |
|----------|---------|---------|
| Visual Studio | 2019 or later | IDE for development and running |
| .NET Framework | 4.7.2 or later | Application framework |
| SQL Server LocalDB | 2016 or later | Local database for task storage |
| Windows OS | 10 or 11 | Operating system |
| NuGet Packages | Latest | Dependencies (auto-restored) |

### Required NuGet Packages:
- `System.Data.SqlClient` - Database connectivity
- The packages will be automatically restored when you build the project

---

## 🗄️ Database Setup Instructions

### Automatic Setup (Recommended)
1. The application will create the database automatically on first run
2. No manual setup is required

### Manual Setup (If Needed)
1. Open **SQL Server Object Explorer** in Visual Studio
2. Connect to `(localdb)\MSSQLLocalDB`
3. Run the following SQL script:

```sql
-- Create Database
CREATE DATABASE TaskChat;
GO

USE TaskChat;
GO

-- Create Task Table
CREATE TABLE dbo.[Task] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Reminder NVARCHAR(255),
    IsCompleted BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- Insert Sample Data (Optional)
INSERT INTO dbo.[Task] (Title, Description, Reminder, IsCompleted)
VALUES 
    ('Complete Security Training', 'Finish online cybersecurity course', 'Next Friday', 0),
    ('Update Passwords', 'Change passwords for all accounts', 'End of month', 0),
    ('Review Phishing Examples', 'Study common phishing techniques', 'Tomorrow', 1);
GO
```

### Connection String
The application uses the following connection string (configured in code):
```
Server=(localdb)\MSSQLLocalDB;Database=TaskChat;Trusted_Connection=True;TrustServerCertificate=True
```

---

## 📝 How to Use the Task Assistant

### Adding a Task
```
User: add task Buy groceries
Bot: 📝 Task title: 'Buy groceries'
     Please enter the description or type 'none' to skip

User: Buy milk, eggs, and bread
Bot: ✅ Description saved: 'Buy milk, eggs, and bread'
     Now enter the reminder or type 'none' to skip

User: Tomorrow at 9am
Bot: ✅ Task saved!
     • Title: Buy groceries
     • Description: Buy milk, eggs, and bread
     • Reminder: Tomorrow at 9am
```

### Alternative: Add Task in One Command
```
User: add task Complete report description Finish project report reminder Friday 5pm
Bot: ✅ Task saved!
     • Title: Complete report
     • Description: Finish project report
     • Reminder: Friday 5pm
```

### Viewing Tasks
```
User: show tasks
Bot: 📋 Your Tasks:
      1. Buy groceries
         📝 Description: Buy milk, eggs, and bread
         ⏰ Reminder: Tomorrow at 9am
         Status: ❌ Pending

      2. Complete report
         📝 Description: Finish project report
         ⏰ Reminder: Friday 5pm
         Status: ❌ Pending
```

### Completing a Task
```
User: done 1
Bot: ✅ Task 1 completed!
```

### Deleting a Task
```
User: delete 2
Bot: ✅ Task 2 deleted!
```

### Task Management Commands Summary
| Command | Example | Description |
|---------|---------|-------------|
| `add task [title]` | `add task Buy groceries` | Create a new task |
| `show tasks` | `show tasks` | View all tasks |
| `done [id]` | `done 1` | Mark task as completed |
| `delete [id]` | `delete 1` | Remove a task |
| `show reminders` | `show reminders` | View tasks with reminders |

---

## 🎮 How to Access the Quiz/Mini-Game

### From the Chatbot
1. Type any of the following commands:
   - `quiz`
   - `question`
   - `test`

2. The bot will respond with the first quiz question:
   ```
   Bot: 🎯 Question 1 of 5:
        What does 2FA stand for?
        
        A) Two-Factor Authentication
        B) Two-Factor Authorization
        C) Two-Factor Access
        D) Two-Factor Approval
        
        Type your answer (A, B, C, or D)
   ```

3. Answer by typing the letter:
   ```
   User: A
   Bot: ✅ Correct! 2FA stands for Two-Factor Authentication,
        which adds an extra layer of security.
   ```

4. Continue through all questions to see your final score:
   ```
   Bot: 🎉 Quiz Completed!
        Your final score: 4 out of 5
   ```

### From the Quiz Game Window
1. Click the **Quiz Game** button in the main window
2. A separate window will open with the quiz interface
3. Select an answer and click **Submit Answer**
4. View your progress and final results

### Sample Quiz Questions
The quiz covers cybersecurity topics including:
- Two-Factor Authentication (2FA)
- Phishing attacks
- Password security
- VPN usage
- Malware
- Email safety
- Firewalls

---

## 🧪 How to Test the NLP Simulation

The chatbot uses **Natural Language Processing (NLP) simulation** through pattern matching and keyword recognition. Here's how to test it:

### Test Greetings
```
User: Hello
Bot: Hi there! How can I help you today?

User: Hey
Bot: Hey! How are you doing?

User: Hi
Bot: Hello! Welcome to our support service.
```

### Test Security Topics
```
User: What is phishing?
Bot: Phishing is a cyber crime where attackers trick people into giving personal information...

User: Tell me about 2FA
Bot: 2FA stands for Two-Factor Authentication - an extra layer of security for your accounts.

User: Explain VPN
Bot: VPN (Virtual Private Network) creates a secure, encrypted tunnel for your internet traffic.
```

### Test "Tell Me More" Feature
```
User: What is phishing?
Bot: [Detailed response about phishing]

User: tell me more
Bot: [Extended information about phishing]
```

### Test Help Command
```
User: help
Bot: I'm here to assist you. What do you need help with?
```

### NLP Pattern Recognition Tests
| User Input | Expected Response |
|------------|-------------------|
| `How can I stay safe online?` | Security tips response |
| `What is malware?` | Malware explanation |
| `add task Buy groceries` | Task creation flow |
| `show my tasks` | List of tasks |
| `delete 1` | Delete task confirmation |
| `I need to finish my report` | Task creation suggestion |

### Advanced NLP Test
```
User: Could you please remind me to update my password?
Bot: [Task creation flow]
```

---

## 📊 How to View the Activity Log

### Chat History
All conversations are displayed in the chat interface:
- **User messages** appear on the right (blue bubbles)
- **Bot messages** appear on the left (gray bubbles)
- The chat scrolls automatically to show the latest messages

### Task History
1. Type `show tasks` to view all tasks
2. Tasks display with:
   - ID number
   - Title
   - Description
   - Reminder
   - Status (Completed/Pending)

### Quiz Results
After completing a quiz:
1. The bot displays your final score
2. Shows correct/incorrect answers
3. Provides explanations for each question

### Activity Log Summary
The application logs:
- 📝 All chat messages (user and bot)
- ✅ Task creation, completion, and deletion
- 🎯 Quiz attempts and scores
- 🔍 Security topic queries
- ⚠️ Error messages (if any)

---

## 🔐 Login Details and Important Notes

### No Login Required
- **The application does not require any login credentials**
- It's designed for local desktop use with a local database

### Database Notes
- The application uses LocalDB, which is included with Visual Studio
- Database file is stored in `C:\Users\[username]\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB`
- Tasks are stored locally and not shared across computers

### Security Notes
- 🔒 No sensitive data is transmitted over the internet
- 🔑 All data is stored locally on your machine
- 🛡️ The application does not collect or share any personal information

### Important Notes
1. **First Run**: The database is created automatically on first launch
2. **Data Persistence**: Tasks are saved even after closing the application
3. **Multi-window Support**: The chatbot and quiz game windows can operate simultaneously
4. **Keyboard Shortcuts**: Press `Enter` to send messages quickly

### Troubleshooting
Issue and Solution 

❌ Database connection error and a solution was  Ensure SQL Server LocalDB is installed |
❌ Build errors fixed by Clean solution and rebuild |
❌ Quiz not loading Check that SQL Server is running |
❌ Chat not responding fix by Restart the application |

### System Requirements
- **RAM**: 2GB minimum (4GB recommended)
- **Storage**: 200MB free space
- **Display**: 1024x768 resolution minimum
- **Framework**: .NET Framework 4.7.2 or later

---

## 🎥 Video Presentation

### Link to Video Presentation
On ARC submisson

The video covers:
- 📋 Project overview and features
- 🚀 Live demonstration of all functionalities
- 💬 Chatbot interaction examples
- 📝 Task management walkthrough
- 🎯 Quiz game demonstration
- 🔧 Setup and installation guide



### Feedback
We welcome feedback to improve the application:
- 💡 Feature suggestions
- 🐛 Bug reports
- 📝 Documentation improvements

---

## 📄 License
This project was created for educational purposes as part of a programming course.

---

## 👨‍💻 Team Members
- ST10464476  - Project Lead & Developer

---

## 🙏 Acknowledgments
- Instructors for guidance and support
- Open-source libraries and frameworks
- Cybersecurity resources for quiz content

---

*Last Updated: June 2024*
