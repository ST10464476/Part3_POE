using System;
using System.Collections.Generic;

namespace   Part3_POE.Services;

public class Question
{
    public string QuestionText { get; set; }
    public string[] Options { get; set; }
    public int CorrectAnswer { get; set; }
    public string Explanation { get; set; }
}

public class Questions
{
    private List<Question> _questions;

    public Questions()
    {
        _questions = new List<Question>();
        LoadCybersecurityQuestions();
    }

    public List<Question> GetAllQuestions()
    {
        return _questions;
    }

    public Question GetQuestion(int index)
    {
        return index >= 0 && index < _questions.Count ? _questions[index] : null;
    }

    public int GetTotalQuestions()
    {
        return _questions.Count;
    }

    private void LoadCybersecurityQuestions()
    {
        _questions.Add(new Question
        {
            QuestionText = "What should you do if you receive an email asking for your password?",
            Options = new string[] {
                "Reply with your password",
                "Delete the email",
                "Report the email as phishing",
                "Ignore it"
            },
            CorrectAnswer = 2,
            Explanation = "Reporting phishing emails helps prevent scams and protects others from falling victim."
        });

        _questions.Add(new Question
        {
            QuestionText = "Which of the following is a sign of a phishing email?",
            Options = new string[] {
                "Email from a known contact",
                "Generic greeting like 'Dear Customer'",
                "Correct spelling and grammar",
                "Official company logo"
            },
            CorrectAnswer = 1,
            Explanation = "Phishing emails often use generic greetings because scammers don't know your name."
        });

        _questions.Add(new Question
        {
            QuestionText = "What should you check before clicking a link in an email?",
            Options = new string[] {
                "The color of the link",
                "The sender's name only",
                "The actual URL by hovering over it",
                "The email subject line"
            },
            CorrectAnswer = 2,
            Explanation = "Hovering over a link shows the real destination URL, which may reveal a fake website."
        });

        _questions.Add(new Question
        {
            QuestionText = "Which of these is a strong password?",
            Options = new string[] {
                "123456",
                "Password",
                "P@ssw0rd!2024",
                "Your birthday"
            },
            CorrectAnswer = 2,
            Explanation = "Strong passwords include uppercase, lowercase, numbers, and special characters."
        });

        _questions.Add(new Question
        {
            QuestionText = "What is multi-factor authentication (MFA)?",
            Options = new string[] {
                "Using multiple passwords",
                "Two or more verification methods",
                "Sharing passwords with others",
                "Storing passwords in a file"
            },
            CorrectAnswer = 1,
            Explanation = "MFA adds an extra layer of security by requiring something you know and something you have."
        });

        _questions.Add(new Question
        {
            QuestionText = "You receive a call from someone claiming to be from IT support asking for your password. What do you do?",
            Options = new string[] {
                "Give them your password",
                "Ask for their employee ID",
                "Hang up and call the official IT number",
                "Write down your password for them"
            },
            CorrectAnswer = 2,
            Explanation = "Legitimate IT support will never ask for your password. Always verify through official channels."
        });

        _questions.Add(new Question
        {
            QuestionText = "What should you do with sensitive documents you no longer need?",
            Options = new string[] {
                "Throw them in the trash",
                "Recycle them",
                "Shred them securely",
                "Give them to a friend"
            },
            CorrectAnswer = 2,
            Explanation = "Secure shredding prevents sensitive information from being recovered from the trash."
        });

        _questions.Add(new Question
        {
            QuestionText = "Which of the following is safe to share on social media?",
            Options = new string[] {
                "Your home address",
                "Your phone number",
                "Your favorite hobby",
                "Your date of birth"
            },
            CorrectAnswer = 2,
            Explanation = "Personal identifiable information (PII) like addresses, phone numbers, and birth dates should be kept private."
        });

        _questions.Add(new Question
        {
            QuestionText = "What does 'phishing' refer to?",
            Options = new string[] {
                "A type of fishing sport",
                "A cybersecurity threat where scammers trick you into revealing information",
                "A type of software update",
                "A social media trend"
            },
            CorrectAnswer = 1,
            Explanation = "Phishing attacks use deceptive emails, messages, or websites to steal personal information."
        });

        _questions.Add(new Question
        {
            QuestionText = "You receive a text message with a link to claim a free prize. What should you do?",
            Options = new string[] {
                "Click the link to claim the prize",
                "Share the link with friends",
                "Ignore and delete the message",
                "Reply to the message"
            },
            CorrectAnswer = 2,
            Explanation = "Unsolicited prize offers are often scams designed to steal your personal information."
        });
    }
}