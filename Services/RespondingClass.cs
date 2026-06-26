using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Part3_POE.Services;

public delegate string ResponseDelegate(string message);

public class RespondingClass
{
    private string currentTopic = null;
    private Dictionary<string, List<string>> topicDetails = new Dictionary<string, List<string>>();
    private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TaskChat;Trusted_Connection=True;TrustServerCertificate=True";
    private string step = "";
    private string pendingTitle = "";
    private string pendingDescription = "";
    private string pendingReminder = "";
    private string status = "";
    private List<string> chatHistory = new List<string>();

    public string Responding(string message)
    {
        string msg = message.Trim();

        // Store message in chat history
        chatHistory.Add($"User: {message}");

        // Check for "tell me more" requests
        if (message.Contains("tell me more", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("give more", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(currentTopic))
            {
                return GetDetailedResponse(currentTopic);
            }
            else
            {
                return "You haven't asked about any security topics yet. What would you like to know about?";
            }
        }

        // Process task-related messages
        string taskResponse = ProcessMessage(message);
        if (!string.IsNullOrEmpty(taskResponse))
        {
            return taskResponse;
        }

        // Detect and store topic
        string detectedTopic = StoreTopic(message);
        if (!string.IsNullOrEmpty(detectedTopic))
        {
            currentTopic = detectedTopic;
        }

        // Check for quiz requests
        if (msg.Contains("quiz") || msg.Contains("question") || msg.Contains("test"))
        {
            return LoadGame();
        }

        // Check for task viewing requests - MOVED HERE to ensure it's checked
        if (msg.Contains("show") || msg.Contains("display") || msg.Contains("list") || msg.Contains("view"))
        {
            if (msg.Contains("task") || msg.Contains("todo") || msg.Contains("reminder"))
            {
                return LoadTasksWithReminders();
            }
            return LoadTasks();
        }

        // Dictionary of basic responses
        Dictionary<string, string> response = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "hello", "Hi there! How can I help you today?" },
            { "hi", "Hello! Welcome to our support service." },
            { "hey", "Hey! How are you doing?" },
            { "help", "I'm here to assist you. What do you need help with?" },
            { "what can i ask", "You can ask me about:\n• Security topics (phishing, passwords, malware, VPN, 2FA)\n• Tasks (add task, show tasks, done 1, delete 1)\n• Quiz questions\n• Reminders" },
            { "phishing", "Phishing is a cyber crime where attackers trick people into giving personal information through fake emails, messages, or websites. Always check links carefully and never share passwords online." },
            { "cyber crime", "Cyber crime includes illegal activities like hacking, scams, identity theft, and spreading malware. Stay safe by using strong passwords, updating software, and avoiding suspicious links." },
            { "how to stay safe on the internet", "Stay safe online by using strong passwords, avoiding suspicious links, and protecting your personal information." },
            { "password", "A secret word or phrase that must be used to gain admission to a place. Use strong passwords with a mix of letters, numbers, and symbols." },
            { "2fa", "2FA stands for Two-Factor Authentication - an extra layer of security for your accounts." },
            { "vpn", "VPN (Virtual Private Network) is a tool that creates a secure, encrypted tunnel for your internet traffic. It hides your real IP address and physical location." },
            { "malware", "Malware is 'malicious software,' an umbrella term that describes any malicious program or code that is harmful to systems." }
        };

        // Check for keyword matches
        foreach (var keyword in response.Keys)
        {
            if (msg.Contains(keyword))
            {
                string botResponse = response[keyword];
                chatHistory.Add($"Bot: {botResponse}");
                return botResponse;
            }
        }

        // Default response with helpful suggestions
        string defaultResponse = "I'm not sure how to respond to that. You can ask me about:\n" +
                                 "• Security topics (phishing, passwords, malware, VPN, 2FA)\n" +
                                 "• Tasks (add task, show tasks, done 1, delete 1)\n" +
                                 "• Quiz questions\n" +
                                 "• Reminders (show reminders, show tasks)\n" +
                                 "Or type 'help' for more options.";

        chatHistory.Add($"Bot: {defaultResponse}");
        return defaultResponse;
    }

    public string StoreTopic(string message)
    {
        string[] keywords = { "password", "security", "phishing", "vpn", "cyber crime",
                             "how to stay safe on the internet", "malware", "2fa" };

        foreach (string keyword in keywords)
        {
            if (message.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                currentTopic = keyword;
                return keyword;
            }
        }
        return null;
    }

    private string GetDetailedResponse(string keyword)
    {
        string response = "";

        switch (keyword.ToLower())
        {
            case "password":
                response = "Passwords should be long, unique, and difficult to guess. " +
                           "Use a combination of uppercase letters, lowercase letters, numbers, and symbols. " +
                           "Avoid using personal information like birthdays or names. " +
                           "A password manager can help store passwords securely, and enabling two-factor authentication adds extra protection.";
                break;

            case "security":
                response = "Cyber security is the practice of protecting devices, networks, and personal information from cyber threats. " +
                           "You can improve your security by keeping software updated, using antivirus programs, avoiding suspicious downloads, and creating strong passwords.";
                break;

            case "phishing":
                response = "Phishing is a scam where attackers pretend to be trusted companies or people to steal sensitive information. " +
                           "These attacks often happen through fake emails, text messages, or websites. " +
                           "Never click suspicious links, download unknown attachments, or share passwords online without verifying the source.";
                break;

            case "cyber crime":
                response = "Cyber crime refers to illegal activities carried out using computers or the internet. " +
                           "Examples include hacking, identity theft, online scams, spreading malware, and stealing personal information. " +
                           "To stay safe, use secure passwords, avoid suspicious websites, and keep your devices updated.";
                break;

            case "how to stay safe on the internet":
                response = "Internet safety means protecting yourself online from scams, hackers, and harmful content. " +
                           "Always use strong passwords, avoid sharing personal information publicly, and be careful when using public Wi-Fi networks. " +
                           "Think carefully before clicking on links or downloading files.";
                break;

            case "malware":
                response = "Malware is harmful software designed to damage devices or steal information. " +
                           "Common types include viruses, worms, ransomware, and spyware. " +
                           "Protect your computer by installing antivirus software and avoiding untrusted downloads.";
                break;

            case "vpn":
                response = "A VPN, or Virtual Private Network, helps protect your online privacy by encrypting your internet connection. " +
                           "It is useful when using public Wi-Fi because it helps prevent hackers from intercepting your data.";
                break;

            case "2fa":
                response = "Two-factor authentication (2FA) adds an extra layer of security to your accounts. " +
                           "After entering your password, you must verify your identity using a code sent to your phone or email.";
                break;

            default:
                response = "I can provide information about passwords, phishing, cyber crime, malware, internet safety, VPNs, and online security tips.";
                break;
        }

        chatHistory.Add($"Bot: {response}");
        return response;
    }

    private string ProcessMessage(string message)
    {
        string msg = message.ToLower();

        // Check for delete request
        if (IsDeleteRequest(message))
        {
            int id = ExtractNumber(message);
            if (id == 0)
            {
                return Bot("Please type task number. e.g: delete 1");
            }
            return DeleteTask(id);
        }

        // Check for complete request
        if (IsCompleteRequest(message))
        {
            int id = ExtractNumber(message);
            if (id == 0)
            {
                return Bot("Please type task number. e.g: done 1");
            }
            return CompleteTask(id);
        }

        // Handle task creation steps
        if (step == "description")
        {
            pendingDescription = message;
            if (pendingDescription.ToLower() == "none")
            {
                pendingDescription = "No description";
            }
            step = "reminder";
            return Bot("Enter reminder or type 'none'");
        }

        if (step == "reminder")
        {
            pendingReminder = message;
            if (pendingReminder.ToLower() == "none")
            {
                pendingReminder = "No reminder";
            }
            string result = SaveTask(pendingTitle, pendingDescription, pendingReminder);
            ClearPendingTask();
            return result;
        }

        // Check for task creation
        if (IsTaskRequest(message))
        {
            string title = ExtractTask(message);
            string description = ExtractDescription(message);
            string reminder = ExtractReminder(message);

            if (string.IsNullOrEmpty(title))
            {
                return Bot("Please type the task title");
            }

            if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(reminder))
            {
                return SaveTask(title, description, reminder);
            }

            pendingTitle = title;

            if (string.IsNullOrEmpty(description))
            {
                step = "description";
                return Bot("Enter the description or type 'none'");
            }

            pendingDescription = description;

            if (string.IsNullOrEmpty(reminder))
            {
                step = "reminder";
                return Bot("Enter reminder or type 'none'");
            }

            return SaveTask(pendingTitle, pendingDescription, pendingReminder);
        }

        return null; // No task-related command found
    }

    private bool IsTaskRequest(string message)
    {
        return Regex.IsMatch(
            message,
            @"\b(add|create|make)\s+(a\s+)?(new\s+)?(task|reminder|todo)\b|" +
            @"\bset\s+(a\s+)?reminder\b|" +
            @"\bremind\s+me\b|" +
            @"\bi\s+need\s+to\b",
            RegexOptions.IgnoreCase);
    }

    private bool IsDeleteRequest(string message)
    {
        return Regex.IsMatch(
            message, @"\b(delete|remove|erase|clear|cancel)\b", RegexOptions.IgnoreCase);
    }

    private bool IsCompleteRequest(string message)
    {
        return Regex.IsMatch(
            message, @"\b(done|finish|complete|completed)\b", RegexOptions.IgnoreCase);
    }

    private int ExtractNumber(string message)
    {
        Match match = Regex.Match(message, @"\d+");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        return 0;
    }

    private string ExtractTask(string message)
    {
        string task = message.Trim();
        task = Regex.Replace(task, @"\bdescription\b.*", "", RegexOptions.IgnoreCase);
        task = Regex.Replace(task, @"\breminder\b.*", "", RegexOptions.IgnoreCase);
        task = Regex.Replace(task, @"^(please\s?(can you\s+)?(could you)\s+)?", "", RegexOptions.IgnoreCase);
        task = Regex.Replace(task, @"[?.!]+$", "");
        return task.Trim();
    }

    private string ExtractDescription(string message)
    {
        Match match = Regex.Match(message,
            @"\bdescription\b\s*[:\-]?\s*(.*?)(\breminder\b|$)",
            RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }
        return "";
    }

    private string ExtractReminder(string message)
    {
        Match match = Regex.Match(message,
            @"\breminder\b\s*[:\-]?\s*(.*?)$",
            RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }
        return "";
    }

    private string SaveTask(string title, string description, string reminder)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO dbo.[Task] (Title, Description, Reminder, IsCompleted)
                                 VALUES (@Title, @Description, @Reminder, 0)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description ?? "No description");
                cmd.Parameters.AddWithValue("@Reminder", reminder ?? "No reminder");
                cmd.ExecuteNonQuery();
            }

            string response = $"✅ Task saved:\n• Title: {title}\n• Description: {description}\n• Reminder: {reminder}";
            chatHistory.Add($"Bot: {response}");
            return Bot(response);
        }
        catch (Exception ex)
        {
            string error = $"❌ Error: Task not saved - {ex.Message}";
            MessageBox.Show(ex.Message);
            return Bot(error);
        }
    }

    private string CompleteTask(int id)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE dbo.Task SET IsCompleted = 1 WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                int rows = cmd.ExecuteNonQuery();

                string response = rows > 0 ? $"✅ Task {id} completed!" : $"❌ Task #{id} does not exist";
                chatHistory.Add($"Bot: {response}");
                return Bot(response);
            }
        }
        catch (Exception ex)
        {
            string error = $"❌ Error: Could not complete task - {ex.Message}";
            return Bot(error);
        }
    }

    private string DeleteTask(int id)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM dbo.Task WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                int rows = cmd.ExecuteNonQuery();

                string response = rows > 0 ? $"✅ Task {id} deleted!" : $"❌ Task #{id} does not exist";
                chatHistory.Add($"Bot: {response}");
                return Bot(response);
            }
        }
        catch (Exception ex)
        {
            string error = $"❌ Error: Could not delete task - {ex.Message}";
            return Bot(error);
        }
    }

    // NEW: Load tasks with reminders only
    private string LoadTasksWithReminders()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Title, Description, Reminder, IsCompleted FROM dbo.[Task] ORDER BY Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    return Bot("📋 No tasks with reminders found. You can add a task with 'add task [title] description [desc] reminder [reminder]'");
                }

                StringBuilder tasksList = new StringBuilder();
                tasksList.AppendLine("📋 Your Tasks & Reminders:");

                while (reader.Read())
                {
                    string status = reader.GetBoolean("IsCompleted") ? "✅" : "❌";
                    string reminder = reader["Reminder"].ToString();

                    if (!string.IsNullOrEmpty(reminder) && reminder != "No reminder")
                    {
                        tasksList.AppendLine($"  {reader["Id"]}. {reader["Title"]}");
                        tasksList.AppendLine($"     📝 Description: {reader["Description"]}");
                        tasksList.AppendLine($"     ⏰ Reminder: {reminder}");
                        tasksList.AppendLine($"     Status: {status}");
                        tasksList.AppendLine("");
                    }
                    else
                    {
                        // Still show tasks without reminders but mark them
                        tasksList.AppendLine($"  {reader["Id"]}. {reader["Title"]} (No reminder set) {status}");
                    }
                }

                string response = tasksList.ToString();
                chatHistory.Add($"Bot: {response}");
                return Bot(response);
            }
        }
        catch (Exception ex)
        {
            string error = $"❌ Error: Failed to load tasks - {ex.Message}";
            MessageBox.Show(ex.Message);
            return Bot(error);
        }
    }

    // Modified LoadTasks with better formatting
    private string LoadTasks()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Title, Description, Reminder, IsCompleted FROM dbo.[Task] ORDER BY Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    return Bot("📋 No tasks saved yet. You can add a task with 'add task [title]'");
                }

                StringBuilder tasksList = new StringBuilder();
                tasksList.AppendLine("📋 Your Tasks:");

                while (reader.Read())
                {
                    string status = reader.GetBoolean("IsCompleted") ? "✅ Completed" : "❌ Pending";
                    string reminder = reader["Reminder"].ToString();

                    tasksList.AppendLine($"  {reader["Id"]}. {reader["Title"]}");
                    tasksList.AppendLine($"     📝 Description: {reader["Description"]}");

                    if (!string.IsNullOrEmpty(reminder) && reminder != "No reminder")
                    {
                        tasksList.AppendLine($"     ⏰ Reminder: {reminder}");
                    }

                    tasksList.AppendLine($"     Status: {status}");
                    tasksList.AppendLine("");
                }

                string response = tasksList.ToString();
                chatHistory.Add($"Bot: {response}");
                return Bot(response);
            }
        }
        catch (Exception ex)
        {
            string error = $"❌ Error: Failed to load tasks - {ex.Message}";
            MessageBox.Show(ex.Message);
            return Bot(error);
        }
    }

    private string LoadGame()
    {
        string response = "🎯 QUIZ TIME!\n\n" +
                         "Question 1: What does 2FA stand for?\n" +
                         "A) Two-Factor Authentication\n" +
                         "B) Two-Factor Authorization\n" +
                         "C) Two-Factor Access\n" +
                         "D) Two-Factor Approval\n\n" +
                         "Type your answer (A, B, C, or D)";

        chatHistory.Add($"Bot: {response}");
        return Bot(response);
    }

    private void ClearPendingTask()
    {
        step = "";
        pendingTitle = "";
        pendingDescription = "";
        pendingReminder = "";
    }

    private string Bot(string text)
    {
        return "Chatbot: " + text;
    }

    // Public method to get chat history
    public List<string> GetChatHistory()
    {
        return chatHistory;
    }
}